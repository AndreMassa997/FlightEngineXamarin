# FlightEngineXamarin
A Xamarin app that display the most convenient flights

The app is written using Visual Studio for Mac, iOS 11 and android 10 as deployment target and MVVM + Coordinator pattern.
UI is created with both XAML and C# code.

This app uses [FlightEngineAPI]

## APP Flow
Flight Engine app starts with a simple page that shows a list of airports. By scrolling on that list the user can be choose departure and destination. 
Users can also choose the number of people that are going to book.
After the selection of the two airports, users can see results with (or without) stop-overs.

## External Libraries
The app use only Newtonsoft.Json from NuGet to parse json files.

