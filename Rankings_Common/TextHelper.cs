using System;
using System.Collections.Generic;
using System.Linq;

namespace Rankings_Common
{
	public static class TextHelper
	{
		public static string GetRanksText (List<Rank> ranksList)
		{
			string result = String.Empty;
			foreach (var rank in ranksList) {
				KeyValuePair<DateTime, int> last = rank.Positions.ElementAt(rank.Positions.Count - 1);
				KeyValuePair<DateTime, int> penultimate = rank.Positions.ElementAt(rank.Positions.Count - 2);

				result += String.Format("{0}: {1}", rank.GetGategoryName(), GetPositionString(last.Value));
				if (penultimate.Value > 0 && last.Value > 0) {
					result += String.Format (" ({0})", GetDeltaText (last.Value - penultimate.Value));
				}
				result += "<br />";
			}

			return TrimLastTag(result);
		}
			
		public static string GetWeeklyRanks (List<Rank> ranksList)
		{
			string result = String.Empty;
			foreach (var rank in ranksList) {
				result += String.Format ("<b>{0}</b><br />", rank.GetGategoryName());
				for (int i = 0; i < rank.Positions.Count; i++) {
					KeyValuePair<DateTime, int> current = rank.Positions.ElementAt(i);

					result += String.Format ("<i>{0}</i>: &nbsp; {1}", current.Key.ToString("dd.MM"), GetPositionString(current.Value));
					if (i > 0 && current.Value > 0) {
						KeyValuePair<DateTime, int> previous = rank.Positions.ElementAt(i - 1);
						if (previous.Value > 0) {
							result += String.Format (" ({0})", GetDeltaText (current.Value - previous.Value));
						}
					}
					result += "<br />";
				}
				result += "<br />";
			}

			return TrimLastTag(result);
		}

		private static string TrimLastTag(string str) 
		{
			int lastTagIndex = str.LastIndexOf ('<');
			if (lastTagIndex > 0) {
				str = str.Substring (0, lastTagIndex);
			}

			return str;
		}

		private static string GetPositionString(int position) 
		{
			return position == 0 ? "-" : position.ToString();
		}

		private static string GetDeltaText(int delta) 
		{
			if (delta == 0) {
				return "-";
			}

			// lower rank means better
			if (delta < 0) {
				return String.Format ("<font color='#008000'>▲ {0}</font>", -delta);
			}

			return String.Format ("<font color='red'>▼ {0}</font>", delta);
		}
	}
}

