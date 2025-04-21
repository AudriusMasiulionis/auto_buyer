using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoDokas.Tests.UI;

[Parallelizable(ParallelScope.Self)]
public class HomePageTests : TestBase
{
    [Test]
    public async Task ShouldLoadHomePage()
    {
        // Navigate to your Blazor app's base address
        await Page.GotoAsync(BaseUrl);
        
        // Check if the page loaded successfully
        await Expect(Page).ToHaveTitleAsync(new Regex("Home"));
        
        // Additional assertions can be added here
    }
}
