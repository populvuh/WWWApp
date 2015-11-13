using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using PropertyChanged;
using Acr.UserDialogs;

namespace FreshMvvmDemo
{
	[ImplementPropertyChanged]
	public class ViewTempDetailsPageModel : FreshMvvm.FreshBasePageModel
	{
		public FreshMvvmDemo.List TempDetails { get; set; }

		public ViewTempDetailsPageModel ()
		{
			System.Diagnostics.Debug.WriteLine ("ViewTempDetailsPageModel.ctor()");
		}

		public override void Init (object initData)
		{
			base.Init (initData);

			TempDetails = initData as FreshMvvmDemo.List;

			System.Diagnostics.Debug.WriteLine ("ViewTempDetailsPageModel.Init( {0} )", TempDetails.dt_txt);
		}
	}
}

