using FlightEngine.ViewModels;
using Xamarin.Forms;

namespace FlightEngine.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
        }
    }
}
