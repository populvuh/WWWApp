namespace WWWApp
{
	using System;

	using Xamarin.Forms;
    using Plugin.Toasts;

    using Android.App;
	using Android.Content;
	using Android.Content.PM;
	using Android.Runtime;
	using Android.Views;
	using Android.Widget;
	using Android.OS;

	using Acr.UserDialogs;

	[Activity (Label = "WWWApp.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		static MainActivity _instance = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_instance = this;

			OxyPlot.Xamarin.Forms.Platform.Android.Forms.Init();
			global::Xamarin.Forms.Forms.Init (this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);
			UserDialogs.Init(this);

            DependencyService.Register<ToastNotificatorImplementation>();   // Register your dependency
            ToastNotificatorImplementation.Init(this);                      //you can pass additional parameters here

            LoadApplication (new App ());
		}

		public static MainActivity Instance
		{
			get { return _instance; }
		}

		public int GetWidth() 
		{
			Android.Graphics.Rect rect = new Android.Graphics.Rect();
			this.Window.DecorView.GetWindowVisibleDisplayFrame (rect);

			return (int)rect.Width();
		}

		public int GetHeight() 
		{
			Android.Graphics.Rect rect = new Android.Graphics.Rect();
			this.Window.DecorView.GetWindowVisibleDisplayFrame (rect);

			System.Diagnostics.Debug.WriteLine ("MainActivity.GetHeight = {0}, {1}", (int)rect.Height (), (int)Resources.DisplayMetrics.HeightPixels);

			return (int)rect.Height();
		}
	}
}

