using System.Xml;

namespace PacketGenerator;

public static class StringExtensions
{
	public static string ToUpperFirst(this string s)
	{
		if (string.IsNullOrEmpty(s)) {
			return s;
		}

		return char.ToUpper(s[0]) + s.Substring(1).ToLower();
	}
}

public static class PacketXmlParser
{
	public static CodeChunk Parse(string path)
	{
		var result = new CodeChunk();
		using XmlReader reader = XmlReader.Create(path);

		while (reader.Read())
		{
			if (reader.IsStartElement())
			{
				switch (reader.Name)
				{
					case "enum":
					{
						var name = reader["name"];
						var value = reader["value"];
						var values = Array.Empty<string>();

						if (!string.IsNullOrEmpty(value))
						{
							values = value.Replace(" ", string.Empty).Split(",");
						}

						var enumType = new EnumType(name!, values);

						result.EnumTypes.Add(enumType);
						break;
					}
					case "packet":
					{
						var name = reader["name"];
						var type = reader["type"];
						var transferType = Enum.Parse<PacketTransferType>(type!.ToUpperFirst());
						var varList = new List<PacketVariable>();

						while (reader.Read())
						{
							if (reader.Name == "variables" && reader.IsStartElement())
							{
								continue;
							}

							if (reader.Name == "variables")
							{
								break;
							}

							if (reader.Name == "variable" && reader.IsStartElement())
							{
								var varName = reader["name"];
								var varType = reader["type"];

								varList.Add(new PacketVariable(varName!, varType!));
							}
						}

						result.PacketTypes.Add(new PacketType(name!, transferType, varList.ToArray()));
						break;
					}
				}
			}
		}

		return result;
	}
}
