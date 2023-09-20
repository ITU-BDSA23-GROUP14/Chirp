namespace Chirp.CLI.Client.Tests;

public class UnitTests
{
    [Fact]
    public void Test_UserInterface_FormatTimestamp()
    {
        var formattedTimestamp = UserInterface.FormatTimestamp(1690891760);
        Assert.Equal("08/01/23 14:09:20", formattedTimestamp);
    }
}