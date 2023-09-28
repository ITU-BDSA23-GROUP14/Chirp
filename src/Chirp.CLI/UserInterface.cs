public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        try
        {
            foreach (Cheep c in cheeps)
            {
                var info = (author: c.Author,
                            message: c.Message,                                                                 // Remove quotation marks from the messages stored in the .csv file
                            timestamp: c.Timestamp);

                var formattedTimestamp = FormatTimestamp(info.timestamp);

                Console.WriteLine($"{info.author} @ {formattedTimestamp}: {info.message}");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }

    public static string FormatTimestamp(long timestamp)
    {
        TimeZoneInfo cestZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        var timeUtc = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        DateTime timeCet = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cestZone);                                  // Convert the timestamp from UTC to CEST

        var formattedTimestamp = timeCet.ToString("MM/dd/yy HH':'mm':'ss");

        return formattedTimestamp;
    }
}