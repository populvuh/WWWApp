using System;
using System.Windows;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

[assembly: Xamarin.Forms.Dependency (typeof(WWWApp.Droid.DeviceInfoProvider))]
namespace WWWApp.Droid
{
	public class DeviceInfoProvider : IDeviceInfoProvider
	{
		public bool IsPortait ()
		{
			return DeviceInfoManager.Width < DeviceInfoManager.Height;
		}

		public int GetWidth ()
		{
			return DeviceInfoManager.Width;
		}

		public int GetHeight ()
		{
			return DeviceInfoManager.Height;
		}
	}

	public static class DeviceInfoManager
	{
		//public static MainActivity MainActivity { 
		//	get; 
		//	set; 
		//}

		public static int Width { get { return MainActivity.Instance.GetWidth (); } }

		public static int Height { get { return MainActivity.Instance.GetHeight (); } }
	}
}

