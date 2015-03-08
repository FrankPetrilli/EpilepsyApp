using System;
using SQLite;

namespace Epilepsy
{
	public class SymptomOccurrence
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public int symptom_id { get; set; }
		public int my_event_id { get; set; }

		public SymptomOccurrence()
		{
			this.id = new Random ().Next (Int32.MaxValue);
		}

		public SymptomOccurrence (int symptom_id, int my_event_id)
		{
			this.id = new Random ().Next (Int32.MaxValue);
			this.symptom_id = symptom_id;
			this.my_event_id = my_event_id;
		}
	}
}

