using System;
using System.Collections.Generic;
using SQLite;
using Android.OS;
using Java.Interop;

namespace Epilepsy
{
	public class SeizureEvent
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }

		public int intensity { get; set; }
		public DateTime date { get; set; }
		public string location { get; set; }
		public int duration { get; set; } // In minutes
		public string description { get; set; }

		public bool immediate_trigger { get; set; }
		public bool woke_up_fuzzy { get; set; }
		public bool aura_felt { get; set; }
		public bool menstruation { get; set; }
		public bool meds_taken { get; set; }
		//public List<Symptom> symptoms { get; set; }

		public SeizureEvent()
		{
			this.id = new Random ().Next (Int32.MaxValue);
		}

		public SeizureEvent(string description)
		{
			this.id = new Random ().Next (Int32.MaxValue);
			this.location = null;
			this.description = description;
		}

		public SeizureEvent (DateTime date, string location, int duration, int intensity, string description, bool immediate_trigger, bool woke_up_fuzzy, bool aura_felt, bool menstruation, bool meds_taken)
		{
			this.id = new Random ().Next (Int32.MaxValue);
			this.date = date;
			this.location = location;
			this.duration = duration;
			this.intensity = intensity;
			this.description = description;

			this.immediate_trigger = immediate_trigger;
			this.woke_up_fuzzy = woke_up_fuzzy;
			this.aura_felt = aura_felt;
			this.menstruation = menstruation;
			this.meds_taken = meds_taken;
		}

		public override string ToString ()
		{
			if (location == null) {
				return description;
			}
			return date.ToString("g") + " @ " + location + " : " + description;
		}
	}
}