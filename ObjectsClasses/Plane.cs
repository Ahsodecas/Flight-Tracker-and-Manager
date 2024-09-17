using Avalonia.Dialogs.Internal;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ExCSS;
using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ood_project1
{
    public abstract class Plane : Object, IObserver
    {
        public string? Serial { get; private set; }
        public string? Country { get; private set; }
        public string? Model { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<Plane, string, string>> PropertyValues = new Dictionary<string, Func<Plane, string, string>>() {
            {"ID", (obj, field) => { return obj.ID.ToString(); }  },
            {"Serial", (obj, field) => { return obj.Serial; } },
            {"Country", (obj, field) => { return obj.Country; } },
            {"Model", (obj, field) => { return obj.Model; } }};
        [JsonIgnore]
        public readonly Dictionary<string, Action<Plane, string, string>> PropertyValuesSet = new Dictionary<string, Action<Plane, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"Serial", (obj, value, field) => { obj.Serial = value; } },
            {"Country", (obj, value, field) => { obj.Country = value;} },
            {"Model", (obj, value, field) => { obj.Model = value; } }
            };
        public Plane() 
        {
            Serial = "";
            Country = "";
            Model = "";
        }
        public Plane(string type, ulong id, string serial, string country, string model) : base(type, id)
        {
            Serial = serial;
            Country = country;
            Model = model;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string Type, ulong ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, Type, ID);
            this.Serial = Parameters[2];
            this.Country = Parameters[3];
            this.Model = Parameters[4];
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            this.Serial = Encoding.ASCII.GetString(data.MessageBytes, 15, 10).TrimEnd('\0');
            this.Country = Encoding.ASCII.GetString(data.MessageBytes, 25, 3);
            UInt16 modelLength = BitConverter.ToUInt16(data.MessageBytes, 28);
            this.Model = Encoding.ASCII.GetString(data.MessageBytes, 30, modelLength);
        }
        public new void UpdatePosition(PositionUpdateArgs args, Log log)
        {
            if (this.ID == args.ObjectID)
            {
                log.AddPositionLogging(args);
            }
        }
        public override string GetProperty(string field)
        {
            string[] parts = field.Split(".");
            if (parts[0] == "PlaneID")
            {
                if ( parts.Length > 1)
                {
                   if (PropertyValues.ContainsKey(parts[1]))
                   {
                        return PropertyValues[parts[1]].Invoke(this, field);
                   }
                   else
                   {
                        throw new Exception("Unknown property");
                   }
                }
                return PropertyValues["ID"].Invoke(this, field);
            }
            else if (PropertyValues.ContainsKey(parts[0]))
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
