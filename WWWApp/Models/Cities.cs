using System;
using System.Collections.Generic;
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
	public class Cities
	{
		public List<CountryList> countryList { get; set; }

		public class CountryList : IComparable<CountryList>
		{
			public string country { get; set; }
			public List<City> cities { get; set; }

			public int CompareTo(CountryList other)
			{
				if (this.country == other.country)
				{
					return 0;
				}
				return this.country.CompareTo(other.country);
			}	
		}

		public class City : IComparable<City>
		{
			public string city { get; set; }
			public string link { get; set; }
			public static SelectCityPageModel selectCityPageModel = null;
			public delegate void DisplayLinkCommandDelegate(string url);
			public static DisplayLinkCommandDelegate displayLinkCommandDelegate;

			public int CompareTo(City other)
			{
				// Alphabetic sort if equal
				if (this.city == other.city)
				{
					return 0;
				}
				return this.city.CompareTo(other.city);
			}	

			public Command DisplayLinkCommand 
			{
				get {
					return new Command<string> ((link) => {
						System.Diagnostics.Debug.WriteLine("DisplayLinkCommand {0}", link);
						displayLinkCommandDelegate( link );
					});
				}
			}
		}

	}
}

