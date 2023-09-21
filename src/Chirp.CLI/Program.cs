/* The following is adapted from 'How to: Read text from a file' by Microsoft.
   Link: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
*/

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.CommandLine;

using SimpleDB;

string path = @"data/chirps.csv";
IDatabaseRepository<Cheep> csvh = CSVDatabase<Cheep>.Instance(path);

/* The following code using System.CommandLine is inspired by "Tutorial: Get started with System.CommandLine" from Microsoft
    Link: https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial
*/

var r = new RootCommand();                                          // base of System.CommandLine that we attach commands to

var readCommand = new Command("read", "Read a Cheep!");
readCommand.SetHandler(() =>
{                                                                   // SetHandler handles what happens when we run the command
    UserInterface.PrintCheeps(csvh.Read());
});

var writeCommand = new Command("cheep", "Cheep a Cheep!");
var writeArgument = new Argument<string>(
    name: "cheep",
    description: "Your Cheep's text"
);
writeCommand.AddArgument(writeArgument);                            // writeArgument forces user to write smth (so "dotnet run -- cheep" on its own would be illegal/impossible)
writeCommand.SetHandler((cheepText) =>
{
    AddChirp(cheepText);
}, writeArgument);

r.AddCommand(readCommand);
r.AddCommand(writeCommand);

r.Invoke(args);                                                     // necessary line to run the program using user input

void AddChirp(string cheepText)
{
    string username = Environment.UserName;
    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    Cheep cheep = new Cheep(username, $"\"{cheepText}\"", currentTime);
    csvh.Store(cheep);
}

