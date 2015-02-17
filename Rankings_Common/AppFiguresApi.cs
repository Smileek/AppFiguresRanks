using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Runtime.Serialization.Formatters;
using Rankings_Common;

namespace Rankings_Common
{
	public static class AppFiguresApi
	{
		private static readonly Uri BaseUrl = new Uri ("https://api.appfigures.com/v2/");

		private static readonly string DateTimeFormat = "yyyy-MM-dd";
		private static readonly string Countries = "US;RU";
		private static readonly DateTime StartDate = DateTime.Today.AddDays (-6);

		private static JsonValue MakeRequest (String path)
		{
			Uri fullUri = new Uri (BaseUrl, path);
			WebRequest request = WebRequest.Create (fullUri);
			request.PreAuthenticate = true;
			request.Credentials = new NetworkCredential (Credentials.Username, Credentials.Password);
			request.Headers.Add ("X-Client-Key: " + Credentials.ClientKey);
			WebResponse response = request.GetResponse ();
			using (StreamReader streamReader = new StreamReader (response.GetResponseStream ())) {
				return JsonValue.Parse (streamReader.ReadToEnd ());
			}
		}

		public static List<Product> GetMyProducts ()
		{
			var list = new List<Product> ();

			JsonObject productsJson = (JsonObject) MakeRequest ("products/mine");

			foreach (string key in productsJson.Keys) {
				list.Add (new Product (productsJson [key]));
			}

			return list;
		}

		public static List<Rank> GetRanks (string ids)
		{
			var ranks = new List<Rank> ();

			List<DateTime> dateList = new List<DateTime> ();

			JsonValue ranksJson = MakeRequest (String.Format ("ranks/{0}/daily/{1}/{2}/?countries={3}", ids, 
				StartDate.ToString (DateTimeFormat), DateTime.Today.ToString (DateTimeFormat), Countries));
			JsonArray dates = (JsonArray) ranksJson ["dates"];
			for (int i = 0; i < dates.Count; i++) {
				dateList.Add (DateTime.Parse(dates[i]));
			}
			JsonArray data = (JsonArray) ranksJson ["data"];
			for (int i = 0; i < data.Count; i++) {
				ranks.Add (new Rank(dateList, data[i]));
			}

			return ranks;
		}
	}
}