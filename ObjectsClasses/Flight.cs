using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Runtime;
using System.Threading.Tasks;
using System.Numerics;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System.Reactive.Joins;
using System.Globalization;
using System.ComponentModel;
using Mapsui.Projections;
using System.ComponentModel.DataAnnotations;
using NetworkSourceSimulator;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.Text.Json.Serialization;
using Avalonia.Dialogs;

namespace ood_project1
{
    public class Flight : Object, IObserver
    {
        public Airport Origin { get; private set; }
        public Airport Target { get; private set; }
        public DateTime TakeoffTime { get; private set; }
        public DateTime LandingTime { get; private set; }
        public Worldposition WorldPosition { get; private set; }
        [JsonIgnore]
        public Worldposition? NewWorldPosition { get; private set; }
        [JsonIgnore]
        public bool CoordinatesChanged = false;
        [JsonIgnore]
        public float? NewAMSL { get; private set; }
        public float AMSL { get; private set; }
        public Plane PlaneID { get; private set; }
        public List<Crew> CrewID { get; private set; }
        public List<ILoad> LoadID { get; private set; }
        [JsonIgnore]
        public double? SpeedX { get; private set; }
        [JsonIgnore]
        public double? SpeedY { get; private set; }
        [JsonIgnore]
        public readonly Dictionary<string, Func<Flight, string, string>> PropertyValues = new Dictionary<string, Func<Flight, string,  string>>() {
            {"ID", (flight, field) => { return flight.ID.ToString(); }  },
            {"PlaneID", (flight, field) => { return (flight.PlaneID == null) ? "" : flight.PlaneID.GetProperty(field); } },
            {"Origin", (flight, field) => { return (flight.Origin == null) ? "" : flight.Origin.GetProperty(field); } },
            {"Target", (flight, field) => { return (flight.Target == null) ? "" :flight.Target.GetProperty(field); } },
            {"TakeoffTime", (flight, field) => { return flight.TakeoffTime.ToString(); } },
            {"LandingTime", (flight, field) => { return flight.LandingTime.ToString(); } },
            {"WorldPosition", (flight, field) => { return flight.WorldPosition.GetProperty(field); } },
            {"AMSL", (flight, field) => { return (flight.AMSL == null) ? "" : flight.AMSL.ToString(); } },
            };
        [JsonIgnore]
        public readonly Dictionary<string, Action<Flight, string, string>> PropertyValuesSet = new Dictionary<string, Action<Flight, string, string>>() {
            {"ID", (obj, value, field) => { obj.SetObjectID(ulong.Parse(value)); }  },
            {"PlaneID", (obj, value, field) => {   obj.PlaneID.SetProperties(field); } },
            {"Origin", (obj, value, field) => {   obj.Origin = new Airport();  obj.Origin.SetProperties(field); } },
            {"Target", (obj, value, field) => {  obj.Target = new Airport(); obj.Target.SetProperties(field); } },
            {"TakeoffTime", (obj, value, field) => { obj.TakeoffTime = DateTime.Parse(value); } },
            {"LandingTime", (obj, value, field) => { obj.LandingTime = DateTime.Parse(value); } },
            {"WorldPosition", (obj, value, field) => { obj.CoordinatesChanged = false;  obj.NewWorldPosition = new Worldposition(0, 0); obj.NewWorldPosition.SetProperties(field); } },
            {"AMSL", (obj, value, field) => { obj.AMSL = float.Parse(value); } },
            };
        public Flight() 
        {
            Origin = null;
            Target = null;
            TakeoffTime = DateTime.Today;
            LandingTime = DateTime.Today;
            WorldPosition = new Worldposition(0, 0);
            NewWorldPosition = null;
            NewAMSL = null;
            AMSL = 0;
            PlaneID = null;
            CrewID = null;
            LoadID = null;
        }
        public Flight(string type, ulong id, Airport origin, Airport target, DateTime takeoffTime, DateTime landingTime, float longtitude, float latitude, float aMSL, Plane planeID, List<Crew> crewID, List<ILoad> loadID) : base(type, id)
        {
            Origin = origin;
            Target = target;
            TakeoffTime = takeoffTime;
            LandingTime = landingTime;
            WorldPosition = new Worldposition (longtitude, latitude);
            NewWorldPosition = null;
            NewAMSL = null;
            AMSL = aMSL;
            PlaneID = planeID;
            CrewID = crewID;
            LoadID = loadID;
        }
        public override string JSONSerializeObject()
        {
            return JsonSerializer.Serialize(this);
        }
        public override void CreateObjectFromString(Data data, string ObjectType, UInt64 ID, string[]? Parameters)
        {
            base.CreateObjectFromString(data, ObjectType, ID);
            Object? foundObject = data.FindObject(UInt64.Parse(Parameters[2]));
            if (foundObject != null && foundObject is Airport originAirport)
                this.Origin = originAirport;
            else
                this.Origin = null;
            foundObject = data.FindObject(UInt64.Parse(Parameters[3]));
            if (foundObject != null && foundObject is Airport targetAirport)
                this.Target = targetAirport;
            else
                this.Target = null;
            this.TakeoffTime = DateTime.ParseExact(Parameters[4], "HH:mm",
                           System.Globalization.CultureInfo.InvariantCulture);
            this.LandingTime = DateTime.ParseExact(Parameters[5], "HH:mm",
                           System.Globalization.CultureInfo.InvariantCulture);
            if (this.LandingTime <= this.TakeoffTime) { this.LandingTime = this.LandingTime.AddDays(1); }
            WorldPosition = new Worldposition(Single.Parse(Parameters[6]), Single.Parse(Parameters[7]));
            this.AMSL = Single.Parse(Parameters[8]); ;
            foundObject = data.FindObject(UInt64.Parse(Parameters[9]));
            if (foundObject != null && foundObject is Plane plane)
                this.PlaneID = plane;
            else
                this.PlaneID = null;
            char[] separators = new char[] { '[', ';', ']' };
            string[] CrewIdStrings = Parameters[10].Split(separators, StringSplitOptions.RemoveEmptyEntries);
            this.CrewID = new List<Crew>(CrewIdStrings.Select((id) => { if (data.FindObject(UInt64.Parse(id)) is Crew crew) return crew; else { return null; } }).ToArray());
            string[] LoadIdStrings = Parameters[11].Split(separators, StringSplitOptions.RemoveEmptyEntries);
            this.LoadID = new List<ILoad>(LoadIdStrings.Select((id) => { if (data.FindObject(UInt64.Parse(id)) is ILoad load) return load; else { return null; } }).ToArray());
        }
        public override void CreateObjectFromBytes(Data readData, NetworkSourceSimulator.Message data)
        {
            base.CreateObjectFromBytes(readData, data);
            Object? foundObject = readData.FindObject(BitConverter.ToUInt64(data.MessageBytes, 15));
            if (foundObject != null && foundObject is Airport originAirport)
                this.Origin = originAirport;
            else
                throw new Exception("No airport found with given id");
            foundObject = readData.FindObject(BitConverter.ToUInt64(data.MessageBytes, 23));
            if (foundObject != null && foundObject is Airport targetAirport)
                this.Target = targetAirport;
            else
                throw new Exception("No airport found with given id");
            Int64 TakeOffTimeMiliseconds = BitConverter.ToInt64(data.MessageBytes, 31);
            DateTimeOffset TakeOffTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(TakeOffTimeMiliseconds);
            this.TakeoffTime = TakeOffTimeOffset.DateTime;
            Int64 LandingOffTimeMiliseconds = BitConverter.ToInt64(data.MessageBytes, 39);
            DateTimeOffset LandingOffTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(LandingOffTimeMiliseconds);
            this.LandingTime = LandingOffTimeOffset.DateTime;
            if (this.LandingTime <= this.TakeoffTime) { this.LandingTime = this.LandingTime.AddDays(1); }
            foundObject = readData.FindObject(BitConverter.ToUInt64(data.MessageBytes, 47));
            if (foundObject != null && foundObject is Plane plane)
                this.PlaneID = plane;
            else
                throw new Exception("No plane found with given id");
            UInt16 crewCount = BitConverter.ToUInt16(data.MessageBytes, 55);
            UInt16 LoadCount = BitConverter.ToUInt16(data.MessageBytes, 57 + 8 * crewCount);
            this.CrewID = new List<Crew>();
            this.LoadID = new List<ILoad>();
            for (int i = 0; i < crewCount; i++)
            {
                if (readData.FindObject(BitConverter.ToUInt64(data.MessageBytes, 57 + 8 * i)) is Crew crew)
                {
                    this.CrewID.Add(crew);
                }
                else
                {
                    throw new Exception("No crew found with given id");
                }
            }
            for (int i = 0; i < LoadCount; i++)
            {
                if (readData.FindObject(BitConverter.ToUInt64(data.MessageBytes, 59 + 8 * crewCount + 8 * i)) is ILoad load)
                {
                    this.LoadID.Add(load);
                }
                else
                {
                    throw new Exception("No crew found with given id");
                }
            }
        }
        public FlightGUI CreateFlightGUI()
        {
            DateTime timeNow = DateTime.Now;
            double flightTimePassedSeconds = (timeNow.Subtract(this.TakeoffTime)).TotalSeconds;

            double thisLongtitude = this.Origin?.Longtitude ?? 0;
            double thisLatitude = this.Origin?.Latitude ?? 0;

            double targetLongtitude = this.Target?.Longtitude ?? 0;
            double targetLatitude = this.Target?.Latitude ?? 0;

            if (NewWorldPosition != null && CoordinatesChanged == false)
            {
                CalculateNewInitialPosition(targetLongtitude, targetLatitude, flightTimePassedSeconds);
                thisLongtitude = NewWorldPosition?.Longtitude ?? 0.0;
                thisLatitude = NewWorldPosition?.Latitude ?? 0.0;
            }
            if (CoordinatesChanged == true)
            {
                thisLongtitude = NewWorldPosition?.Longtitude ?? 0.0;
                thisLatitude = NewWorldPosition?.Latitude ?? 0.0;
            }

            if (this.SpeedX == null || this.SpeedY == null)
            {
                double flightDurationSeconds = (this.LandingTime.Subtract(this.TakeoffTime)).TotalSeconds;
                this.SpeedX = (targetLongtitude - thisLongtitude) / flightDurationSeconds;
                this.SpeedY = (targetLatitude - thisLatitude) / flightDurationSeconds;
            }

            double speedX = SpeedX ?? 0.0;
            double speedY = SpeedY ?? 0.0;

            double newLatitude = thisLatitude + speedY * flightTimePassedSeconds;
            double newLongtitude = thisLongtitude + speedX * flightTimePassedSeconds;

            double MapCoordRotation = Math.Atan2(targetLongtitude - thisLongtitude, targetLatitude - thisLatitude);

            return new FlightGUI() { ID = this.ID, WorldPosition = new WorldPosition(newLatitude, newLongtitude), MapCoordRotation = MapCoordRotation };
        }
        public new void UpdateID(IDUpdateArgs args, Log log)
        {
            base.UpdateID(args, log);
        }
        public new void UpdatePosition(PositionUpdateArgs args, Log log)
        {
            if (this.ID == args.ObjectID || this.PlaneID?.ID == args.ObjectID)
            {
                NewAMSL = args.AMSL;
                NewWorldPosition = new Worldposition(args.Latitude, args.Longitude);
                SpeedX = null;
                SpeedY = null;
                log.AddPositionLogging(args);
            }
        }
        private void CalculateNewInitialPosition(double targetLongtitude, double targetLatitude, double flightTimePassedSeconds)
        {
            double newInitialLongtitude = ((NewWorldPosition?.Longtitude ?? 0.0) - flightTimePassedSeconds * targetLongtitude) / (this.LandingTime.Subtract(this.TakeoffTime).TotalSeconds - flightTimePassedSeconds);
            NewWorldPosition.Longtitude = (float)newInitialLongtitude;
            double newInitialLatitude = ((NewWorldPosition?.Latitude ?? 0.0) - flightTimePassedSeconds * targetLatitude) / (this.LandingTime.Subtract(this.TakeoffTime).TotalSeconds - flightTimePassedSeconds);
            NewWorldPosition.Latitude = (float)newInitialLatitude;
            CoordinatesChanged = true;
        }
   
        public override string GetProperty(string field)
        {
            string[] parts = field.Split(".");
            if (PropertyValues.ContainsKey(parts[0]))
            {
                return PropertyValues[parts[0]].Invoke(this, field);
            }
            else
            {
                throw new Exception("Unknown property");
            }
        }
        public override void SetProperties(string field)
        {
            string[] parts = field.Split(new char[] { '=' });
            string[] fields = parts[0].Split(new char[] { '.' });
            if (PropertyValues.ContainsKey(fields[0]))
            {
                PropertyValuesSet[fields[0]].Invoke(this, parts[1], field);
            }
            else
            {
                throw new Exception("Unknown property");
            }
        }
    }
}
