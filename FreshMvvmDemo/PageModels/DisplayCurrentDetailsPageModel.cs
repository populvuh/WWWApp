using System;

using FreshMvvm;
using Xamarin.Forms;

namespace FreshMvvmDemo
{
	public class DisplayDetailsPageModel : FreshMvvm.FreshBasePageModel
	{

		public DisplayDetailsPageModel ()
		{
			System.Diagnostics.Debug.WriteLine ("DisplayDetailsPageModel.ctor ()" );
		}


		public override void Init (object initData)
		{
			System.Diagnostics.Debug.WriteLine ("DisplayDetailsPageModel.Init ()" );
			base.Init (initData);

			string cityName = initData as string;

			LoadTabbedNav (cityName);

			System.Diagnostics.Debug.WriteLine ("DisplayDetailsPageModel.Init () - Fin" );
		}

		public void LoadTabbedNav (string cityName)
		{
			TabbedPage tabbedPage = new TabbedPage {
				Title = "TabbedPage"
			};
			tabbedPage.Children.Add (new ContentPage {
				Title = "Blue",
				Content = new BoxView {
					Color = Color.Blue,
					HeightRequest = 100f,
					VerticalOptions = LayoutOptions.Center
				},
			});
			tabbedPage.Children.Add (new ContentPage {
				Title = "Blue and Red",
				Content = new StackLayout {
					Children = {
						new BoxView { Color = Color.Blue },
						new BoxView { Color = Color.Red }
					}
				}
			});

			//var tabbedNavigation = new FreshTabbedNavigationContainer ();
			//tabbedNavigation.AddTab<DisplayCurrentPageModel> ("Current", null);
			//tabbedNavigation.AddTab<DisplayGraphPageModel> ("Forecast", null, cityName);
			CurrentPage = tabbedPage;
		}
	}
}


