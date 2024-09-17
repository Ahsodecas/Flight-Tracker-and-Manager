using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ood_project1
{
    public class Passenger : Person, IObserver, ILoad
    {
        public string? Class { get; private set; }
        public ulong? Miles { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<Passenger, string, string>> PropertyValues = new Dictionary<string, Func<Passenger, string, string>>() {
            {"ID", (obj, field) => { return obj.ID.ToString(); }  },
            {"Miles", (obj, field) => { return obj.Miles.ToString(); } },
            {"Class", (obj, field) => { return obj.Class; } }
            };
        [JsonIgnore]
        public readonly Dictionary<string, Action<Passenger, string, string>> PropertyValuesSet = new Dictionary<string, Action<Passenger, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"Miles", (obj, value, field) => { obj.Miles = ulong.Parse(value); } },
            {"Class", (obj, value, field) => { obj.Class = value; } }
            };
        public Passenger() : base()
        {
            Class = "";
            Miles = null;
        }
        public Passenger(string type, ulong id, string name, ulong age, string phone, string email, string _class, ulong miles) : base(type, id, name, age, phone, email)
        {
            Class = _class;
            Miles = miles;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, UInt64 ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID, Parameters);
            this.Class = Parameters[6];
            this.Miles = UInt16.Parse(Parameters[7]);
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            UInt32 dataLength = BitConverter.ToUInt32(data.MessageBytes, 3) + 7;
            this.Class = Encoding.ASCII.GetString(data.MessageBytes, (int)dataLength - 9, 1);
            this.Miles = BitConverter.ToUInt64(data.MessageBytes, (int)dataLength - 8);
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
