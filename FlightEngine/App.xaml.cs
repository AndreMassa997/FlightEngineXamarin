using System;
using FlightEngine.ViewModels;
using FlightEngine.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Raleway-Bold.ttf", Alias = "FontRegular")]
[assembly: ExportFont("Raleway-Regular.ttf", Alias = "FontBold")]

namespace FlightEngine
{
    public partial class App : Application
    {
        public static BaseCoordinator BaseCoordinator { get; private set; }
        static App()
        {
            BaseCoordinator = new BaseCoordinator();
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage(BaseCoordinator.MainViewModel))
            {
                BarBackgroundColor = (Color)Resources["primaryDarkColor"],
                BarTextColor = Color.White,
                Title = "LET'S FLY!",
            };          
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
