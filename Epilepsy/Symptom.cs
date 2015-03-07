using System;

namespace Epilepsy
{
	public class Symptom
	{
		public string short_name;
		public string description;
		public int intensity;

		public Symptom (string short_name, string description, int intensity)
		{
			this.short_name = short_name;
			this.description = description;
			this.intensity = intensity;
		}

		public override string ToString ()
		{
			return "[Symptom] " + short_name + ". Intensity: " + intensity;
		}
	}
}