using MemoryPack;
using Network.NetCommand;
using Network.NetCommand.Client.Player;

namespace Network.Packet.Handler.Clinet
{
	[PacketOpcode(Opcode.CCMD_PLAYER_JOIN)]
	public class PlayerJoinPacketHandler : IPacketHandler
	{
		public INetCommand ToCommand(ReadOnlySpan<byte> bytes)
		{
			// 패킷을 풀로 부터 가져온다
			var command = CCMD_PLAYER_SERVERJOIN.Create();

			// 디시리얼라이즈
			MemoryPackSerializer.Deserialize(bytes, ref command);

			// 디시리얼라이즈 한 패킷을 반환한다. 사용이 완료된 커맨드는 무조건 dispose로 pool에 반환 해야한다.
			return command;
		}
	}
}
