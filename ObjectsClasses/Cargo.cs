using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ood_project1
{
    public class Cargo : Object, IObserver, ILoad
    {
        public float? Weight { get; private set; }
        public string? Code { get; private set; }
        public string? Description { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<Cargo, string, string>> PropertyValues = new Dictionary<string, Func<Cargo, string, string>>() {
            {"ID", (obj, field) => { return obj.ID.ToString(); }  },
            {"Description", (obj, field) => { return obj.Description; } },
            {"Code", (obj, field) => { return obj.Code; } }
            };
        [JsonIgnore]
        public readonly Dictionary<string, Action<Cargo, string, string>> PropertyValuesSet = new Dictionary<string, Action<Cargo, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"Code", (obj, value, field) => {   obj.Code = value; } },
            {"Description", (obj, value, field) => { obj.Description = value; } },
            {"Weight", (obj, value, field) => { obj.Weight = float.Parse(value); } },
            };
        public Cargo()
        {
            this.Weight = null;
            this.Code = "" ;
            this.Description = "";
        }
        public Cargo(string type, UInt64 id, Single weight, string code, string description) : base(type, id)
        {
            this.Weight = weight;
            this.Code = code;
            this.Description = description;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, UInt64 ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID);
            this.Weight = Single.Parse(Parameters[2]);
            this.Code = Parameters[3];
            this.Description = Parameters[4];
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            UInt32 dataLength = BitConverter.ToUInt32(data.MessageBytes, 3) + 7;
            this.Weight = BitConverter.ToSingle(data.MessageBytes, 15);
            this.Code = Encoding.ASCII.GetString(data.MessageBytes, 19, 6);
            this.Description = Encoding.ASCII.GetString(data.MessageBytes, 27, (int)dataLength - 27);
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
                throw new Exception("Unknown property");
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
                throw new Exception("Unknown property");
            }
        }
    }
}
    
