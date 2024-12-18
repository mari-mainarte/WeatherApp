﻿using System;
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

        public async Task<City> GetCityGpsAsync()
        {
            var location = await Geolocation.GetLocationAsync();

            if (location != null)
            {
                Uri uri = new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={location.Latitude}&lon={location.Longitude}&units=metric&appid={apiKey}&lang=pt_br");
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        city = JsonSerializer.Deserialize<City>(content, jsonSerializerOptions);
                        Debug.WriteLine(location.Latitude.ToString(), location.Longitude.ToString());

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(@"\tERROR {0}", e.Message);
                }
            }
            return city;
        }
    }
}
