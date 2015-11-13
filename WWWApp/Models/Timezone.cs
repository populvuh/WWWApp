using System;
using System.Collections.Generic;

using PropertyChanged;
using Xamarin.Forms;


namespace WWWApp
{
	[ImplementPropertyChanged]
	public class Timezone
	{
		public int dstOffset { get; set; }
		public int rawOffset { get; set; }
		public string status { get; set; }
		public string timeZoneId { get; set; }
		public string timeZoneName { get; set; }

		public Timezone ()
		{
		}
	}
}


