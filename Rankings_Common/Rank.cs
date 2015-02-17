using System;
using System.Collections.Generic;
using System.Json;

namespace Rankings_Common
{
	public class Rank
	{
		private static string defaultCategoryType = "handheld";

		public Rank (List<DateTime> dateList, JsonValue json)
		{
			Country = json ["country"];

			JsonValue category = json ["category"];
			CategoryName = defaultCategoryType.Equals (category ["device"]) 
				? (string)category ["name"] 
				: String.Format ("{0} ({1})", (string)category ["name"], (string)category ["device"]); 

			ProductId = json ["product_id"];

			JsonArray positions = (JsonArray)json ["positions"];
			Positions = new Dictionary<DateTime, int> ();
			for (int i = 0; i < Math.Min (dateList.Count, positions.Count); i++) {
				Positions.Add (dateList [i], positions [i] != null ? (int)positions [i] : 0);
			}
		}

		public string Country { get; set; }

		public string CategoryName { get; set; }

		public int ProductId { get; set; }

		public Dictionary<DateTime, int> Positions { get; set; }

		public string GetGategoryName() 
		{
			return String.Format ("{0}-{1}", Country, CategoryName);
		}
	}
}

