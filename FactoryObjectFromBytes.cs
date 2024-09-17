using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace ood_project1
{
    public class FactoryObjectFromBytes
    {
        private readonly Dictionary<string, Func<Object>> TypesFromBytes;
        public List<Object> createdObjects;
        public FactoryObjectFromBytes()
        {
            this.createdObjects = new List<Object>();
            this.TypesFromBytes = new Dictionary<string, Func<Object>>()
            { { "NCR", () => {return new Crew(); }},
              { "NPA", () => {return new Passenger(); }},
              { "NCA", () => {return new Cargo(); }},
              { "NCP", () => {return new CargoPlane(); }},
              { "NPP", () => {return new PassengerPlane(); }},
              { "NAI", () => {return new Airport(); }},
              { "NFL", () => {return new Flight(); }}
            };
        }
        public void FactoryObject(Data readData, NetworkSourceSimulator.NetworkSourceSimulator dataSource, NewDataReadyArgs index)
        {
            NetworkSourceSimulator.Message message = dataSource.GetMessageAt(index.MessageIndex);
            string ObjectType = Encoding.ASCII.GetString(message.MessageBytes, 0, 3);
             if (TypesFromBytes.ContainsKey(ObjectType))
             {
                var tempObject = TypesFromBytes[ObjectType].Invoke();
                tempObject.CreateObjectFromBytes(readData, message);
                this.createdObjects.Add(tempObject);
             }
            else
            {
                throw new Exception("Cannot create object of " + ObjectType + " type");
            }
        }
    }
}
