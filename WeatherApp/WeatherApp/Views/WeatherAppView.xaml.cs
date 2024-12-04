using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class WeatherAppView : ContentPage
{
	public WeatherAppView()
	{
		InitializeComponent();
		BindingContext = new WeatherAppViewModel();
	}
}