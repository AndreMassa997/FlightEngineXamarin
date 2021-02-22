using System;
using Xamarin.Forms;

namespace FlightEngine.Views
{
    public class LoaderCell: ViewCell
    {
        private string title;
        private Label LabelTitle;
        public LoaderCell()
        {
            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Margin = new Thickness(20)
            };

            ActivityIndicator activityIndicator = new ActivityIndicator()
            {
                Color = (Color)Application.Current.Resources["primaryColor"]
            };

            stackLayout.Children.Add(activityIndicator);

            LabelTitle = new Label()
            {
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = "regularFont",
                FontSize = 20,
                TextColor = Color.Black
            };

            stackLayout.Children.Add(LabelTitle);
        }

        public void config(string text)
        {
            title = text;
            LabelTitle.Text = text;
        }
    }
}
