using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1.Commands;

public class Filter
{
    private Dictionary<string, Func<double, double, bool>> operatorsMap;
    public Filter()
    {
        operatorsMap = new Dictionary<string, Func<double, double, bool>>() {
                                                                { "<=", (first, second) => { return first <= second; } },
                                                                { ">=", (first, second) => { return first >= second; } },
                                                                { "=", (first, second) => { return first == second; } },
                                                                { "!=", (first, second) => { return first != second; } },
                                                                { "<", (first, second) => { return first < second; } },
                                                                { ">", (first, second) => { return first > second; } }};
    }
    public List<Airport> FilterAirport(string conditions, Data data)
    {
        List<Airport> objects = data.SelectAirports();
        if (conditions == "")
        {
            return objects;
        }
        List<Airport> filteredObjects = new List<Airport>();
        string[] orConditions = conditions.Split("or", StringSplitOptions.RemoveEmptyEntries);
        foreach (string orCondition in orConditions)
        {
            string[] andConditions = conditions.Split("and", StringSplitOptions.RemoveEmptyEntries);
            List<Airport> tempFiltered = objects;
            foreach (string andCondition in andConditions)
            {
                List<Airport> copyTempFiltered = new List<Airport>();
                (string property, string oper, double value) parsedCondition = ParseCondition(andCondition);
                foreach (var flight in tempFiltered)
                {
                    double propertyValue;
                    if (double.TryParse(flight.GetProperty(parsedCondition.property), out propertyValue))
                    {
                        if (operatorsMap[parsedCondition.oper].Invoke(propertyValue, parsedCondition.value))
                        {
                            copyTempFiltered.Add(flight);
                        }
                    }
                }
                tempFiltered = copyTempFiltered;
            }
            filteredObjects.AddRange(tempFiltered);
        }
        return filteredObjects;
    }
    public List<Flight> FilterFlight(string conditions, Data data)
    {
        List<Flight> objects = data.SelectFlights();
        List<Flight> filteredObjects = new List<Flight>();
        if (conditions == "")
        {
            return objects;
        }
        string[] orConditions = conditions.Split("or", StringSplitOptions.RemoveEmptyEntries);
        foreach (string orCondition in orConditions)
        {
            string[] andConditions = conditions.Split("and", StringSplitOptions.RemoveEmptyEntries);
            List<Flight> tempFiltered = objects;
            foreach (string andCondition in andConditions)
            {
                List<Flight> copyTempFiltered = new List<Flight>();
                (string property, string oper, double value) parsedCondition = ParseCondition(andCondition);
                foreach(var flight in tempFiltered) 
                {
                    double propertyValue;
                    if (double.TryParse(flight.GetProperty(parsedCondition.property), out propertyValue))
                    {
                        if (operatorsMap[parsedCondition.oper].Invoke(propertyValue, parsedCondition.value))
                        {
                            copyTempFiltered.Add(flight);
                        }
                    }
                }
                tempFiltered = copyTempFiltered;
            }
            filteredObjects.AddRange(tempFiltered);
        }
        return filteredObjects;
    }
    public List<Crew> FilterCrew(string conditions, Data data)
    {
        List<Crew> objects = data.SelectCrew();
        if (conditions == "")
        {
            return objects;
        }
        List<Crew> filteredObjects = new List<Crew>();
        string[] orConditions = conditions.Split("or", StringSplitOptions.RemoveEmptyEntries);
        foreach (string orCondition in orConditions)
        {
            string[] andConditions = conditions.Split("and", StringSplitOptions.RemoveEmptyEntries);
            List<Crew> tempFiltered = objects;
            foreach (string andCondition in andConditions)
            {
                List<Crew> copyTempFiltered = new List<Crew>();
                (string property, string oper, double value) parsedCondition = ParseCondition(andCondition);
                foreach (var flight in tempFiltered)
                {
                    double propertyValue;
                    if (double.TryParse(flight.GetProperty(parsedCondition.property), out propertyValue))
                    {
                        if (operatorsMap[parsedCondition.oper].Invoke(propertyValue, parsedCondition.value))
                        {
                            copyTempFiltered.Add(flight);
                        }
                    }
                }
                tempFiltered = copyTempFiltered;
            }
            filteredObjects.AddRange(tempFiltered);
        }
        return filteredObjects;
    }
    public List<Passenger> FilterPassenger(string conditions, Data data)
    {
        List<Passenger> objects = data.SelectPassenger();
        if (conditions == "")
        {
            return objects;
        }
        List<Passenger> filteredObjects = new List<Passenger>();
        string[] orConditions = conditions.Split("or", StringSplitOptions.RemoveEmptyEntries);
        foreach (string orCondition in orConditions)
        {
            string[] andConditions = conditions.Split("and", StringSplitOptions.RemoveEmptyEntries);
            List<Passenger> tempFiltered = objects;
            foreach (string andCondition in andConditions)
            {
                List<Passenger> copyTempFiltered = new List<Passenger>();
                (string property, string oper, double value) parsedCondition = ParseCondition(andCondition);
                foreach (var flight in tempFiltered)
                {
                    double propertyValue;
                    if (double.TryParse(flight.GetProperty(parsedCondition.property), out propertyValue))
                    {
                        if (operatorsMap[parsedCondition.oper].Invoke(propertyValue, parsedCondition.value))
                        {
                            copyTempFiltered.Add(flight);
                        }
                    }
                }
                tempFiltered = copyTempFiltered;
            }
            filteredObjects.AddRange(tempFiltered);
        }
        return filteredObjects;
    }
    public List<Cargo> FilterCargo(string conditions, Data data)
    {
        List<Cargo> objects = data.SelectCargo();
        if (conditions == "")
        {
            return objects;
        }
        List<Cargo> filteredObjects = new List<Cargo>();
        string[] orConditions = conditions.Split("or", StringSplitOptions.RemoveEmptyEntries);
        foreach (string orCondition in orConditions)
        {
            string[] andConditions = conditions.Split("and", StringSplitOptions.RemoveEmptyEntries);
            List<Cargo> tempFiltered = objects;
            foreach (string andCondition in andConditions)
            {
                List<Cargo> copyTempFiltered = new List<Cargo>();
                (string property, string oper, double value) parsedCondition = ParseCondition(andCondition);
                foreach (var flight in tempFiltered)
                {
                    double propertyValue;
                    if (double.TryParse(flight.GetProperty(parsedCondition.property), out propertyValue))
                    {
                        if (operatorsMap[parsedCondition.oper].Invoke(propertyValue, parsedCondition.value))
                        {
                            copyTempFiltered.Add(flight);
                        }
                    }
                }
                tempFiltered = copyTempFiltered;
            }
            filteredObjects.AddRange(tempFiltered);
        }
        return filteredObjects;
    }
    public List<CargoPlane> FilterCargoPlane(string conditions, Data data)
    {
        List<CargoPlane> objects = data.SelectCargoPlane();
        if (conditions == "")
        {
            return objects;
        }
        List<CargoPlane> filteredObjects = new List<CargoPlane>();
        string[] orConditions = conditions.Split("or", StringSplitOptions.RemoveEmptyEntries);
        foreach (string orCondition in orConditions)
        {
            string[] andConditions = conditions.Split("and", StringSplitOptions.RemoveEmptyEntries);
            List<CargoPlane> tempFiltered = objects;
            foreach (string andCondition in andConditions)
            {
                List<CargoPlane> copyTempFiltered = new List<CargoPlane>();
                (string property, string oper, double value) parsedCondition = ParseCondition(andCondition);
                foreach (var flight in tempFiltered)
                {
                    double propertyValue;
                    if (double.TryParse(flight.GetProperty(parsedCondition.property), out propertyValue))
                    {
                        if (operatorsMap[parsedCondition.oper].Invoke(propertyValue, parsedCondition.value))
                        {
                            copyTempFiltered.Add(flight);
                        }
                    }
                }
                tempFiltered = copyTempFiltered;
            }
            filteredObjects.AddRange(tempFiltered);
        }
        return filteredObjects;
    }
    public List<PassengerPlane> FilterPassengerPlane(string conditions, Data data)
    {
        List<PassengerPlane> objects = data.SelectPassengerPlane();
        if (conditions == "")
        {
            return objects;
        }
        List<PassengerPlane> filteredObjects = new List<PassengerPlane>();
        string[] orConditions = conditions.Split("or", StringSplitOptions.RemoveEmptyEntries);
        foreach (string orCondition in orConditions)
        {
            string[] andConditions = conditions.Split("and", StringSplitOptions.RemoveEmptyEntries);
            List<PassengerPlane> tempFiltered = objects;
            foreach (string andCondition in andConditions)
            {
                List<PassengerPlane> copyTempFiltered = new List<PassengerPlane>();
                (string property, string oper, double value) parsedCondition = ParseCondition(andCondition);
                foreach (var flight in tempFiltered)
                {
                    double propertyValue;
                    if (double.TryParse(flight.GetProperty(parsedCondition.property), out propertyValue))
                    {
                        if (operatorsMap[parsedCondition.oper].Invoke(propertyValue, parsedCondition.value))
                        {
                            copyTempFiltered.Add(flight);
                        }
                    }
                }
                tempFiltered = copyTempFiltered;
            }
            filteredObjects.AddRange(tempFiltered);
        }
        return filteredObjects;
    }
    public (string field, string oper, double value) ParseCondition(string condition)
    {
        string[] parts = condition.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        double Value;
        string[] operators = { "<=", ">=", "<", ">", "=", "!=" };
        if (parts.Length != 3 || !operators.Contains(parts[1].Trim()) || !double.TryParse(parts[2].Trim(), out Value)) 
        {
            throw new ArgumentException("Incorrect condition " + condition);
        }
        return (parts[0].Trim(), parts[1].Trim(), Value);
    }

}
