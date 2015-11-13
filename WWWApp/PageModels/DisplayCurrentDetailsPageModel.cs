namespace WWWApp
{
	using System;
	//using System.TimeZone;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Threading.Tasks;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Xml.Serialization;

	using FreshMvvm;
	using Xamarin.Forms;
	using PropertyChanged;


	public class CityTime {
		public string cityId;
		public string day;
		public long timeOffset;
	}

	public class DisplayCurrentDetailsPageModel : CityWeatherPage
	{
		//public string message	{get; set; }
		public string dateTime	{get; set; }
		public string detailsAt	{get; set; }
		public string temp		{get; set; }
		public string tempMax	{get; set; }
		public string tempMin	{get; set; }
		public string wind		{get; set; }
		public string wind2		{get; set; }
		public string humidity	{get; set; }
		public string pressure	{get; set; }
		public string clouds	{get; set; }
		public string sunrise	{get; set; }
		public string sunset	{get; set; }
		public string sunriseset {get; set; }
		public string latlong	{get; set; }
		public string timezone	{get; set; }
		public string timezoneOffset	{get; set; }
		public string description {get; set; }
		public ImageSource weatherImage	{get; set; }
		public CityTime cityTime = new CityTime();

		public CurrentWeather currentWeather 	{ get; set; }
		public CurrentWeatherXml.Current currentWeather2 	{ get; set; }
		ITimeZoneApi _timezoneApi 		{ get; set; }

		public DisplayCurrentDetailsPageModel (IWeatherApi weatherAPI, ITimeZoneApi timezoneApi)
		{
			System.Diagnostics.Debug.WriteLine ("DisplayCurrentDetailsPageModel.ctor ()" );

			_weatherAPI = weatherAPI;
			_timezoneApi = timezoneApi;

			System.Diagnostics.Debug.WriteLine ("DisplayCurrentDetailsPageModel.ctor () Fin" );
		}


		public override void Init (object initData)
		{
			System.Diagnostics.Debug.WriteLine ("DisplayCurrentDetailsPageModel.Init ()" );
			base.Init (initData);
			cityId = initData as string;
			cityTime.cityId = cityId;

			bool bLoadedOK = false;
			if (!DownloadWeatherXml (cityId)) {
				System.Diagnostics.Debug.WriteLine ("DisplayCurrentDetailsPageModel.Init () - DownloadWeather failed");
			} else {
				LoadWeatherDetails ();
				if (null != currentWeather2.City.Coord) {
					// cvt ticks to timestamp
					long timestamp = (DateTime.Parse(currentWeather2.Lastupdate.Value).Ticks - 621355968000000000) / 10000000;

					System.Diagnostics.Debug.WriteLine ("lat {0}, lon {1}, time {2}, {3}", currentWeather2.City.Coord.Lat, currentWeather2.City.Coord.Lon, currentWeather2.Lastupdate.Value, timestamp);
					int timeOffset = DownloadTimezone (	Double.Parse(currentWeather2.City.Coord.Lat), 
														Double.Parse(currentWeather2.City.Coord.Lon), 
														timestamp ); 
					UpdateDateTimes (timeOffset);
					bLoadedOK = true;
				}
			}

			System.Diagnostics.Debug.WriteLine ("DisplayCurrentDetailsPageModel.Init () - Fin" );

			if (!bLoadedOK) {
				DisplayErrorMessage ();
			}
		}

		async Task DisplayErrorMessage () {
			await CoreMethods.DisplayAlert ("Error", "Failed to load weather details\n :(", "OK");
			await CoreMethods.PopPageModel ();
		}


		bool DownloadWeatherXml(string city)
		{
			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.DownloadWeather( {0} )", city);

			try
			{
				string weatherXml = GetWeatherXml(city).Result;

				using (var reader = new System.IO.StringReader (weatherXml)) {
					var serializer = new XmlSerializer (typeof(CurrentWeatherXml.Current));
					currentWeather2 = (CurrentWeatherXml.Current)serializer.Deserialize (reader);
				}
				System.Diagnostics.Debug.WriteLine("Downloaded OK {0}", currentWeather2.City.Name);
			} 
			catch (Exception ex) 
			{	
				string message = string.Format("DownloadWeather( {0} ) failed\n{1}", city, ex.Message);
				System.Diagnostics.Debug.WriteLine (message);
				return false;
			}
			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.DownloadWeather() - Fin");

			return true;
		}

		private const string BaseUrL = "http://api.openweathermap.org";

		private async Task<string> GetWeatherXml(string city)
		{
			var client = new System.Net.Http.HttpClient ();
			client.BaseAddress = new Uri(BaseUrL);
			var response = client.GetAsync(string.Format("/data/2.5/weather?q={0}&units=metric&APPID={1}&mode=xml", city, _apiKey )).Result;
			response.EnsureSuccessStatusCode();

			return response.Content.ReadAsStringAsync().Result;
		}


		private bool DownloadWeatherJson(string city)
		{
			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.DownloadWeather( {0} )", city);

			try
			{
				currentWeather = _weatherAPI.WeatherForCity (city, _apiKey).Result;

				System.Diagnostics.Debug.WriteLine("Downloaded OK {0}", currentWeather.name, currentWeather);
			} 
			catch (Exception ex) 
			{	
				string message = string.Format("DownloadWeather( {0} ) failed\n{1}", city, ex.Message);
				System.Diagnostics.Debug.WriteLine (message);
				return false;
			}
			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.DownloadWeather() - Fin");

			return true;
		}


		private void LoadWeatherDetails() {
			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.LoadWeatherDetails ()");

			title = String.Format("{0} weather", currentWeather2.City.Name);

			//dateTime = DateTime.Parse(currentWeather2.City.Sun.Rise).ToString();

			//description = String.Format ("{0}°C, {1}", currentWeather2.Temperature.Value, currentWeather2.Weather.Value );
			description = UppercaseFirst( currentWeather2.Weather.Value );
			string icon = currentWeather2.Weather.Icon;
			string imageName = String.Format("WWWApp.Resources.Icons.{0}.png", icon );
			weatherImage = ImageSource.FromResource(imageName);

			clouds = String.Format("{0} ({1}% cover)", UppercaseFirst(currentWeather2.Clouds.Name), currentWeather2.Clouds.Value);

			if ( null != currentWeather2.City.Coord)
				latlong	= String.Format("[{0:0.00}, {1:0.00}]", currentWeather2.City.Coord.Lat, currentWeather2.City.Coord.Lon);

			temp = String.Format (" {0}°C", currentWeather2.Temperature.Value);
			humidity = String.Format (" {0}{1}", currentWeather2.Humidity.Value, currentWeather2.Humidity.Unit);
			pressure = String.Format (" {0}{1}", currentWeather2.Pressure.Value, currentWeather2.Pressure.Unit);

			if (null != currentWeather2.Wind) {
				wind = String.Format (" {0} ({1}metres/sec)", currentWeather2.Wind.Speed.Name, currentWeather2.Wind.Speed.Value);
				wind2 = String.Format (" {0} ({1})", currentWeather2.Wind.Direction.Name, currentWeather2.Wind.Direction.Value);
			}

			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.LoadWeatherDetails () - Fin");
		}


		int DownloadTimezone(double lat, double lng, long timestamp)
		{
			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.DownloadTimezone( {0}, {1}, {2} )", lat, lng, timestamp);

			string message = "";
			bool bError = false;
			int timeOffset = 0;
			try
			{
				Timezone tz = _timezoneApi.TimeZoneForCity(lat, lng, timestamp).Result;
				timeOffset = tz.rawOffset + tz.dstOffset; 
				timezone = tz.timeZoneName;

				TimeSpan tsRaw = new TimeSpan( tz.rawOffset * TimeSpan.TicksPerSecond );			// cvt ticks to secs
				timezoneOffset = String.Format("{0:c}", tsRaw);										// hh:mm:ss
				if ( 0 != tz.dstOffset ) {
					// only add in if it is != 0
					TimeSpan tsDst = new TimeSpan( tz.dstOffset * TimeSpan.TicksPerSecond );
					timezoneOffset += String.Format(" + {0:c}DST", tsDst);
				}

				if ( "INVALID_REQUEST" == tz.status  ) {
					bError = true;
					message = string.Format("DownloadTimezone({0}, {1}, {2}) failed", lat, lng, timestamp);
				} else {
					System.Diagnostics.Debug.WriteLine("Downloaded OK {0}, {1}, {2}, {3}, {4}", tz.timeZoneId, tz.timeZoneName, tz.rawOffset, tz.dstOffset, tz.status);
				}
				
			} 
			catch (Exception ex) 
			{	
				bError = true;
				message = string.Format("DownloadTimezone({0}, {1}, {2}) failed - {3}", lat, lng, timestamp, ex.Message);
			}

			if (bError) {
				System.Diagnostics.Debug.WriteLine (message);
			}

			cityTime.timeOffset = timeOffset;
			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.DownloadTimezone() - Fin");

			return timeOffset;
		}

		private void UpdateDateTimes (int timeOffset)
		{
			sunrise = DateTime.Parse(currentWeather2.City.Sun.Rise).AddSeconds(timeOffset).ToString ("HH:mm");
			sunset = DateTime.Parse(currentWeather2.City.Sun.Set).AddSeconds(timeOffset).ToString ("HH:mm");
			sunriseset = string.Format ("{0}, {1}", sunrise, sunset);

			DateTime now = DateTime.Parse (currentWeather2.Lastupdate.Value).AddSeconds (timeOffset);
			detailsAt = String.Format ("at {0}, {1} ({2})", now.ToString ("HH:mm"), now.DayOfWeek.ToString(), timezone);

			cityTime.day = String.Format ("{0:r}", now).Split(',').ElementAt(0);


			System.Diagnostics.Debug.WriteLine("DisplayCurrentDetailsPageModel.LoadSunriseSunSet ( {0} ) - {1}, {2}", timeOffset, sunrise, sunset );
		}

		string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}

		public Command DisplayForecastCommand 
		{
			get {
				return new Command (() => {
					System.Diagnostics.Debug.WriteLine("Command DisplayForecastCommand");
					CoreMethods.PushPageModel<DisplayForecastPageModel> (cityTime);
				});
			}			
		}

		public Command DisplayMapCommand
		{
			get {
				return new Command (() => {
					System.Diagnostics.Debug.WriteLine("Command DisplayFullDetails");
					CoreMethods.PushPageModel<DisplayMapPageModel> (currentWeather2);
				});
			}
		}

	}
}


