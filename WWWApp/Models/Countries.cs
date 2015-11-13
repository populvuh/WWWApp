using System;
using System.Collections;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using PropertyChanged;

namespace WWWApp
{
	public class Countries
	{
		public List<Countrycode> countrycodes { get; set; }

		public Countries ()
		{
		}
	}

	public class Countrycode : FreshMvvm.FreshBasePageModel, IComparable<Countrycode>
	{
		public string name { get; set; }
		public string code { get; set; }
		public string image { get; set; }

		public ImageSource imageSource { get; set; }
		public delegate void DisplayLinkCommandDelegate (string url);
		public static DisplayLinkCommandDelegate displayLinkCommandDelegate;


		public int CompareTo (Countrycode other)
		{
			if (this.name == other.name) {
				return 0;
			}
			// sort low 2 high
			return this.name.CompareTo (other.name);
		}


		public Command DisplayLinkCommand {
			get {
				return new Command<string> ((link) => {
					System.Diagnostics.Debug.WriteLine ("DisplayLinkCommand {0}", link);
					displayLinkCommandDelegate (link);
				});
			}
		}
	}
}

