﻿
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
	[Activity (Label = "Event View")]			
	public class SeizureEventView : Activity
	{
		private SeizureEvent my_event;
		private DataManager manager;

		private EditText date_text;
		private EditText time_text;

		private DateTime date;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.SeizureEventView);
			// Hide keyboard:
			Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);
			// UI objects:
			TextView intensity_number = FindViewById<TextView> (Resource.Id.intensityNumber);
			SeekBar intensity_bar = FindViewById<SeekBar> (Resource.Id.seekBar1);

			EditText description_box = FindViewById<EditText> (Resource.Id.descriptionBox);
			EditText location_box = FindViewById<EditText> (Resource.Id.locationText);

			// Date & Time text
			date_text = FindViewById<EditText> (Resource.Id.dateText);
			time_text = FindViewById<EditText> (Resource.Id.timeText);
			// Check boxes
			CheckBox immediate_trigger = FindViewById<CheckBox> (Resource.Id.checkBox1);
			CheckBox woke_up_fuzzy = FindViewById<CheckBox> (Resource.Id.checkBox2);
			CheckBox aura_felt = FindViewById<CheckBox> (Resource.Id.checkBox3);
			CheckBox menstruation = FindViewById<CheckBox> (Resource.Id.checkBox4);
			CheckBox meds_taken = FindViewById<CheckBox> (Resource.Id.checkBox5);
			// Start loading our object.
			my_event = SharedObjects.my_event;
			// Get the database
			manager = SharedObjects.manager;
			// Date & Time
			date = my_event.date;
			date_text.Text = date.ToString ("D");
			time_text.Text = date.ToString ("t");
			// Intensity
			int intensity = my_event.intensity;
			intensity_bar.Progress = intensity;
			intensity_number.Text = intensity.ToString ();
			// Description
			description_box.Text = my_event.description;
			// Location
			location_box.Text = my_event.location;

			immediate_trigger.Checked = my_event.immediate_trigger;
			woke_up_fuzzy.Checked = my_event.woke_up_fuzzy;
			aura_felt.Checked = my_event.aura_felt;
			menstruation.Checked = my_event.menstruation;
			meds_taken.Checked = my_event.meds_taken;

			// Symptom list
			LinearLayout available_symptoms = FindViewById<LinearLayout> (Resource.Id.availableSymptomsView);
			List<Symptom> observed_symptom_list = manager.GetSymptomOccurences (my_event);
			System.Diagnostics.Debug.WriteLine ("Observed symptom list is: " + observed_symptom_list.Count);
			List<Symptom> symptom_list = manager.GetSymptoms ();

			ArrayAdapter available_adapter = new ArrayAdapter<Symptom> (this, Android.Resource.Layout.SimpleListItemMultipleChoice, (IList<Symptom>)symptom_list);
			Dictionary<int, bool> checked_map = new Dictionary<int, bool> ();
			for (int i = 0; i < available_adapter.Count; i++) {
				// Fix issue with i getting updated. -.-
				int index = i;
				checked_map.Add (index, false); // Add the empty checkbox into the map.
				CheckBox new_box = new CheckBox (ApplicationContext); // Make a new checkbox.
				new_box.Text = symptom_list [index].ToString (); // Set its text.
				if (observed_symptom_list.Contains (symptom_list [index])) {
					new_box.Checked = true; // Set it checked if they marked it as observed.
					System.Diagnostics.Debug.WriteLine ("Setting " + symptom_list [index].ToString () + " to true.");
				}
				new_box.Click += delegate(object sender, EventArgs e) { // Set up what happens when it's checked.
					bool previous = checked_map[index]; // Get previous entry
					checked_map.Remove(index); // Kill it.
					checked_map.Add (index, !previous); // Add back with it reversed.
				};
				available_symptoms.AddView (new_box);
			}
			Toast.MakeText(this, "Event loaded", ToastLength.Long);
		}
	}
}

