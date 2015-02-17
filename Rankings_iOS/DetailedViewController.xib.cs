
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace Rankings_iOS
{
	public partial class DetailedViewController : UIViewController
	{
		public NSAttributedString Content {
			get { return this.content.AttributedText; }
			set { this.content.AttributedText = value; }
		}

		public DetailedViewController ()
		{
			Foundation.NSBundle.MainBundle.LoadNib ("DetailedViewController", this, null);
			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			content.SizeToFit ();
		}
	}
}

