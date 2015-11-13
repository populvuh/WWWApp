using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
//using System.ComponentModel;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PropertyChanged;
using Acr.UserDialogs;

namespace FreshMvvmDemo
{
	[ImplementPropertyChanged]
	public class ViewWeatherPageModel : FreshMvvm.FreshBasePageModel
	{
		string _Title;
		public string Title 
		{ 	get	{ return _Title; }
			set { _Title = value; }
		}

		private double _radius = 5.0;
		public WeatherMain weather { get; set; } 


		public ViewWeatherPageModel ()
		{
			System.Diagnostics.Debug.WriteLine("ViewWeatherPageModel.ctor ()");
		}


		public override void Init (object initData)
		{
			System.Diagnostics.Debug.WriteLine("ViewWeatherPageModel.Init ()");
			base.Init (initData);

			weather = initData as WeatherMain;
			_Title = String.Format ("{0} map", weather.city.name);

			System.Diagnostics.Debug.WriteLine("ViewWeatherPageModel.Init ( {0} ) Fin", weather.city.name);
		}


		async protected override void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing (sender, e);
			System.Diagnostics.Debug.WriteLine("ViewWeatherPageModel.ViewIsAppearing ( {0}, {1} )", weather.city.coord.lat, weather.city.coord.lon);

			MapDescriptor mapDescriptor = new MapDescriptor { latitude=weather.city.coord.lat, longitude=weather.city.coord.lon, radius=_radius };
			MessagingCenter.Send(mapDescriptor, "Position" );
		}

		async protected override void ViewIsDisappearing (object sender, EventArgs e)
		{
			base.ViewIsDisappearing (sender, e);
			System.Diagnostics.Debug.WriteLine ("ViewWeatherPageModel.ViewIsDisappearing ()");
		}

		public Command ZoomCommand
		{
			get {
				return new Command<string> ((s) => {
					System.Diagnostics.Debug.WriteLine("Command ZoomCommand");
					MessagingCenter.Send(s, "Zoom" );
				});
			}
		}

		public Command UpdateMapTypeCommand
		{
			get {
				return new Command<string> ((s) => {
					System.Diagnostics.Debug.WriteLine("Command UpdateMapTypeCommand");
					MessagingCenter.Send(s, "MapType" );
				});
			}
		}


	}
}

