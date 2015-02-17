using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;
using Rankings_Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.Text;
using Android.Content.PM;

namespace Rankings_Android
{
	[Activity (Label = "@string/appTitle", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : ListActivity, IMenuItemOnMenuItemClickListener
	{
		private List<Rank> ranks;
		private Dictionary<int, int> positionsToIds;

		private ListView mainContainer;
		private ProgressBar progressBar;
		private TextView errorText;
		private IMenuItem refreshMenuItem;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			ThreadPool.QueueUserWorkItem (o => GetData ());
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.MainMenu, menu);       

			refreshMenuItem = menu.FindItem (Resource.Id.refresh);           
			refreshMenuItem.SetOnMenuItemClickListener (this);

			return true;
		}

		public bool OnMenuItemClick (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.refresh:
				{
					item.SetEnabled (false);
					ThreadPool.QueueUserWorkItem (o => GetData ());
					return true;
				}
			}
			return false;
		}

		private void GetData ()
		{
			mainContainer = FindViewById<ListView> (Android.Resource.Id.List);
			progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			errorText = FindViewById<TextView> (Resource.Id.errorText);

			RunOnUiThread (() => {
				mainContainer.Visibility = ViewStates.Gone;
				progressBar.Visibility = ViewStates.Visible;
			});

			try {
				List<Product> products = AppFiguresApi.GetMyProducts ();
				products.Sort ((x1, x2) => x1.Name.CompareTo (x2.Name));
				string ids = String.Join (";", products.Select (x => x.Id));
				ranks = AppFiguresApi.GetRanks (ids);

				positionsToIds = new Dictionary<int, int> ();
				List<ListItemData> items = new List<ListItemData> ();

				for (int i = 0; i < products.Count; i++) {
					Product product = products [i];
					items.Add (new ListItemData (String.Format ("{0} ({1})", product.Name, product.Store), 
						TextHelper.GetRanksText (ranks.Where (x => x.ProductId == product.Id).ToList ()), product.IconUrl));
					positionsToIds.Add (i, product.Id);
				}
				
				RunOnUiThread (() => { 
					ListAdapter = new RankingListAdapter (this, items);
					mainContainer.Visibility = ViewStates.Visible;
					progressBar.Visibility = ViewStates.Gone;
				});
			} catch (Exception ex) {
				RunOnUiThread (() => { 
					OnException (ex);
				});
			} finally {
				RunOnUiThread (() => { 
					refreshMenuItem.SetEnabled(true);
				});
			}
		}


		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			try {
				ShowDialogForId (positionsToIds.First (x => x.Key == position).Value);
			} catch (Exception ex) {
				OnException (ex);
			}
		}

		private void ShowDialogForId (int id)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetMessage (Html.FromHtml (TextHelper.GetWeeklyRanks (ranks.Where (x => x.ProductId == id).ToList ())));
			builder.SetCancelable (true);
			builder.SetPositiveButton ("OK", delegate {
			});
			builder.Show ();
		}

		private void OnException (Exception ex)
		{
			mainContainer.Visibility = ViewStates.Gone;
			progressBar.Visibility = ViewStates.Gone;

			errorText.Text = ex.ToString ();
			errorText.Visibility = ViewStates.Visible;
		}
	}
}


