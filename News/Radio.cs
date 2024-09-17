using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class Radio : INewsProviders
{
    public string Name { get; private set; }
    public Radio(string name) {  Name = name; }
    public string Report(Airport airport)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Reporting for ").Append(this.Name) 
        .Append(". Ladies and Gentlemen, we are at the ").Append(airport.Name).Append(" airport");
        return sb.ToString();
    }

    public string Report(CargoPlane cargoPlane)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Reporting for ").Append(this.Name)
        .Append(". Ladies and Gentlemen, we are at the ").Append(cargoPlane.Serial).Append(" airport");
        return sb.ToString();
    }

    public string Report(PassengerPlane passengerPlane)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Reporting for ").Append(this.Name)
        .Append(". Ladies and Gentlemen, we are at the ").Append(passengerPlane.Serial).Append(" airport");
        return sb.ToString();
    }
}
