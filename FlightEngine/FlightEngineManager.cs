using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightEngine.API;
using FlightEngine.Models;

namespace FlightEngine
{
    public class FlightEngineManager
    {
        private FlightsAPI fligtsAPI = new FlightsAPI();

        public async Task<Airports> GetAirports()
        {
            return await fligtsAPI.GetAllAirports();
        }

        public async Task<Flights> GetFlights(string fromCodeIata, string toCodeIata)
        {
            return await fligtsAPI.GetFlights(fromCodeIata, toCodeIata);
        }

        public async Task<Airlines> GetAirlines()
        {
            return await fligtsAPI.GetAllAirlines();
        }
    }
}
