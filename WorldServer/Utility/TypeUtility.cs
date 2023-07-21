using System.Reflection;

namespace WorldServer.Utility;

public static class TypeUtility
{
	public static Type[] GetTypesWithInterface(Assembly assembly, Type interfaceType)
	{
		List<Type> results = new List<Type>();

		var types = assembly.GetTypes();
		foreach (var type in types)
		{
			if (interfaceType == type) continue;

			var interfaces = type.GetInterfaces();
			var resultIndex = Array.FindIndex(interfaces, t => t == interfaceType);
			if (resultIndex >= 0)
			{
				results.Add(type);
			}
		}

		return results.ToArray();
	}
}
