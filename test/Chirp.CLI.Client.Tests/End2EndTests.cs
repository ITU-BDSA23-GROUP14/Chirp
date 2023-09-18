namespace Chirp.CLI.Client.Tests;
using System;
using System.Diagnostics;

public class End2EndTests
{
    [Fact]
    public void Test()
    {
        using StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        using (Process p = new Process())
        {
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "Chirp.CLI.exe";
            p.StartInfo.WorkingDirectory = "";
            p.Start();
        }
    }
}