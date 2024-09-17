using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class Television : INewsProviders
{
    public string Name { get; private set; }
    public Television(string name) { Name = name; }
    public string Report(Airport airport)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.Name).Append(": <An image of ").Append(airport.Name).Append(" airport>");
        return sb.ToString();
    }

    public string Report(CargoPlane cargoPlane)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.Name).Append(": <An image of ").Append(cargoPlane.Serial).Append(" cargo plane>");
        return sb.ToString();
    }

    public string Report(PassengerPlane passengerPlane)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.Name).Append(": <An image of ").Append(passengerPlane.Serial).Append(" passenger plane>");
        return sb.ToString();
    }
}
