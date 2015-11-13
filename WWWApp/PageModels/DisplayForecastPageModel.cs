
namespace WWWApp
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Threading.Tasks;
	using System.Linq;
	using System.Runtime.CompilerServices;

	using Xamarin.Forms;
	using PropertyChanged;
	using Acr.UserDialogs;
	using OxyPlot;
	using OxyPlot.Axes;
	using OxyPlot.Series;
	using OxyPlot.Xamarin.Forms;

	public class ForecastDetail 
	{
		public string date { get; set; }
		public string time { get; set; }
		public string wind { get; set; }
		public string cloudsPressure { get; set; }
		public string tempDescription { get; set; }
		public ImageSource imageSource { get; set; }
	}

	public class ForecastDayDetail
	{
		public string day { get; set; }
		public double mmax { get; set; }
		public double mmin { get; set; }
		public ImageSource imageSource { get; set; }
	}

	[ImplementPropertyChanged]
	public class DisplayForecastPageModel : FreshMvvm.FreshBasePageModel
	{
		public string title 	{ get; set; }
		public string message	{ get; set; }
		public WeatherMain weather 	{ get; set; }
		public List<ForecastDetail> forecastDetails { get; set; } 
		public List<ForecastDayDetail> forecastDayDetails { get; set; } 
		public ForecastDayDetail forecastDayDetail1	{ get; set; } 
		public ForecastDayDetail forecastDayDetail2 { get; set; } 
		public ForecastDayDetail forecastDayDetail3 { get; set; } 
		public ForecastDayDetail forecastDayDetail4 { get; set; } 
		public ForecastDayDetail forecastDayDetail5 { get; set; } 

		IWeatherApi _weatherAPI;
		string _apiKey = "2c77aa68a5f34133569fd664c5f902b4";

		public DisplayForecastPageModel (IWeatherApi weatherAPI )
		{
			System.Diagnostics.Debug.WriteLine ("DisplayForecastPageModel.ctor()");

			_weatherAPI = weatherAPI;

			System.Diagnostics.Debug.WriteLine ("DisplayForecastPageModel.ctor()");
		}

		public override void Init (object initData)
		{
			base.Init (initData);

			CityTime ct = initData as CityTime;

			if (DownloadForecast (ct.cityId).Result) {
				title = String.Format("{0} forecast", weather.city.name);
				//weather.timeOffset = ct.timeOffset;
				CreateForecast (ct);	
			}

			System.Diagnostics.Debug.WriteLine ("DisplayForecastPageModel.Init () - Fin" );
		}



		async Task<bool> DownloadForecast(string city)
		{
			System.Diagnostics.Debug.WriteLine("DisplayForecastPageModel.DownloadWeather( {0} )", city);

			try
			{
				weather = _weatherAPI.ForecastForCity (city, _apiKey).Result;
				System.Diagnostics.Debug.WriteLine("Downloaded OK {0} {1}", weather.city.name, weather.list.Count);
			} 
			catch (Exception ex) 
			{	
				string message = string.Format("DownloadWeather( {0} ) failed\n{1}", city, ex.Message);
				System.Diagnostics.Debug.WriteLine (message);
				return false;
			}

			return true;
		}


		private void CreateForecast ( CityTime ct) {
			System.Diagnostics.Debug.WriteLine ("DisplayForecastPageModel.CreateForecast()");

			string day = ct.day;		// save the forecasts for the next days, NOT today
			ImageSource imageSource;
			ForecastDayDetail forecastDayDetail = null;
			forecastDetails = new List<ForecastDetail> ();
			forecastDayDetails = new List<ForecastDayDetail> ();

			List listElement = weather.list.ElementAt(0);
			DateTime dt1 = WeatherMain.FromUnixTimeSeconds(listElement.dt).AddSeconds(ct.timeOffset);

			int i = 0;
			foreach (List l in weather.list) {
				DateTime dt = WeatherMain.FromUnixTimeSeconds(l.dt).AddSeconds(ct.timeOffset);
				System.Diagnostics.Debug.WriteLine ("item {0}\t{1}\t{2}\t{3}\t{4}", i++, l.main.temp, l.dt_txt, l.dt, dt.ToString());

				string imageName = String.Format("WWWApp.Resources.Icons.{0}.png", l.weather[0].icon );
				imageSource = ImageSource.FromResource(imageName);

				string day2 = String.Format ("{0:r}", dt ).Split(',').ElementAt(0);
				if (day.CompareTo (day2) != 0) {
					day = day2;
					forecastDayDetail = new ForecastDayDetail {day=day2, mmax=l.main.temp, mmin=l.main.temp, imageSource=imageSource };
					forecastDayDetails.Add (forecastDayDetail);
				} else {
					if (null != forecastDayDetail) {
						if (forecastDayDetail.mmax < l.main.temp) {
							forecastDayDetail.mmax = l.main.temp;
							forecastDayDetail.imageSource = imageSource;
						}
						if (forecastDayDetail.mmin > l.main.temp) {
							forecastDayDetail.mmin = l.main.temp;
						}
					}
				}

				ForecastDetail fd = new ForecastDetail ();
				fd.date = String.Format("{0:ddd, MMM d, yyyy}", dt);
				fd.time = String.Format("{0:HH:mm}", dt);
				fd.wind = String.Format("Wind is {0} metres/sec ({1}deg)", l.wind.speed, l.wind.deg);
				fd.cloudsPressure = String.Format("Cloud cover is {0}%, {1}hpa", l.clouds.all, l.main.pressure);
				fd.tempDescription = String.Format("{0}°C, {1}", l.main.temp, l.weather[0].description);
				fd.imageSource = imageSource;

				forecastDetails.Add (fd);
			}

			// unfortunately, Forms doesn't do horizontal listviews, so I'm just hard coding 5 days 
			forecastDayDetail1 = new ForecastDayDetail();
			forecastDayDetail2 = new ForecastDayDetail();
			forecastDayDetail3 = new ForecastDayDetail();
			forecastDayDetail4 = new ForecastDayDetail();
			forecastDayDetail5 = new ForecastDayDetail();

			int size = forecastDayDetails.Count ();
			if ( size >= 1 )	forecastDayDetail1 = forecastDayDetails [0];
			if ( size >= 2 )	forecastDayDetail2 = forecastDayDetails [1];
			if ( size >= 3 )	forecastDayDetail3 = forecastDayDetails [2];
			if ( size >= 4 )	forecastDayDetail4 = forecastDayDetails [3];
			if ( size >= 5 )	forecastDayDetail5 = forecastDayDetails [4];

			/*foreach (ForecastDayDetail fdd in forecastDayDetails ) {
				System.Diagnostics.Debug.WriteLine ("day {0}\t{1}\t{2}", fdd.day, fdd.mmax, fdd.mmin);
			}*/
		}		

		public Command DisplayGraphCommand 
		{
			get {
				return new Command (() => {
					System.Diagnostics.Debug.WriteLine("Command DisplayForecastCommand");
					CoreMethods.PushPageModel<DisplayForecastGraphPageModel> (weather);
				});
			}			
		}


	}
}

