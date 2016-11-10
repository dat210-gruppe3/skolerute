using UserNotifications;
using Foundation;
using UIKit;
using Xamarin;

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

            #if ENABLE_TEST_CLOUD
                //Requires Xamarin Test Cloud Agent
		        Xamarin.Calabash.Start();
            #endif

            return base.FinishedLaunching(application, launchOptions);
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			//TODO: show an alert
			/*UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
			okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

			Window.RootViewController.PresentViewController(okayAlertController, true, null);*/

			UIAlertView alert = new UIAlertView()
			{
				Title = notification.AlertTitle,
				Message = notification.AlertBody,
			};
			alert.AddButton("OK");
			alert.Show();
			// reset badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// reset badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}
    }
}