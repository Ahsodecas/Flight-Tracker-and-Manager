using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class Worldposition
{
    public double Latitude { get; set; }

    public double Longtitude { get; set; }

    public Worldposition(double latitude, double longitude)
    {
        Latitude = latitude;
        Longtitude = longitude;
    }
    public string GetProperty(string field)
    {
        string[] parts = field.Split(".");
        if (parts.Length > 1)
        {
            if (parts[1] == "Lon")
            {
                return Longtitude.ToString();
            }
            else if (parts[1] == "Lat")
            {
                return Latitude.ToString();
            }
            else
            {
                throw new Exception("Unknown property");
            }
        }
        else
        {
            return "{" + Longtitude.ToString() + "," + Latitude.ToString() + "}";
        }
    }
    public void SetProperties(string field)
    {
        string[] parts = field.Split(new char[] { '='});
        string[] fields = parts[0].Split(new char[] { '.' });
        if (fields[1] == "Lon")
        {
            Longtitude = float.Parse(parts[1]);
        }
        else if (fields[1] == "Lat")
        {
            Latitude = float.Parse(parts[1]);
        }
        else
        {
            throw new Exception("Unknown property");
        }
    }
}
