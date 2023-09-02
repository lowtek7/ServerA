using System.Collections.Generic;

namespace World
{
	public class World
	{
		private readonly Dictionary<int, UserEntity> _entities = new Dictionary<int, UserEntity>();

		public bool TryCreateEntity(int netId)
		{
			if (_entities.ContainsKey(netId))
			{
				return false;
			}

			var entity = new UserEntity
			{
				NetId = netId
			};

			_entities.Add(netId, entity);
			return true;
		}

		public bool TryGetEntity(int netId, out UserEntity entity)
		{
			return _entities.TryGetValue(netId, out entity);
		}
	}
}
