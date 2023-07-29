namespace WorldServer.Utility;

public static class UnixTimeConverter
{
	public static DateTime UnixTimeToDateTime(uint unixTimeStamp)
	{
		// Unix timestamp is seconds past epoch
		var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
		return dateTime;
	}

	public static uint ToUnixTime(this DateTime dateTime)
	{
		return (uint)dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
	}
}
