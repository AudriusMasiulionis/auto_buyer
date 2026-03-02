# Stack Research: Technical SEO for Blazor Server

**Domain:** Technical SEO optimization for Blazor Server (.NET 9) web application
**Researched:** 2026-03-02
**Confidence:** HIGH

## Current System Baseline

Before recommending additions, here is what already exists and must not be re-implemented:

| Existing | Version | Status |
|----------|---------|--------|
| .NET / Blazor Server | 9.0 | Target framework |
| EF Core + SQLite | 9.0 | Data layer |
| Serilog | 9.0 | Logging |
| Google Analytics (gtag.js) | Consent Mode v2 | Tracking |
| Google Search Console | Verified | Monitoring |
| `<HeadOutlet/>` in App.razor | Built-in | Head tag rendering |
| `<PageTitle>` on all pages | Built-in | Title tags |
| Cookie consent (orestbida) | 3.1.0 | GDPR compliance |
| Static `robots.txt` | N/A | Broken -- blocks crawlers |
| Static `sitemap.xml` | N/A | Only lists homepage |

**Critical existing advantage:** Public SEO pages (Home, TermsAndConditions, PrivacyPolicy) use static SSR (no `@rendermode InteractiveServer`). Only the Contract tool pages are interactive. This means `<HeadContent>` tags on public pages are rendered server-side during the initial HTTP response -- exactly what search engine crawlers need. No prerendering workaround is required.

**Critical existing bug:** `<html lang="en">` in App.razor -- must be changed to `<html lang="lt">` for a Lithuanian-only site. This directly affects how Google classifies page language.

## Recommended Stack

### Core Technologies (Built-in -- No Packages Needed)

| Technology | Version | Purpose | Why Recommended |
|------------|---------|---------|-----------------|
| `<HeadContent>` | .NET 9 built-in | Meta descriptions, canonical URLs, Open Graph tags | Native Blazor component, renders during static SSR, no dependencies. Microsoft official approach for head tag management. Already have `<HeadOutlet/>` in App.razor. |
| `<PageTitle>` | .NET 9 built-in | SEO-optimized title tags per page | Already in use. Just needs better keyword-optimized content. |
| `MarkupString` | .NET 9 built-in | JSON-LD structured data rendering | Workaround for Blazor RZ9992 error ("Script tags should not be placed inside components"). Cast JSON-LD `<script>` block to `MarkupString` and place inside `<HeadContent>`. Verified pattern from dotnet/aspnetcore#37230. |
| ASP.NET Core Middleware | .NET 9 built-in | Dynamic robots.txt endpoint | Replace the broken static robots.txt with a minimal endpoint (`app.MapGet("/robots.txt", ...)`) that returns correct directives. Four pages is too few to justify a package. |

**Confidence: HIGH** -- All verified from Microsoft official documentation (learn.microsoft.com/aspnet/core/blazor/components/control-head-content).

### SEO Libraries (NuGet Packages)

| Library | Version | Purpose | Why Recommended |
|---------|---------|---------|-----------------|
| Schema.NET | 13.0.0 | Strongly-typed JSON-LD generation for Organization, Service, WebPage, WebSite schemas | 3.3M total downloads. Generates schema.org types as C# POCOs with proper JSON-LD serialization. Use `ToHtmlEscapedString()` for XSS-safe output. Targets .NET Standard 2.0 + .NET 6.0+, confirmed compatible with .NET 9. |

**Confidence: HIGH** -- NuGet page verified, 3.3M downloads, maintained by Rehan Saeed (former Microsoft engineer), targets .NET Standard 2.0 for broad compatibility.

### Sitemap Generation

| Approach | Version | Purpose | Why Recommended |
|----------|---------|---------|-----------------|
| Hand-written XML endpoint | N/A (built-in) | Generate sitemap.xml dynamically | For 4 public pages, a simple `app.MapGet("/sitemap.xml", ...)` returning XML is the right approach. Adding a package for 4 URLs is over-engineering. Use `XDocument` or string interpolation with proper `lastmod` dates. |

**Why NOT Sidio.Sitemap.Blazor:** While it is the best Blazor sitemap package (v2.0.1, updated Feb 2026, supports .NET 8/9), it has only 7.2K total downloads and adds attribute-based discovery overhead. With exactly 4 public URLs (`/`, `/contract`, `/terms-and-conditions`, `/privacy-policy`), a 15-line middleware endpoint is simpler, has zero dependencies, and is easier to maintain.

**When to reconsider:** If the site grows to 20+ pages or adds dynamic content pages (blog, FAQ), switch to Sidio.Sitemap.Blazor for automatic route discovery.

### Development/Validation Tools

