using DynamicData.Binding;
using NetTopologySuite.Simplify;
using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ood_project1
{
    public abstract class Object: IObserver
    {
        public string Type { get; private set; }
        public ulong ID { get; private set; }
        public Object()
        {
        }
        public Object(string type, UInt64 id)
        {
            this.Type = type;
            this.ID = id;
        }
        public virtual string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public virtual void CreateObjectFromString(Data data, string type, ulong id, string[]? parameters = null)
        {
            this.ID = id;
            this.Type = type;
        }
        public virtual void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            this.ID = BitConverter.ToUInt64(data.MessageBytes, 7);
            this.Type = Encoding.ASCII.GetString(data.MessageBytes, 0, 3);
        }

        public void UpdateID(IDUpdateArgs args, Log log)
        {
            if (this.ID == args.ObjectID)
            {
                this.ID = args.NewObjectID;
                log.AddIDLogging(args);
            }
        }
        public virtual string GetProperty(string field)
        {
            return "";
        }

        public void UpdatePosition(PositionUpdateArgs args, Log log)
        {
            if (this.ID == args.ObjectID)
                log.AddErrorLogging(args.ObjectID);
        }

        public void UpdateContactInfo(ContactInfoUpdateArgs args, Log log)
        {
            if (this.ID == args.ObjectID)
                log.AddErrorLogging(args.ObjectID);
        }
        public void SetObjectID(ulong newID)
        {
            this.ID = newID;
        }
        public virtual void SetProperties(string field)
        {
            
        }
    }
}
