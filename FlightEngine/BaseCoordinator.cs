using System.ComponentModel;
using System.Runtime.CompilerServices;
using FlightEngine.Models;
using FlightEngine.ViewModels;
using FlightEngine.Views;
using Xamarin.Forms;

namespace FlightEngine
{
    public class BaseCoordinator
    {
        public FlightEngineManager FlightEngineManager = new FlightEngineManager();
        public MainViewModel MainViewModel;
        public ResultViewModel ResultViewModel;

        public BaseCoordinator()
        {
            MainViewModel = new MainViewModel();
            MainViewModel.start(baseCoordinator: this);
        }

        public async void GoToResultsAsync(Flights flights, int numPerson, Airport fromAirport, Airport toAirport)
        {
            ResultViewModel = new ResultViewModel();
            ResultViewModel.start(this, fromAirport, toAirport, flights, numPerson);
            await Application.Current.MainPage.Navigation.PushAsync(new ResultsPage(resultViewModel: ResultViewModel));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }
    }
}

