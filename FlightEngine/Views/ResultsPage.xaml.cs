using System;
using System.Collections.Generic;
using FlightEngine.ViewModels;
using Xamarin.Forms;

namespace FlightEngine.Views
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage(ResultViewModel resultViewModel)
        {
            InitializeComponent();
            BindingContext = resultViewModel;
        }
    }
}
