using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class Newspaper : INewsProviders
{
    public string Name { get; private set; }
    public Newspaper(string name) { Name = name; }
    public string Report(Airport airport)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.Name).Append(" - A report from the ").Append(airport.Name)
        .Append(" airport, ").Append(airport.Country);
        return sb.ToString();
    }
    public string Report(CargoPlane cargoPlane)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.Name).Append(" - An interview with the crew of ").Append(cargoPlane.Serial)
        .Append(".");
        return sb.ToString();
    }
    public string Report(PassengerPlane passengerPlane)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.Name).Append(" - Breaking news! ").Append(passengerPlane.Model)
        .Append(" aircraft loses EASA fails certification after inspection of ").Append(passengerPlane.Serial);
        return sb.ToString();
    }
}
