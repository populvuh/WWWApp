using System;
using System.Windows.Input;


using Xamarin.Forms;

namespace WWWApp
{
	public interface ILocationViewModel
	{
		string Title { get; set; }
		string Description { get; }
		double Latitude { get; }
		double Longitude { get; }
		ICommand Command { get; }
	}
}

