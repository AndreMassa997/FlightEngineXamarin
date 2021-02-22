using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightEngine.Models;
using FlightEngine.Views;
using Xamarin.Forms;

namespace FlightEngine.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BaseCoordinator BaseCoordinator;
        public INavigation Navigation { get; set; }
        private Airports Airports;
        private Airlines Airlines;

        private int numPerson = 1;

        public event PropertyChangedEventHandler PropertyChanged;
        private Airport departureAirport;
        private Airport destinationAirport;

        public MainViewModel()
        {
            //Tap on add adult
            AddPersonCommand = new Command((key) =>
            {
                numPerson += 1;
                IsRemovePersonButtonEnabled = true;
                PeopleText = numPerson + " ADULTS";
            });

            //Tap on remove adult
            RemovePersonCommand = new Command((key) =>
            {
                numPerson -= 1;
                if (numPerson == 1)
                {
                    IsRemovePersonButtonEnabled = false;
                    PeopleText = numPerson + " ADULT";
                }
                PeopleText = numPerson + " ADULTS";
            });

            PeopleText = numPerson + " ADULT";

            DepartureCommand = new Command((key) =>
            {
                if (Airports == null)
                {
                    _listTitleString = "Unable to load airports";
                    ListTitleString = _listTitleString;
                }
                else
                {
                    _listTitleString = "Choose departure";
                    ListTitleString = _listTitleString;
                    _bottomListViewItems.Clear();
                    if (Airports != null)
                    {
                        //populate list with airports except the destination airport if present
                        Airports.data.ForEach(delegate (Airport airport) {
                            if (destinationAirport != null)
                            {
                                if (airport != destinationAirport)
                                {
                                    SingleAirportCell singleAirportCell = new SingleAirportCell();
                                    singleAirportCell.config(this, airport, true);
                                    _bottomListViewItems.Add(singleAirportCell);
                                }
                            }
                            else
                            {
                                SingleAirportCell singleAirportCell = new SingleAirportCell();
                                singleAirportCell.config(this, airport, true);
                                _bottomListViewItems.Add(singleAirportCell);
                            }
                        });
                    }
                }
            });

            DestinationCommand = new Command((key) =>
            {
                if (Airports == null)
                {
                    _listTitleString = "Unable to load airports";
                    ListTitleString = _listTitleString;
                }
                else
                {
                    _listTitleString = "Choose destination";
                    ListTitleString = _listTitleString;

                    _bottomListViewItems.Clear();
                    if (Airports != null)
                    {
                        //populate list with airports except the departure airport if present
                        Airports.data.ForEach(delegate (Airport airport) {
                            if (departureAirport != null)
                            {
                                if (airport != departureAirport)
                                {
                                    SingleAirportCell singleAirportCell = new SingleAirportCell();
                                    singleAirportCell.config(this, airport, false);
                                    _bottomListViewItems.Add(singleAirportCell);
                                }
                            }
                            else
                            {
                                SingleAirportCell singleAirportCell = new SingleAirportCell();
                                singleAirportCell.config(this, airport, false);
                                _bottomListViewItems.Add(singleAirportCell);
                            }
                        });
                    }
                }
            });

            SearchCommand = new Command((key) =>
            {
                GetFlights();
            });
        }

        public void start(BaseCoordinator baseCoordinator)
        {
            BaseCoordinator = baseCoordinator;
            InitializeAirportsAsync();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Get all airports from server
        public async void InitializeAirportsAsync()
        {
            _activityLoaderRunning = true;
            ActivityLoaderRunning = _activityLoaderRunning;
            _listTitleString = "Loading airports...";
            ListTitleString = _listTitleString;
            try
            {
                Airports = await BaseCoordinator.FlightEngineManager.GetAirports();
                if (Airports != null)
                {
                    foreach (Airport airport in Airports.data)
                    {
                        await airport.setLocationAsync();
                    }
                }
            }
            catch
            {

            }
            InitializeAirlinesAsync();
        }

        //Get all airlines from server
        public async void InitializeAirlinesAsync()
        {
            _listTitleString = "Loading airlines...";
            ListTitleString = _listTitleString;
            try
            {
                Airlines = await BaseCoordinator.FlightEngineManager.GetAirlines();
                _activityLoaderRunning = false;
                ActivityLoaderRunning = _activityLoaderRunning;
                _listTitleString = "Choose departure";
                ListTitleString = _listTitleString;
                DepartureCommand.Execute(null);
            }
            catch
            {

            }
        }

        //Get flight between two airports from server
        public async void GetFlights()
        {
            try
            {
                if (departureAirport != null && destinationAirport != null)
                {
                    _activityLoaderRunning = true;
                    ActivityLoaderRunning = _activityLoaderRunning;
                    Flights flights = await BaseCoordinator.FlightEngineManager.GetFlights(departureAirport.CodeIata, destinationAirport.CodeIata);
                    _activityLoaderRunning = false;
                    ActivityLoaderRunning = _activityLoaderRunning;
                    if (flights != null)
                    {
                        flights.data.ForEach(delegate (Flight flight)
                        {
                            if (Airlines != null)
                            {
                                Airline airline = Airlines.data.Find(x => x.id == flight.airlineId);
                                if (airline != null)
                                {
                                    flight.setAirline(airline);
                                }
                            }
                            Airport fromAirport = Airports.data.Find(x => x.Id == flight.departureAirportId);
                            {
                                if (fromAirport != null)
                                {
                                    flight.setFromAirport(fromAirport);
                                }
                            }
                            Airport arrivalAirport = Airports.data.Find(x => x.Id == flight.arrivalAirportId);
                            {
                                if (arrivalAirport != null)
                                {
                                    flight.setToAirport(arrivalAirport);
                                }
                            }
                        });
                    }
                    //Go to result page
                    BaseCoordinator.GoToResultsAsync(flights, numPerson, departureAirport, destinationAirport);
                }
            }
            catch
            {

            }
        }

        //tapped on item of list
        public void OnAirportTapped(Airport airport, bool isDeparture)
        {
            if (isDeparture)
            {
                departureAirport = airport;
                DepartureAirportString = airport.CodeIata;
                DestinationCommand.Execute(null);
            }
            else
            {
                destinationAirport = airport;
                DestinationAirportString = airport.CodeIata;
            }
        }

        //Commands for adults button and remove button visibility
        public ICommand AddPersonCommand { protected set; get; }
        public ICommand RemovePersonCommand { protected set; get; }
        public bool IsRemovePersonButtonEnabled
        {
            get
            {
                return numPerson > 1;
            }
            set
            {
                OnPropertyChanged(nameof(IsRemovePersonButtonEnabled));
            }
        }
        //Number of adults
        public string PeopleText
        {
            get
            {
                if (numPerson == 1)
                {
                    return "1 ADULT";
                }
                else
                {
                    return numPerson + " ADULTS";
                }
            }
            set
            {
                OnPropertyChanged(nameof(PeopleText));
            }
        }

        //Departure airport string
        public string DepartureAirportString
        {
            get
            {
                if (departureAirport == null)
                {
                    return "Choose departure";
                }
                else
                {
                    return departureAirport.CodeIata;
                }
            }
            set
            {
                OnPropertyChanged(nameof(DepartureAirportString));
            }
        }

        //destination airport string
        public string DestinationAirportString
        {
            get
            {
                if (destinationAirport == null)
                {
                    return "Choose destination";
                }
                else
                {
                    return destinationAirport.CodeIata;
                }
            }
            set
            {
                OnPropertyChanged(nameof(DestinationAirportString));
            }
        }

        //Title of white view
        private string _listTitleString = "";
        public string ListTitleString
        {
            get
            {
                return _listTitleString;
            }
            set
            {
                OnPropertyChanged(nameof(ListTitleString));
            }
        }

        //activity loader
        private bool _activityLoaderRunning = false;
        public bool ActivityLoaderRunning
        {
            get
            {
                return _activityLoaderRunning;
            }
            set
            {
                OnPropertyChanged(nameof(ActivityLoaderRunning));
            }
        }

        //Commands for buttons
        public ICommand DepartureCommand { protected set; get; }
        public ICommand DestinationCommand { protected set; get; }

        //list population
        ObservableCollection<SingleAirportCell> _bottomListViewItems = new ObservableCollection<SingleAirportCell>();
        public ObservableCollection<SingleAirportCell> BottomListViewItems
        {
            get { return _bottomListViewItems; }
            set
            {
                if (_bottomListViewItems != value)
                    _bottomListViewItems = value;
                OnPropertyChanged(nameof(BottomListViewItems));
            }
        }

        //Fly button command
        public ICommand SearchCommand { protected set; get; }
    }
}

