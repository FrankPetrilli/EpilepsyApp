
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
	[Activity (Label = "Symptom Manager")]			
	public class SymptomManager : Activity
	{
		private DataManager manager;
		private ArrayAdapter adapter;
		private IList<Symptom> list;
		private ListView my_symptoms;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.SymptomManager);
			manager = SharedObjects.manager;
			EditText short_name = FindViewById<EditText> (Resource.Id.shortName);
			AutoCompleteTextView description = FindViewById<AutoCompleteTextView> (Resource.Id.symptomDescription);
			Button add_button = FindViewById<Button> (Resource.Id.symptomManagerAddSymptom);
			my_symptoms = FindViewById<ListView> (Resource.Id.listView1);

			UpdateFromDB ();
			UpdateList ();
			// Check if empty


			add_button.Click += delegate {
				var new_symptom = new Symptom();
				new_symptom.description = description.Text;
				new_symptom.short_name = short_name.Text;
				manager.AddSymptom(new_symptom);
				UpdateFromDB();
				UpdateList();
				short_name.Text = "";
				description.Text = "";
			};
		}

		void UpdateFromDB() 
		{
			list = manager.GetSymptoms ();
		}

		void UpdateList()
		{
			if (list.Count == 0) {
				IList<String> empty_list = new List<String> ();
				empty_list.Add ("No items");
				adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, empty_list);
			} else {
				// Only if we have items do we want a tap to do anything.
				adapter = new ArrayAdapter<Symptom> (this, Android.Resource.Layout.SimpleListItem1, list);
				my_symptoms.ItemClick += OnListItemClick;
				my_symptoms.ItemLongClick += OnListItemLongClick;
			}
			// Regardless, let's connect the list to the adapter.
			my_symptoms.Adapter = this.adapter;
			adapter.NotifyDataSetChanged ();
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Symptom detail: " + list[e.Position].short_name);
			builder.SetMessage (list[e.Position].description);
			builder.SetPositiveButton("OK", delegate {
				return;
			});
			var dialog = builder.Create();
			dialog.Show ();
		}

		void OnListItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Delete symptom?");
			builder.SetMessage ("Are you sure you want to delete this? Deleting a symptom will make it no longer show up in the list of symptoms for an event.");
			builder.SetPositiveButton("OK", delegate {
				// They confirmed...
				manager.RemoveSymptom (list [e.Position]);
				UpdateFromDB ();
				UpdateList ();
			});
			builder.SetNegativeButton("Cancel", delegate { return; });
			var dialog = builder.Create();
			dialog.Show ();
		}
	}
}

