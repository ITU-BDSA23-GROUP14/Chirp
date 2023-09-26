namespace Chirp.CLI.Client.Tests;

public class IntegrationTest
{
    [Fact]
    public void UserInterface_PrintCheeps_Test()
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