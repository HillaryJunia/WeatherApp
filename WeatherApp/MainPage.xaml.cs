using Microsoft.VisualBasic;
using WeatherApp.Sevices;
namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public List<Models.List> Weatherlist;
        private double latitude;
        private double longitude;
        private HorizontalStackLayout _previousTappedLayout;
        private Color _initialBackgroundColor = Colors.White; // Assume initial background color is white

        public MainPage()
        {
            InitializeComponent();
            Weatherlist = new List<Models.List>();

            ForecastDay1.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ShowForecastDetails(1))
            });
            ForecastDay2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ShowForecastDetails(2))
            });
            ForecastDay3.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ShowForecastDetails(3))
            });
            ForecastDay4.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ShowForecastDetails(4))
            });
            ForecastDay5.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ShowForecastDetails(5))
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetLocation();
            await GetWeatherDataByLocation(latitude, longitude);
        }

        public async Task GetLocation()
        {
            var location = await Geolocation.GetLocationAsync();
            latitude = location.Latitude;
            longitude = location.Longitude;
        }

        private async void TapLocation_Tapped(object sender, EventArgs e)
        {
            await GetLocation();
            await GetWeatherDataByLocation(latitude, longitude);
        }

        public async Task GetWeatherDataByLocation(double latitude, double longitude)
        {
            var result = await ApiSevice.GetWeather(latitude, longitude);
            UpdateUI(result);
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            var response = await DisplayPromptAsync(title: "", message: "", placeholder: "Search weather by city", accept: "Search", cancel: "Cancel");
            if (response != null)
            {
                await GetWeatherDataByCity(response);
            }
        }

        public async Task GetWeatherDataByCity(string city)
        {
            var result = await ApiSevice.GetWeatherByCIty(city);
            UpdateUI(result);
        }

        public void UpdateUI(dynamic result)
        {
            foreach (var item in result.list)
            {
                Weatherlist.Add(item);
            }

            LblCity.Text = result.city.name;
            LblWeatherDescription.Text = result.list[0].weather[0].description;
            LblTemperature.Text = result.list[0].main.temperature + "℃";
            LblHumidity.Text = result.list[0].main.humidity + "%";
            LblWind.Text = result.list[0].wind.speed + "Km/h";

            for (int i = 1; i <= 5; i++)
            {
                var forecast = result.list[i];
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(forecast.dt).DateTime;
                string dayOfWeek = date.ToString("dddd");

                switch (i)
                {
                    case 1:
                        ImgIcon1.Source = $"https://openweathermap.org/img/wn/{forecast.weather[0].icon}.png";
                        LblDate1.Text = date.ToString("dd/MM/yyyy");
                        LblDescription1.Text = dayOfWeek;
                        break;
                    case 2:
                        ImgIcon2.Source = $"https://openweathermap.org/img/wn/{forecast.weather[0].icon}.png";
                        LblDate2.Text = date.ToString("dd/MM/yyyy");
                        LblDescription2.Text = dayOfWeek;
                        break;
                    case 3:
                        ImgIcon3.Source = $"https://openweathermap.org/img/wn/{forecast.weather[0].icon}.png";
                        LblDate3.Text = date.ToString("dd/MM/yyyy");
                        LblDescription3.Text = dayOfWeek;
                        break;
                    case 4:
                        ImgIcon4.Source = $"https://openweathermap.org/img/wn/{forecast.weather[0].icon}.png";
                        LblDate4.Text = date.ToString("dd/MM/yyyy");
                        LblDescription4.Text = dayOfWeek;
                        break;
                    case 5:
                        ImgIcon5.Source = $"https://openweathermap.org/img/wn/{forecast.weather[0].icon}.png";
                        LblDate5.Text = date.ToString("dd/MM/yyyy");
                        LblDescription5.Text = dayOfWeek;
                        break;
                }
            }
        }

        private void ShowForecastDetails(int dayIndex)
        {
            var forecast = Weatherlist[dayIndex];
            LblTemperature.Text = forecast.main.temperature + "℃";
            LblHumidity.Text = forecast.main.humidity + "%";
            LblWind.Text = forecast.wind.speed + "Km/h";
        }

        private void OnHorizontalStackLayoutTapped(object sender, EventArgs e)
        {
            var currentLayout = sender as HorizontalStackLayout;

            if (currentLayout != null)
            {
                // If a layout was previously tapped, reset its background color
                if (_previousTappedLayout != null && _previousTappedLayout != currentLayout)
                {
                    _previousTappedLayout.BackgroundColor = _initialBackgroundColor; // Reset to initial background color
                }

                // Save the initial background color of the current layout if _previousTappedLayout is null
                if (_previousTappedLayout == null)
                {
                    _initialBackgroundColor = currentLayout.BackgroundColor;
                }

                // Change the background color of the current layout
                currentLayout.BackgroundColor = Color.FromHex("#80FFFFFF");

                // Update the reference to the current layout
                _previousTappedLayout = currentLayout;
            }
        }
    }
}
