using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ood_project1
{
    public class Airport : Object, IReportable, IObserver
    {
        public string? Name { get; private set; }
        public string? Code { get; private set; }
        public float? Longtitude { get; private set; }
        public float? Latitude { get; private set; }
        public float? AMSL { get; private set; }
        public string? Country { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<Airport, string, string>> PropertyValues = new Dictionary<string, Func<Airport, string, string>>() {
            {"ID", (airport, field) => { return airport.ID.ToString(); }  },
            {"Name", (airport, field) => { return airport.Name; } },
            {"Code", (airport, field) => { return airport.Code; } },
            {"Longtitude", (airport, field) => { return airport.Longtitude.ToString(); } },
            {"Latitude", (airport, field) => { return airport.Latitude.ToString(); } },
            {"AMSL", (airport, field) => { return airport.AMSL.ToString(); } },
            {"Country", (airport, field) => { return airport.Country; } },
            };
        [JsonIgnore]
        public readonly Dictionary<string, Action<Airport, string, string>> PropertyValuesSet = new Dictionary<string, Action<Airport, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"Name", (obj, value, field) => {   obj.Name = value; } },
            {"Code", (obj, value, field) => {   obj.Code = value; } },
            {"Longtitude", (obj, value, field) => {  obj.Longtitude = float.Parse(value); } },
            {"Latitude", (obj, value, field) => { obj.Latitude = float.Parse(value); } },
            {"Country", (obj, value, field) => { obj.Country = value; } },
            {"AMSL", (obj, value, field) => { obj.AMSL = float.Parse(value); } },
            };
        public Airport() 
        {
            Name = "";
            Code = "";
            Longtitude = 0;
            Latitude = 0;
            AMSL = 0;
            Country = "";
        }
            public Airport(string type, ulong id, string name, string code, float longtitude, float latitude, float aMSL, string country) : base(type, id)
        {
            Name = name;
            Code = code;
            Longtitude = longtitude;
            Latitude = latitude;
            AMSL = aMSL;
            Country = country;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, UInt64 ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID);
            this.Name = Parameters[2];
            this.Code = Parameters[3];
            this.Longtitude = Single.Parse(Parameters[4]);
            this.Latitude = Single.Parse(Parameters[5]); ;
            this.AMSL = Single.Parse(Parameters[6]);
            this.Country = Parameters[7];
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            UInt16 nameLength = BitConverter.ToUInt16(data.MessageBytes, 15);
            this.Name = Encoding.ASCII.GetString(data.MessageBytes, 17, nameLength);
            this.Code = Encoding.ASCII.GetString(data.MessageBytes, 17 + nameLength, 3);
            this.Longtitude = BitConverter.ToSingle(data.MessageBytes, 20 + nameLength);
            this.Latitude = BitConverter.ToSingle(data.MessageBytes, 24 + nameLength);
            this.AMSL = BitConverter.ToSingle(data.MessageBytes, 28 + nameLength);
            this.Country = Encoding.ASCII.GetString(data.MessageBytes, 32 + nameLength, 3);
        }
        public string Accept(INewsProviders newsProvider)
        {
            return newsProvider.Report(this);
        }
        public override string GetProperty(string field)
        {
            string[] parts = field.Split(".");

            if (parts[0] == "Origin" || parts[0] == "Target")
            {
                if (parts.Length > 1)
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
                return PropertyValues["Name"].Invoke(this, field);
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
            if (fields[0] == "Origin" || fields[0] == "Target")
            {
                if (fields.Length > 1)
                {
                    if (PropertyValuesSet.ContainsKey(fields[1]))
                    {
                        PropertyValuesSet[fields[1]].Invoke(this, parts[1], field);
                    }
                    else
                    {
                        throw new Exception("Unknown property");
                    }
                }
            }
            else if (PropertyValuesSet.ContainsKey(fields[0]))
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
