using MemoryPack;
using Network.NetCommand;

namespace WorldServer.NetCommand.Shared.Entity
{
	[MemoryPackable]
	public partial class CMD_ENTITY_ROTATE : PooledNetCommand<CMD_ENTITY_ROTATE>
	{
		public int Id { get; set; }

		public uint Time { get; set; }

		public float X { get; set; }

		public float Y { get; set; }

		public float Z { get; set; }

		public float W { get; set; }

		public void SetRotate(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static CMD_ENTITY_ROTATE Create()
		{
			return GetOrCreate();
		}

		public override short Opcode => (short) Network.Packet.Opcode.CMD_ENTITY_ROTATE;
	}
}
