using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ood_project1
{
    public class Crew : Person, IObserver
    {
        public ulong? Practice { get; private set; }
        public string? Role { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<Crew, string, string>> PropertyValues = new Dictionary<string, Func<Crew, string, string>>() {
            {"ID", (obj, field) => { return obj.ID.ToString(); }  },
            {"Practice", (obj, field) => { return obj.Practice.ToString(); } },
            {"Role", (obj, field) => { return obj.Role; } }
            };
        [JsonIgnore]
        public readonly Dictionary<string, Action<Crew, string, string>> PropertyValuesSet = new Dictionary<string, Action<Crew, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"Practice", (obj, value, field) => { obj.Practice = ulong.Parse(value); } },
            {"Role", (obj, value, field) => { obj.Role = value; } }
            };
        public Crew(): base()
        {
            this.Practice = null;
            this.Role = "";
        }
        public Crew(string type, UInt64 id, string name, UInt64 age, string phone, string email, UInt64 practice, string role) : base(type, id, name, age, phone, email)
        {
            this.Practice = practice;
            this.Role = role;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, UInt64 ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID, Parameters);
            this.Practice = UInt16.Parse(Parameters[6]);
            this.Role = Parameters[7];
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            UInt32 dataLength = BitConverter.ToUInt32(data.MessageBytes, 3) + 7;
            this.Practice = BitConverter.ToUInt16(data.MessageBytes, (int)dataLength - 3);
            this.Role = Encoding.ASCII.GetString(data.MessageBytes, (int)dataLength - 1, 1);
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
