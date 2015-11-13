namespace WWWApp
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;

	using ModernHttpClient;
	using Newtonsoft.Json;

	public class HttpWeather
	{
		private const string ApiBaseAddress = "http://api.openweathermap.org";
		string _apiKey = "c9154a4b313eb851e450f7c27d22c969";

		public async Task<CurrentWeather> GetWeather (string city)
		{
			//System.Diagnostics.Debug.WriteLine("HttpWeather.GetWeather ( {0} )", city);
			CurrentWeather currentWeather = null;
			string cityUrl = string.Format ("/data/2.5/weather?q={0}&APPID={1}&units=metric", city, _apiKey);

			using (var httpClient = CreateClient ()) {
				//System.Diagnostics.Debug.WriteLine("HttpWeather.GetWeather () - var response\n{0}{1}", httpClient.BaseAddress, cityUrl);

				var response = httpClient.GetAsync (cityUrl).Result;
				//System.Diagnostics.Debug.WriteLine("HttpWeather.GetWeather () - if (response.IsSuccessStatusCode) {0}, {1}", response.IsSuccessStatusCode, response.StatusCode);

				if (response.IsSuccessStatusCode) {
					//System.Diagnostics.Debug.WriteLine ("HttpWeather.GetWeather () - json = await ");
					var json = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
					//System.Diagnostics.Debug.WriteLine ("HttpWeather.GetWeather () - json = {0}", json);
					if (!string.IsNullOrWhiteSpace (json)) {
						//System.Diagnostics.Debug.WriteLine ("HttpWeather.GetWeather () - DeserializeObject");
						currentWeather = await Task.Run (() => 
							JsonConvert.DeserializeObject<CurrentWeather> (json)
						).ConfigureAwait (false);
					}
				} else {
					System.Diagnostics.Debug.WriteLine ("HttpWeather.GetWeather () - failed to get valid response\n{0}", httpClient.ToString());
				}
			}

			//System.Diagnostics.Debug.WriteLine("HttpWeather.GetWeather () Fin");
			return currentWeather;
		}

		private HttpClient CreateClient ()
		{
			//System.Diagnostics.Debug.WriteLine("HttpWeather.CreateClient ()");
			var httpClient = new HttpClient(new NativeMessageHandler()) 
			{ 
				BaseAddress = new Uri(ApiBaseAddress)
			};

			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			//System.Diagnostics.Debug.WriteLine("HttpWeather.CreateClient () Fin");
			return httpClient;
		}
	}
}
