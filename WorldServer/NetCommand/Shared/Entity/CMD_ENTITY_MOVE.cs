using MemoryPack;
using Network.NetCommand;

namespace WorldServer.NetCommand.Shared.Entity
{
	public enum MoveType
	{
		None = 0,
		Walk,
		Run
	}

	[MemoryPackable]
	public partial class CMD_ENTITY_MOVE : PooledNetCommand<CMD_ENTITY_MOVE>
	{
		public int Id { get; set; }

		public uint Time { get; set; }

		public float X { get; set; }

		public float Y { get; set; }

		public float Z { get; set; }

		public MoveType MoveType { get; set; }

		public void SetPosition(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static CMD_ENTITY_MOVE Create()
		{
			return GetOrCreate();
		}

		public override short Opcode => (short) Network.Packet.Opcode.CMD_ENTITY_MOVE;
	}
}
