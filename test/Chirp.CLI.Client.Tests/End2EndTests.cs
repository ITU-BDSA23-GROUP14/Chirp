namespace Chirp.CLI.Client.Tests;
using System.Diagnostics;
using Xunit.Abstractions;

public class End2EndTests
{
    /*
    private readonly ITestOutputHelper output;

    public End2EndTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    */

    [Fact]
    public void ReadTest()
    {
        // Inspired by https://stackoverflow.com/questions/285760/how-to-spawn-a-process-and-capture-its-stdout-in-net
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            Arguments = "Chirp.CLI.dll read"
        };

        var process = Process.Start(processStartInfo);
        var processOutput = process?.StandardOutput.ReadToEnd();
        process?.WaitForExit();
        Assert.Contains("Hello, BDSA students", processOutput);

        /*
        string[] lines = processOutput.Trim().Split( new string[] {Environment.NewLine}, StringSplitOptions.None );
        foreach(var l in lines) {
            output.WriteLine(l);
        }
        */
    }

    [Fact]
    public void WriteTest()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            Arguments = "Chirp.CLI.dll cheep \"I love cookies\""
        };

        var process = Process.Start(processStartInfo);
        process?.WaitForExit();

        processStartInfo.Arguments = "Chirp.CLI.dll read";
        process = Process.Start(processStartInfo);
        var processOutput = process?.StandardOutput.ReadToEnd();
        process?.WaitForExit();

        Assert.Contains("I love cookies", processOutput);
    }
}