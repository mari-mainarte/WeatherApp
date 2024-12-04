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
        City cidade;

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

        WeatherAppService weatherAppService;
        public ICommand getCidadeCommand { get; }

        public WeatherAppViewModel()
        {
            getCidadeCommand = new Command(getCidade);
            weatherAppService = new WeatherAppService();
        }

        public async void getCidade()
        {
            Cidade = await weatherAppService.GetCidadeAsync(cityName);
            string country = Cidade.Sys.Country;
            Name = Cidade.Name + ", " + country;
            Temp = Math.Round(Cidade.Main.Temp).ToString() + "ºC";
            Description = Cidade.Weather[0].Description;
            Humidity = Cidade.Main.Humidity.ToString() + "%";
            Speed = Cidade.Wind.Speed.ToString() + "km/h";
            FlagIcon = $"https://flagsapi.com/{country}/flat/64.png";
            WeatherIcon = $"https://openweathermap.org/img/wn/{Cidade.Weather[0].Icon}@2x.png";
            IsVisible = true;
        }
    }
}
