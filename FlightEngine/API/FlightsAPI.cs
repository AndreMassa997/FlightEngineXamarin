using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FlightEngine.Models;
using Newtonsoft.Json;

namespace FlightEngine.API
{
    public class FlightsAPI
    {
        HttpClient _client;
        private const string baseUrl = "https://recruitment.shippypro.com/flight-engine/api";
        private const string token = "1|MN9ruQV0MFEsgOzMo8crw8gB575rsTe2H5U1y2Lj";

        public FlightsAPI()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<Airports> GetAllAirports()
        {
            Airports airports = null;
            try
            {
                string uri = "/airports/all";
                HttpResponseMessage response = await _client.GetAsync(baseUrl + uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    airports = JsonConvert.DeserializeObject<Airports>(content);
                }
            }
            catch
            {

            }
            return airports;
        }

        public async Task<Flights> GetFlights(string fromCodeIata, string toCodeIata)
        {
            Flights flights = null;
            try
            {
                string uri = "/flights/from/" + fromCodeIata + "/to/" + toCodeIata;
                HttpResponseMessage response = await _client.GetAsync(baseUrl + uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    flights = JsonConvert.DeserializeObject<Flights>(content);
                }
            }
            catch
            {

            }
            return flights;
        }

        public async Task<Airlines> GetAllAirlines()
        {
            Airlines airlines = null;
            try
            {
                string uri = "/airlines/all";
                HttpResponseMessage response = await _client.GetAsync(baseUrl + uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    airlines = JsonConvert.DeserializeObject<Airlines>(content);
                }
            }
            catch
            {

            }
            return airlines;
        }
    }
}
