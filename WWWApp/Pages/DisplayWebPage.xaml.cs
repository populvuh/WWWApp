using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WWWApp
{
	public partial class DisplayWebPage : ContentPage
	{
		public DisplayWebPage ()
		{
			InitializeComponent ();
		}

		void webOnNavigating (object sender, WebNavigatingEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine ("DisplayWebPage.webOnNavigating ()");
			LoadingLabel.IsVisible = true;
		}

		void webOnEndNavigating (object sender, WebNavigatedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine ("DisplayWebPage.webOnEndNavigating ()");
			LoadingLabel.IsVisible = false;
		}
	}
}

