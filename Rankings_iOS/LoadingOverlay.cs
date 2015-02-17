using System;
using UIKit;
using System.Drawing;

namespace Rankings_iOS
{
	public class LoadingOverlay : UIView
	{
		UIActivityIndicatorView activitySpinner;
		UILabel loadingLabel;

		public LoadingOverlay (RectangleF frame) : base (frame)
		{
			BackgroundColor = UIColor.Black;
			Alpha = 0.75f;
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			float labelHeight = 22;
			float labelWidth = (float)Frame.Width - 20;

			float centerX = (float)Frame.Width / 2;
			float centerY = (float)Frame.Height / 2;

			activitySpinner = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new RectangleF (
				(float)(centerX - (activitySpinner.Frame.Width / 2)),
				(float)(centerY - activitySpinner.Frame.Height - 20),
				(float)activitySpinner.Frame.Width,
				(float)activitySpinner.Frame.Height);
			activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (activitySpinner);
			activitySpinner.StartAnimating ();

			loadingLabel = new UILabel (new RectangleF (
				centerX - (labelWidth / 2),
				centerY + 20,
				labelWidth,
				labelHeight
			));
			loadingLabel.BackgroundColor = UIColor.Clear;
			loadingLabel.TextColor = UIColor.White;
			loadingLabel.Text = "Loading...";
			loadingLabel.TextAlignment = UITextAlignment.Center;
			loadingLabel.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (loadingLabel);
		}

		public void Hide ()
		{
			UIView.Animate (
				0.5,
				() => {
					Alpha = 0;
				},
				() => {
					RemoveFromSuperview ();
				}
			);
		}
	};
}

