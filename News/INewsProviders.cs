using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public interface INewsProviders
{
    public string Report(Airport airport);
    public string Report(CargoPlane cargoPlane);
    public string Report(PassengerPlane passengerPlane);
}
