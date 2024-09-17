using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1
{

    public class FileWriter
    {
        public Dictionary<string, string> ObjectJSONSerializationFileNames;
        public Dictionary<string, StreamWriter> ObjectJSONStreamWriters;
        public FileWriter()
        {
            ObjectJSONSerializationFileNames = new Dictionary<string, string>()
                { { "C", "CrewJSONSerialization.json" },
              { "P", "PassengerJSONSerialization.json"},
              { "CA", "CargoJSONSerialization.json"},
              { "CP", "CargoPlaneJSONSerialization.json"},
              { "PP", "PassengerPlaneJSONSerialization.json"},
              { "AI", "AirportJSONSerialization.json"},
              { "FL", "FlightJSONSerialization.json"}
            };
        }
        public void CreateJSONStreamWriters()
        {
            ObjectJSONStreamWriters = new Dictionary<string, StreamWriter>()
              { { "C", new StreamWriter(ObjectJSONSerializationFileNames["C"]) },
              { "P", new StreamWriter(ObjectJSONSerializationFileNames["P"])},
              { "CA", new StreamWriter(ObjectJSONSerializationFileNames["CA"])},
              { "CP", new StreamWriter(ObjectJSONSerializationFileNames["CP"])},
              { "PP", new StreamWriter(ObjectJSONSerializationFileNames["PP"])},
              { "AI", new StreamWriter(ObjectJSONSerializationFileNames["AI"])},
              { "FL", new StreamWriter(ObjectJSONSerializationFileNames["FL"])}
            };
        }
        public void CloseJSONStreamWriters()
        {
            foreach (var sreamWriter in ObjectJSONStreamWriters.Values)
            {
                sreamWriter.Close();
            }
        }
        public void JSONWriteOneObjectType(List<Object> Objects)
        {
            int counter = 0;
            foreach (var ObjectToSerialize in Objects)
            {
                string jsonString = ObjectToSerialize.JSONSerializeObject();
                if (counter == 0)
                    jsonString = "[" + jsonString;
                else if (counter == Objects.Count() - 1)
                    jsonString += "]";
                else
                    jsonString += ",";
                ObjectJSONStreamWriters[ObjectToSerialize.Type].WriteLine(jsonString);
                counter++;
            }
        }
        public void JSONWriteManyObjectTypes(Dictionary<string, List<Object>> ObjectsLists)
        {
            this.CreateJSONStreamWriters();
            foreach (var objectsList in ObjectsLists.Values)
            {
                this.JSONWriteOneObjectType(objectsList);
            }
            this.CloseJSONStreamWriters();
        }
        public void JSONWriteRandomObjectTypes(List<Object> Objects)
        {
            int numberOfElements = Objects.Count();
            DateTime dateTime = DateTime.Now;
            StreamWriter streamWriter = new StreamWriter("snapshot_" + dateTime.Hour + "_" + dateTime.Minute + "_" + dateTime.Second + ".json");
            int counter = 0;
            for (int i = 0; i < numberOfElements;  i++)
            {
                string jsonString = Objects[i].JSONSerializeObject();
                if (counter == 0)
                    jsonString = "[" + jsonString;
                else if (counter == numberOfElements - 1)
                    jsonString += "]";
                else
                    jsonString += ",";
                
                streamWriter.WriteLine(jsonString);
                counter++;
            }
            streamWriter.Close();
        }
    }
}
