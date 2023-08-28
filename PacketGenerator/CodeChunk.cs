namespace PacketGenerator;

public class CodeChunk
{
	private List<EnumType> _enumTypes = new List<EnumType>();

	private List<PacketType> _packetTypes = new List<PacketType>();

	public List<EnumType> EnumTypes => _enumTypes;

	public List<PacketType> PacketTypes => _packetTypes;
}
