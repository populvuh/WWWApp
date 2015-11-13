using System;
using System.Threading.Tasks;
using Refit;
using System.Collections.Generic;

namespace WWWApp
{
	public interface ITimeZoneApi 
	{
		//https://maps.googleapis.com/maps/api/timezone/json?location=43.29712312,5.382137115&timestamp=1374868635&sensor=false
		[Get("/json?location={lat},{lng}&timestamp={timestamp}&sensor=false")]
		Task<Timezone> TimeZoneForCity(double lat, double lng, long timestamp);
	}
}
