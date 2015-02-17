
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Rankings_iOS
{
	public partial class RankingCellController : UIViewController
	{
		public UITableViewCell Cell {
			get { return this.celMain; }
		}

		public string Heading {
			get { return this.lblHeading.Text; }
			set { this.lblHeading.Text = value; }
		}

		public UILabel SubHeading {
			get { return this.lblSubHeading; }
			set { this.lblSubHeading = value; }
		}

		public UIImage Image {
			get { return this.imgMain.Image; }
			set { this.imgMain.Image = value; }
		}

		public RankingCellController ()
		{
			Foundation.NSBundle.MainBundle.LoadNib ("RankingCellController", this, null);
		}
	}
}

