namespace PacketGenerator;

public struct EnumType
{
	public string Name { get; }

	public string[] Values { get; }

	public string TypeOption { get; }

	public EnumType(string name, string[] values, string typeOption)
	{
		Name = name;
		Values = values;
		TypeOption = typeOption;
	}

	public EnumType(string name, string[] values)
	{
		Name = name;
		Values = values;
		TypeOption = string.Empty;
	}
}
