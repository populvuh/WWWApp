using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WWWApp
{
	public partial class DisplayForecastPage : ContentPage
	{
		public DisplayForecastPage ()
		{
			InitializeComponent ();

            // there's a bug in Forms which puts a large blank space (20-30cms) above the listView on some devices 
            // (e.g. my G4, tho oddly enough, it works fine on all the emulators I've tried it on)
            // adding a blank Header solves it
            if (listView.Header == null)
                listView.Header = new Label { HeightRequest = 0 };
        }



        /*bool hasAppearedOnce = false;
        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasAppearedOnce) {

                hasAppearedOnce = true;
                //var padding = (NameGrid.Width - MessagesListView.Height) / 2;
                var padding =  (MessagesListView.Height) / 2;

                MessagesListView.HeightRequest = MessagesLayoutFrame.Width;
                MessagesLayoutFrameInner.WidthRequest = MessagesLayoutFrame.Width;
                MessagesLayoutFrameInner.Padding = new Thickness(0);
                MessagesLayoutFrame.Padding = new Thickness(0);
                MessagesLayoutFrame.IsClippedToBounds = true;
                Xamarin.Forms.AbsoluteLayout.SetLayoutBounds(MessagesLayoutFrameInner, new Rectangle(0, 0 - padding, AbsoluteLayout.AutoSize, MessagesListView.Height - padding));
                MessagesLayoutFrameInner.IsClippedToBounds = true;
                // * /
            } 
        }*/
    }
}

