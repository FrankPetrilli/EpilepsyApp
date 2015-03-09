using System;
using SQLite;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;

namespace Epilepsy
{
	public class DataManager
	{
		public SQLiteConnection connection;
		public string my_db;

		public DataManager ()
		{
			string folder = System.Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			my_db = System.IO.Path.Combine (folder, "seizure.db");
			this.connection = new SQLiteConnection (my_db);
			this.Initialize();
		}

		public void Initialize()
		{
			// Store seizures
			connection.CreateTable<SeizureEvent> ();
			// Store available symptoms
			connection.CreateTable<Symptom> ();
			// Store observed symptoms (Ugh.)
			connection.CreateTable<SymptomOccurrence> ();

			// Initialize symptom table with new data if we don't have any.
			if (connection.Table<Symptom> ().Count () == 0) {
				connection.Insert (new Symptom ("Preserved conciousness", "Conciousness preserved through the event."));
				connection.Insert (new Symptom ("Sudden emotions", "Sudden and inexplicable feelings of fear, anger, sadness, happiness"));
				connection.Insert (new Symptom ("Nausea", "Nausea, rising feeling"));
				connection.Insert (new Symptom ("Falling Feeling", "Sensations of falling or movement"));
				connection.Insert (new Symptom ("Unusual feelings", "Experiencing of unusual feelings or sensations"));
				connection.Insert (new Symptom ("Altered senses", "Altered sense of hearing, smelling, tasting, seeing, and tactile perception (sensory illusions or hallucinations)"));
				connection.Insert (new Symptom ("Derealization or depersonalization", "Feeling as though the environment is not real (derealization) or dissociation from the environment or self (depersonalization)"));
				connection.Insert (new Symptom ("Spatial distortion", "A sense of spatial distortion—things close by may appear to be at a distance. (Palinopsia)"));
				connection.Insert (new Symptom ("Déjà or jamais vu", "Odd sense of familiarity or unfamiliarity"));
				connection.Insert (new Symptom ("Speech issues", "Laboured speech or inability to speak at all"));
				connection.Insert (new Symptom ("Staring", "Staring off into the distance uncontrollably."));
				connection.Insert (new Symptom ("Lip smacking", "Uncontrollable or involuntary lip smacking"));
				connection.Insert (new Symptom ("Repeated swallowing or chewing", "Uncontrollable feeling to swallow or chew."));
				connection.Insert (new Symptom ("Yawning", "Repeated, frequent occurences of yawning, for long durations (2-10 minutes)"));
				connection.Insert (new Symptom ("Finger movements", "Unusual finger movements, such as picking motions"));
				connection.Insert (new Symptom ("Muscle control issues", "Hands feeling disconnected, movement is labored"));
				connection.Insert (new Symptom ("Small motor skills", "Small motor skills jittery/choppy"));
				connection.Insert (new Symptom ("Jacksonian March", "Traveling feelings of jittering or electrical activity, feet may feel cramped."));
			}
		}

		// SeizureEvents
		public List<SeizureEvent> GetEvents()
		{
			//return connection.Query<SeizureEvent> ("SELECT * FROM [SeizureEvent]"); 
			return GetEvents (0);
		}

		public List<SeizureEvent> GetEvents(int ordering)
		{
			// 0 is date.
			switch (ordering) {
			case 0:
				return connection.Query<SeizureEvent> ("SELECT * FROM [SeizureEvent] ORDER BY date DESC"); 
				/*var query = connection.Table<SeizureEvent> ().OrderByDescending (v => v.date);
				List<SeizureEvent> result = new List<SeizureEvent> ();
				foreach (SeizureEvent seizure in query) {
					result.Add(seizure);
				}
				return result;*/
			default:
				return null;
			}
		}

		public int GetID(SeizureEvent my_event)
		{
			var table = connection.Table<SeizureEvent>();
			var table_event = connection.Table<SeizureEvent> ().Skip ((connection.Table<SeizureEvent> ().Count () - 1));
			return table_event.Take(0).First().id;

			//return connection.Table<SeizureEvent>().Where(v => v.Equals(my_event)).FirstOrDefault().id;
			//return 0;
		}

		public void AddEvent(SeizureEvent my_event)
		{
			connection.InsertOrReplace(my_event);
		}

		public void RemoveEvent(SeizureEvent my_event)
		{
			connection.Delete (my_event);
		}

		// Symptoms

		public List<Symptom> GetSymptoms()
		{
			return connection.Query<Symptom> ("SELECT * FROM [Symptom]");
		}

		public void AddSymptom(Symptom my_symptom)
		{
			connection.InsertOrReplace (my_symptom);
		}

		public void RemoveSymptom(Symptom my_symptom)
		{
			connection.Delete (my_symptom);
		}

		// Symptom Occurences

		public List<Symptom> GetSymptomOccurences(SeizureEvent my_event)
		{
			int event_id = my_event.id;
			System.Diagnostics.Debug.WriteLine ("Looking for id: " + my_event.id);
			var query = connection.Table<SymptomOccurrence>().Where (v => v.my_event_id == event_id);
			var my_list = new List<Symptom> ();
			foreach (var my_symptom_occurrence in query) {
				var interior_query = connection.Table<Symptom> ().Where (v => v.id == my_symptom_occurrence.symptom_id);
				foreach (var symptom in interior_query) {
					my_list.Add (symptom);
				}
			}
			return my_list;
		}

		public void AddSymptomOccurrence(SymptomOccurrence my_symptom_occurrence)
		{
			connection.InsertOrReplace (my_symptom_occurrence);
		}

		public void RemoveSymptomOccurrence(Symptom my_symptom_occurrence)
		{
			connection.Delete (my_symptom_occurrence);
		}
	}
}

