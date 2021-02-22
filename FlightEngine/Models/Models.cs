using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace FlightEngine.Models
{
    public class Models
    {
        public Models()
        {
        }
    }

    public class Flight
    {
        [JsonProperty("id")]
        public int id;
        [JsonProperty("airlineId")]
        public int airlineId;
        [JsonProperty("departureAirportId")]
        public int departureAirportId;
        [JsonProperty("arrivalAirportId")]
        public int arrivalAirportId;
        [JsonProperty("price")]
        public float price;

        public Airline airline;
        public Airport fromAirport;
        public Airport toAirport;

        public void setAirline(Airline airline)
        {
            this.airline = airline;
        }

        public void setFromAirport(Airport airport)
        {
            fromAirport = airport;
        }

        public void setToAirport(Airport airport)
        {
            toAirport = airport;
        }
    }

    public class Flights
    {
        [JsonProperty("data")]
        public List<Flight> data { get; set; }
    }

    public class Airports
    {
        [JsonProperty("data")]
        public List<Airport> data { get; set; }
    }

    public class Airport
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("codeIata")]
        public string CodeIata { get; set; }
        [JsonProperty("latitude")]
        public string Latitude { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        public string Locality;
        public string SubArea;

        public async Task setLocationAsync()
        {
            try
            {
                double latitude = double.Parse(Latitude, CultureInfo.InvariantCulture);
                double longitude = double.Parse(Longitude, CultureInfo.InvariantCulture);
                var locations = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                List<Placemark> placemarks = new List<Placemark>(locations);
                SubArea = placemarks[0].CountryName;
                if (Locality == null)
                {
                    Locality = placemarks[0].AdminArea;
                }
                if (placemarks[0].FeatureName != null)
                {
                    if (placemarks[0].FeatureName.ToLower().Contains("airport"))
                    {
                        Locality = placemarks[0].FeatureName;
                    }
                    else
                    {
                        Locality = placemarks[0].Locality;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }

    public class Airlines
    {
        [JsonProperty("data")]
        public List<Airline> data { get; set; }
    }

    public class Airline
    {
        [JsonProperty("id")]
        public int id;
        [JsonProperty("name")]
        public string name;
        [JsonProperty("codeIataPrefix")]
        public string codeIataPrefix;
        [JsonProperty("logoFilename")]
        public string logoFilename;
    }


    public class CustomFlight
    {
        public List<Flight> flights;
        public int stopOverNum;
    }
}
