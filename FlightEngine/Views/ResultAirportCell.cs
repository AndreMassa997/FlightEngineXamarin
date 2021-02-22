using System;
using FlightEngine.Models;
using Xamarin.Forms;

namespace FlightEngine.Views
{
    public class ResultAirportCell: ViewCell
    {
        Label airlineName, codeIATAName;
        Label priceSingle, pricePeople;

        StackLayout StackLayout;

        //inital layout
        public ResultAirportCell()
        {
            StackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Margin = new Thickness(20, 10, 10, 20),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            View = StackLayout;
        }

        public void config(CustomFlight customFlight, int numPeople)
        {
            setFlights(customFlight, numPeople);
        }

        //result cell flight
        private StackLayout getFlightStackLayout(Flight flight, int numPeople)
        {
            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
            };

            StackLayout airlineStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            airlineName = new Label()
            {
                FontFamily = "boldFont",
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };

            codeIATAName = new Label
            {
                FontFamily = "regularFont",
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };

            if (flight.airline != null)
            {
                airlineName.Text = flight.airline.name;
            }

            codeIATAName.Text = "";
            if (flight.fromAirport != null)
            {
                codeIATAName.Text += flight.fromAirport.CodeIata;
            }

            if (flight.toAirport != null)
            {
                codeIATAName.Text += " - " + flight.toAirport.CodeIata;
            }

            airlineStackLayout.Children.Add(airlineName);
            airlineStackLayout.Children.Add(codeIATAName);


            StackLayout priceInfos = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            pricePeople = new Label()
            {
                FontFamily = "boldFont",
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
            };

            priceSingle = new Label
            {
                FontFamily = "regularFont",
                TextColor = Color.Gray,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                FontSize = pricePeople.FontSize - 4
            };

            priceSingle.Text = string.Format("{0:N2}€ x{1}", flight.price, numPeople);
            pricePeople.Text = string.Format("{0:N2}€", flight.price * numPeople);

            priceInfos.Children.Add(priceSingle);
            priceInfos.Children.Add(pricePeople);

            stackLayout.Children.Add(airlineStackLayout);
            stackLayout.Children.Add(priceInfos);

            return stackLayout;
        }

        //result cell amount
        public StackLayout getTotalStackLayout(float total, int numStopOver)
        {
            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };

            StackLayout labelsStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };


            Label totalLabel = new Label()
            {
                FontFamily = "boldFont",
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Text = string.Format("{0:N2}€", total)
            };

            Label descriptionLabel = new Label()
            {
                FontFamily = "regularFont",
                TextColor = Color.Black,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Text = "Total with " + numStopOver + " stop-overs"
            };

            labelsStackLayout.Children.Add(descriptionLabel);
            labelsStackLayout.Children.Add(totalLabel);

            StackLayout frame = new StackLayout()
            {
                HeightRequest = 1,
                BackgroundColor = Color.Black,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
            };

            stackLayout.Children.Add(frame);
            stackLayout.Children.Add(labelsStackLayout);

            return stackLayout;
        }

        //result cell flights (or flight)
        private void setFlights(CustomFlight customFlight, int numPeople)
        {
            StackLayout.Children.Clear();
            if (customFlight.stopOverNum == 0 && customFlight.flights[0] != null)
            {
                StackLayout.Children.Add(getFlightStackLayout(customFlight.flights[0], numPeople));
                float total = customFlight.flights[0].price * numPeople;
                StackLayout.Children.Add(getTotalStackLayout(total, customFlight.stopOverNum));
            }
            else
            {
                float total = 0;
                customFlight.flights.ForEach(delegate (Flight flight){
                    total += flight.price * numPeople;
                    StackLayout.Children.Add(getFlightStackLayout(flight, numPeople));
                });
                StackLayout.Children.Add(getTotalStackLayout(total, customFlight.stopOverNum));
            }

            View = StackLayout;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                ResultAirportCell resultAirportCell = (ResultAirportCell)BindingContext;
                View = resultAirportCell.View;
            }
        }
    }
}
