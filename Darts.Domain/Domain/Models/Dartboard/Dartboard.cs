using System;
using System.Collections.Generic;
using System.Linq;
using Darts.Infrastructure;

namespace Darts.Domain.Models.Dartboard
{
	public class Dartboard
	{
		private static readonly int[] sectorValues = { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };
		private readonly double sectorAngularWidth;

		public Dartboard(double scale = 1)
		{
			var sectorsCount = sectorValues.Length;
			sectorAngularWidth = Math.PI * 2 / sectorsCount;

			var sectorColors = new[] { DartboardColor.Dark, DartboardColor.Light };
			var doublesColors = new[] { DartboardColor.Red, DartboardColor.Green };
			Sectors = Enumerable.Range(0, sectorsCount)
				.Select(i => new Section(
					sectorValues[i],
					Math.PI / 2 - sectorAngularWidth / 2 - sectorAngularWidth * i,
					sectorAngularWidth,
					sectorColors[i % 2],
					doublesColors[i % 2]))
				.ToArray();
			DoubleOuterRadius = scale * 170;
			DoubleInnerRadius = DoubleOuterRadius - scale * 8;
			TripleOuterRadius = scale * 107;
			TripleInnerRadius = TripleOuterRadius - scale * 8;
			InnerBullRadius = scale * 12.7 / 2;
			OuterBullRadius = scale * 31.8 / 2;
			InnerBullColor = DartboardColor.Red;
			OuterBullColor = DartboardColor.Green;
		}

		public IReadOnlyList<Section> Sectors { get; private set; }
		public double InnerBullRadius { get; private set; }
		public double OuterBullRadius { get; private set; }
		public DartboardColor InnerBullColor { get; private set; }
		public DartboardColor OuterBullColor { get; private set; }
		public double TripleInnerRadius { get; private set; }
		public double TripleOuterRadius { get; private set; }
		public double DoubleInnerRadius { get; private set; }
		public double DoubleOuterRadius { get; private set; }

		public ThrowResult GetResult(double x, double y)
		{
			var r = Math.Sqrt(x * x + y * y);
			if (r <= InnerBullRadius) return ThrowResult.InnerBull;
			if (r <= OuterBullRadius) return ThrowResult.OuterBull;
			if (r > DoubleOuterRadius) return ThrowResult.Outside;

			var sector = GetSector(x, y);
			var multiplier = r.InRange(DoubleInnerRadius, DoubleOuterRadius)
				? SectionArea.Double
				: r.InRange(TripleInnerRadius, TripleOuterRadius)
					? SectionArea.Triple
					: SectionArea.Single;
			return new ThrowResult(sector.Value, multiplier);
		}

		public Section GetSector(double x, double y)
		{
			return Sectors.FirstOrDefault(s => s.Inside(x, y));
		}
	}
}