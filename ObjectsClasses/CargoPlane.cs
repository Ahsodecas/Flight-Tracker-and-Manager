using ExCSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ood_project1
{
    public class CargoPlane : Plane, IReportable, IObserver
    {
        public float? MaxLoad { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<CargoPlane, string, string>> PropertyValues = new Dictionary<string, Func<CargoPlane, string, string>>() {
            {"ID", (obj, field) => { return obj.ID.ToString(); }  },
            {"MaxLoad", (obj, field) => { return obj.MaxLoad.ToString(); } }
            };
        [JsonIgnore]
        public readonly Dictionary<string, Action<CargoPlane, string, string>> PropertyValuesSet = new Dictionary<string, Action<CargoPlane, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"MaxLoad", (obj, value, field) => { obj.MaxLoad = float.Parse(value); } },
            };
        public CargoPlane(): base()
        {
            MaxLoad = null;
        }
        public CargoPlane(string type, ulong id, string serial, string country, string model, float maxload) : base(type, id, serial, country, model)
        {
            MaxLoad = maxload;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, UInt64 ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID, Parameters);
            this.MaxLoad = Single.Parse(Parameters[5]);
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            UInt32 dataLength = BitConverter.ToUInt32(data.MessageBytes, 3) + 7;
            this.MaxLoad = BitConverter.ToSingle(data.MessageBytes, (int)dataLength - 4);
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
