using Foundation;
using System;
using UIKit;
using SQLite;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using SharedCode;
namespace LeapUser
{
    public partial class QuestionViewController : UIViewController
    {
        String chosenAnswer;

        public string dbPath
        {
            get;
            set;

        }
        public int session_count
        {
            get;
            set;
        }

        private int primary_key;
        private Session session;
        public QuestionViewController(IntPtr handle) : base(handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var plist = NSUserDefaults.StandardUserDefaults;
            primary_key = Convert.ToInt32(plist.StringForKey("primaryKey"));
            var db = new SQLiteConnection(dbPath);
            session = db.Table<Session>().Where(x => x.Id == primary_key).FirstOrDefault();

            titleSession.Title = session.Session_Name;

            buttonA.TouchUpInside += delegate
            {
                if (!buttonA.Selected)
                {
                    buttonA.Selected = true;
                    buttonB.Selected = false;
                    buttonC.Selected = false;
                    buttonD.Selected = false;
                    chosenAnswer = "Option A";
                    buttonA.SetImage(UIImage.FromFile("chk_selected_2x.png"), UIControlState.Normal);
                    buttonB.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonC.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonD.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);

                }
                else
                {
                    chosenAnswer = "";
                    buttonA.Selected = false;
                    buttonA.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                }
            };
            buttonB.TouchUpInside += delegate
            {
                if (!buttonB.Selected)
                {
                    buttonB.Selected = true;
                    buttonA.Selected = false;
                    buttonC.Selected = false;
                    buttonD.Selected = false;
                    chosenAnswer = "Option B";
                    buttonB.SetImage(UIImage.FromFile("chk_selected_2x.png"), UIControlState.Normal);
                    buttonA.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonC.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonD.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                }
                else
                {
                    chosenAnswer = "";
                    buttonB.Selected = false;
                    buttonB.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                }
            };

            buttonC.TouchUpInside += delegate
            {
                if (!buttonC.Selected)
                {
                    buttonC.Selected = true;
                    buttonA.Selected = false;
                    buttonB.Selected = false;
                    buttonD.Selected = false;
                    chosenAnswer = "Option C";
                    buttonC.SetImage(UIImage.FromFile("chk_selected_2x.png"), UIControlState.Normal);
                    buttonA.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonB.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonD.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                }
                else
                {
                    chosenAnswer = "";
                    buttonC.Selected = false;
                    buttonC.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                }
            };
            buttonD.TouchUpInside += delegate
            {
                if (!buttonD.Selected)
                {
                    buttonD.Selected = true;
                    buttonA.Selected = false;
                    buttonB.Selected = false;
                    buttonC.Selected = false;
                    chosenAnswer = "Option D";
                    buttonD.SetImage(UIImage.FromFile("chk_selected_2x.png"), UIControlState.Normal);
                    buttonA.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonB.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                    buttonC.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                }
                else
                {
                    chosenAnswer = "";
                    buttonD.Selected = false;
                    buttonD.SetImage(UIImage.FromFile("chk_notselected_2x.png"), UIControlState.Normal);
                }
            };




            //displaySessionName.Text = session.Session_Name;
            textQuestion.Text = session.Question;
            textOptionA.Text = session.OptionA;
            textOptionB.Text = session.OptionB;
            textOptionC.Text = session.OptionC;
            textOptionD.Text = session.OptionD;
            chosenAnswer = "";
        }

        public override void ViewDidAppear(bool animated)
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
        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.Identifier == "backOTP")
            {
                var controller = segue.DestinationViewController as OTPViewController;
                controller.session_count = session_count;
                controller.dbPath = dbPath;
            }
            if (segue.Identifier == "sessionComplete")
            {
                var controller = segue.DestinationViewController as SessionCompleteViewController;
            }
        }

        partial void ButtonSubmit_Activated(UIBarButtonItem sender)
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            primary_key = Convert.ToInt32(plist.StringForKey("primaryKey")) + 1;
            FirebaseHelper firebaseHelper = new FirebaseHelper();
            if (chosenAnswer != "")
            {
                if (primary_key <= session_count)
                {
                    try
                    {
                        var x = firebaseHelper.getAllSession();
                        if (chosenAnswer == session.CorrectAnswer)
                        {
                            SessionResponse response = new SessionResponse();
                            response.mobilenumber = plist.DoubleForKey("mobilenumber");
                            response.name = plist.StringForKey("name");
                            var result = firebaseHelper.postResponse(session.Session_Name, response);
                        }
                        plist.SetString("" + primary_key, "primaryKey");

                        var displayAlert = UIAlertController.Create("Answer Submitted", "Thank you.", UIAlertControllerStyle.Alert);
                        displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => PerformSegue("backOTP", null)));
                        PresentViewController(displayAlert, true, null);
                    }
                    catch (Exception)
                    {
                        var displayAlert = UIAlertController.Create("Connection Error", "Please check the Internet Connection and Try Again.", UIAlertControllerStyle.Alert);
                        displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
                        PresentViewController(displayAlert, true, null);
                    }

                }
                if (primary_key == session_count + 1)
                {
                    try
                    {
						var x = firebaseHelper.getAllSession();
                        if (chosenAnswer == session.CorrectAnswer)
                        {
                            
                            SessionResponse response = new SessionResponse();
                            response.mobilenumber = plist.DoubleForKey("mobilenumber");
                            response.name = plist.StringForKey("name");
                            var result = firebaseHelper.postResponse(session.Session_Name, response);
                        }
                        plist.SetString("true", "sessionsComplete");
                        plist.SetString("" + primary_key, "primaryKey");

                        var displayAlert = UIAlertController.Create("Answer Submitted", "Thank you.", UIAlertControllerStyle.Alert);
                        displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => PerformSegue("sessionComplete", null)));
                        PresentViewController(displayAlert, true, null);

                    }
                    catch (Exception)
                    {
                        var displayAlert = UIAlertController.Create("Connection Error", "Please check the Internet Connection and Try Again.", UIAlertControllerStyle.Alert);
                        displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
                        PresentViewController(displayAlert, true, null);
                    }

                }
                else
                {
                    var displayAlert = UIAlertController.Create("Submit Failed", "Please select an Option.", UIAlertControllerStyle.Alert);
                    displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
                    PresentViewController(displayAlert, true, null);
                }
            }
        }
    }
}