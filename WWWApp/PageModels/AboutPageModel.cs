using System;

using Xamarin.Forms;


namespace WWWApp
{
	public class AboutPageModel : FreshMvvm.FreshBasePageModel
	{

		public AboutPageModel ()
		{
		}

		public override void Init (object initData)
		{
			base.Init (initData);
		}

		public Command DisplayWeatherMapCommand
		{
			get {
				return new Command (() => {
					System.Diagnostics.Debug.WriteLine("Command DisplayFullDetails");
					CoreMethods.PushPageModel<DisplayWebPageModel> ("http://www.openweathermap.org");
				});
			}
		}

	}
}

