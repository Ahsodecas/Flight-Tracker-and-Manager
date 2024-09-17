using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1
{
    public class FileReader
    { 
        public FileReader()
        {
        }
        public Dictionary<string, List<Object>> FTRRead(string FilePath, Data data)
        {
            List<Object> ReadFTRObjects = new List<Object>();
            Dictionary<string, List<Object>> ReadObjectFTR = new Dictionary<string, List<Object>>();
            StreamReader streamReader = new StreamReader(FilePath);
            FactoryObjectFromString factoryObjectFromString = new FactoryObjectFromString();
            string[] ObjectParameters;
            string? line;
            while (!streamReader.EndOfStream)
            {
                line = streamReader.ReadLine();
                if (line != null)
                {
                    ObjectParameters = line.Split(',');
                    if (!ReadObjectFTR.ContainsKey(ObjectParameters[0]))
                    {
                        ReadObjectFTR.Add(ObjectParameters[0], new List<Object>());
                    }
                    Object tempObject = factoryObjectFromString.FactoryObject(data, ObjectParameters);
                    ReadObjectFTR[ObjectParameters[0]].Add(tempObject);
                    ReadFTRObjects.Add(tempObject);
                    data.AddObject(tempObject);
                }
            }
            streamReader.Close();
            return ReadObjectFTR;
        }
    }
}
