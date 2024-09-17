using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1
{
    public static class CreateFlightsGUIData
    {
        public static FlightsGUIData CreateFlightsGuiData(List<Flight> flightsList, List<Airport> airportsList)
        {
            DateTime timeNow = DateTime.Now;
            List<FlightGUI> dataGUI = new List<FlightGUI>();
            foreach (var flight in flightsList) {

                if (timeNow >= flight.TakeoffTime && timeNow <= flight.LandingTime) {
                    dataGUI.Add(flight.CreateFlightGUI());
                }
            }
            return new FlightsGUIData(dataGUI);
        }
    }
}
