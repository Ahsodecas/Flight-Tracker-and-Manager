using ood_project1.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class DeleteCommand : ICommand
{
    private Data data;
    private Filter filter;

    public string ObjectClass { get; set; }
    public string Conditions { get; set; }
    public List<string> Fields { get; set; }
    private Dictionary<string, Action<string>> objectsTypesMap;
    public DeleteCommand(string objectClass, string conditions, List<string> fields)
    {
        this.objectsTypesMap = new Dictionary<string, Action<string>>() { { "flight", DeleteFlight },
                                                                { "crew", DeleteCrew },
                                                                { "passenger", DeletePassenger },
                                                                { "cargo", DeleteCargo },
                                                                { "cargoplane", DeleteCargoPlane },
                                                                { "passengerplane", DeletePassengerPlane },
                                                                { "airport", DeleteAirport }};
        filter = new Filter();
        this.Conditions = conditions;
        this.ObjectClass = objectClass;
        this.Fields = fields;
    }
    public void ExecuteCommand(string objectType, string command, Data data)
    {
        this.data = data;
        if (objectsTypesMap.ContainsKey(objectType))
        {
            objectsTypesMap[objectType].Invoke(command);
        }
        else
        {
            throw new ArgumentException("Invalid object type");
        }
    }
    private void DeleteFlight(string command)
    {
        List<Flight> objects = filter.FilterFlight(Conditions, data);
        foreach (var obj in objects)
        {
            data.ReadObjects.Remove(obj);
        }
    }
    private void DeleteCrew(string command)
    {
        List<Crew> objects = filter.FilterCrew(Conditions, data);
        foreach (var obj in objects)
        {
            data.ReadObjects.Remove(obj);
        }
    }
    private void DeletePassenger(string command)
    {
        List<Passenger> objects = filter.FilterPassenger(Conditions, data);
        foreach (var obj in objects)
        {
            data.ReadObjects.Remove(obj);
        }
    }
    private void DeleteCargo(string command)
    {
        List<Cargo> objects = filter.FilterCargo(Conditions, data);
        foreach (var obj in objects)
        {
            data.ReadObjects.Remove(obj);
        }
    }
    private void DeleteCargoPlane(string command)
    {
        List<CargoPlane> objects = filter.FilterCargoPlane(Conditions, data);
        foreach (var obj in objects)
        {
            data.ReadObjects.Remove(obj);
        }
    }
    private void DeletePassengerPlane(string command)
    {
        List<PassengerPlane> objects = filter.FilterPassengerPlane(Conditions, data);
        foreach (var obj in objects)
        {
            data.ReadObjects.Remove(obj);
        }
    }
    private void DeleteAirport(string command)
    {
        List<Airport> objects = filter.FilterAirport(Conditions, data);
        foreach (var obj in objects)
        {
            data.ReadObjects.Remove(obj);
        }
    }
}
