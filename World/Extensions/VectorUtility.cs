using Vim.Math3d;

namespace World.Extensions
{
	public static class VectorUtility
	{
		public static Vector3 GetX0Z(this Vector3 value, float y = 0)
		{
			return new Vector3(value.X, y, value.Z);
		}

		public static bool IsAlmostZero(this Vector3 value)
		{
			return value.X.IsAlmostZero() && value.Y.IsAlmostZero() && value.Z.IsAlmostZero();
		}

		public static bool IsAlmostCloseTo(this Vector3 value, Vector3 target)
		{
			return value.X.IsAlmostCloseTo(target.X) && value.Y.IsAlmostCloseTo(target.Y) && value.Z.IsAlmostCloseTo(target.Z);
		}
	}
}
