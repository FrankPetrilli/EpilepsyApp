
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

namespace Epilepsy
{
	[Activity (Label = "Event List")]			
	public class EventList : Activity
	{
		private DataManager manager;
		private ArrayAdapter adapter;
		IList<SeizureEvent> list;
		ListView my_events;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.EventList);
			//manager = new DataManager ();
			manager = SharedObjects.manager;
			UpdateFromDB ();
			my_events = FindViewById<ListView> (Resource.Id.my_events);
			UpdateList ();

		}

		void UpdateList()
		{
			if (list.Count == 0) {
				IList<String> empty_list = new List<String> ();
				empty_list.Add ("No items");
				adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, empty_list);
			} else {
				// Only if we have items do we want a tap to do anything.
				adapter = new ArrayAdapter<SeizureEvent> (this, Android.Resource.Layout.SimpleListItem1, list);
				my_events.ItemClick += OnListItemClick;
				my_events.ItemLongClick += OnListItemLongClick;
			}
			// Regardless, let's connect the list to the adapter.
			my_events.Adapter = this.adapter;
			adapter.NotifyDataSetChanged ();
		}

		void UpdateFromDB() 
		{
			list = manager.GetEvents ();
		}
		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var view_activity = new Intent(this, typeof(SeizureEventView));
			SharedObjects.my_event = list [e.Position];
			StartActivity (view_activity);
		}

		void OnListItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Delete event?");
			builder.SetMessage ("Are you sure you want to delete this?");
			builder.SetPositiveButton("OK", delegate {
				// They confirmed...
				manager.RemoveEvent (list [e.Position]);
				UpdateFromDB ();
				UpdateList ();
			});
			builder.SetNegativeButton("Cancel", delegate { return; });
			var dialog = builder.Create();
			dialog.Show ();		
		}
	}
}

