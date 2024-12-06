using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public partial class WeatherAppViewModel: ObservableObject
    {
        [ObservableProperty]
        City city;

        [ObservableProperty]
        string cityName;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string temp;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string humidity;

        [ObservableProperty]
        private string speed;

        [ObservableProperty]
        private string flagIcon;

        [ObservableProperty]
        private bool isVisible = false;

        [ObservableProperty]
        private string weatherIcon;

        [ObservableProperty]
        private string tempMin;

        [ObservableProperty]
        private string tempMax;

        WeatherAppService weatherAppService;
        public ICommand getCityCommand { get; }
        public ICommand getCityGpsCommand { get; }

        public WeatherAppViewModel()
        {
            getCityCommand = new Command(getCity);
            getCityGpsCommand = new Command(getCityGps);
            weatherAppService = new WeatherAppService();
        }

        public async void defCityProperties(City city)
        {
            string country = city.Sys.Country;
            Name = city.Name + ", " + country;
            Temp = Math.Round(city.Main.Temp).ToString() + "ºC";
            Description = city.Weather[0].Description;
            Humidity = city.Main.Humidity.ToString() + "%";
            Speed = city.Wind.Speed.ToString() + "km/h";
            FlagIcon = $"https://flagsapi.com/{country}/flat/64.png";
            WeatherIcon = $"https://openweathermap.org/img/wn/{city.Weather[0].Icon}@2x.png";
            TempMin = "Mínima: " + ((int)city.Main.Temp_min).ToString() + "º";
            TempMax = "Máxima: " + ((int)city.Main.Temp_max).ToString() + "º";
            IsVisible = true;
        }

        public async void getCity()
        {
            City = await weatherAppService.GetCityAsync(CityName);
            defCityProperties(City);
        }

        public async void getCityGps()
        {
            City = await weatherAppService.GetCityGps();
            defCityProperties(City);
        }
    }
}
