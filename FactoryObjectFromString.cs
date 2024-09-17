using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1
{
    public class FactoryObjectFromString
    {
        private readonly Dictionary<string, Func<Object>> TypesFromString;
        public FactoryObjectFromString()
        {
            TypesFromString = new Dictionary<string, Func<Object>>()
            { { "C", () => { return new Crew(); } },
              { "P",  () => { return new Passenger(); }},
              { "CA", () => { return new Cargo(); }},
              { "CP", () => { return new CargoPlane(); }},
              { "PP", () => { return new PassengerPlane(); }},
              { "AI", () => { return new Airport(); }},
              { "FL", () => { return new Flight(); }}
            };
        }
        public Object FactoryObject(Data data, string[] Parameters)
        {
            string ObjectType = Parameters[0];
            if (TypesFromString.ContainsKey(ObjectType))
            {
                UInt64 ID = UInt64.Parse(Parameters[1]);
                var tempObject = TypesFromString[ObjectType].Invoke();
                tempObject.CreateObjectFromString(data, ObjectType, ID, Parameters);
                return tempObject;
            }
            else
            {
                throw new Exception("Cannot create object of " + ObjectType + " type");
            }
        }
    }
}
