using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Chirp.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class E2ETests : PageTest
{
    [Test]
    public async Task PlaywrightTest1()
    {
        // Assertions use the expect API.
        await Expect(Page).ToHaveURLAsync("https://playwright.dev/");
    }

    [SetUp]
    public async Task SetUp()
    {
        await Page.GotoAsync("https://playwright.dev");
    }
}