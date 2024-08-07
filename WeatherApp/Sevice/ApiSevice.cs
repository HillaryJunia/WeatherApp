using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Sevices
{
    internal static class ApiSevice
    {
        public static async Task<Root> GetWeather(double latitude, double longtime)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(String.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&units=metric&appid=65e5c46f158ea6a4cfc33d8eb8d7a54d", latitude, longtime));
            return JsonConvert.DeserializeObject<Root>(response)!;
        }

        public static async Task<Root> GetWeatherByCIty(string city)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(String.Format("https://api.openweathermap.org/data/2.5/forecast?p={0}&units=metric&appid=65e5c46f158ea6a4cfc33d8eb8d7a54d", city));
            return JsonConvert.DeserializeObject<Root>(response)!;
        }
    }
}
