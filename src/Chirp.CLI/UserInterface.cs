public static class UserInterface 
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        try
        {
            TimeZoneInfo cestZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            foreach (Cheep c in cheeps)
            {
                var info = (author: c.Author,
                            message: c.Message,                                                                 // Remove quotation marks from the messages stored in the .csv file
                            timestamp: c.Timestamp);
                var timeUtc = DateTimeOffset.FromUnixTimeSeconds(info.timestamp).DateTime;                      // Parse the timestap as a DateTime object
                DateTime timeCet = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cestZone);                          // Convert the timestamp from UTC to CEST

                var formattedTimestamp = timeCet.ToString("MM/dd/yy HH':'mm':'ss");

                Console.WriteLine($"{info.author} @ {formattedTimestamp}: {info.message}");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
}