using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FlightEngine.Models;
using FlightEngine.Views;

namespace FlightEngine.ViewModels
{
    public class ResultViewModel: INotifyPropertyChanged
    {
        private BaseCoordinator BaseCoordinator;
        private Flights Flights;

        public ResultViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void start(BaseCoordinator baseCoordinator, Airport fromAirport, Airport toAirport, Flights flights, int numPerson)
        {
            if (flights == null)    //no flight founded
            {
                _resultTextString = string.Format("No results founded for flights from {0} to {1}, for {2}" + (numPerson > 1 ? " adults" : " adult"), fromAirport.CodeIata, toAirport.CodeIata, numPerson);
                ResultTextString = _resultTextString;
            }
            else
            {
                _resultTextString = string.Format("Results for flights from {0} to {1}, for {2}" + (numPerson > 1 ? " adults" : " adult"), fromAirport.CodeIata, toAirport.CodeIata, numPerson);
                ResultTextString = _resultTextString;
                BaseCoordinator = baseCoordinator;
                Flights = flights;

                Flights tmpFlights = Flights;

                //search for not stop-over flights
                tmpFlights.data.ForEach(delegate (Flight flight)
                {
                    if (flight.departureAirportId == fromAirport.Id && flight.arrivalAirportId == toAirport.Id)
                    {
                        CustomFlight customFlight = new CustomFlight();
                        customFlight.flights = new List<Flight>()
                    {
                        flight
                    };
                        customFlight.flights.Add(flight);
                        customFlight.stopOverNum = 0;

                        ResultAirportCell resultAirportCell = new ResultAirportCell();
                        resultAirportCell.config(customFlight, numPerson);
                        _listViewItems.Add(resultAirportCell);
                    }
                });

                //search other flights with stop-overs
                List<CustomFlight> itineraries = new List<CustomFlight>();
                List<Flight> departureFlights = tmpFlights.data.FindAll(x => x.departureAirportId == fromAirport.Id && x.arrivalAirportId != toAirport.Id);
                List<Flight> middleFlights = tmpFlights.data.FindAll(x => x.departureAirportId != fromAirport.Id && x.arrivalAirportId != toAirport.Id);
                List<Flight> finalFlights = tmpFlights.data.FindAll(x => x.arrivalAirportId == toAirport.Id && x.departureAirportId != fromAirport.Id);

                foreach (Flight flight in departureFlights)
                {
                    CustomFlight itinerary = new CustomFlight();
                    itinerary.flights = new List<Flight>()
                {
                    flight
                };
                    itineraries.Add(itinerary);
                }

                foreach (CustomFlight itinerary in itineraries)
                {
                    foreach (Flight flight in middleFlights)
                    {
                        if (itinerary.flights[itinerary.flights.Count - 1].arrivalAirportId == flight.departureAirportId)
                        {
                            itinerary.flights.Add(flight);
                        }
                    }
                    foreach (Flight flight in finalFlights)
                    {
                        if (itinerary.flights[itinerary.flights.Count - 1].arrivalAirportId == flight.departureAirportId)
                        {
                            itinerary.flights.Add(flight);
                        }
                    }
                }

                foreach (CustomFlight itinerary in itineraries)
                {
                    ResultAirportCell resultAirportCell = new ResultAirportCell();
                    itinerary.stopOverNum = itinerary.flights.Count - 1;
                    resultAirportCell.config(itinerary, numPerson);
                    _listViewItems.Add(resultAirportCell);
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        //List items collection
        ObservableCollection<ResultAirportCell> _listViewItems = new ObservableCollection<ResultAirportCell>();
        public ObservableCollection<ResultAirportCell> ListViewItems
        {
            get { return _listViewItems; }
            set
            {
                if (_listViewItems != value)
                    _listViewItems = value;
                OnPropertyChanged(nameof(ListViewItems));
            }
        }

        //Title of page
        public string ResultTextString
        {
            set
            {
                OnPropertyChanged(nameof(ResultTextString));
            }

            get
            {
                return _resultTextString;
            }
        }
        private string _resultTextString = "";
    }
}
