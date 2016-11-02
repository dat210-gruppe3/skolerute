using System;
using System.Collections.Generic;
using System.Linq;

using UserNotifications;
using Foundation;
using UIKit;

namespace skolerute.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
   //     //
   //     // This method is invoked when the application has loaded and is ready to run. In this 
   //     // method you should instantiate the window, load the UI into it and then make the window
   //     // visible.
   //     //
   //     // You have 17 seconds to return from this method, or iOS will terminate your application.
   //     //
   //     public override bool FinishedLaunching(UIApplication app, NSDictionary options)
   //     {
   //         global::Xamarin.Forms.Forms.Init();
   //         LoadApplication(new App());

   //         // Request notification permissions from user
   //         UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
   //         {
   //             // TODO: Handle approval
   //         });
			//return base.FinishedLaunching(app, options);
   //     }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			//reset notification badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new App());

			var notifacationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
			UIApplication.SharedApplication.RegisterUserNotificationSettings(notifacationSettings);

			return base.FinishedLaunching(application, launchOptions);
		}
    }
}