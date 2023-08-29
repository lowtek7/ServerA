namespace PacketGenerator;

public struct PacketVariable
{
	public string Name { get; }

	public string Type { get; }

	public PacketVariable(string name, string type)
	{
		Name = name;
		Type = type;
	}
}

public enum PacketTransferType
{
	Server = 0,
	Client,
	Shared
}

public struct PacketType
{
	public string Name { get; }

	public PacketTransferType Type { get; }

	public PacketVariable[] Variables { get; }

	public string FullName
	{
		get
		{
			var result = string.Empty;

			switch (Type)
			{
				case PacketTransferType.Server:
					result += "S";
					break;
				case PacketTransferType.Client:
					result += "C";
					break;
				case PacketTransferType.Shared:
					break;
			}

			result += "CMD_";
			result += Name.ToUpper();

			return result;
		}
	}

	public PacketType(string name, PacketTransferType type, PacketVariable[] variables)
	{
		Name = name;
		Type = type;
		Variables = variables;
	}
}
