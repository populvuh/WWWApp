using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Windows.Input;

using FreshMvvm;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Acr.UserDialogs;


namespace WWWApp
{
	public class SelectCityPageModel : FreshMvvm.FreshBasePageModel
	{
		string countryCode;
		public string title { get; set; }
		public List<CityLink> cities { get; set; }

		public SelectCityPageModel ()
		{
			MessagingCenter.Subscribe<string> (this, "CitySearch", (cityPrefix) => {
				System.Diagnostics.Debug.WriteLine("SelectCityPageModel.MessagingCenter () - CitySearch recieved - {0}", cityPrefix);
				FindItem(cityPrefix);
			});
			System.Diagnostics.Debug.WriteLine("SelectCityPageModel.MessagingCenter () - fin");

			/*this.DisplayLinkCommand = new Command<string> ((link) =>
				{
					System.Diagnostics.Debug.WriteLine("DisplayLinkCommand {0}", link);
					CoreMethods.PushPageModel<SelectCountryPageModel> ();
				},
				(nothing) =>
				{
					// Return true if there's something to delete.
					System.Diagnostics.Debug.WriteLine("DisplayLinkCommand.CanExecute()");
					return true;
				}
			);*/
		}


		public override void Init (object initData)
		{
			base.Init (initData);

			CountryCityLink selectedCountry = initData as CountryCityLink;
			cities = selectedCountry.cityList;
			countryCode = selectedCountry.name;
			title = String.Format ("{0} cities", selectedCountry.fullName);

			CityLink.displayLinkCommandDelegate = DisplayLinkCommand;
			CityLink.selectCityPageModel = this;
		}


		private CityLink _ItemSelected;
		public CityLink listItemSelected {
			get {
				return _ItemSelected;
			}
			set {
				if (_ItemSelected != value) {
					_ItemSelected = value;
					string cityId = string.Format ("{0},{1}", _ItemSelected.name, countryCode);
					System.Diagnostics.Debug.WriteLine ("SelectCityPageModel.listItemSelected( {0} )", cityId);

					CoreMethods.PushPageModel<DisplayCurrentDetailsPageModel> (cityId);
					//LoadTabbedNav (_ItemSelected.name);
					_ItemSelected = null;						// so it unsets the selected item in the listview
				}
			}
		}


		private void LoadTabbedNav (string cityName)
		{
			var tabbedNavigation = new FreshTabbedNavigationContainer ();
			//tabbedNavigation.AddTab<DisplayCurrentPageModel> ("Current", null);
			tabbedNavigation.AddTab<DisplayForecastPageModel> ("Forecast", null, cityName);

			CurrentPage = tabbedNavigation;
		}

		private void FindItem(string searchFor) 
		{
			System.Diagnostics.Debug.WriteLine ("SelectCityPageModel.FindItem( {0} )", searchFor);
			foreach (CityLink c in cities) {
				if (c.name.StartsWith (searchFor, StringComparison.OrdinalIgnoreCase)) {
					MessagingCenter.Send(c, "CityFound" );
					break;
				}
			}
			System.Diagnostics.Debug.WriteLine ("SelectCityPageModel.FindItem()");
		}



		public void DisplayLinkCommand(string link)
		{
			CoreMethods.PushPageModel<DisplayWebPageModel> (link);
			//CoreMethods.PushPageModel<SelectCountryPageModel> ();
		}
	}
}

