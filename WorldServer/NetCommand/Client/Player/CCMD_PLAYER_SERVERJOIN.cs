using MemoryPack;

namespace Network.NetCommand.Client.Player
{
	[MemoryPackable]
	public partial class CCMD_PLAYER_SERVERJOIN : PooledNetCommand<CCMD_PLAYER_SERVERJOIN>
	{
		public int Id { get; set; }

		public uint Time { get; set; }

		public static CCMD_PLAYER_SERVERJOIN Create()
		{
			return GetOrCreate();
		}

		public override short Opcode => (short) Packet.Opcode.CCMD_PLAYER_JOIN;
	}
}
