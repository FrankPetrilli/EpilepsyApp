using System;
using SQLite;

namespace Epilepsy
{
	public class Symptom : IEquatable<Symptom>
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public string short_name { get; set; }
		public string description { get; set; }

		public Symptom()
		{
			this.id = new Random ().Next (Int32.MaxValue);
		}

		public Symptom (string short_name, string description)
		{
			this.id = new Random ().Next (Int32.MaxValue);
			this.short_name = short_name;
			this.description = description;
		}

		public override string ToString ()
		{
			//return short_name + ": " + description;
			return short_name;
		}

		public bool Equals (Symptom other)
		{
			return this.id == other.id;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}