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
        private City city;
        private JsonSerializerOptions jsonSerializerOptions;
        private string apiKey = "21a98e40b60436fb3cc8fe3abc8f3f4b";
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public WeatherAppService()
        {
            httpClient = new HttpClient();
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<City> GetCityAsync(string cityName)
        {
            Uri uri = new Uri($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&appid={apiKey}&lang=pt_br");
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    city = JsonSerializer.Deserialize<City>(content, jsonSerializerOptions);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"\tERROR {0}", e.Message);
            }
            return city;
        }

        public async Task<City> GetCurrentLocation()
        {
           
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(0.2));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location.Latitude != null && location.Longitude != null)
                {
                    Uri uri = new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={location.Latitude}&lon={location.Longitude}&units=metric&appid={apiKey}&lang=pt_br");
                    try
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(uri);
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            city = JsonSerializer.Deserialize<City>(content, jsonSerializerOptions);
                            
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(@"\tERROR {0}", e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro: " + e.Message);
            }
            finally
            {
                _isCheckingLocation = false;
            }
            return city;
        }
    }
}
