﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WWWApp
{
	public partial class SelectCityPage : ContentPage
	{
		public SelectCityPage ()
		{
			InitializeComponent ();

			MessagingCenter.Subscribe<CityLink> (this, "CityFound", (cityLink) => {
				//System.Diagnostics.Debug.WriteLine("SelectCityPage.MessagingCenter () - CityFound recieved - {0}", cityLink.name);
				GoTo(cityLink);
			});

            // there's a bug in Forms which puts a large blank space (20-30cms) above the listView on some devices 
            // (e.g. my G4, tho oddly enough, it works fine on all the emulators I've tried it on)
            // adding a blank Header solves it
            if (listView.Header == null)
                listView.Header = new Label { HeightRequest = 0 };
        }

        private void GoTo(CityLink cityLink)
		{
			try {
				listView.ScrollTo (cityLink, ScrollToPosition.MakeVisible, false);
			} catch (Exception ex) {
				// this sometime happened due to a double "CityFound" message being recieved 
				// no idea why that happened, but on the second calling of GoTo, a crash would occur
				// this stops it
				System.Diagnostics.Debug.WriteLine ("SelectCityPage.GoTo ( {0} ) Failed - {0}", cityLink.name, ex.Message);
			}
		}

		void OnValueChanged (object sender, TextChangedEventArgs e) {
			var t = e.NewTextValue;
			//System.Diagnostics.Debug.WriteLine ("SelectCityPage.OnValueChanged ( {0} )", t);

			MessagingCenter.Send(t, "CitySearch" );
		}

		void OnSearch (object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("SelectCityPage.OnSearch ( {0} )", SearchFor.Text);
		}
	}
}

