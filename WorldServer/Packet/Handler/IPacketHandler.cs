using MemoryPack;
using Network.NetCommand;

namespace Network.Packet.Handler
{
	public interface IPacketHandler
	{
		INetCommand ToCommand(ReadOnlySpan<byte> bytes);
	}
}
