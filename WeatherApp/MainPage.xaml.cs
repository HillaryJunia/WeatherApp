using Microsoft.VisualBasic;
using WeatherApp.Sevices;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        public List<Models.List> Weatherlist;
        private double latitude;
        private double longitude;
        private HorizontalStackLayout _previousTappedLayout;
        private Color _initialBackgroundColor = Colors.White;

        public MainPage()
        {
            InitializeComponent();
            Weatherlist = new List<Models.List>();
        }

        private void AddTapGestureRecognizer(HorizontalStackLayout layout, int dayIndex)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => OnHorizontalStackLayoutTapped(s, e, dayIndex);
            layout.GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetLocation();
            await GetWeatherDataByLocation(latitude, longitude);
            UpdateDateAndTime();
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

        //private async void ImageButton_Clicked(object sender, EventArgs e)
        //{
        //    var response = await DisplayPromptAsync(title: "", message: "", placeholder: "Tìm kiếm thời tiết theo thành phố", accept: "Tìm kiếm", cancel: "Hủy");
        //    if (response != null)
        //    {
        //        await GetWeatherDataByCity(response);
        //    }
        //}
        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = (SearchBar)sender;
            string searchText = searchBar.Text;

            if (!string.IsNullOrEmpty(searchText))
            {
                await GetWeatherDataByCity(searchText);
                searchBar.Text = "";
            }
        }


        public async Task GetWeatherDataByCity(string city)
        {
            var result = await ApiSevice.GetWeatherByCIty(city);
            UpdateUI(result);
        }

        public void UpdateUI(dynamic result)
        {
            Weatherlist.Clear();

            foreach (var item in result.list)
            {
                Weatherlist.Add(item);
            }
            // CvWeather.ItemsSource = Weatherlist;

            LblCity.Text = result.city.name;
            ShowForecastDetails(1);
            var today = DateTime.Now.Date;
            LblDate1.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            LblDescription1.Text = today.ToString("dddd");

            int dayCounter = 1;
            DateTime? lastDate = null;

            foreach (var forecast in result.list)
            {
                DateTime forecastDate = DateTime.Parse(forecast.dt_txt).Date;

                // Check if this forecast is for the next day
                if (lastDate.HasValue && forecastDate > lastDate.Value)
                {
                    string dayOfWeek = forecastDate.ToString("dddd");

                    switch (dayCounter)
                    {
                        case 1:
                            LblDate2.Text = forecastDate.ToString("dd/MM/yyyy");
                            LblDescription2.Text = dayOfWeek;
                            break;
                        case 2:
                            LblDate3.Text = forecastDate.ToString("dd/MM/yyyy");
                            LblDescription3.Text = dayOfWeek;
                            break;
                        case 3:
                            LblDate4.Text = forecastDate.ToString("dd/MM/yyyy");
                            LblDescription4.Text = dayOfWeek;
                            break;
                        case 4:
                            LblDate5.Text = forecastDate.ToString("dd/MM/yyyy");
                            LblDescription5.Text = dayOfWeek;
                            break;
                        case 5:
                            LblDate6.Text = forecastDate.ToString("dd/MM/yyyy");
                            LblDescription6.Text = dayOfWeek;
                            break;
                    }

                    dayCounter++;
                    if (dayCounter > 5) break;
                }

                lastDate = forecastDate;
            }
        }


        private void ShowForecastDetails(int dayIndex)
        {
            int currentHour = DateTime.Now.Hour;
            DateTime firstForecastTime = DateTime.Parse(Weatherlist[0].dt_txt);
            int firstForecastHour = firstForecastTime.Hour;

            // Tính toán phần tử trong danh sách Weatherlist
            int index;

            if (dayIndex == 1)
            {
                index = (currentHour - firstForecastHour) / 3;
            }
            else
            {
                index = ((24 - firstForecastHour) / 3) + (8 * (dayIndex - 2));
            }

            if (index < Weatherlist.Count)
            {
                var forecast = Weatherlist[index];

                LblWeatherDescription.Text = forecast.weather[0].description;
                LblTemperature.Text = forecast.main.temperature + "℃";
                LblHumidity.Text = forecast.main.humidity + "%";
                LblWind.Text = forecast.wind.speed + "Km/h";
                ImgWeatherIcon.Source = forecast.weather[0].customIcon;
                LbltimeDate.Text = forecast.dt_txt;
            }
            else
            {
                // Nếu index vượt quá danh sách, hãy xử lý hoặc thông báo lỗi.
                Console.WriteLine("Index out of range: " + index);
            }
        }

        private void UpdateDateAndTime()
        {
            var now = DateTime.Now;
            nowDay.Text = now.ToString("dd/MM/yyyy");
            nowTime.Text = now.ToString("HH:mm");
        }

        private void OnHorizontalStackLayoutTapped1(object sender, EventArgs e)
        {
            OnHorizontalStackLayoutTapped(sender, e, 1);
        }

        private void OnHorizontalStackLayoutTapped2(object sender, EventArgs e)
        {
            OnHorizontalStackLayoutTapped(sender, e, 2);
        }

        private void OnHorizontalStackLayoutTapped3(object sender, EventArgs e)
        {
            OnHorizontalStackLayoutTapped(sender, e, 3);
        }

        private void OnHorizontalStackLayoutTapped4(object sender, EventArgs e)
        {
            OnHorizontalStackLayoutTapped(sender, e, 4);
        }

        private void OnHorizontalStackLayoutTapped5(object sender, EventArgs e)
        {
            OnHorizontalStackLayoutTapped(sender, e, 5);
        }
        private void OnHorizontalStackLayoutTapped6(object sender, EventArgs e)
        {
            OnHorizontalStackLayoutTapped(sender, e, 6);
        }

        private void OnHorizontalStackLayoutTapped(object sender, EventArgs e, int dayIndex)
        {
            var currentLayout = sender as HorizontalStackLayout;

            if (currentLayout != null)
            {
                // Nếu một layout đã được nhấn trước đó, đặt lại màu nền của nó
                if (_previousTappedLayout != null && _previousTappedLayout != currentLayout)
                {
                    _previousTappedLayout.BackgroundColor = _initialBackgroundColor; // Đặt lại màu nền ban đầu
                }

                // Lưu màu nền ban đầu của layout hiện tại nếu _previousTappedLayout là null
                if (_previousTappedLayout == null)
                {
                    _initialBackgroundColor = currentLayout.BackgroundColor;
                }

                // Thay đổi màu nền của layout hiện tại
                currentLayout.BackgroundColor = Color.FromHex("#80634f");

                // Cập nhật tham chiếu tới layout hiện tại
                _previousTappedLayout = currentLayout;

                // Hiển thị chi tiết dự báo thời tiết cho ngày tương ứng
                ShowForecastDetails(dayIndex);
            }
        }
    }
}
