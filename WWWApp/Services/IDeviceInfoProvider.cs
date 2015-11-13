using System;

namespace WWWApp
{
	public interface IDeviceInfoProvider
	{
		bool IsPortait ();
		int GetWidth ();
		int GetHeight ();
	}
}


