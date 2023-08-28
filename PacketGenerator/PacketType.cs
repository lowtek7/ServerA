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

	public PacketType(string name, PacketTransferType type, PacketVariable[] variables)
	{
		Name = name;
		Type = type;
		Variables = variables;
	}
}
