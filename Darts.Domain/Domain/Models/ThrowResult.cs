using System;
using System.Text.RegularExpressions;

namespace Darts.Domain.Models
{
	public class ThrowResult
	{
		public ThrowResult(int section, SectionArea sectionArea)
		{
			if (section < 0 || section > 20 && section != 25)
				throw new ArgumentException("Section should be in [0..20, 25] but was " + section);
			Section = section;
			SectionArea = sectionArea;
		}

		public readonly SectionArea SectionArea;
	
		public readonly int Section;

		public int Score
		{
			get { return Section * (int)SectionArea; }
		}

		#region convinient methods
		public static ThrowResult Triple(int section)
		{
			return new ThrowResult(section, SectionArea.Triple);
		}
		public static ThrowResult Double(int section)
		{
			return new ThrowResult(section, SectionArea.Double);
		}
		public static ThrowResult Single(int section)
		{
			return new ThrowResult(section, SectionArea.Single);
		}

		public static readonly ThrowResult Outside = Single(0);
		public static readonly ThrowResult OuterBull = Single(25);
		public static readonly ThrowResult InnerBull = Double(25);
		#endregion

		#region value sematics
		protected bool Equals(ThrowResult other)
		{
			return Section == other.Section && SectionArea == other.SectionArea;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((ThrowResult)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Section * 397) ^ (int)SectionArea;
			}
		}

		private static readonly string[] MultiplierNames = { "", "", "D", "T" };
		public override string ToString()
		{
			return string.Format("{0}{1}", MultiplierNames[(int)SectionArea], Section);
		}

		#endregion
	
		#region parsing
		public static bool IsValid(string representation)
		{
			return Regex.IsMatch(representation, @"^[DT]?([0-9]|1[0-9]|20|25)$", RegexOptions.IgnoreCase);
		}

		public static ThrowResult Parse(string representation)
		{
			var multiplier = 
				representation.StartsWith("D", StringComparison.InvariantCultureIgnoreCase) ? SectionArea.Double 
				: representation.StartsWith("T", StringComparison.InvariantCultureIgnoreCase) ? SectionArea.Triple 
				: SectionArea.Single;
			var sector = int.Parse(representation.TrimStart('D', 'T', 'd', 't'));
			return new ThrowResult(sector, multiplier);
		}
		#endregion


	}
}