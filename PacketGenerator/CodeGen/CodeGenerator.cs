using System.Text;

namespace PacketGenerator.CodeGen;

public static class CodeGenerator
{
	public static Encoding SourceEncoding => new UTF8Encoding(false);

	public static void Generate(CodeChunk chunk, string outputPath)
	{
		if (!Directory.Exists(outputPath))
		{
			Directory.CreateDirectory(outputPath);
		}

		var serverCodeOutput = Path.Combine(outputPath, "Server");
		var clientCodeOutput = Path.Combine(outputPath, "Client");

		if (!Directory.Exists(serverCodeOutput))
		{
			Directory.CreateDirectory(serverCodeOutput);
		}

		if (!Directory.Exists(clientCodeOutput))
		{
			Directory.CreateDirectory(clientCodeOutput);
		}

		var serverPacketHandlerOutput = Path.Combine(serverCodeOutput, "PacketHandlers");

		if (!Directory.Exists(serverPacketHandlerOutput))
		{
			Directory.CreateDirectory(serverPacketHandlerOutput);
		}

		var clientPacketHandlerOutput = Path.Combine(clientCodeOutput, "PacketHandlers");

		if (!Directory.Exists(clientPacketHandlerOutput))
		{
			Directory.CreateDirectory(clientPacketHandlerOutput);
		}

		// ----- Enum 코드 생성 시작 -----
		// 추후 리팩토링 예정
		var enumSourceFileName = "PacketEnums.generated.cs";
		var enumSourceText = EnumCode.EnumSourceGlobalBegin;
		var isFirst = true;

		foreach (var enumType in chunk.EnumTypes)
		{
			if (!isFirst)
			{
				enumSourceText += '\n';
			}

			if (isFirst)
			{
				isFirst = false;
			}

			var enumBody = new EnumCode(enumType);
			enumSourceText += enumBody.ToSource();
		}

		enumSourceText += EnumCode.EnumSourceGlobalEnd;

		File.WriteAllText(Path.Combine(serverCodeOutput, enumSourceFileName), enumSourceText, SourceEncoding);
		File.WriteAllText(Path.Combine(clientCodeOutput, enumSourceFileName), enumSourceText, SourceEncoding);

		// ----- Enum 코드 생성 끝 -----

		// ----- 패킷 코드 생성 시작 -----

		foreach (var packetType in chunk.PacketTypes)
		{
			var packetCode = new PacketCode(packetType);
			var packetSourceText = packetCode.ToSource();
			var packetFileName = packetCode.FileName;

			File.WriteAllText(Path.Combine(serverCodeOutput, packetFileName), packetSourceText, SourceEncoding);
			File.WriteAllText(Path.Combine(clientCodeOutput, packetFileName), packetSourceText, SourceEncoding);

			var serverPacketHandlerCode = new ServerPacketHandlerCode(packetType);
			var serverPacketHandlerSourceText = serverPacketHandlerCode.ToSource();

			File.WriteAllText(Path.Combine(serverPacketHandlerOutput, serverPacketHandlerCode.FileName), serverPacketHandlerSourceText, SourceEncoding);

			var clientPacketHandlerCode = new ClientPacketHandlerCode(packetType);
			var clientPacketHandlerSourceText = clientPacketHandlerCode.ToSource();

			File.WriteAllText(Path.Combine(clientPacketHandlerOutput, clientPacketHandlerCode.FileName), clientPacketHandlerSourceText, SourceEncoding);
		}

		// ----- 패킷 코드 생성 끝 -----
	}
}
