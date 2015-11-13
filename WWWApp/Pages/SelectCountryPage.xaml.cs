using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WWWApp
{
	public partial class SelectCountryPage : ContentPage
	{
		string lastSearchedFor = "";
			
		public SelectCountryPage ()
		{
			InitializeComponent ();

			MessagingCenter.Subscribe<Countrycode> (this, "CountryFound", (country) => {
				System.Diagnostics.Debug.WriteLine("SelectCountryPage.MessagingCenter () - CountryFound recieved - {0}", country.name);
				GoTo(country);
			});


			/*SearchFor.TextChanged += (sender, e) => {
				System.Diagnostics.Debug.WriteLine ("SelectCountryPage.TextChanged ( {0} )", e.NewTextValue);
			};
			SearchFor.SearchButtonPressed += (sender, e) => {
				System.Diagnostics.Debug.WriteLine ("SelectCountryPage.SearchButtonPressed ( {0} )", SearchFor.Text);
			};*/
		}

		private void GoTo(Countrycode cc)
		{
			listView.ScrollTo (cc, ScrollToPosition.Start, false);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			listView.SelectedItem = null;
			//if (null != listView) {
			//	listView.ClearValue (ListView.SelectedItemProperty);
			//}
		}

		void OnValueChanged (object sender, TextChangedEventArgs e) {
			var t = e.NewTextValue;
			System.Diagnostics.Debug.WriteLine ("SelectCountryPage.OnValueChanged ( {0} )", t);

			MessagingCenter.Send(t, "CountrySearch" );

			//listView.ClearValue (ListView.SelectedItemProperty);
		}

		void OnSearch (object sender, EventArgs e) {
			System.Diagnostics.Debug.WriteLine ("SelectCountryPage.OnSearch ( {0} )", SearchFor.Text);
		}
	}
}

