using MemoryPack;
using Network.NetCommand;
using Network.Packet;
using Network.Packet.Handler;
using WorldServer.NetCommand.Shared.Entity;

namespace WorldServer.Packet.Handler.Shared
{
	[PacketOpcode(Opcode.CMD_ENTITY_ROTATE)]
	public class EntityRotatePacketHandler : IPacketHandler
	{
		public INetCommand ToCommand(ReadOnlySpan<byte> bytes)
		{
			var command = CMD_ENTITY_ROTATE.Create();

			MemoryPackSerializer.Deserialize(bytes, ref command);

			return command;
		}
	}
}
