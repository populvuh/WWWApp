using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace WWWApp
{
	public class DisplayWebPageModel : FreshMvvm.FreshBasePageModel
	{
		//public bool IsLoading 	{ get; set; }
		public string url { get; set; }
		public bool IsLoading { get; set; }

		public DisplayWebPageModel ()
		{
		}

		public override void Init (object initData)
		{
			base.Init (initData);

			url = initData as string;
			IsLoading = true;
		}

		/*void webOnNavigating (object sender, WebNavigatingEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine ("DisplayWebPage.webOnNavigating ()");
			//LoadingLabel.IsVisible = true;
		}

		void webOnEndNavigating (object sender, WebNavigatedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine ("DisplayWebPage.webOnEndNavigating ()");
			//LoadingLabel.IsVisible = false;
		}*/
	}
}

