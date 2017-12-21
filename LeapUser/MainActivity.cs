using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System.Collections.Generic;
using System.IO;
using SQLite;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Android.Content.PM;

namespace LeapUser
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private string dbPath;
        static int session_count = 0;
        static int session_new_count = 0;
        Context mContext;
        AppPreferences ap;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Set your main view here
            SetContentView(Resource.Layout.main);
            dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "SessionDB.db");
            mContext = Android.App.Application.Context;
            ap = new AppPreferences(mContext);          
            if(session_count<1)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View mView = layoutInflater.Inflate(Resource.Layout.ProgressLayout, null);
                Android.Support.V7.App.AlertDialog.Builder alertProgress = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertProgress.SetView(mView).SetCancelable(false);
                var alert = alertProgress.Show();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                while (session_count < 1)
                {
                    stopWatch.Stop();
                    if (stopWatch.ElapsedMilliseconds >= 10000)
                    {
                        mView.FindViewById<TextView>(Resource.Id.displayLoading).Text = "Please Check the Internet Connection";
                    }
                    else
                    {
                        stopWatch.Start();
                    }

                    await fetchData();
                }
                alert.Dismiss();
            }               
                       

            if (ap.getValue("name") == "" && ap.getValue("mobilenumber") == "")
            {                
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View mView = layoutInflater.Inflate(Resource.Layout.user_pref, null);
                Button buttonRegitser = mView.FindViewById<Button>(Resource.Id.buttonRegister);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialogBuilder.SetView(mView);
                alertDialogBuilder.SetCancelable(false);
                var alert = alertDialogBuilder.Show();
                buttonRegitser.Click += async delegate
                {
                   
                    var username = mView.FindViewById<TextView>(Resource.Id.editUsername).Text;
                    var mobilenumber = mView.FindViewById<TextView>(Resource.Id.editMobileNumber).Text;
                    try
                    {                       
                        if (username.Length >= 3 && mobilenumber.Length == 10 && Convert.ToDouble(mobilenumber) < 10000000000)
                        {                           
                            ap.saveValue("name", username);
                            ap.saveValue("mobilenumber", mobilenumber);
                            ap.saveValue("primaryKey", "1");                          
                            int index = Convert.ToInt32(ap.getValue("primaryKey"));
                            alert.Dismiss();
                            displayOTP(index);
                            
                        }
                        else
                        {
                            Toast.MakeText(this, "Please enter a valid Name and MobileNumber.", ToastLength.Short).Show();
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Exception from MainActitvity in the Inputs " + ex);
                        Toast.MakeText(this, "Please enter a valid Name and MobileNumber", ToastLength.Short).Show();
                    }
                  
                };

            }
            else
            {
                int index = Convert.ToInt32(ap.getValue("primaryKey"));
                Console.WriteLine("TEST ATTEMPTED :" + index);
                if (index <=session_count)
                {
                    displayOTP(index);
                }
                else
                {
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert1 = dialog.Create();
                    alert1.SetTitle("Sessions Complete");
                    alert1.SetMessage("Thank You!");
                    alert1.SetIcon(Resource.Drawable.icon);
                    alert1.SetCancelable(false);
                    alert1.SetButton("EXIT", (c, ev) =>
                    {
                        MoveTaskToBack(true);
                        this.Finish();
                        this.FinishAffinity();
                    });
                    alert1.Show();
                }
            }

        }  

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

      
        private void displayOTP(int primaryKey)
        {
            var db = new SQLiteConnection(dbPath);
            Session session = new Session();
            session = db.Table<Session>().Where(x => x.Id==primaryKey).FirstOrDefault();

            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View mView = layoutInflater.Inflate(Resource.Layout.OTPLayout, null);
            TextView textSessionName = mView.FindViewById<TextView>(Resource.Id.textSessionName);
            Button buttonTakeTest = mView.FindViewById<Button>(Resource.Id.buttonStartTest);
            textSessionName.Text = session.Session_Name;
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
            alertDialogBuilder.SetView(mView);
            alertDialogBuilder.SetCancelable(false);
            var alert = alertDialogBuilder.Show();
            buttonTakeTest.Click += delegate
            {
                var OTP = mView.FindViewById<TextView>(Resource.Id.editOTP).Text;              
                if (session.OTP.ToString() == OTP)
                {

                    Toast.MakeText(this, "All the Best !", ToastLength.Short).Show();                    
                    var testActivity = new Intent(this, typeof(TestActivity));
                    //testActivity.PutExtra("sessionName", name);
                    testActivity.PutExtra("dbPath", dbPath);

                    alert.Dismiss();
                    this.StartActivity(testActivity);
                }
                else
                {
                    Toast.MakeText(this, "SORRY WRONG OTP", ToastLength.Short).Show();
                }
            };
        }

        private async Task fetchData()
        {
            FirebaseHelper firebaseHelper = new FirebaseHelper();

            var db = new SQLiteConnection(dbPath);     
            //db.DropTable<Session>();
            try
            {
                db.DropTable<Session>();
                db.CreateTable<Session>();                
            }
            catch(Exception)
            {
                db.CreateTable<Session>();                
            }
            try
            {
                //var firebase = new FirebaseClient("https://leapproject-b603d.firebaseio.com/");              
                //var items = await firebase.Child("Session").OnceAsync<Session>();               
                List<Session> sessionList = new List<Session>();
                sessionList = await firebaseHelper.getAllSession();
                foreach (var item in sessionList)
                {
                    db.Insert(item);
                }
                session_count = db.Table<Session>().Count();                
            }
            catch (Exception ex)
            {   
                Console.WriteLine("Please Check your internet connection"+ex);
            }
           
            

        }
    }
}

