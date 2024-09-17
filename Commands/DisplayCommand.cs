using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using ood_project1.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ood_project1;

public class DisplayCommand : ICommand
{
    private Dictionary<string, Action<string>> objectsTypesMap;
    private Data data;
    private Filter filter;

    public string ObjectClass { get; set; }
    public string Conditions { get; set; }
    public List<string> Fields { get; set; }

    public DisplayCommand(string objectClass, string conditions, List<string> fields) 
    {
        this.objectsTypesMap = new Dictionary<string, Action<string>>() { { "flight", DisplayFlight },
                                                                { "crew", DisplayCrew },
                                                                { "passenger", DisplayPassenger },
                                                                { "cargo", DisplayCargo },
                                                                { "cargoplane", DisplayCargoPlane },
                                                                { "passengerplane", DisplayPassengerPlane },
                                                                { "airport", DisplayAirport }};
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
    private void DisplayFlight(string command)
    {
        List<Flight> flights = filter.FilterFlight(Conditions, data);
        if (Fields[0] == "*" && flights.Count > 0)
        {
            Fields = (flights[0].PropertyValues.Keys).ToList();
        }
        List<List<string>> fieldsValues = new List<List<string>>(); 
        foreach (var field in Fields)
        {
            fieldsValues.Add(new List<string>() { field});
        }
        for (int i = 0; i < Fields.Count; i++)
        {
            foreach (var flight in flights)
            {
                fieldsValues[i].Add(flight.GetProperty(Fields[i]));
            }
        }
        Display(fieldsValues);
    }
    private void Display(List<List<string>> fieldsValues)
    {
        List<int> maxLength = new List<int>();
        foreach (var field in fieldsValues)
        {
            maxLength.Add(field.Max((item) => { if (item != null) { return (item.Length + 2); } else return 0; }));
        }
        for (int i = 0; i < fieldsValues.Count; i++)
        {
            Console.Write(" " + fieldsValues[i][0].PadRight(maxLength[i]) + " |");
        }
        Console.WriteLine();
        for (int i = 0; i < fieldsValues.Count; i++)
        {
            for (int j = 0; j < maxLength[i] + 3; j++)
            {
                if (i < fieldsValues.Count && j < maxLength[i]  + 2)
                    Console.Write("-");
                else 
                    Console.Write("+");
            }
        }
        Console.WriteLine();
        for (int i = 1; i < fieldsValues[0].Count; i++)
        {
            for (int j = 0; j < fieldsValues.Count; j++)
            {
                Console.Write(" " + fieldsValues[j][i].PadLeft(maxLength[j]) + " |");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    private void DisplayCrew(string command)
    {
        List<Crew> crews = filter.FilterCrew(Conditions, data);
        if (Fields[0] == "*" && crews.Count > 0)
        {
            Fields = (crews[0].PropertyValues.Keys).ToList();
        }
        List<List<string>> fieldsValues = new List<List<string>>();
        foreach (var field in Fields)
        {
            fieldsValues.Add(new List<string>() { field });
        }
        for (int i = 0; i < Fields.Count; i++)
        {
            foreach (var crew in crews)
            {
                fieldsValues[i].Add(crew.GetProperty(Fields[i]));
            }
        }
        Display(fieldsValues);
    }
    private void DisplayPassenger(string command)
    {
        List<Passenger> passengers = filter.FilterPassenger(Conditions, data);
        if (Fields[0] == "*" && passengers.Count > 0)
        {
            Fields = (passengers[0].PropertyValues.Keys).ToList();
        }
        List<List<string>> fieldsValues = new List<List<string>>();
        foreach (var field in Fields)
        {
            fieldsValues.Add(new List<string>() { field });
        }
        for (int i = 0; i < Fields.Count; i++)
        {
            foreach (var passenger in passengers)
            {
                fieldsValues[i].Add(passenger.GetProperty(Fields[i]));
            }
        }
        Display(fieldsValues);
    }
    private void DisplayCargo(string command)
    {
        List<Cargo> cargos = filter.FilterCargo(Conditions, data);
        if (Fields[0] == "*" && cargos.Count > 0)
        {
            Fields = (cargos[0].PropertyValues.Keys).ToList();
        }
        List<List<string>> fieldsValues = new List<List<string>>();
        foreach (var field in Fields)
        {
            fieldsValues.Add(new List<string>() { field });
        }
        for (int i = 0; i < Fields.Count; i++)
        {
            foreach (var cargo in cargos)
            {
                fieldsValues[i].Add(cargo.GetProperty(Fields[i]));
            }
        }
        Display(fieldsValues);
    }
    private void DisplayCargoPlane(string command)
    {
        List<CargoPlane> cargoPlanes = filter.FilterCargoPlane(Conditions, data);
        if (Fields[0] == "*" && cargoPlanes.Count > 0)
        {
            Fields = (cargoPlanes[0].PropertyValues.Keys).ToList();
        }
        List<List<string>> fieldsValues = new List<List<string>>();
        foreach (var field in Fields)
        {
            fieldsValues.Add(new List<string>() { field });
        }
        for (int i = 0; i < Fields.Count; i++)
        {
            foreach (var cargoPlane in cargoPlanes)
            {
                fieldsValues[i].Add(cargoPlane.GetProperty(Fields[i]));
            }
        }
        Display(fieldsValues);
    }
    private void DisplayPassengerPlane(string command)
    {
        List<PassengerPlane> passengerPlanes = filter.FilterPassengerPlane(Conditions, data);
        if (Fields[0] == "*" && passengerPlanes.Count > 0)
        {
            Fields = (passengerPlanes[0].PropertyValues.Keys).ToList();
        }
        List<List<string>> fieldsValues = new List<List<string>>();
        foreach (var field in Fields)
        {
            fieldsValues.Add(new List<string>() { field });
        }
        for (int i = 0; i < Fields.Count; i++)
        {
            foreach (var passengerPlane in passengerPlanes)
            {
                fieldsValues[i].Add(passengerPlane.GetProperty(Fields[i]));
            }
        }
        Display(fieldsValues);
    }
    private void DisplayAirport(string command)
    {
        List<Airport> airports = filter.FilterAirport(Conditions, data);
        if (Fields[0] == "*" && airports.Count > 0)
        {
            Fields = (airports[0].PropertyValues.Keys).ToList();
        }
        List<List<string>> fieldsValues = new List<List<string>>();
        foreach (var field in Fields)
        {
            fieldsValues.Add(new List<string>() { field });
        }
        for (int i = 0; i < Fields.Count; i++)
        {
            foreach (var airport in airports)
            {
                fieldsValues[i].Add(airport.GetProperty(Fields[i]));
            }
        }
        Display(fieldsValues);
    }


}
