using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Foundation;
using SQLite;
using UIKit;
using SharedCode;
namespace LeapUser
{
    public partial class ViewController : UIViewController
    {

        static int session_count = 0;
		public string dbPath;
        static int flag = 0;

		protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            var plist = NSUserDefaults.StandardUserDefaults;
            //path to the local database
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			dbPath = Path.Combine(documents, "SessionDB.db");
            //plist.SetString("","name");
            //plist.SetDouble(0,"mobilenumber");

        }

        //directly route to the OTP screen if the shared preference is already there
		public override async void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
            UIGraphics.BeginImageContext(View.Frame.Size);
            UIImage.FromBundle("750x1334.png").Draw(View.Bounds);
            var bgImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            var uiImageView = new UIImageView(View.Frame);
            uiImageView.Image = bgImage;
            View.AddSubview(uiImageView);
            View.SendSubviewToBack(uiImageView);
			var plist = NSUserDefaults.StandardUserDefaults;
			Console.WriteLine("" + plist.StringForKey("name"));
			Console.WriteLine("" + plist.DoubleForKey("mobilenumber"));
			
            //for testing
            //plist.SetString("", "name");
			//plist.SetDouble(0, "mobilenumber");
			//plist.SetString("", "primaryKey");
			//plist.SetString("false", "sessionsComplete");

			if (plist.StringForKey("sessionsComplete") == "true")
			{
				PerformSegue("jumpSessionsComplete", null);
			}
            else
            {
				if (session_count < 1)
				{
					while (session_count < 1)
					{
						await fetchData();
					}
				}
				if (plist.StringForKey("name") != "" && plist.DoubleForKey("mobilenumber") > 0)
				{
					Console.WriteLine("Hello");
					try
                    {
                        PerformSegue("registerSuccessful", null);
                    }
                    catch (Exception ex) {
					    Console.WriteLine(ex);
					}
				}
            }
			
           
			
		}
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
            if(segue.Identifier=="registerSuccessful")
            {
				var controller = segue.DestinationViewController as OTPViewController;
				controller.session_count = session_count;
                controller.dbPath = dbPath;

			}
            if(segue.Identifier=="jumpSessionsComplete")
            {
                var controller = segue.DestinationViewController as SessionCompleteViewController;
            }
			
		}
       
        public override bool ShouldAutorotate()
        {
            return false;
        }
        partial void ButtonRegister_Activated(UIBarButtonItem sender)
        {
            var plist = NSUserDefaults.StandardUserDefaults;
          
            try
            {
                //validation
                if (textName.Text.Length >= 3 && textMobileNumber.Text.Length == 10 && Convert.ToDouble(textMobileNumber.Text) < 10000000000 && flag == 2)
				{
					string name = textName.Text;
					double mobileNumber = Convert.ToDouble(textMobileNumber.Text);
					plist.SetString(name, "name");
					plist.SetDouble(mobileNumber, "mobilenumber");
                    plist.SetString("1", "primaryKey");
                    plist.SetString("false","sessionsComplete");
                    PerformSegue("registerSuccessful", null);

				}
				else
				{
                    if (flag != 2)
                    {
						var displayAlert = UIAlertController.Create("Connection Error", "Please Check the Internet Connection and Try Again.", UIAlertControllerStyle.Alert);
						displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
						PresentViewController(displayAlert, true, null);
						PerformSegue("test", null);
                    }
                    else
                    {
						var displayAlert = UIAlertController.Create("Invalid Sign Up", "Please enter a valid Name and Mobile Number", UIAlertControllerStyle.Alert);
						displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
						PresentViewController(displayAlert, true, null);
						PerformSegue("test", null);
                    }
					

				}
            }
            //caught if the mobile number contains a alpha character or text
            catch(Exception ex)
            {
				var displayAlert = UIAlertController.Create("Invalid Sign Up", "Please enter a valid Name and Mobile Number", UIAlertControllerStyle.Alert);
				displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
				PresentViewController(displayAlert, true, null);
            }


        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		private async Task fetchData()
		{
            
			var db = new SQLiteConnection(dbPath);
            try
            {
				db.DropTable<Session>();
				db.CreateTable<Session>();
                Console.WriteLine("Table created!");
            }
            catch (Exception ex)
            {
                db.CreateTable<Session>();
            }

			try
			{
                FirebaseHelper firebaseHelper = new FirebaseHelper();
                List<Session> sessionList = await firebaseHelper.getAllSession();
				Console.WriteLine("" + sessionList);
				foreach (var item in sessionList)
				{
					db.Insert(item);
				}
				session_count = sessionList.Count;
                flag = 2;
			}
			catch(Exception)
			{
                if (flag == 0)
                {
                    var displayAlert = UIAlertController.Create("Connection Error", "Please Check the Internet Connection and Try Again.", UIAlertControllerStyle.Alert);
                    displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
                    PresentViewController(displayAlert, true, null);
                    flag = 1;
                }
			}

		}

	

	}
}
