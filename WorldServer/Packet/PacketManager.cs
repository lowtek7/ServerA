using System.Reflection;
using MemoryPack;
using Network.NetCommand;
using WorldServer.Utility;

namespace Network.Packet.Handler
{
	public class PacketManager
	{
		/// <summary>
		/// Opcode를 매핑해두는 패킷 핸들러
		/// </summary>
		private readonly Dictionary<RAMG.Packets.Opcode, IPacketHandler> PacketHandlers = new Dictionary<RAMG.Packets.Opcode, IPacketHandler>();

		public void Init()
		{
			PacketHandlers.Clear();

			// 패킷 핸들러 타입 미리 캐시
			var packetHandlerType = typeof(IPacketHandler);
			// 어셈블리 가져오기 (무조건 Network 어셈블리 내에만 패킷 핸들러 클래스들이 존재 해야한다.)
			var assembly = packetHandlerType.Assembly;
			var types = TypeUtility.GetTypesWithInterface(assembly, packetHandlerType);

			// 핸들러들을 미리 생성해주자.
			foreach (var handlerType in types)
			{
				var handler = Activator.CreateInstance(handlerType) as IPacketHandler;
				var attributes = handlerType.GetCustomAttributes<PacketOpcodeAttribute>();

				foreach (var opcodeAttribute in attributes)
				{
					var opcode = opcodeAttribute.Opcode;

					// 한 opcode에 두가지 핸들러가 존재 할 수 가 없다.
					PacketHandlers.Add(opcode, handler);
				}
			}
		}

		public INetCommand ToCommand(RAMG.Packets.Opcode opcode, ReadOnlySpan<byte> bytes)
		{
			if (PacketHandlers.TryGetValue(opcode, out var packetHandler))
			{
				return packetHandler.ToCommand(bytes);
			}

			return null;
		}
	}
}
