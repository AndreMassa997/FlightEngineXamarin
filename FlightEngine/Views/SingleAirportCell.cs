using System;
using System.Collections.Generic;
using FlightEngine.Models;
using FlightEngine.ViewModels;
using Xamarin.Forms;

namespace FlightEngine.Views
{
    public class SingleAirportCell: ViewCell
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create("ImageString", typeof(string), typeof(SingleAirportCell), "");

        private string ImageString = "";
        private string Title = "";
        private string Subtitle = "";

        Label titleLabel, subTitleLabel;
        Image image;

        public Airport Airport;
        public bool isDeparture;

        private MainViewModel MainViewModel;

        //airport cell layout
        public SingleAirportCell()
        {
            StackLayout StackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(20, 10, 10, 20)
            };

            image = new Image
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 30,
                HeightRequest = 30,
                Margin = new Thickness(0, 0, 20, 0)
            };

            titleLabel = new Label()
            {
                FontFamily = "boldFont",
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };

            subTitleLabel = new Label
            {
                FontFamily = "regularFont",
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };

            StackLayout.Children.Add(image);

            StackLayout verticalStack = new StackLayout();
            verticalStack.Orientation = StackOrientation.Vertical;
            verticalStack.HorizontalOptions = LayoutOptions.StartAndExpand;
            verticalStack.VerticalOptions = LayoutOptions.Center;
            verticalStack.Children.Add(titleLabel);
            verticalStack.Children.Add(subTitleLabel);

            StackLayout.Children.Add(verticalStack);
            View = StackLayout;
        }

        //airport cell initial inital configuration
        public void config(MainViewModel mainViewModel, Airport airport, bool isDeparture)
        {
            MainViewModel = mainViewModel;
            Airport = airport;
            this.isDeparture = isDeparture;

            ImageString = isDeparture ? "flight_takeoff_black" : "flight_land_black";
            Title = airport.CodeIata;
            Subtitle = airport.Locality;
            if (airport.SubArea != null)
            {
               if (Subtitle == null)
               {
                   Subtitle = airport.SubArea;
               }
               else
               {
                  Subtitle += " - " + airport.SubArea;
               }
            }

        }

        protected override void OnTapped()
        {
            base.OnTapped();
            MainViewModel.OnAirportTapped(Airport, isDeparture);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                SingleAirportCell singleAirportCell = (SingleAirportCell)BindingContext;
                titleLabel.Text = singleAirportCell.Title;
                subTitleLabel.Text = singleAirportCell.Subtitle;
                image.Source = singleAirportCell.ImageString;
                MainViewModel = singleAirportCell.MainViewModel;
                Airport = singleAirportCell.Airport;
                isDeparture = singleAirportCell.isDeparture;

            }
        }
    }


}
