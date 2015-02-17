using System;
using UIKit;
using System.Collections.Generic;
using Rankings_Common;
using Foundation;
using System.Linq;

namespace Rankings_iOS
{
	public class RankingItemSource : UITableViewSource
	{
		List<ListItemData> items;
		UINavigationController navigationController;
		List<Rank> ranks;
		Dictionary<int, int> positionsToIds;

		string cellIdentifier = "TableCell";
		Dictionary<int, RankingCellController> cellControllers = new Dictionary<int, RankingCellController> ();

		public RankingItemSource (List<ListItemData> items, UINavigationController navigationController, 
		                          List<Rank> ranks, Dictionary<int, int> positionsToIds)
		{
			this.items = items;
			this.navigationController = navigationController;
			this.ranks = ranks;
			this.positionsToIds = positionsToIds;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return items.Count;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 90;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			ListItemData item = items [indexPath.Row];
			RankingCellController cellController = null;

			if (cell == null) { 
				cellController = new RankingCellController ();
				cell = cellController.Cell;
				cell.Tag = Environment.TickCount;
				cellControllers [(int)cell.Tag] = cellController;
			} else {
				cellController = cellControllers [(int)cell.Tag];
			}

			cellController.Heading = item.Title;

			var attr = new NSAttributedStringDocumentAttributes () { StringEncoding = NSStringEncoding.UTF8 };
			var nsError = new NSError ();
			attr.DocumentType = NSDocumentType.HTML;
			string htmlRanksData = item.RanksData;
			cellController.SubHeading.AttributedText = new NSAttributedString (htmlRanksData, attr, ref nsError);
			cellController.SubHeading.SizeToFit ();

			if (!string.IsNullOrEmpty (item.IconUrl)) {
				using (var url = new NSUrl (item.IconUrl)) {
					using (var data = NSData.FromUrl (url)) {
						cellController.Image = UIImage.LoadFromData (data);
					}
				}
			}

			return cell;				
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			int id = positionsToIds.First (x => x.Key == indexPath.Item).Value;

			DetailedViewController detailsViewController = new DetailedViewController ();
			var attr = new NSAttributedStringDocumentAttributes () { StringEncoding = NSStringEncoding.UTF8 };
			var nsError = new NSError ();
			attr.DocumentType = NSDocumentType.HTML;
			string htmlRanksData = TextHelper.GetWeeklyRanks (ranks.Where (x => x.ProductId == id).ToList ());
			detailsViewController.Content = new NSAttributedString (htmlRanksData, attr, ref nsError);

			navigationController.PushViewController (detailsViewController, true);
		}
	}
}

