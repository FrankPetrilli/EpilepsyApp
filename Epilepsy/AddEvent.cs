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
using System.Collections;

namespace Epilepsy
{
	[Activity (Label = "Add Event")]			
	public class AddEvent : Activity
	{
		private const int DATE_DIALOG_ID = 0;
		private const int TIME_DIALOG_ID = 1;

		private EditText date_text;
		private EditText time_text;

		private DateTime date;
		private DateTime time;

		private DataManager manager;

		private IList<Symptom> my_symptoms;

		private ArrayAdapter available_adapter;
		private ListView available_symptoms;

		protected override void OnCreate (Bundle bundle)
		{
			//manager = new DataManager ();
			manager = SharedObjects.manager;



			// DEBUG
			foreach (Symptom s in manager.GetSymptoms()) {
				System.Diagnostics.Debug.WriteLine (s);
			}




			// Hide keyboard:
			Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);
			date = DateTime.Today;
			time = DateTime.Now;
			SetContentView (Resource.Layout.AddEvent);
			base.OnCreate (bundle);
			Button add_button = FindViewById<Button> (Resource.Id.event_add);
			//TextView intensity_text = FindViewById<TextView> (Resource.Id.textView1);
			TextView intensity_number = FindViewById<TextView> (Resource.Id.intensityNumber);
			SeekBar intensity_bar = FindViewById<SeekBar> (Resource.Id.seekBar1);

			EditText description_box = FindViewById<EditText> (Resource.Id.descriptionBox);
			EditText location_box = FindViewById<EditText> (Resource.Id.locationText);

			// Date & Time text
			date_text = FindViewById<EditText> (Resource.Id.dateText);
			UpdateDateDisplay ();
			time_text = FindViewById<EditText> (Resource.Id.timeText);
			UpdateTimeDisplay ();

			// Date & Time buttons
			Button pick_date = FindViewById<Button> (Resource.Id.pickDateButton);
			Button pick_time = FindViewById<Button> (Resource.Id.pickTimeButton);

			// Check boxes:

			CheckBox immediate_trigger = FindViewById<CheckBox> (Resource.Id.checkBox1);
			CheckBox woke_up_fuzzy = FindViewById<CheckBox> (Resource.Id.checkBox2);
			CheckBox aura_felt = FindViewById<CheckBox> (Resource.Id.checkBox3);
			CheckBox menstruation = FindViewById<CheckBox> (Resource.Id.checkBox4);
			CheckBox meds_taken = FindViewById<CheckBox> (Resource.Id.checkBox5);

			// Symptom list
			my_symptoms = new List<Symptom> ();

			available_symptoms = FindViewById<ListView> (Resource.Id.availableSymptomsAdd);
			ListView noted_symptoms = FindViewById<ListView> (Resource.Id.notedSymptoms);

			ArrayAdapter noted_adapter = new ArrayAdapter<Symptom> (this, Android.Resource.Layout.SimpleListItem1, my_symptoms);
			noted_symptoms.Adapter = noted_adapter;

			available_adapter = new ArrayAdapter<Symptom> (this, Android.Resource.Layout.SimpleListItem1, (IList<Symptom>)manager.GetSymptoms ());
			available_symptoms.Adapter = available_adapter;

			available_symptoms.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				my_symptoms.Add(manager.GetSymptoms()[e.Position]);
				noted_adapter = new ArrayAdapter<Symptom> (this, Android.Resource.Layout.SimpleListItem1, my_symptoms);
				noted_symptoms.Adapter = noted_adapter;
				noted_adapter.NotifyDataSetChanged();
			};

			noted_symptoms.ItemLongClick += delegate(object sender, AdapterView.ItemLongClickEventArgs e) {
				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle ("Delete observed symptom?");
				builder.SetMessage ("Are you sure you want to delete this?");
				builder.SetPositiveButton("OK", delegate {
					// They confirmed...
					my_symptoms.Remove(my_symptoms[e.Position]);
					noted_adapter = new ArrayAdapter<Symptom> (this, Android.Resource.Layout.SimpleListItem1, my_symptoms);
					noted_symptoms.Adapter = noted_adapter;
					noted_adapter.NotifyDataSetChanged();
				});
				builder.SetNegativeButton("Cancel", delegate { return; });
				var dialog = builder.Create();
				dialog.Show ();
			};



			pick_date.Click += delegate {
				ShowDialog (DATE_DIALOG_ID);
			};

			pick_time.Click += delegate {
				ShowDialog (TIME_DIALOG_ID);
			};
				
			// Update text when progress bar changes
			intensity_bar.ProgressChanged += delegate {
				intensity_number.Text = Convert.ToString(intensity_bar.Progress);
			};

			// Add the event when we click add
			add_button.Click += delegate {
				SeizureEvent new_event = new SeizureEvent();
				DateTime event_date = this.date.AddHours(time.Hour);
				event_date = event_date.AddMinutes(time.Minute);

				new_event.intensity = intensity_bar.Progress;
				new_event.description = description_box.Text;
				new_event.date = event_date;
				new_event.location = location_box.Text;


				new_event.immediate_trigger = immediate_trigger.Checked;
				new_event.woke_up_fuzzy = woke_up_fuzzy.Checked;
				new_event.aura_felt = aura_felt.Checked;
				new_event.menstruation = menstruation.Checked;
				new_event.meds_taken = meds_taken.Checked;

				manager.AddEvent(new_event);

				//int row = manager.GetID(new_event); This would get the id from the DB.
				foreach (Symptom symptom in my_symptoms)
				{
					manager.AddSymptomOccurrence(new SymptomOccurrence(symptom.id, new_event.id));
				}
				Toast.MakeText(this, "Event added!", ToastLength.Long);
				Finish(); // Close this out.
			};
		}
		/*
		protected override void OnResume()
		{
			base.OnResume ();
			List<Symptom> available_symptom_list = manager.GetSymptoms ();
			available_adapter = new ArrayAdapter<Symptom> (this, Android.Resource.Layout.SimpleListItem1, available_symptom_list);
			available_symptoms.Adapter = available_adapter;
			available_adapter.NotifyDataSetChanged ();
		}
		*/
		void OnDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			this.date = e.Date;
			UpdateDateDisplay ();
		}

		void OnTimeSet (object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			DateTime new_time = new DateTime ();
			new_time = new_time.AddHours(e.HourOfDay);
			new_time = new_time.AddMinutes(e.Minute);
			this.time = new_time;
			UpdateTimeDisplay ();
		}
			
		void UpdateDateDisplay()
		{
			date_text.Text = date.ToString("D");
		}

		void UpdateTimeDisplay()
		{
			time_text.Text = time.ToString ("t");
		}

		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) {
			case DATE_DIALOG_ID:
				return new DatePickerDialog (this, OnDateSet, date.Year, date.Month - 1, date.Day); 
			case TIME_DIALOG_ID:
				return new TimePickerDialog (this, OnTimeSet, time.Hour, time.Minute, false);
			}
			return null;
		}
	}
}

