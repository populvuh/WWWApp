using System;

using Xamarin.Forms;

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


	[ImplementPropertyChanged]
	public class DisplayForecastGraphPageModel  : FreshMvvm.FreshBasePageModel
	{
		public int width 	{ get; set; }
		public int height 	{ get; set; }
		public string title 	{ get; set; }
		public PlotModel Model	{ get; set; }
		//public WeatherMain weather 	{ get; set; }

		public DisplayForecastGraphPageModel ()
		{
			System.Diagnostics.Debug.WriteLine ("DisplayForecastGraphPageModel.ctor()");

			bool bIsPortrait = DependencyService.Get<IDeviceInfoProvider> ().IsPortait ();
			width = DependencyService.Get<IDeviceInfoProvider>().GetWidth ();
			height = (int)(DependencyService.Get<IDeviceInfoProvider>().GetHeight () * 0.6);

			System.Diagnostics.Debug.WriteLine ("DisplayForecastGraphPageModel.ctor( {0}, {1} )", width, height);
		}

		public override void Init (object initData)
		{
			base.Init (initData);

			WeatherMain weather = initData as WeatherMain;
			title = String.Format("{0} forecast", weather.city.name);
			Model = CreatePlotModel (weather);	

			System.Diagnostics.Debug.WriteLine ("DisplayForecastGraphPageModel.Init () - Fin" );
		}


		private PlotModel CreatePlotModel ( WeatherMain weather ) {
			System.Diagnostics.Debug.WriteLine ("DisplayForecastPageModel.CreatePlotModel()");

			var plotModel = new PlotModel ();	
			List listElement = weather.list.ElementAt(0);
			DateTime dt1 = WeatherMain.FromUnixTimeSeconds(listElement.dt).AddSeconds(weather.timeOffset);
			plotModel.Title = String.Format("Forecast temperature from {0}", dt1.ToString("f"));
			plotModel.TextColor = OxyColors.White;
			plotModel.TitleColor = OxyColors.White;

			var series1 = new TwoColorAreaSeries ();
			series1.LabelFormatString = "{0}";

			// above 0C colours
			//series1.Fill = OxyColors.LightGreen;
			series1.Color = OxyColors.Green;
			series1.MarkerSize = 2;
			//series1.MarkerType = MarkerType.Plus;
			series1.MarkerFill = OxyColors.Green;
			series1.MarkerStroke = OxyColors.Green;

			// below 0C colours
			series1.Fill2 = OxyColors.LightBlue;
			series1.Color2 = OxyColors.Blue;
			series1.MarkerFill2 = OxyColors.Red;
			series1.MarkerStroke2 = OxyColors.Blue;

			series1.Smooth = true;
			series1.StrokeThickness = 1;
			plotModel.Series.Add(series1);

			int i = 0;
			DateTime dt = dt1;
			bool setMinMax = true;
			double minTemp = 0, maxTemp = 0;
			foreach (List l in weather.list) {
				dt = WeatherMain.FromUnixTimeSeconds(l.dt).AddSeconds(weather.timeOffset);
				System.Diagnostics.Debug.WriteLine ("item {0}\t{1}\t{2}\t{3}\t{4}", i++, l.main.temp, l.dt_txt, l.dt, dt.ToString());

				series1.Points.Add (new DataPoint (DateTimeAxis.ToDouble(dt), l.main.temp));
				if (setMinMax || l.main.temp < minTemp)
					minTemp = l.main.temp;
				if (setMinMax || l.main.temp > maxTemp)
					maxTemp = l.main.temp;
				setMinMax = false;
			}


			double maxXaxis = weather.list.LastOrDefault ().dt;
			var linearAxis1 = new LinearAxis {
				Title = "Temperature",
				Unit = "°C",
				AxislineColor = OxyColors.White,
				TicklineColor = OxyColors.White,
				TextColor = OxyColors.White,
				TitleColor = OxyColors.White,
				Maximum = maxTemp + 2,
				Minimum = (minTemp <= 0) ? (int)(minTemp - 3) : 0,
				IsZoomEnabled = false,
				IsPanEnabled = false
			};
			plotModel.Axes.Add(linearAxis1);

			var linearAxis2 = new DateTimeAxis {
				Title = String.Format("From {0} to {1}", dt1.ToString("dd-MMM-yyyy HH:mm"), dt.ToString("dd-MMM-yyyy HH:mm") ),
				AxislineColor = OxyColors.White,
				TicklineColor = OxyColors.White,
				TextColor = OxyColors.White,
				TitleColor = OxyColors.White,
				StringFormat = "dd HH:mm",													//"yyyy-MM-dd h:MM",
				//IntervalLength = 360,
				MinorIntervalType = DateTimeIntervalType.Hours,
				IntervalType = DateTimeIntervalType.Hours,
				MajorGridlineStyle = LineStyle.Solid,
				MajorGridlineColor = OxyColors.White,
				Position = AxisPosition.Bottom,
				IsZoomEnabled = false,
				IsPanEnabled = false
			};
			plotModel.Axes.Add(linearAxis2);

			System.Diagnostics.Debug.WriteLine ("DisplayForecastPageModel.CreatePlotModel() Fin");

			return plotModel;
		}
	}
}


