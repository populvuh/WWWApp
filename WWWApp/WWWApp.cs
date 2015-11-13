namespace WWWApp
{
	using System;

	using Xamarin.Forms;

	using Acr.UserDialogs;
	using Refit;

	public class App : Application
	{
		public App ()
		{
			SetIOC ();

			var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<SelectCountryPageModel> ();
			var nav = new FreshMvvm.FreshNavigationContainer (page);

			MainPage = nav;
		}

		public void SetIOC()
		{
			var weatherApi = RestService.For<IWeatherApi>("http://api.openweathermap.org");
			FreshMvvm.FreshIOC.Container.Register<IWeatherApi> (weatherApi);

			var timezoneApi = RestService.For<ITimeZoneApi>("https://maps.googleapis.com/maps/api/timezone");
			FreshMvvm.FreshIOC.Container.Register<ITimeZoneApi> (timezoneApi);

			FreshMvvm.FreshIOC.Container.Register<IUserDialogs> (UserDialogs.Instance);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

