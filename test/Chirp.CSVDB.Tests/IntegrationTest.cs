namespace Chirp.CSVDB.Tests;
using Xunit.Abstractions;
using SimpleDB;

record Cheep(string Author, string Message, long Timestamp);

public class IntegrationTest
{
    private readonly ITestOutputHelper output;

    public IntegrationTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ReadWriteTest()
    {
        string path = @"chirps.csv";
        IDatabaseRepository<Cheep> csvh = new CSVDatabase<Cheep>(path);
        List<Cheep> temp = csvh.Read().ToList();

        Cheep newCheep = new Cheep("me", "hello", 10);
        csvh.Store(newCheep);
        List<Cheep> temp2 = csvh.Read().ToList();

        for (int i = 0; i < temp2.Count; i++)
        {
            if (i < 4)
            {
                Assert.True(temp[i] == temp2[i]);
            }
            else
            {
                output.WriteLine(temp2[i].Author);
                Assert.True(temp2[i] == newCheep);
            }
        }
    }
}