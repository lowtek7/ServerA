// generated by RAMG Packet Generator
using MemoryPack;
using Network.NetCommand;

namespace RAMG.Packets
{
	[MemoryPackable]
	public partial class SCMD_PLAYER_LOGIN : PooledNetCommand<SCMD_PLAYER_LOGIN>
	{
		public int Id { get; set; }
		public uint Time { get; set; }
		public static SCMD_PLAYER_LOGIN Create()
		{
			return GetOrCreate();
		}

		public override short Opcode => (short) RAMG.Packets.Opcode.SCMD_PLAYER_LOGIN;
	}
}