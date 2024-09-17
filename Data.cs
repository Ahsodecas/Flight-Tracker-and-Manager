using DynamicData;

namespace ood_project1;

public class Data
{
    public List<Object> ReadObjects { get; private set; }
    public Data()
    { 
        this.ReadObjects = new List<Object>();
    }
    public void AddData(List<Object> readObjects)
    {
        this.ReadObjects = readObjects;
    }
    public void AddObject(Object obj)
    {
        this.ReadObjects.Add(obj);
    }
    public List<IReportable> SelectIReportableObjects()
    {
        List<IReportable> reportableObjects = new List<IReportable>();
        foreach (var obj in ReadObjects)
        {
            if (obj is IReportable repObj)
            {
                reportableObjects.Add(repObj);
            }
        }
        return reportableObjects;
    }
    public List<Flight> SelectFlights()
    {
        List <Flight> flights = new List <Flight>();
        foreach (var obj in ReadObjects)
        {
            if (obj is Flight flight)
            {
                flights.Add(flight);
            }
        }
        return flights;
    }
    public List<Airport> SelectAirports()
    {
        List<Airport> airports = new List<Airport>();
        foreach (var obj in ReadObjects)
        {
            if (obj is Airport airport)
            {
                airports.Add(airport);
            }
        }
        return airports;
    }
    public List<Crew> SelectCrew()
    {
        List<Crew> crews = new List<Crew>();
        foreach (var obj in ReadObjects)
        {
            if (obj is Crew crew)
            {
                crews.Add(crew);
            }
        }
        return crews;
    }
    public List<Cargo> SelectCargo()
    {
        List<Cargo> cargos = new List<Cargo>();
        foreach (var obj in ReadObjects)
        {
            if (obj is Cargo cargo)
            {
                cargos.Add(cargo);
            }
        }
        return cargos;
    }
    public List<CargoPlane> SelectCargoPlane()
    {
        List<CargoPlane> cargoPlanes = new List<CargoPlane>();
        foreach (var obj in ReadObjects)
        {
            if (obj is CargoPlane cargoPlane)
            {
                cargoPlanes.Add(cargoPlane);
            }
        }
        return cargoPlanes;
    }
    public List<PassengerPlane> SelectPassengerPlane()
    {
        List<PassengerPlane> passengerPlanes = new List<PassengerPlane>();
        foreach (var obj in ReadObjects)
        {
            if (obj is PassengerPlane passengerPlane)
            {
                passengerPlanes.Add(passengerPlane);
            }
        }
        return passengerPlanes;
    }
    public List<Passenger> SelectPassenger()
    {
        List<Passenger> passengers = new List<Passenger>();
        foreach (var obj in ReadObjects)
        {
            if (obj is Passenger passenger)
            {
                passengers.Add(passenger);
            }
        }
        return passengers;
    }
    public Object? FindObject(ulong id)
    {
        foreach (var obj in ReadObjects)
        {
            if (id == obj.ID)
            {
                return obj;
            }
        }
        return null;
    }
}
