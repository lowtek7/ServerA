using PacketGenerator;
using PacketGenerator.CodeGen;

class Program
{
	static void Main(string[] args)
	{
		var path = "./Packets.xml";
		var outputPath = "./Output";
		var chunk = PacketXmlParser.Parse(path);

		if (!Directory.Exists(outputPath))
		{
			Directory.CreateDirectory(outputPath);
		}

		CodeGenerator.Generate(chunk, outputPath);
	}
}
