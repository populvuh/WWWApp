
namespace FreshMvvmDemo
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


	[ImplementPropertyChanged]
	public class DisplayGraphPageModel : FreshMvvm.FreshBasePageModel
	{
		public int width 	{ get; set; }
		public int height 	{ get; set; }
		public string cityName 	{ get; set; }
		public string message	{ get; set; }
		public PlotModel Model	{ get; set; }
		public WeatherMain weather 	{ get; set; }

		IWeatherApi _weatherAPI;
		string _apiKey = "2c77aa68a5f34133569fd664c5f902b4";

		public DisplayGraphPageModel (IWeatherApi weatherAPI )
		{
			System.Diagnostics.Debug.WriteLine ("DisplayGraphPageModel.ctor()");

			_weatherAPI = weatherAPI;

			bool bIsPortrait = DependencyService.Get<IDeviceInfoProvider> ().IsPortait ();
			width = DependencyService.Get<IDeviceInfoProvider>().GetWidth ();
			height = (int)(DependencyService.Get<IDeviceInfoProvider>().GetHeight () * 0.6);

			System.Diagnostics.Debug.WriteLine ("DisplayGraphPageModel.ctor( {0}, {1} )", width, height);
		}

		public override void Init (object initData)
		{
			base.Init (initData);

			cityName = initData as string;

			IsLoading = true;
			//DownloadWeather (cityName);
				
			if (DownloadForecast (cityName).Result) {
				Model = CreatePlotModel (weather);	
				//cityName = weather.city.name;
			}

			IsLoading = false;

			System.Diagnostics.Debug.WriteLine ("DisplayGraphPageModel.Init () - Fin" );
		}



		async Task<bool> DownloadForecast(string city)
		{
			System.Diagnostics.Debug.WriteLine("DisplayGraphPageModel.DownloadWeather( {0} )", city);

			try
			{
				message = "Updating weather ...";
				weather =  _weatherAPI.ForecastForCity (city, _apiKey).Result;
				System.Diagnostics.Debug.WriteLine("Downloaded OK {0} {1}", weather.city.name, weather.list.Count);
			} 
			catch (Exception ex) 
			{	
				message = string.Format("DownloadWeather( {0} ) failed\n{1}", city, ex.Message);
				System.Diagnostics.Debug.WriteLine (message);
				return false;
			}

			return true;
		}


		private PlotModel CreatePlotModel ( WeatherMain weather ) {
			System.Diagnostics.Debug.WriteLine ("DisplayGraphPageModel.CreatePlotModel()");

			var plotModel = new PlotModel ();	
			List listElement = weather.list.ElementAt(0);
			plotModel.Title = String.Format("Temperature from {0}", listElement.dt_txt);
			plotModel.TextColor = OxyColors.White;
			plotModel.TitleColor = OxyColors.White;

			var series1 = new TwoColorAreaSeries ();
			series1.LabelFormatString = "{0}";

			// above 0C colours
			//series1.Fill = OxyColors.LightGreen;
			series1.Color = OxyColors.Green;
			series1.MarkerFill = OxyColors.Green;
			series1.MarkerSize = 1;
			series1.MarkerStroke = OxyColors.Green;
			series1.MarkerType = MarkerType.Circle;

			// below 0C colours
			series1.Fill2 = OxyColors.LightBlue;
			series1.Color2 = OxyColors.Blue;
			series1.MarkerFill2 = OxyColors.Red;
			series1.MarkerStroke2 = OxyColors.Blue;

			series1.Smooth = true;
			series1.StrokeThickness = 1;
			//List listElement = weather.list.ElementAtOrDefault(0);
			//series1.Title = String.Format("Temperature at {0} from {1}", weather.city.name, listElement.dt_txt);
			//series1.TrackerFormatString = "December {2:0}: {4:0.0} °C";
			plotModel.Series.Add(series1);

			int i = 0;
			//int startTime = -1;
			bool setMinMax = true;
			double minTemp = 0, maxTemp = 0;
			foreach (FreshMvvmDemo.List l in weather.list) {
				//if (startTime < 0)
				//	startTime = l.dt;
				//System.Diagnostics.Debug.WriteLine ("item {0}\t{1}\t{2}\t{3}\t{4}\t{5}", i++, l.main.temp, l.dt, l.dt_txt, DateTimeAxis.ToDouble(DateTime.Parse(l.dt_txt)), DateTimeAxis.ToDouble(new DateTime(2015, 10, 23, 3,0,0)));


				series1.Points.Add (new DataPoint (DateTimeAxis.ToDouble(DateTime.Parse(l.dt_txt)), l.main.temp));
				if (setMinMax || l.main.temp < minTemp)
					minTemp = l.main.temp;
				if (setMinMax || l.main.temp > maxTemp)
					maxTemp = l.main.temp;
				setMinMax = false;
			}

			//i = 0;
			//System.Diagnostics.Debug.WriteLine ("\nseries1.Points");
			//foreach (DataPoint dp in series1.Points) {
			//	System.Diagnostics.Debug.WriteLine ("item {0}\t{1}\t{2}", i++, dp.X, dp.Y);
			//}

			double maxXaxis = weather.list.LastOrDefault ().dt;


			var linearAxis1 = new LinearAxis {
				Title = "Temperature",
				Unit = "°C",
				TextColor = OxyColors.White,
				TitleColor = OxyColors.White,
				Maximum = maxTemp + 2,
				Minimum = (minTemp <= 0) ? minTemp - 2 : 0
			};
			plotModel.Axes.Add(linearAxis1);

			var linearAxis2 = new DateTimeAxis {
				Title = "Date/Time",
				Position = AxisPosition.Bottom,
				TextColor = OxyColors.White,
				TitleColor = OxyColors.White,
				StringFormat = "H:MM",													//"yyyy-MM-dd h:MM",
				//IntervalType = DateTimeIntervalType.Days,
				//IntervalLength = 100,
				MajorGridlineStyle = LineStyle.Solid,
				MajorGridlineColor = OxyColors.White,
				//MinorGridlineStyle = LineStyle.None,
			};
			plotModel.Axes.Add(linearAxis2);

			System.Diagnostics.Debug.WriteLine ("DisplayGraphPageModel.CreatePlotModel() Fin");

			return plotModel;
		}



		public Command DisplayFullDetailsCommand
		{
			get {
				return new Command (() => {
					System.Diagnostics.Debug.WriteLine("Command DisplayFullDetails");
					CoreMethods.PushPageModel<DisplayMapPageModel> (weather);
				});
			}
		}

		public void OnCancel (object sender, EventArgs e) 
		{
			System.Diagnostics.Debug.WriteLine("OnCancel");
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get	{ return this._isLoading; }
			set	{
				this._isLoading = value;
				RaisePropertyChanged("IsLoading");
			}
		}	
	}
}

