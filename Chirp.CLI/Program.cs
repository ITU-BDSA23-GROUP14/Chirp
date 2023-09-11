/* The following is adapted from 'How to: Read text from a file' by Microsoft.
   Link: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
*/

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

using SimpleDB;

string path = @"chirp_cli_db.csv";
IDatabaseRepository<Cheep> csvh = new CSVDatabase<Cheep>(path);

if (args[0] == "read") PrintChirps();    
else if (args[0] == "cheep" && args.Length == 2) AddChirp();


void AddChirp()
{
    string username = Environment.UserName;
    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    
    Cheep cheep = new Cheep(username, $"\"{args[1]}\"", currentTime);
    csvh.Store(cheep);
}

void PrintChirps()
{
    try
    {
        var lines = csvh.Read();                                                                            // Read rows in CSV database using the CSVHelper library                                                                                  // Skip the first line, which contains the category names (Author, Message, Timestamp)
                                                                                                                                     
        // Taken from https://stackoverflow.com/a/34265869
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        TimeZoneInfo cestZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        foreach (Cheep l in lines)
        {
            var info = (author: l.Author,
                        message: l.Message,                                                                 // Remove quotation marks from the messages stored in the .csv file
                        timestamp: l.Timestamp);
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