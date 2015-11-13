using System;

using Xamarin.Forms;

namespace WWWApp
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Threading.Tasks;
	using System.Linq;
	using System.Runtime.CompilerServices;

	using FreshMvvm;
	using Xamarin.Forms;
	using PropertyChanged;

	public class CityWeatherPage : CityPage
	{
		public string cityId	{ get; set; }
		protected IWeatherApi _weatherAPI;
		protected string _apiKey = "c9154a4b313eb851e450f7c27d22c969";

		public CityWeatherPage ()
		{
		}
	}
}


