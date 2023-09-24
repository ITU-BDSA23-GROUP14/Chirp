namespace Chirp.CLI.Client.Tests;
using CheepRecordType;

public class IntegrationTest
{
    [Fact]
    public void Test_UserInterface_PrintCheeps()
    {
        using (StringWriter stringWriter = new StringWriter())
        {
            Console.SetOut(stringWriter);
            Cheep testCheep = new Cheep("ropf", "Hello, BDSA students!", 1690891760);
            List<Cheep> testList = new List<Cheep>() { testCheep };

            UserInterface.PrintCheeps(testList);
            string consoleOutput = stringWriter.ToString();

            Assert.Contains("ropf @ 08/01/23 14:09:20: Hello, BDSA students!", consoleOutput);
        }


    }
}