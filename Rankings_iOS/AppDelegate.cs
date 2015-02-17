using System;
using System.Linq;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace Rankings_iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			var window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.MakeKeyAndVisible ();

			var navController = new UINavigationController();
			navController.NavigationBar.Translucent = false;

			HomeViewController homeController = new HomeViewController();
			navController.PushViewController(homeController, false);

			window.RootViewController = navController;

			return true;
		}
	}
}

