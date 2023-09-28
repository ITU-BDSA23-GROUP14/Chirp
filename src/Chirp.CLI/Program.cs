/* The following is adapted from 'How to: Read text from a file' by Microsoft.
   Link: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
*/

using System.CommandLine;
using CheepRecordType;
using RemoteDatabase;

/* The following code using System.CommandLine is inspired by "Tutorial: Get started with System.CommandLine" from Microsoft
    Link: https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial
*/

var r = new RootCommand();                                                      // base of System.CommandLine that we attach commands to

var baseURL = "http://localhost:5000";

var db = RemoteDatabase<Cheep>.Instance(baseURL);

var readCommand = new Command("read", "Read a Cheep!");
readCommand.SetHandler(() =>                                              // SetHandler handles what happens when we run the command
{
    var cheep = db.Read();

    if (cheep != null)
    {
        UserInterface.PrintCheeps(cheep);
    }
    else
    {
        Console.WriteLine("No cheeps retrieved.");
    }
});

var writeCommand = new Command("cheep", "Cheep a Cheep!");
var writeArgument = new Argument<string>(
    name: "cheep",
    description: "Your Cheep's text"
);
writeCommand.AddArgument(writeArgument);                                        // writeArgument forces user to write smth (so "dotnet run -- cheep" on its own would be illegal/impossible)
writeCommand.SetHandler(AddChirp, writeArgument);

// Add all commands
r.AddCommand(readCommand);
r.AddCommand(writeCommand);

r.Invoke(args);                                                                 // necessary line to run the program using user input

void AddChirp(string cheepText)
{
    string username = Environment.UserName;
    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    Cheep? cheep = new(username, $"\"{cheepText}\"", currentTime);
    
    db.Store(cheep);
}