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

namespace WWWApp
{
	[ImplementPropertyChanged]
	public class DisplayMapPageModel : CityPage
	{
		private double _radius = 5.0;
		public CurrentWeatherXml.Current weather { get; set; } 


		public DisplayMapPageModel ()
		{
			System.Diagnostics.Debug.WriteLine("DisplayMapPageModel.ctor ()");
		}


		public override void Init (object initData)
		{
			System.Diagnostics.Debug.WriteLine("DisplayMapPageModel.Init ()");
			base.Init (initData);

			weather = initData as CurrentWeatherXml.Current;
			title = String.Format ("{0} map", weather.City.Name);

			System.Diagnostics.Debug.WriteLine("DisplayMapPageModel.Init ( {0} ) Fin", weather.City.Name);
		}


		async protected override void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing (sender, e);
			System.Diagnostics.Debug.WriteLine("DisplayMapPageModel.ViewIsAppearing ( {0}, {1} )", weather.City.Coord.Lat, weather.City.Coord.Lon);

			MapDescriptor mapDescriptor = new MapDescriptor { latitude=Double.Parse(weather.City.Coord.Lat), longitude=Double.Parse(weather.City.Coord.Lon), radius=_radius };
			MessagingCenter.Send(mapDescriptor, "Position" );
		}

		async protected override void ViewIsDisappearing (object sender, EventArgs e)
		{
			base.ViewIsDisappearing (sender, e);
			System.Diagnostics.Debug.WriteLine ("DisplayMapPageModel.ViewIsDisappearing ()");
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

