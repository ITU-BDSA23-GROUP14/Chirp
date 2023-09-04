/* The following is adapted from 'How to: Read text from a file' by Microsoft.
   Link: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
*/

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

try
{
    using var sr = new StreamReader("chirp_cli_db.csv");                                                   // Open the text file using a stream reader.
    var text = sr.ReadToEnd();                                                                          // Read the stream as a string, and write the string to the console.

    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);       // Split at every newline  
    lines = lines.Skip(1).ToArray();                                                                    // Skip the first line, which contains the category names (Author,Message,Timestamp)                                                               

    // Taken from https://stackoverflow.com/a/34265869
    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

    TimeZoneInfo cestZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    foreach (string l in lines)
    {
        string[] infoArr = CSVParser.Split(l);
        var info = (author: infoArr[0],
                    message: infoArr[1].Replace("\"", ""),                                              // Remove quotation marks from the messages stored in the .csv file
                    timestamp: infoArr[2]);
        var timeUtc = DateTimeOffset.FromUnixTimeSeconds(long.Parse(info.timestamp)).DateTime;          // Parse the timestap as a DateTime object
        DateTime timeCet = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cestZone);                           // Convert the timestamp from UTC to CEST

        var formattedTimestamp = timeCet.ToString("MM/dd/yy HH':'mm':'ss");

        Console.WriteLine($"{info.author} @ {formattedTimestamp}: {info.message}");
    }
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}
    
