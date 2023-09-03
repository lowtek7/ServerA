using System;
using System.Collections.Generic;
using System.Linq;
using Vim.Math3d;
using World.Component;

namespace World
{
	public class UserEntity : IDisposable
	{
		private readonly List<IComponent> _components = new List<IComponent>();
		
		public int NetId { get; set; }

		public Vector3 Position { get; set; }

		public Quaternion Rotation { get; set; }

		public IComponent? GetComponent<T>() where T : IComponent
		{
			return _components.FirstOrDefault(x => x.GetType() == typeof(T)) ?? null;
		}

		public IEnumerable<IComponent> GetComponents<T>() where T : IComponent =>
			_components.Where(x => x.GetType() == typeof(T));

		public void AddComponent(IComponent component)
		{
			_components.Add(component);
		}

		public void AddComponent<T>(IComponent component) where T : IComponent, new()
		{
			_components.Add(new T());
		}

		public void Dispose()
		{
			_components.Clear();
		}
	}
}
