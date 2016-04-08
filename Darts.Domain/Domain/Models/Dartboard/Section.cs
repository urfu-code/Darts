using System;
using Darts.Infrastructure;

namespace Darts.Domain.Models.Dartboard
{
	public class Section
	{
		public Section(int value, double startAngle, double sweepAngle,
			DartboardColor sectorColor, DartboardColor doubleColor)
		{
			Value = value;
			StartAngle = startAngle.AsSignedNormalizedAngle();
			SweepAngle = sweepAngle;
			EndAngle = (startAngle + sweepAngle).AsSignedNormalizedAngle();
			SectorColor = sectorColor;
			DoubleColor = doubleColor;
		}

		public bool Inside(double x, double y)
		{
			var a = Math.Atan2(y, x);
			return a.InAngle(StartAngle, EndAngle);
		}

		public int Value { get; set; }
		public double StartAngle { get; private set; }
		public double SweepAngle { get; set; }
		public double EndAngle { get; private set; }
		public DartboardColor SectorColor { get; private set; }
		public DartboardColor DoubleColor { get; private set; }
		public override string ToString()
		{
			return string.Format("{0} (from {1:0.0} to {2:0.0})", Value, StartAngle, EndAngle);
		}
	}
}