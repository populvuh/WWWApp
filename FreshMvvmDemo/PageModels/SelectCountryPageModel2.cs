using System;

/*using System.Collections.Generic;
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
*/

namespace FreshMvvmDemo
{
	//[ImplementPropertyChanged]
	public class SelectCountryPageModel : FreshMvvm.FreshBasePageModel
	{
		public bool LoadedOK { get; set; }
		public string ErrorMessage { get; set; }
		//public List<string> CountryList { get; set; }
		public Countries countries { get; set; }

		//HashSet<string> Countries = new HashSet<string>();
		//IUserDialogs _userDialogs;

		public SelectCountryPageModel ()
		{
		}

		/*public SelectCountryPageModel (IUserDialogs userDialogs)
		{
			_userDialogs = userDialogs;
		}

		public override void Init (object initData)
		{
			base.Init (initData);

			//string city = initData as string;
			//_userDialogs.ShowLoading ();

			LoadCountries ();
			LoadCities ();

			//_userDialogs.HideLoading ();
		}*/

		/*void LoadCountries ()
		{
			System.Diagnostics.Debug.WriteLine("SelectCountryPageModel.LoadCountries()");

			LoadedOK = false;
			List<CountryCity> CountryCityList = new List<CountryCity>();

			try
			{
				var assembly = typeof(SelectCityPageModel).GetTypeInfo().Assembly;
				using (Stream stream = assembly.GetManifestResourceStream("FreshMvvmDemo.Resources.Countries.json"))
				using (StreamReader sr = new StreamReader(stream))
				{
					using (JsonReader reader = new JsonTextReader(sr))
					{
						JsonSerializer serializer = new JsonSerializer();
						countries = serializer.Deserialize<Countries>(reader);
					}
					//'reader' will be disposed by this point
					System.Diagnostics.Debug.WriteLine("Finished loading countries");
				}
				//'sr' will be disposed by this point
				//CountryList = Countries.OrderBy(s => s).ToList();						// sort

				LoadedOK = true;
				System.Diagnostics.Debug.WriteLine("LoadCountries OK");
			} 
			catch (Exception ex) 
			{	
				ErrorMessage = string.Format("LoadCountries() failed\n{0}\n{1}", ex.Message, CountryCityList.Last().city.name);
				System.Diagnostics.Debug.WriteLine (ErrorMessage);
				LoadedOK = false;
			}
			finally 
			{
				_userDialogs.HideLoading ();
			}
		}


		void LoadCities ()
		{
			System.Diagnostics.Debug.WriteLine("SelectCountryPageModel.LoadCities()");
		}


		void ExtractCountries()
		{
			System.Diagnostics.Debug.WriteLine("SelectCountryPageModel.ExtractCountries()");
			_userDialogs.ShowLoading ();

			LoadedOK = false;
			List<CountryCity> CountryCityList = new List<CountryCity>();

			try
			{
				var assembly = typeof(SelectCityPageModel).GetTypeInfo().Assembly;
				using (Stream stream = assembly.GetManifestResourceStream("FreshMvvmDemo.CountryCityList.json"))
				using (StreamReader sr = new StreamReader(stream))
				{
					string jsonText = sr.ReadToEnd();
					foreach(string result in jsonText.Split('\n'))
					{
						JObject jsonObj = JObject.Parse(result);
						CountryCity cc = jsonObj.ToObject<CountryCity>();
						CountryCityList.Add(cc);
						Countries.Add(cc.city.country);
					}
					System.Diagnostics.Debug.WriteLine("Finished loading data");
					//'reader' will be disposed by this point
				}
				//'sr' will be disposed by this point
				CountryList = Countries.OrderBy(s => s).ToList();						// sort

				LoadedOK = true;
				System.Diagnostics.Debug.WriteLine("ExtractCountries OK");
			} 
			catch (Exception ex) 
			{	
				ErrorMessage = string.Format("ExtractCountries() failed\n{0}\n{1}", ex.Message, CountryCityList.Last().city.name);
				System.Diagnostics.Debug.WriteLine (ErrorMessage);
				LoadedOK = false;
			}
			finally 
			{
				_userDialogs.HideLoading ();
			}
		}

		private string _ItemSelected;
		public string listItemSelected {
			get {
				return _ItemSelected;
			}
			set {
				if (_ItemSelected != value) {
					_ItemSelected = value;
					System.Diagnostics.Debug.WriteLine ("SelectCountryPageModel.listItemSelected( {0} )", _ItemSelected);
					//PropertyChanged (this, new PropertyChangedEventArgs ("ItemSelected"));
					//OnPropertyChanged ("ItemSelected");
					foreach (string cty in CountryList) {
						if (_ItemSelected == cty) {
							//CoreMethods.PushPageModel<SelectCityPageModel> ();
							break;
						}
					}
				}
			}
		}*/
	}
}


