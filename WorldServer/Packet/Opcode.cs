namespace Network.Packet
{
	/// <summary>
	/// 패킷 명령 코드들
	/// </summary>
	public enum Opcode : short
	{
		// 0x000은 절대로 사용하면 안된다.
		ERROR_CODE = 0x000,

		CCMD_PLAYER_JOIN = 0x001,

		CMD_ENTITY_MOVE = 0x100,
	}
}
