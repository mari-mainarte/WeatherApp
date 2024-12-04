using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherAppService
    {
        private HttpClient httpClient;
        private City cidade;
        private JsonSerializerOptions jsonSerializerOptions;
        private string apiKey = "21a98e40b60436fb3cc8fe3abc8f3f4b";

        public WeatherAppService()
        {
            httpClient = new HttpClient();
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<City> GetCidadeAsync(string cityName)
        {
            Uri uri = new Uri($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&appid={apiKey}&lang=pt_br");
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    cidade = JsonSerializer.Deserialize<City>(content, jsonSerializerOptions);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"\tERROR {0}", e.Message);
            }
            return cidade;
        }
    }
}
