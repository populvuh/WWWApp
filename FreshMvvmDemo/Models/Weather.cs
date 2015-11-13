using System;
using System.Collections.Generic;

using PropertyChanged;
using Xamarin.Forms;

namespace FreshMvvmDemo
{
	[ImplementPropertyChanged]
	public class WeatherMain
	{
		public bool Error = true;
		public City city { get; set; }
		public string cod { get; set; }
		public double message { get; set; }
		public int cnt { get; set; }
		public List<List> list { get; set; }

		public WeatherMain ()
		{
		}


		public List<string> Temperature { 
			get {
				List<string> _temperature = new List<string> (this.list.Count);
				foreach (FreshMvvmDemo.List l in this.list) {
					_temperature.Add(l.dt_txt);
				}	
				return _temperature;
			}
		}


		public ImageSource CityPicture {
			get { return city.name + ".jpg";	} 
		} 
		public ImageSource TempPicture {
			get { 
				if (null == list || 0 == list.Count)
					return "sunny.png";
				
				return (list[0].main.temp < 20) ? "snow.png" : (list[0].main.temp<25) ? "rain.jpg" : "sunny.jpg"; 
			}
		}
		public string MapUrl
		{
			get { 
				string url = null;
				if (0.0 != city.coord.lat || 0.0 != city.coord.lon) {
					url = string.Format ("http://maps.google.com/?q={0},{1}", city.coord.lat, city.coord.lon); 
					//url = string.Format ("http://www.google.com"); 
					System.Diagnostics.Debug.WriteLine (url);
				} 
				return url;
			}
		}
		string _ErrorMessage = "";
		public string ErrorMessage {
			get { return _ErrorMessage; }
			set { _ErrorMessage = value; }
		}

		public bool ShowWeather {
			get { 
				//bool show = ("Sydney"==city.name);		//(0.0 != longitude || 0.0 != latitude || 0.0 != Humidity || 0.0 != Temperature);
				bool show = !Error;
				System.Diagnostics.Debug.WriteLine ("ShowWeather - {0}", show);
				return show;
			}
		}

	}

	//{"coord":{"lon":153.03,"lat":-27.47},"weather":[{"id":801,"main":"Clouds","description":"few clouds","icon":"02d"}],"base":"cmc stations","main":{"temp":298.53,"pressure":1026,"humidity":49,"temp_min":295.93,"temp_max":301.48},"wind":{"speed":3.1,"deg":10},"clouds":{"all":20},"dt":1443660007,"sys":{"type":1,"id":8164,"message":0.0095,"country":"AU","sunrise":1443641282,"sunset":1443685677},"id":2174003,"name":"Brisbane","cod":200}

	public class Coord
	{
		public double lon { get; set; }
		public double lat { get; set; }
	}

	public class Sys
	{
		public int population { get; set; }
	}

	public class City
	{
		public int id { get; set; }
		public string name { get; set; }
		public Coord coord { get; set; }
		public string country { get; set; }
		public int population { get; set; }
		public string findname { get; set; }
		public int zoom { get; set; }
		public Sys sys { get; set; }
	}

	public class Main
	{
		public double temp { get; set; }
		public double temp_min { get; set; }
		public double temp_max { get; set; }
		public double pressure { get; set; }
		public double sea_level { get; set; }
		public double grnd_level { get; set; }
		public int humidity { get; set; }
		public double temp_kf { get; set; }
	}

	public class Weather
	{
		public int id { get; set; }
		public string main { get; set; }
		public string description { get; set; }
		public string icon { get; set; }
	}

	public class Clouds
	{
		public int all { get; set; }
	}

	public class Wind
	{
		public double speed { get; set; }
		public double gust { get; set; }
		public double deg { get; set; }
	}

	public class Rain
	{
		public double __invalid_name__3h { get; set; }
	}

	public class Sys2
	{
		public string pod { get; set; }
	}

	public class List
	{
		public int dt { get; set; }
		public Main main { get; set; }
		public List<Weather> weather { get; set; }
		public Clouds clouds { get; set; }
		public Wind wind { get; set; }
		public Rain rain { get; set; }
		public Sys2 sys { get; set; }
		public string dt_txt { get; set; }
	}
}

