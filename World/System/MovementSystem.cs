using System;
using Vim.Math3d;

namespace World.System
{
	public class MovementSystem
	{
		/// <summary>
		/// 해당 엔티티가 도착할 예측 움직임
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="velocity"></param>
		/// <param name="senderPing"></param>
		/// <param name="targetPing"></param>
		/// <returns></returns>
		public static Vector3 MoveSimulate(UserEntity entity, Vector3 velocity, int senderPing, int targetPing)
		{
			if (velocity == Vector3.Zero)
			{
				return entity.Position;
			}

			var startPos = entity.Position;
			var speed = Convert.ToSingle(velocity.Magnitude());
			var dir = velocity.Normalize();

			// 보내는 이의 핑과 받는 이의 핑을 더하면 이동 도착 지점 시간이 예측이 가능하다.
			var time = (senderPing * 0.001f) + (targetPing * 0.001f);

			return startPos + (dir * (speed * time));
		}
	}
}
