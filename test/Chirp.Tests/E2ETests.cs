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
}