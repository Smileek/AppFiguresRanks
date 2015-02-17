using System;

namespace Rankings_Common
{
	public class ListItemData
	{
		public ListItemData (string title, string ranks, string iconUrl)
		{
			Title = title;
			RanksData = ranks;
			IconUrl = iconUrl;
		}

		public string Title { get; set; }

		public string RanksData { get; set; }

		public string IconUrl { get; set; }
	}
}

