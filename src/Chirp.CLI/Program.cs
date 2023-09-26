/* The following is adapted from 'How to: Read text from a file' by Microsoft.
   Link: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
*/

using System.CommandLine;
using SimpleDB;
using System.Net.Http.Json;

/* The following code using System.CommandLine is inspired by "Tutorial: Get started with System.CommandLine" from Microsoft
    Link: https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial
*/

var r = new RootCommand();                                                      // base of System.CommandLine that we attach commands to

var baseURL = "https://bdsagroup14chirpremotedb.azurewebsites.net/";
using HttpClient client = new();
client.BaseAddress = new Uri(baseURL);

var readCommand = new Command("read", "Read a Cheep!");
readCommand.SetHandler(async () =>                                              // SetHandler handles what happens when we run the command
{                                                                   
    var cheep = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");
    
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

async Task AddChirp(string cheepText)
{
    string username = Environment.UserName;
    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    Cheep? cheep = new(username, $"\"{cheepText}\"", currentTime);

    var HTTPResponse = await client.PostAsJsonAsync("cheep", cheep);

    if (HTTPResponse.IsSuccessStatusCode)
    {
        Console.WriteLine("Cheep successfully posted.");
    }
    else
    {
        Console.WriteLine($"Failed to post cheep. Status code: {HTTPResponse.StatusCode} ");
    }
}