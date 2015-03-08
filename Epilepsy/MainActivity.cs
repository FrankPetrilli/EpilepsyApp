using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Runtime.Serialization;

namespace Epilepsy
{
	[Activity (Label = "Epilepsy", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private DataManager manager = SharedObjects.manager;

		protected override void OnCreate (Bundle bundle)
		{
			SharedObjects.manager = new DataManager ();
			manager = SharedObjects.manager;
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button list_button = FindViewById<Button> (Resource.Id.listButton);

			list_button.Click += delegate {
				StartActivity(typeof(EventList));
			};

			Button add_button = FindViewById<Button> (Resource.Id.addButton);

			add_button.Click += delegate {
				StartActivity(typeof(AddEvent));
			};

			Button symptom_button = FindViewById<Button> (Resource.Id.symptomButton);

			symptom_button.Click += delegate {
				StartActivity(typeof(SymptomManager));
			};
		}
	}
}


