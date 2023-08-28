namespace PacketGenerator;

public struct EnumType
{
	public string Name { get; }

	public string[] Values { get; }

	public EnumType(string name, string[] values)
	{
		Name = name;
		Values = values;
	}
}
