using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using System.Threading;
using System.Diagnostics;
using Android.Content.PM;

namespace LeapUser
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,ScreenOrientation = ScreenOrientation.Portrait)]
    public class TestActivity : Activity
    {        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TakeTestLayout);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar1);          
            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);
            string dbPath = Intent.GetStringExtra("dbPath");
            int primaryKey = Convert.ToInt32(ap.getValue("primaryKey"));
            var db = new SQLiteConnection(dbPath);
            var session = db.Table<Session>().Where(x => x.Id == primaryKey).FirstOrDefault();
            toolbar.Title = session.Session_Name;
            TextView displayQuestion = FindViewById<TextView>(Resource.Id.displayQuestion);            
            CheckBox optionA = FindViewById<CheckBox>(Resource.Id.checkBoxOptionA);
            CheckBox optionB = FindViewById<CheckBox>(Resource.Id.checkBoxOptionB);
            CheckBox optionC = FindViewById<CheckBox>(Resource.Id.checkBoxOptionC);
            CheckBox optionD = FindViewById<CheckBox>(Resource.Id.checkBoxOptionD);

            string sessionName = Intent.GetStringExtra("sessionName");
            string correctAnswer = "";
            
            
            var sessionList = db.Table<Session>();

           
            displayQuestion.Text = session.Question;
            optionA.Text = session.OptionA;
            optionB.Text = session.OptionB;
            optionC.Text = session.OptionC;
            optionD.Text = session.OptionD;
            Button submit = FindViewById<Button>(Resource.Id.buttonSubmit);


            optionA.Click += delegate
            {
                optionA.Checked = true;
                optionB.Checked = false;
                optionC.Checked = false;
                optionD.Checked = false;
                correctAnswer = "Option A";

            };

            optionB.Click += delegate
            {
                optionB.Checked = true;
                optionA.Checked = false;
                optionC.Checked = false;
                optionD.Checked = false;
                correctAnswer = "Option B";

            };

            optionC.Click += delegate
            {
                optionC.Checked = true;
                optionA.Checked = false;
                optionB.Checked = false;
                optionD.Checked = false;
                correctAnswer = "Option C";

            };

            optionD.Click += delegate
            {
                optionD.Checked = true;
                optionA.Checked = false;
                optionB.Checked = false;
                optionC.Checked = false;
                correctAnswer = "Option D";

            };

            submit.Click += async delegate
            {
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();
                alert.SetTitle("Answer Submitted");
                alert.SetMessage("Thank You!");
                alert.SetIcon(Resource.Drawable.icon);

                Android.App.AlertDialog.Builder dialogError = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alertError = dialog.Create();
                alertError.SetTitle("Connection Error");
                alertError.SetMessage("Please check your Internet Connection and Try Again.");
                alertError.SetIcon(Resource.Drawable.icon);

                if (correctAnswer=="")
                {
                    Toast.MakeText(this, "You have to select one of the option", ToastLength.Short).Show();
                }
                else
                {
                    alert.Show();
                    FirebaseHelper firebaseHelper = new FirebaseHelper();
                    
                    if (correctAnswer == session.CorrectAnswer)
                    {
                        //submit response
                        try
                        {                         
                            SessionResponse response = new SessionResponse();
                            response.name = ap.getValue("name");
                            response.mobilenumber = Convert.ToDouble(ap.getValue("mobilenumber"));
                            var result = await firebaseHelper.postResponse(session.Session_Name, response);
                            primaryKey++;
                            ap.saveValue("primaryKey", "" + primaryKey);
                            var mainActivity = new Intent(this, typeof(MainActivity));
                            StartActivity(mainActivity);
                            //FinishAfterTransition();
                            this.Finish();
                        }
                        catch (Exception ex)
                        {
                            alert.Dismiss();
                            alertError.Show();
                        }
                    }
                    else
                    {     
                        try
                        {
                            var items = await firebaseHelper.getAllSession();//only waste the time and match of the correct response submission time
                            primaryKey++;
                            ap.saveValue("primaryKey", "" + primaryKey);
                            var mainActivity = new Intent(this, typeof(MainActivity));
                            StartActivity(mainActivity);
                            //FinishAfterTransition();                       
                            this.Finish();
                        }
                        catch(Exception)
                        {
                            alert.Dismiss();
                            alertError.Show();
                        }                       
                    }                   
                }              
                             

                //LayoutInflater layoutInflater = LayoutInflater.From(this);
                //View mView = layoutInflater.Inflate(Resource.Layout.RatingLayout, null);
                //RatingBar ratingBar = mView.FindViewById<RatingBar>(Resource.Id.ratingBar1);
                //Button buttonRating = mView.FindViewById<Button>(Resource.Id.buttonRating);
                //Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                //alertDialogBuilder.SetView(mView);
                //var alert = alertDialogBuilder.Show();
                //alertDialogBuilder.SetCancelable(false);
                //buttonRating.Click += async delegate
                //{
                //    try
                //    {
                //        var firebase = new FirebaseClient("https://leapproject-b603d.firebaseio.com/");
                //        int rating = Convert.ToInt32(ratingBar.Rating);
                //        var items = await firebase.Child(session.Session_Name).PostAsync<int>(rating);
                //        //Toast.MakeText(this, "your Score is " + ap.getValue("score"), ToastLength.Short).Show();

                //        if (ap.getValue("testAttempted") == "" + sessionList.Count())
                //        {

                //            Toast.MakeText(this, "WOWEQAW", ToastLength.Short).Show();


                //            //for (int i = 0; i < 10; i++)
                //            //{
                //            //    Random rand = new Random();
                //            //    SessionResponse sessionResponse = new SessionResponse();
                //            //    sessionResponse.name = ap.getValue("name");
                //            //    sessionResponse.mobilenumber = Convert.ToDouble(ap.getValue("mobilenumber"));
                //            //    //sessionResponse.score = Convert.ToInt32(ap.getValue("score"));
                //            //    sessionResponse.score = rand.Next(1, 9);
                //            //    var scoreItem = await firebase.Child("Score").Child("" + sessionResponse.score).PostAsync<SessionResponse>(sessionResponse);

                //            //}

                //        }

                //        alert.Dismiss();
                //        var mainActivity = new Intent(this, typeof(MainActivity));
                //        StartActivity(mainActivity);
                //    }
                //    catch
                //    {
                //        Console.WriteLine("Failed to update the database");
                //    }
                //};



            };          
        }
        public override void OnBackPressed()
        {
        }

      
    }
}