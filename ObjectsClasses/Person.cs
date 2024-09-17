using NetTopologySuite.Mathematics;
using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ood_project1
{
    public abstract class Person : Object, IObserver
    {
        public string? Name { get; private set; }
        public ulong? Age { get; private set; }
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<Person, string, string>> PropertyValues = new Dictionary<string, Func<Person, string, string>>() {
            {"ID", (obj, field) => { return obj.ID.ToString(); }  },
            {"Name", (obj, field) => { return obj.Name; } },
            {"Phone", (obj, field) => { return obj.Phone; } },
            {"Age", (obj, field) => { return obj.Age.ToString(); } },
            {"Email", (obj, field) => { return obj.Email; } }};
        [JsonIgnore]
        public readonly Dictionary<string, Action<Person, string, string>> PropertyValuesSet = new Dictionary<string, Action<Person, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"Age", (obj, value, field) => { obj.Age = ulong.Parse(value); } },
            {"Name", (obj, value, field) => { obj.Name = value; } },
            {"Phone", (obj, value, field) => { obj.Phone = value;} },
            {"Email", (obj, value, field) => { obj.Email = value; } }
            };
        public Person() 
        {
            this.Name = "";
            this.Age = null;
            this.Phone = "";
            this.Email = "";
        }
        public Person(string type, UInt64 id, string name, UInt64 age, string phone, string email) : base(type, id)
        {
            this.Name = name;
            this.Age = age;
            this.Phone = phone;
            this.Email = email;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, ulong ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID);
            this.Name = Parameters[2];
            this.Age = UInt64.Parse(Parameters[3]);
            this.Phone = Parameters[4];
            this.Email = Parameters[5];
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            UInt16 nameLength = BitConverter.ToUInt16(data.MessageBytes, 15);
            this.Name = Encoding.ASCII.GetString(data.MessageBytes, 17, nameLength);
            this.Age = BitConverter.ToUInt16(data.MessageBytes, 17 + nameLength);
            this.Phone = Encoding.ASCII.GetString(data.MessageBytes, 19 + nameLength, 12);
            UInt16 emailLength = BitConverter.ToUInt16(data.MessageBytes, 31 + nameLength);
            this.Email = Encoding.ASCII.GetString(data.MessageBytes, 33 + nameLength, emailLength);
        }
        public new void UpdateContactInfo(ContactInfoUpdateArgs args, Log log) 
        {
            if (this.ID == args.ObjectID)
            {
                this.Email = args.EmailAddress;
                this.Phone = args.PhoneNumber;
                log.AddContactInfoLogging(args);
            }
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
