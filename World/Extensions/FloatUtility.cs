using System;

namespace World.Extensions
{
	public static class FloatUtility
	{
		public static float Epsilon => 0.001f;

		public static bool IsAlmostZero(this float value)
		{
			return Math.Abs(value) <= Epsilon;
		}

		public static bool IsAlmostCloseTo(this float value, float target)
		{
			return Math.Abs(value - target) <= Epsilon;
		}
	}
}
