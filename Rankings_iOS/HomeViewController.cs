using System;
using System.Drawing;

using Foundation;
using UIKit;
using System.Collections.Generic;
using Rankings_Common;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace Rankings_iOS
{
	public partial class HomeViewController : UITableViewController
	{
		private List<Rank> ranks;
		private Dictionary<int, int> positionsToIds;
		private LoadingOverlay loadingOverlay;

		public HomeViewController () : base (UITableViewStyle.Plain)
		{
			Title = "MyRanks";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			GetData ();

			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Refresh, (s, e) => {
				NavigationItem.RightBarButtonItem.Enabled = false;

				RectangleF bounds = (RectangleF)UIScreen.MainScreen.Bounds;
				if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft
				    || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
					bounds.Size = new SizeF ((float)bounds.Size.Height, (float)bounds.Size.Width);
				}

				loadingOverlay = new LoadingOverlay (bounds);
				View.Add (this.loadingOverlay);

				Task dataTask = new Task (() => GetData ());
				dataTask.Start ();
			}), false);
		}

		private void GetData ()
		{
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

				InvokeOnMainThread (() => { 
					base.TableView.Source = new RankingItemSource (items, this.NavigationController, ranks, positionsToIds);
				});
			} catch (Exception ex) {
				InvokeOnMainThread (() => {
					DetailedViewController errorViewController = new DetailedViewController ();
					var attr = new NSAttributedStringDocumentAttributes () { StringEncoding = NSStringEncoding.UTF8 };
					var nsError = new NSError ();
					attr.DocumentType = NSDocumentType.HTML;
					string errorData = ex.ToString ();
					errorViewController.Content = new NSAttributedString (errorData, attr, ref nsError);

					NavigationController.PushViewController (errorViewController, false);
				});
			} finally {
				if (loadingOverlay != null) {
					InvokeOnMainThread (() => {
						loadingOverlay.Hide ();
						NavigationItem.RightBarButtonItem.Enabled = true;
					});
				}
			}
		}
	}
}

