using System;
using System.Collections.Generic;
using System.Linq;
using Vim.Math3d;
using World.Component;

namespace World
{
	public class UserEntity : IDisposable
	{
		public int NetId { get; set; }

		public Vector3 Position { get; set; }

		public Quaternion Rotation { get; set; }

		private readonly Dictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();

		public T GetOrCreateComponent<T>() where T : IComponent, new()
		{
			if (_components.TryGetValue(typeof(T), out var result))
			{
				return (T) result;
			}

			result = new T();
			_components.Add(typeof(T), result);

			return (T) result;
		}

		public void AddComponent(IComponent component)
		{
			_components.TryAdd(component.GetType(), component);
		}

		public void AddComponent<T>(IComponent component) where T : IComponent, new()
		{
			_components.Add(typeof(T), new T());
		}

		public void Dispose()
		{
			_components.Clear();
		}
	}
}
