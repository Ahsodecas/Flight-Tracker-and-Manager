using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ood_project1
{ 
    public class PassengerPlane : Plane, IReportable, IObserver
    {
        public ulong? FirstClassSize { get; private set; }
        public ulong? BusinessClassSize { get; private set; }
        public ulong? EconomyClassSize { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<PassengerPlane, string, string>> PropertyValues = new Dictionary<string, Func<PassengerPlane, string, string>>() {
            {"ID", (obj, field) => { return obj.ID.ToString(); }  },
            {"FirstClassSize", (obj, field) => { return obj.FirstClassSize.ToString(); } },
            {"EconomyClassSize", (obj, field) => { return obj.EconomyClassSize.ToString(); } },
            {"BusinessClassSize", (obj, field) => { return obj.BusinessClassSize.ToString(); } }
            };
        [JsonIgnore]
        public readonly Dictionary<string, Action<PassengerPlane, string, string>> PropertyValuesSet = new Dictionary<string, Action<PassengerPlane, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"FirstClassSize", (obj, value, field) => { obj.FirstClassSize = ulong.Parse(value); } },
            {"BusinessClassSize", (obj, value, field) => { obj.BusinessClassSize = ulong.Parse(value); } },
            {"EconomyClassSize", (obj, value, field) => { obj.EconomyClassSize = ulong.Parse(value); } }
            };
        public PassengerPlane() :base()
        {
            FirstClassSize = null;
            BusinessClassSize = null;
            EconomyClassSize = null;
        }
        public PassengerPlane(string type, ulong id, string serial, string country, string model, ulong firstClassSize, ulong businessClassSize, ulong economyClassSize) : base(type, id, serial, country, model)
        {
            FirstClassSize = firstClassSize;
            BusinessClassSize = businessClassSize;
            EconomyClassSize = economyClassSize;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, UInt64 ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID, Parameters);
            this.FirstClassSize = UInt16.Parse(Parameters[5]);
            this.BusinessClassSize = UInt16.Parse(Parameters[6]);
            this.EconomyClassSize = UInt16.Parse(Parameters[7]);
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            UInt32 dataLength = BitConverter.ToUInt32(data.MessageBytes, 3) + 7;
            this.FirstClassSize = BitConverter.ToUInt16(data.MessageBytes, (int)dataLength - 6);
            this.BusinessClassSize = BitConverter.ToUInt16(data.MessageBytes, (int)dataLength - 4);
            this.EconomyClassSize = BitConverter.ToUInt16(data.MessageBytes, (int)dataLength - 2);
        }

        public string Accept(INewsProviders newsProvider)
        {
            return newsProvider.Report(this);
        }
        public override string GetProperty(string field)
        {
            string[] parts = field.Split(".");
            if (PropertyValues.ContainsKey(parts[0]))
            {
                return PropertyValues[parts[0]].Invoke(this, field);
            }
            else
            {
                return base.GetProperty(field);
            }
        }
        public override void SetProperties(string field)
        {
            string[] parts = field.Split(new char[] { '=' });
            string[] fields = parts[0].Split(new char[] { '.' });
            if (PropertyValues.ContainsKey(fields[0]))
            {
                PropertyValuesSet[fields[0]].Invoke(this, parts[1], field);
            }
            else
            {
                base.SetProperties(field);
            }
        }
    }
}
