using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;

namespace WWWApp
{
	[JsonObject]
	[ImplementPropertyChanged]
	public class CountryCity : IEnumerable<Weather>
	{
		public City city { get; set; }
		public int time { get; set; }
		public Main main { get; set; }
		public Wind wind { get; set; }
		public Clouds clouds { get; set; }
		public List<Weather> weather { get; set; }

		public CountryCity ()
		{
		}

		public IEnumerator<Weather> GetEnumerator ()
		{
			return weather.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

	}

}

