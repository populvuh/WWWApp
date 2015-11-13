using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace WWWApp
{
	public partial class DisplayMapPage : ContentPage
	{
		private bool firstPosition = true;

		public DisplayMapPage ()
		{
			InitializeComponent ();

			MessagingCenter.Subscribe<MapDescriptor> (this, "Position", (mapDescriptor) => {
				System.Diagnostics.Debug.WriteLine("ViewWeatherPage.MessagingCenter () - Position Recieved - {0}x{1}", mapDescriptor.latitude, mapDescriptor.longitude);
				cityMap.MoveToRegion (MapSpan.FromCenterAndRadius (new Position(mapDescriptor.latitude, mapDescriptor.longitude), Distance.FromKilometers (mapDescriptor.radius)));
			});

			MessagingCenter.Subscribe<string> (this, "Zoom", (zoom) => {
				MapSpan currSpan = cityMap.VisibleRegion;
				System.Diagnostics.Debug.WriteLine("ViewWeatherPage.MessagingCenter () - Zoom - {0}, {1}x{2}, {3}", zoom, currSpan.Center.Latitude, currSpan.Center.Longitude, currSpan.Radius.Kilometers);
				if ( null != currSpan ) {
					double radius = currSpan.Radius.Kilometers;
					if ( Int32.Parse(zoom) < 0 )
						radius /= 3;
					else
						radius *= 3;
					cityMap.MoveToRegion (MapSpan.FromCenterAndRadius (new Position(currSpan.Center.Latitude, currSpan.Center.Longitude), Distance.FromKilometers (radius)));
				}
			});

			MessagingCenter.Subscribe<String> (this, "MapType", (mapType) => {
				System.Diagnostics.Debug.WriteLine("ViewWeatherPage.MessagingCenter () - MapType Recieved - {0}", mapType);
				if ( "Satellite" == mapType )
					cityMap.MapType = MapType.Satellite;
				else if ( "Hybrid" == mapType )
					cityMap.MapType = MapType.Hybrid;
				else //( "Street" == mapType )
					cityMap.MapType = MapType.Street;
			});

		}

		protected override void OnAppearing()
		{
		}

		protected override void OnDisappearing()
		{
			MessagingCenter.Unsubscribe<MapDescriptor> (this, "Position");
		}
	}
}