| Tool | Purpose | Notes |
|------|---------|-------|
| Google Rich Results Test | Validate JSON-LD structured data | Free, web-based (search.google.com/test/rich-results). Test after deploying each schema type. |
| Google Search Console URL Inspection | Verify how Googlebot renders each page | Already set up. Use "Request Indexing" after deploying SEO changes. |
| Schema Markup Validator | Validate schema.org compliance | validator.schema.org -- catches errors Rich Results Test may miss. |
| Google Lighthouse (Chrome DevTools) | Audit SEO score, Core Web Vitals, accessibility | Built into Chrome. Run on each public page. Target SEO score 90+. |
| Chrome DevTools "View Page Source" | Verify meta tags in initial HTML response | Critical for Blazor: confirms tags are in static SSR output, not added by JS. |

**Confidence: HIGH** -- All are free, industry-standard Google tools. No alternatives needed.

## Installation

```bash
# Only one NuGet package needed
dotnet add package Schema.NET --version 13.0.0
```

That is the entire package installation. Everything else uses .NET 9 built-in capabilities.

## What NOT to Use

| Avoid | Why | Use Instead |
|-------|-----|-------------|
| SeoTags NuGet package (v2.1.0) | Designed for ASP.NET Core MVC/Razor Pages, not Blazor Server. Uses `Html.SeoTags()` and `Html.SetSeoInfo()` which are Razor Page helpers. No Blazor `<HeadContent>` integration. | Built-in `<HeadContent>` + Schema.NET for JSON-LD |
| Sidio.Sitemap.Blazor | Over-engineering for 4 static URLs. Adds package dependency, attribute decorators, and middleware for what is a 15-line endpoint. | Hand-written `MapGet("/sitemap.xml")` endpoint |
| BlazorSiteMapper | Only 1.0.4, minimal community adoption, fewer features than Sidio | Hand-written endpoint or Sidio if site grows |
| Toolbelt.Blazor.HeadElement | Third-party replacement for built-in `<HeadContent>`. Was useful before .NET 6 when `<HeadContent>` did not exist. Now redundant. | Built-in `<HeadContent>` and `<PageTitle>` |
| json-ld.net | Low-level JSON-LD processor (W3C spec). No schema.org type safety. You would build JSON manually. | Schema.NET provides strongly-typed C# classes for all schema.org types |
| FoxLearn.AspNet.JsonLd | Very new (v1.1.0), minimal downloads, injects JSON-LD via middleware -- does not integrate with Blazor `<HeadContent>`. | Schema.NET + MarkupString in `<HeadContent>` |
| Third-party robots.txt packages | Adds complexity for a file that contains 5 lines. | Simple `MapGet("/robots.txt")` endpoint |

## Architecture Patterns for This Stack

### Pattern: Reusable SeoHead Component

Create a single `SeoHead.razor` component that encapsulates all SEO tags. Each page passes its specific metadata to this component.

```csharp
// Components/Shared/SeoHead.razor
<PageTitle>@Title</PageTitle>
<HeadContent>
    <meta name="description" content="@Description">
    <meta name="robots" content="@Robots">
    <link rel="canonical" href="@CanonicalUrl">

    <!-- Open Graph -->
    <meta property="og:title" content="@Title">
    <meta property="og:description" content="@Description">
    <meta property="og:url" content="@CanonicalUrl">
    <meta property="og:type" content="website">
    <meta property="og:locale" content="lt_LT">
    <meta property="og:site_name" content="AutoDokas">

    <!-- JSON-LD Structured Data -->
    @if (JsonLd is not null)
    {
        @((MarkupString)$"<script type=\"application/ld+json\">{JsonLd}</script>")
    }
</HeadContent>

@code {
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public string Description { get; set; } = "";
    [Parameter] public string CanonicalUrl { get; set; } = "";
    [Parameter] public string Robots { get; set; } = "index, follow";
    [Parameter] public string? JsonLd { get; set; }
}
```

### Pattern: Schema.NET Usage for JSON-LD

```csharp
using Schema.NET;

// Organization schema (site-wide, in App.razor or layout)
var org = new Organization
{
    Name = "AutoDokas",
    Url = new Uri("https://autodokas.lt"),
    Logo = new Uri("https://autodokas.lt/android-chrome-512x512.png")
};
var jsonLd = org.ToHtmlEscapedString();

// WebSite schema with search action
var website = new WebSite
{
    Name = "AutoDokas",
    Url = new Uri("https://autodokas.lt")
};

// Service schema (for the contract tool)
var service = new Service
{
    Name = "Automobilio pirkimo pardavimo sutartis internetu",
    Description = "Sukurkite automobilio pirkimo pardavimo sutarti internetu per kelias minutes",
    Provider = org
};
```

### Pattern: Minimal Sitemap Endpoint

