using System;

namespace Network.Packet.Handler
{
	/// <summary>
	/// Opcode 정보를 넣어주기 위해 무조건 IPacketHandler의 구현체들에게 넣어줘야한다.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class PacketOpcodeAttribute : Attribute
	{
		public PacketOpcodeAttribute(RAMG.Packets.Opcode opcode)
		{
			this.Opcode = opcode;
		}

		public RAMG.Packets.Opcode Opcode { get; }
	}
}
