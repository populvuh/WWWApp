using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Toasts;

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

            // there's a bug in Forms which puts a large blank space (20-30cms) above the listView on some devices 
            // (e.g. my G4, tho oddly enough, it works fine on all the emulators I've tried it on)
            // adding a blank Header solves it
            if (listView.Header == null)
                listView.Header = new Label { HeightRequest = 0 };

            /*SearchFor.TextChanged += (sender, e) => {
				System.Diagnostics.Debug.WriteLine ("SelectCountryPage.TextChanged ( {0} )", e.NewTextValue);
			};
			SearchFor.SearchButtonPressed += (sender, e) => {
				System.Diagnostics.Debug.WriteLine ("SelectCountryPage.SearchButtonPressed ( {0} )", SearchFor.Text);
			};*/
        }

		private void GoTo(Countrycode cc)
		{
            try
            {
                listView.ScrollTo(cc, ScrollToPosition.Start, false);
            }
            catch (ArgumentException ex)
            {
                var notificator = DependencyService.Get<IToastNotificator>();
                notificator.Notify(ToastNotificationType.Error, "Error", "Sorry, "+cc.name+" wasn't found", TimeSpan.FromSeconds(2));
            }
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