```csharp
// In Program.cs
app.MapGet("/sitemap.xml", () =>
{
    var baseUrl = "https://autodokas.lt";
    var lastMod = DateTime.UtcNow.ToString("yyyy-MM-dd");
    var xml = $"""
        <?xml version="1.0" encoding="UTF-8"?>
        <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
          <url><loc>{baseUrl}/</loc><lastmod>{lastMod}</lastmod><priority>1.0</priority></url>
          <url><loc>{baseUrl}/contract</loc><lastmod>{lastMod}</lastmod><priority>0.9</priority></url>
          <url><loc>{baseUrl}/terms-and-conditions</loc><lastmod>{lastMod}</lastmod><priority>0.3</priority></url>
          <url><loc>{baseUrl}/privacy-policy</loc><lastmod>{lastMod}</lastmod><priority>0.3</priority></url>
        </urlset>
        """;
    return Results.Content(xml, "application/xml");
});
```

### Pattern: Minimal Robots.txt Endpoint

```csharp
// In Program.cs -- replaces the broken static file
app.MapGet("/robots.txt", () =>
{
    var robots = """
        User-agent: *
        Allow: /
        Disallow: /contract/
        Disallow: /buyer/
        Disallow: /BuyerNotificationSent
        Disallow: /ContractCompleted

        Sitemap: https://autodokas.lt/sitemap.xml
        """;
    return Results.Content(robots, "text/plain");
});
```

## Version Compatibility

| Package | Targets | Compatible With | Notes |
|---------|---------|-----------------|-------|
| Schema.NET 13.0.0 | .NET Standard 2.0, .NET 6.0+ | .NET 9.0 | Works via .NET Standard 2.0 fallback or .NET 6.0+ forward compatibility |
| Sidio.Sitemap.Blazor 2.0.1 | .NET 8.0+ | .NET 9.0, 10.0 | NOT recommended for this project, but documented for future reference |

## Key Technical Decisions

### Why no SEO framework/package?

This project has 4 public pages with static content in a single language. The entire SEO implementation is:
1. Fix `<html lang="lt">` (1 line change)
2. Fix robots.txt (5 lines)
3. Expand sitemap.xml (15 lines)
4. Add `<HeadContent>` blocks with meta/OG tags (reusable component + per-page params)
5. Add JSON-LD structured data via Schema.NET (the only external dependency)

A monolithic SEO package would add unnecessary abstraction over what is fundamentally simple HTML tag management in a well-designed framework (Blazor SSR).

### Why Schema.NET is the only package?

Schema.org has 800+ types with complex nesting. Writing JSON-LD by hand is error-prone (typos in `@type`, wrong property names, incorrect nesting). Schema.NET provides compile-time safety and `ToHtmlEscapedString()` for XSS protection. The cost is one small, well-maintained dependency. The benefit is correct, validatable structured data.

### Why the Contract page (/contract) should still be in the sitemap

Even though `/contract` uses `@rendermode InteractiveServer`, the initial prerender still sends static HTML that crawlers can parse. The page title and basic structure are crawlable. The form interactivity happens after hydration, which crawlers ignore -- but the page still has SEO value as a landing page for "automobilio sutartis online" queries.

Parametrized routes (`/contract/{id}`, `/buyer/{id}`) should NOT be in the sitemap because they are user-specific contract instances.

## Sources

- [Microsoft Docs: Control head content in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content?view=aspnetcore-9.0) -- **HIGH confidence**, official documentation, updated Nov 2025
- [Schema.NET GitHub](https://github.com/RehanSaeed/Schema.NET) -- **HIGH confidence**, 3.3M NuGet downloads, .NET Standard 2.0+
- [Schema.NET NuGet](https://www.nuget.org/packages/Schema.NET) -- **HIGH confidence**, v13.0.0, Dec 2023
- [dotnet/aspnetcore#37230](https://github.com/dotnet/aspnetcore/issues/37230) -- **HIGH confidence**, official issue confirming MarkupString workaround for JSON-LD
- [dotnet/aspnetcore#54300](https://github.com/dotnet/aspnetcore/issues/54300) -- **HIGH confidence**, ASP.NET team confirms HeadContent works only during static SSR prerendering
- [Sidio.Sitemap.Blazor GitHub](https://github.com/marthijn/Sidio.Sitemap.Blazor) -- **MEDIUM confidence**, 7.2K downloads, documented as future option
- [Google Rich Results Test](https://search.google.com/test/rich-results) -- **HIGH confidence**, official Google tool
- [Google Search Console](https://search.google.com/search-console) -- **HIGH confidence**, already integrated

---
*Stack research for: Technical SEO on Blazor Server (.NET 9)*
*Researched: 2026-03-02*
