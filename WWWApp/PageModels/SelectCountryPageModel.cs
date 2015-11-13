using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Windows.Input;

using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Acr.UserDialogs;


namespace WWWApp
{
	public class SelectCountryPageModel : FreshMvvm.FreshBasePageModel
	{

		public bool LoadedOK { get; set; }
		public string ErrorMessage { get; set; }
		//public List<string> CountryList { get; set; }

		public Countries countries { get; set; }
		public List<Countrycode> countrycodes { get; set; }
		public Cities citiesList { get; set; }
		Dictionary<string, List<Cities.City>> countriesCitiesMap = new Dictionary<string, List<Cities.City>>();

		IUserDialogs _userDialogs;

		public SelectCountryPageModel (IUserDialogs userDialogs)
		{
			_userDialogs = userDialogs;
			Countrycode.displayLinkCommandDelegate = DisplayLinkCommand;

			MessagingCenter.Subscribe<string> (this, "CountrySearch", (countryPrefix) => {
				System.Diagnostics.Debug.WriteLine("SelectCountryPage.MessagingCenter () - CountrySearch recieved - {0}", countryPrefix);
				FindItem(countryPrefix);
			});
		}

		public override void Init (object initData)
		{
			base.Init (initData);

			//string city = initData as string;
			_userDialogs.ShowLoading ();

			LoadCountries ();
			//LoadCities ();

			_userDialogs.HideLoading ();
		}


		// Methods are automatically wired up to page
		protected override void ViewIsAppearing (object sender, System.EventArgs e)
		{
			base.ViewIsAppearing (sender, e);
		}

		protected override void ViewIsDisappearing (object sender, System.EventArgs e)
		{
			base.ViewIsDisappearing (sender, e);
		}

		void LoadCountries ()
		{
			System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.LoadCountries()");

			LoadedOK = false;
			//ImageSource imageSource = ImageSource.FromResource("FreshMvvmDemo.Resources.Afghanistan.jpg");
			List<CountryCity> CountryCityList = new List<CountryCity> ();

			try {
				var assembly = typeof(SelectCityPageModel).GetTypeInfo ().Assembly;
				using (Stream stream = assembly.GetManifestResourceStream ("WWWApp.Resources.Countries2.json"))
				using (StreamReader sr = new StreamReader (stream)) {
					using (JsonReader reader = new JsonTextReader (sr)) {
						JsonSerializer serializer = new JsonSerializer ();
						countries = serializer.Deserialize<Countries> (reader);
					}
					//'reader' will be disposed by this point
					System.Diagnostics.Debug.WriteLine ("Finished loading countries");
				}
				//'sr' will be disposed by this point
				countrycodes = countries.countrycodes;
				countrycodes.Sort();

				//foreach (Countries.Countrycode cc in countries.countrycodes) {
				foreach (Countrycode cc in countrycodes) {
					string imageName = String.Format("WWWApp.Resources.Flags.{0}", cc.image );
					cc.imageSource = ImageSource.FromResource(imageName);
					System.Diagnostics.Debug.WriteLine ("Entry {0}, {1}", cc.name, cc.image);
				}
				//CountryList = .ToList();						// sort

				LoadedOK = true;
				System.Diagnostics.Debug.WriteLine ("LoadCountries OK");

			} catch (Exception ex) {	
				ErrorMessage = string.Format ("LoadCountries() failed\n{0}", ex.Message);
				System.Diagnostics.Debug.WriteLine (ErrorMessage);
				LoadedOK = false;
			} finally {
				_userDialogs.HideLoading ();
			}
		}


