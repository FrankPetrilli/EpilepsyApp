using System;
using SQLite;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
			connection.CreateTable<SeizureEvent> ();
		}

		public List<SeizureEvent> GetEvents()
		{
			return connection.Query<SeizureEvent> ("SELECT * FROM [SeizureEvent]"); 
		}

		public void AddEvent(SeizureEvent my_event)
		{
			connection.InsertOrReplace(my_event);
		}

		public void RemoveEvent(SeizureEvent my_event)
		{
			connection.Delete (my_event);
		}
	}
}

