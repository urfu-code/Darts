using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Darts.Infrastructure
{
	public static class NumbersExtensions
	{
		///<summary>All angles should be in -PI..PI range</summary>
		public static bool InAngle(this double angle, double startAngle, double endAngle)
		{
			if (endAngle >= startAngle)
				return startAngle <= angle && angle <= endAngle;
			return InAngle(angle, startAngle, Math.PI) || InAngle(angle, -Math.PI, endAngle);
		}
		public static double AsSignedNormalizedAngle(this double angle)
		{
			angle = angle%(2*Math.PI);
			if (angle > Math.PI) angle -= 2*Math.PI;
			if (angle <= -Math.PI) angle += 2*Math.PI;
			return angle;
		}

		public static bool InRange(this double x, double low, double hi)
		{
			return low <= x && x <= hi;
		}
	}
}
