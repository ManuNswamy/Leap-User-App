using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using SharedCode;
namespace LeapUser
{
	public class FirebaseHelper
	{

		private string FirebaseURL = "https://leapproject-b603d.firebaseio.com/";


		public async Task<List<Session>> getAllSession()
		{
			List<Session> sessionList = new List<Session>();
			try
			{
				var firebase = new FirebaseClient(FirebaseURL);
				var items = await firebase.Child("Session").OnceAsync<Session>();
				foreach (var item in items)
					sessionList.Add(item.Object);

				Console.WriteLine("SESSION LIST FIREBASE ACTIVITY : " + sessionList);
				return sessionList;
			}
			catch
			{
				return null;
			}

		}

		public async Task<bool> postResponse(string sessionName, SessionResponse sessionResponse)
		{
			try
			{
				var firebase = new FirebaseClient(FirebaseURL);
				var items = await firebase.Child(sessionName).Child("Score").PostAsync<SessionResponse>(sessionResponse);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

}
