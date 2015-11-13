using System;
using System.Threading.Tasks;
using Refit;
using System.Collections.Generic;

namespace WWWApp
{
	public interface IWeatherApi
	{
		[Get("/data/2.5/weather?q={city}&units=metric&APPID={apiKey}")]
		Task<CurrentWeather> WeatherForCity(string city, string apiKey);
		[Get("/data/2.5/forecast/city?q={city}&units=metric&APPID={apiKey}")]
		Task<WeatherMain> ForecastForCity(string city, string apiKey);
	}
}

