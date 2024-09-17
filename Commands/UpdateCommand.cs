using ood_project1.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class UpdateCommand : ICommand
{
    private Data data;
    private Filter filter;

    public string ObjectClass { get; set; }
    public string Conditions { get; set; }
    public List<string> Fields { get; set; }
    private Dictionary<string, Action<string>> objectsTypesMap;

    public UpdateCommand(string objectClass, string conditions, List<string> fields)
    {
        filter = new Filter();
        this.Conditions = conditions;
        this.ObjectClass = objectClass;
        this.Fields = fields;
        this.objectsTypesMap = new Dictionary<string, Action<string>>() { { "flight", UpdateFlight },
                                                                { "crew", UpdateCrew },
                                                                { "passenger", UpdatePassenger },
                                                                { "cargo", UpdateCargo },
                                                                { "cargoplane", UpdateCargoPlane },
                                                                { "passengerplane", UpdatePassengerPlane },
                                                                { "airport", UpdateAirport }};
    }

    public void ExecuteCommand(string objectType, string command, Data data)
    {
        this.data = data;
        string[] parts = command.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        int whereIndex = Array.IndexOf(parts, "set");
        if (whereIndex != -1)
        {
            Fields = new List<string>();
            for (int i = whereIndex + 1; i < parts.Length && (parts[i] != "where"); i++)
            {
                Fields.Add(parts[i]);
            }
        }

        if (objectsTypesMap.ContainsKey(objectType))
        {
            objectsTypesMap[objectType].Invoke(command);
        }
        else
        {
            throw new ArgumentException("Invalid object type");
        }
    }
    private void UpdateFlight(string command)
    {
        List<Flight> objects = filter.FilterFlight(Conditions, data);
        foreach (var obj in objects)
        {
            foreach (var field in Fields)
            {
                string[] parts = field.Split(new char[] { '=' });
                if (parts[0] == "ID")
                {
                    SetID(data, obj.ID, parts[1]);
                }
                obj.SetProperties(field);
            }
        }
    }
    private void UpdateCrew(string command)
    {
        List<Crew> objects = filter.FilterCrew(Conditions, data);
        foreach (var obj in objects)
        {
            foreach (var field in Fields)
            {
                string[] parts = field.Split(new char[] { '=' });
                if (parts[0] == "ID")
                {
                    SetID(data, obj.ID, parts[1]);
                }
                obj.SetProperties(field);
            }
        }
    }
    private void UpdatePassenger(string command)
    {
        List<Passenger> objects = filter.FilterPassenger(Conditions, data);
        foreach (var obj in objects)
        {
            foreach (var field in Fields)
            {
                string[] parts = field.Split(new char[] { '=' });
                if (parts[0] == "ID")
                {
                    SetID(data, obj.ID, parts[1]);
                }
                obj.SetProperties(field);
            }
        }
    }
    private void UpdateCargo(string command)
    {
        List<Cargo> objects = filter.FilterCargo(Conditions, data);
        foreach (var obj in objects)
        {
            foreach (var field in Fields)
            {
                string[] parts = field.Split(new char[] { '=' });
                if (parts[0] == "ID")
                {
                    SetID(data, obj.ID, parts[1]);
                }
                obj.SetProperties(field);
            }
        }
    }
    private void UpdateCargoPlane(string command)
    {
        List<CargoPlane> objects = filter.FilterCargoPlane(Conditions, data);
        foreach (var obj in objects)
        {
            foreach (var field in Fields)
            {
                string[] parts = field.Split(new char[] { '=' });
                if (parts[0] == "ID")
                {
                    SetID(data, obj.ID, parts[1]);
                }
                obj.SetProperties(field);
            }
        }
    }
    private void UpdatePassengerPlane(string command)
    {
        List<PassengerPlane> objects = filter.FilterPassengerPlane(Conditions, data);
        foreach (var obj in objects)
        {
            foreach (var field in Fields)
            {
                string[] parts = field.Split(new char[] { '=' });
                if (parts[0] == "ID")
                {
                    SetID(data, obj.ID, parts[1]);
                }
                obj.SetProperties(field);
            }
        }
    }
    private void UpdateAirport(string command)
    {
        List<Airport> objects = filter.FilterAirport(Conditions, data);
        foreach (var obj in objects)
        {
            foreach (var field in Fields)
            {
                string[] parts = field.Split(new char[] { '=' });
                if (parts[0] == "ID")
                {
                    SetID(data, obj.ID, parts[1]);
                }
                obj.SetProperties(field);
            }
        }

    }
    public void SetID(Data data, ulong oldID, string value)
    {
        ulong newID = ulong.Parse(value);
        if (data.FindObject(newID) != null)
        {
            throw new ArgumentException("Such id already exists");
        }
        data.FindObject(oldID)?.SetProperties("ID=" + newID.ToString());
    }
}
