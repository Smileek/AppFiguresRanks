using System;
using Android.Widget;
using Rankings_Common;
using Android.App;
using Android.Views;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using System.Net;
using Android.Graphics.Drawables;
using Android.Text;

namespace Rankings_Android
{
	public class RankingListAdapter : BaseAdapter<ListItemData>
	{
		List<ListItemData> items;
		Activity context;

		public RankingListAdapter (Activity context, List<ListItemData> items) : base ()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override ListItemData this [int position] {  
			get { return items [position]; }
		}

		public override int Count {
			get { return items.Count; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView ?? context.LayoutInflater.Inflate (Resource.Layout.RankingItem, null);

			view.FindViewById<TextView> (Resource.Id.title).Text = items [position].Title;
			view.FindViewById<TextView> (Resource.Id.ranks).TextFormatted = Html.FromHtml(items [position].RanksData);
			view.FindViewById<ImageView> (Resource.Id.icon).SetImageBitmap (GetImageBitmapFromUrl (items [position].IconUrl));

			return view;
		}

		private Bitmap GetImageBitmapFromUrl (string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient ()) {
				byte[] imageBytes = webClient.DownloadData (url);
				if (imageBytes != null && imageBytes.Length > 0) {
					imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}
	}
}

