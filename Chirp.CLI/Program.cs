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

if (args[0] == "read") UserInterface.PrintCheeps(csvh.Read());    
else if (args[0] == "cheep" && args.Length == 2) AddChirp();


void AddChirp()
{
    string username = Environment.UserName;
    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    
    Cheep cheep = new Cheep(username, $"\"{args[1]}\"", currentTime);
    csvh.Store(cheep);
}

