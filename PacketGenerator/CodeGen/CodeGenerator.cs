using System.Text;

namespace PacketGenerator.CodeGen;

public static class CodeGenerator
{
	public static Encoding SourceEncoding => new UTF8Encoding(false);

	public static void Generate(CodeChunk chunk, string outputPath)
	{
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

		var enumSourceFileName = "PacketEnums.generated.cs";
		var enumSourceText = EnumBody.EnumSourceGlobalBegin;

		foreach (var enumType in chunk.EnumTypes)
		{
			var enumBody = new EnumBody(enumType);
			enumSourceText += enumBody.ToSource();
		}

		enumSourceText += EnumBody.EnumSourceGlobalEnd;

		File.WriteAllText(Path.Combine(serverCodeOutput, enumSourceFileName), enumSourceText, SourceEncoding);
		File.WriteAllText(Path.Combine(clientCodeOutput, enumSourceFileName), enumSourceText, SourceEncoding);
	}
}