		void LoadCities ()
		{
			System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.LoadCities()");

			try {
				var assembly = typeof(SelectCityPageModel).GetTypeInfo ().Assembly;
				using (Stream stream = assembly.GetManifestResourceStream ("WWWApp.Resources.Cities.json"))
				using (StreamReader sr = new StreamReader (stream)) {
					using (JsonReader reader = new JsonTextReader (sr)) {
						JsonSerializer serializer = new JsonSerializer ();
						citiesList = serializer.Deserialize<Cities> (reader);
					}
					//'reader' will be disposed by this point
					System.Diagnostics.Debug.WriteLine ("Finished loading countries");
				}
				//'sr' will be disposed by this point
				List<Cities.City> listCities = new List<Cities.City>();
				//citiesList.countryList.Sort();
				foreach (Cities.CountryList country in citiesList.countryList) {
					System.Diagnostics.Debug.WriteLine ("Entry {0} ", country.country);
					country.cities.Sort();
					countriesCitiesMap.Add(country.country, country.cities);
					/*if ( countriesCitiesMap.TryGetValue(country.country, out listCities)) {
						foreach (Cities.City c in listCities) {
							System.Diagnostics.Debug.WriteLine ("\tcity {0}, {1} ", c.city, c.link);
						}
					}*/
				}
				//CountryList = .ToList();						// sort

				LoadedOK = true;
				System.Diagnostics.Debug.WriteLine ("LoadCities OK");

			} catch (Exception ex) {	
				ErrorMessage = string.Format ("LoadCities() failed\n{0}\n{1}", ex.Message, citiesList.countryList.Last ().country);
				System.Diagnostics.Debug.WriteLine (ErrorMessage);
				LoadedOK = false;
			} 
		}

		private Countrycode _ItemSelected;
		public Countrycode listItemSelected {
			get {
				return _ItemSelected;
			}
			set {
				if (_ItemSelected != value) {
					_ItemSelected = value;
					System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.listItemSelected( {0} )", _ItemSelected.code);

					ListCountryCityLink listCountryCityLink;
					List<Cities.City> listCities = new List<Cities.City>();

					var assembly = typeof(SelectCityPageModel).GetTypeInfo ().Assembly;
					string resourceName = String.Format ("WWWApp.Resources.Cities.{0}.json", _ItemSelected.code.ToCharArray ().ElementAt (0));
					using (Stream stream = assembly.GetManifestResourceStream (resourceName))
					using (StreamReader sr = new StreamReader (stream)) {
						using (JsonReader reader = new JsonTextReader (sr)) {
							JsonSerializer serializer = new JsonSerializer ();
							listCountryCityLink = serializer.Deserialize<ListCountryCityLink> (reader);
						}
						//'reader' will be disposed by this point
						System.Diagnostics.Debug.WriteLine ("Finished loading countries");
					}
					//'sr' will be disposed by this point

					CountryCityLink selectedCountry = null;
					foreach (CountryCityLink ccl in listCountryCityLink.countryCityList) {
						//System.Diagnostics.Debug.WriteLine ("Entry {0} ", ccl.name);
						if (ccl.name == _ItemSelected.code) {
							ccl.fullName = _ItemSelected.name;
							selectedCountry = ccl;
							break;
						}

					}

					_ItemSelected = null;						// so it unsets the selected item in the listview

					if ( null != selectedCountry ) {
						CoreMethods.PushPageModel<SelectCityPageModel> (selectedCountry);
					} else {
						CoreMethods.DisplayAlert ("Error Message", "No cities found for country", "Ok");
					}
				}
			}
		}

		private void FindItem(string searchFor) 
		{
			System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.FindItem( {0} )", searchFor);
			foreach (Countrycode cc in countrycodes) {
				if (cc.name.StartsWith (searchFor, StringComparison.OrdinalIgnoreCase)) {
					MessagingCenter.Send(cc, "CountryFound" );
					break;
				}
			}
			System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.FindItem() - Not Found");
		}



		public Command DisplayAboutCommand
		{
			get {
				return new Command (() => {
					System.Diagnostics.Debug.WriteLine("Command DisplayAboutCommand");
					CoreMethods.PushPageModel<AboutPageModel> ();
				});
			}
		}

		public void DisplayLinkCommand(string name)
		{
			System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.DisplayLinkCommand( {0} )", name);
			string link = String.Format ("https://en.wikipedia.org/wiki/{0}", name.Replace(' ', '_'));
			System.Diagnostics.Debug.WriteLine ("loading {0}", link);
			CoreMethods.PushPageModel<DisplayWebPageModel> (link);
		}

		public Command SearchCommand
		{
			get {
				return new Command (() => {
					System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.SearchCommand()");
				});
			}
		}	
	}
}



