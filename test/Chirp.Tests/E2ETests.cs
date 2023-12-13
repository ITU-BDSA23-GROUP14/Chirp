using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Chirp.Tests;

[TestFixture]
public class E2ETests : PageTest
{
    private readonly E2EWebApplicationFactory<Program> _fixture;
    private readonly string _serverAddress;

    public E2ETests()
    {
        _fixture = new E2EWebApplicationFactory<Program>();
        _serverAddress = _fixture.ServerAddress;
    }

    [SetUp]
    public async Task SetUp()
    {
        await _fixture.CreateClient().GetAsync("/"); // wait for the server to start up
        await Page.GotoAsync(_serverAddress);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _fixture.DisposeAsync();
    }

    [Test]
    public async Task FollowTurnsToUnfollow()
    {
        var firstCheep = Page.GetByRole(AriaRole.Listitem).First;
        await firstCheep.GetByRole(AriaRole.Button, new() { Name = "Follow" }).First.ClickAsync();

        await Expect(firstCheep
            .GetByRole(AriaRole.Button, new() { Name = "Unfollow" })
            .First)
            .ToBeVisibleAsync();
        await firstCheep.GetByRole(AriaRole.Button).First.ClickAsync();
    }

    [Test]
    public async Task PrivateTimelineShowsFollowingCheeps()
    {
        var firstCheep = Page.GetByRole(AriaRole.Listitem).First;
        await firstCheep.GetByRole(AriaRole.Button, new() { Name = "Follow" }).First.ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Expect(Page.GetByText("Jacqualine Gilcoine").First).ToBeVisibleAsync();
        await Expect(Page.GetByText("Starbuck now is what we hear the worst.").First).ToBeVisibleAsync();
        await firstCheep.GetByRole(AriaRole.Button).First.ClickAsync();
    }

    [Test]
    public async Task DefaultImageIsAdded()
    {
        var firstCheep = Page.GetByRole(AriaRole.Listitem).First;

        await Expect(firstCheep.GetByRole(AriaRole.Img).First).ToHaveAttributeAsync("src", "/images/icon1.png");
    }

    [Test]
    public async Task PublicTimelineNextPageGoesToNextPageAndPreviousPageGoesToPreviousPage()
    {
        await Page.Locator("body").ClickAsync();

        //Check for first page
        await Expect(Page.GetByText("Starbuck now is what we hear the worst.").First).ToBeVisibleAsync();

        //Go to next page
        await Page.GetByRole(AriaRole.Link, new() { Name = "Next Page" }).ClickAsync();

        //Check for second page
        await Expect(Page.GetByText("At the same height.").First).ToBeVisibleAsync();

        //Go to previous page
        await Page.GetByRole(AriaRole.Link, new() { Name = "Previous Page" }).ClickAsync();

        //Check for first page
        await Expect(Page.GetByText("Starbuck now is what we hear the worst.").First).ToBeVisibleAsync();

    }

    [Test]
    public async Task PublicTimelinePreviousPageFromFirstPageFails()
    {
        await Page.Locator("body").ClickAsync();

        //Try to go to previous page
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Previous Page" })).ToBeHiddenAsync();
    }

    [Test]
    public async Task PrivateTimelineNextPageFromLastPageFails()
    {
        //Go to Quintin Sitts private timeline
        await Page.Locator("body").ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Quintin Sitts" }).First.ClickAsync();

        //Go to last page
        await Page.GetByRole(AriaRole.Link, new() { Name = "Next Page" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Next Page" }).ClickAsync();

        //Try to go to next page
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Next Page" })).ToBeHiddenAsync();
    }

    [Test]
    public async Task PrivateTimelineNoButtonsForUserWithFewerThan32Cheeps()
    {
        //Go to Quintin Sitts private timeline
        await Page.Locator("body").ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).First.ClickAsync();

        //Try to go to next page
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Previous Page" })).ToBeHiddenAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Next Page" })).ToBeHiddenAsync();
    }
}