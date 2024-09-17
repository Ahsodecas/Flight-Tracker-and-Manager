using ood_project1.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class AddCommand : ICommand
{
    private Data data;
    private Filter filter;

    public string ObjectClass { get; set; }
    public string Conditions { get; set; }
    public List<string> Fields { get; set; }
    private Dictionary<string, Action<string>> objectsTypesMap;

    public AddCommand(string objectClass, string conditions, List<string> fields)
    {
        filter = new Filter();
        this.Conditions = conditions;
        this.ObjectClass = objectClass;
        this.Fields = fields;
        this.objectsTypesMap = new Dictionary<string, Action< string>>() { { "flight", AddFlight },
                                                                { "crew", AddCrew },
                                                                { "passenger", AddPassenger },
                                                                { "cargo", AddCargo },
                                                                { "cargoplane", AddCargoPlane },
                                                                { "passengerplane", AddPassengerPlane },
                                                                { "airport", AddAirport }};
    }

    public void ExecuteCommand(string objectType, string command, Data data)
    {
        this.data = data;
        string[] parts = command.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        int whereIndex = Array.IndexOf(parts, "new");
        if (whereIndex != -1)
        {
            Fields = new List<string>();
            for (int i = whereIndex + 1; i < parts.Length; i++)
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
    private void AddFlight(string command)
    {
        Flight obj = new Flight();
        foreach (var field in Fields)
        {
            string[] parts = field.Split(new char[] { '=' });
            if (parts[0] == "ID")
            {
                SetID(data, obj.ID, parts[1]);
            }
            obj.SetProperties(field);
        }
        data.AddObject(obj);
    }
    private void AddCrew(string command)
    {
        Crew obj = new Crew();
        foreach (var field in Fields)
        {
            string[] parts = field.Split(new char[] { '=' });
            if (parts[0] == "ID")
            {
                SetID(data, obj.ID, parts[1]);
            }
            obj.SetProperties(field);
        }
        data.AddObject(obj);
    }
    private void AddPassenger(string command)
    {
        Passenger obj = new Passenger();
        foreach (var field in Fields)
        {
            string[] parts = field.Split(new char[] { '=' });
            if (parts[0] == "ID")
            {
                SetID(data, obj.ID, parts[1]);
            }
            obj.SetProperties(field);
        }
        data.AddObject(obj);
    }
    private void AddCargo(string command)
    {
        Cargo obj = new Cargo();
        foreach (var field in Fields)
        {
            string[] parts = field.Split(new char[] { '=' });
            if (parts[0] == "ID")
            {
                SetID(data, obj.ID, parts[1]);
            }
            obj.SetProperties(field);
        }
        data.AddObject(obj);
    }
    private void AddCargoPlane(string command)
    {
        CargoPlane obj = new CargoPlane();
        foreach (var field in Fields)
        {
            string[] parts = field.Split(new char[] { '=' });
            if (parts[0] == "ID")
            {
                SetID(data, obj.ID, parts[1]);
            }
            obj.SetProperties(field);
        }
        data.AddObject(obj);
    }
    private void AddPassengerPlane(string command)
    {
        PassengerPlane obj = new PassengerPlane();
        foreach (var field in Fields)
        {
            string[] parts = field.Split(new char[] { '=' });
            if (parts[0] == "ID")
            {
                SetID(data, obj.ID, parts[1]);
            }
            obj.SetProperties(field);
        }
        data.AddObject(obj);
    }
    private void AddAirport(string command)
    {
        Airport obj = new Airport();
        foreach (var field in Fields)
        {
            string[] parts = field.Split(new char[] { '=' });
            if (parts[0] == "ID")
            {
                SetID(data, obj.ID, parts[1]);
            }
            obj.SetProperties(field);
        }
        data.AddObject(obj);
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
