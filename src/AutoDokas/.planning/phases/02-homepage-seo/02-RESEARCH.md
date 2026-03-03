# Phase 2: Homepage SEO - Research

**Researched:** 2026-03-03
**Domain:** Homepage on-page SEO (meta tags, canonical URL, Open Graph, JSON-LD structured data) for a Blazor .NET 9 application targeting Lithuanian search queries
**Confidence:** HIGH

## Summary

Phase 2 optimizes the autodokas.lt homepage for Lithuanian vehicle sale contract search queries across four domains: meta tags (title, description), canonical URL, Open Graph social tags, and JSON-LD structured data (Organization, WebSite, WebPage, Service). All changes target two files: `Components/Pages/Home.razor` for page-specific tags and `Components/App.razor` for site-wide structured data, plus `Resources/Text.resx` for the page title resource string.

The primary keyword is "automobilio pirkimo pardavimo sutartis" (vehicle purchase-sale contract) -- confirmed by competitor analysis showing this exact phrase in the top-ranking titles across autosutartis.lt, ledauto.lt, regitra.lt, and other Lithuanian sites. The current homepage title is "Pagrindinis" (meaning "Home") which has zero keyword value. The hero heading already contains "Automobilio pirkimo pardavimo sutartis internetu" which is well-aligned -- the title tag should match this intent.

For structured data, Schema.NET 13.0.0 is the standard .NET library for type-safe JSON-LD generation. It targets .NET 6+/netstandard2.0 and uses System.Text.Json, which is fully compatible with the project's .NET 9 target. The Blazor RZ9992 compiler error (script tags not allowed in components) requires a `MarkupString` workaround to embed JSON-LD `<script>` tags within `<HeadContent>` or directly in page markup. This is a well-documented, stable workaround used across the Blazor ecosystem.

**Primary recommendation:** Use Schema.NET 13.0.0 for type-safe JSON-LD generation with `ToHtmlEscapedString()` for XSS-safe output. Place meta tags (description, canonical, OG) in Home.razor via `<HeadContent>`. Place JSON-LD structured data using `MarkupString` casting. Update the `HomeTitle` resource string from "Pagrindinis" to a keyword-rich Lithuanian title.

<user_constraints>

## User Constraints (from CONTEXT.md)

### Locked Decisions
- Page title: Replace "Pagrindinis" with keyword-optimized Lithuanian title. Format: "[Primary keyword] | AutoDokas" (brand at end). Research Google Trends for "automobilio pirkimo pardavimo sutartis" and related terms during planning.
- Meta description: Lithuanian, max 160 chars, keyword-rich. Should read as a compelling SERP snippet. Mention key benefit: online, no printer needed, digital signing.
- Canonical URL: Self-referential canonical on homepage: https://autodokas.lt/
- Open Graph tags: og:title, og:description, og:url, og:type (website), og:site_name (AutoDokas). No OG image for v1 -- deferred to v2.
- Structured data (JSON-LD): Organization, WebSite, WebPage, Service schemas. Use Schema.NET NuGet package (v13.0.0) for type-safe generation. Use MarkupString workaround for Blazor RZ9992.

### Claude's Discretion
- Exact keyword phrasing in title and description (informed by Google Trends research)
- JSON-LD property values for Organization (no specific business info provided -- use domain info)
- Whether to place structured data in Home.razor or App.razor
- SeoMetadata reusable component design vs inline HeadContent

### Deferred Ideas (OUT OF SCOPE)
- OG image asset (1200x630px) -- needs design work, deferred to v2 (ESEO-03)
- FAQ schema -- requires content creation, deferred to v2 (CONT-03)

</user_constraints>

<phase_requirements>

## Phase Requirements

| ID | Description | Research Support |
|----|-------------|-----------------|
| META-01 | Homepage has keyword-optimized title using researched Lithuanian search terms | Update `HomeTitle` resource string in `Text.resx` from "Pagrindinis" to keyword-rich title. Competitor analysis confirms "automobilio pirkimo pardavimo sutartis" is the primary target keyword. Format: `Automobilio pirkimo pardavimo sutartis internetu \| AutoDokas`. |
| META-02 | Homepage has meta description with Lithuanian keywords (max 160 chars) | Add `<meta name="description">` via `<HeadContent>` in Home.razor. Lithuanian text under 160 chars describing the online contract service with primary keyword. |
| META-03 | Homepage has self-referential canonical URL | Add `<link rel="canonical" href="https://autodokas.lt/">` via `<HeadContent>` in Home.razor. Trailing slash on homepage is the standard practice. |
| META-04 | Homepage has Open Graph tags (og:title, og:description, og:url, og:type, og:site_name) | Add five `<meta property="og:*">` tags via `<HeadContent>` in Home.razor. No og:image for v1 (deferred). |
| SCHEMA-01 | Organization JSON-LD schema present on homepage | Generate via Schema.NET `Organization` class. Properties: name, url, logo (if available). Place in App.razor (site-wide) or Home.razor. |
| SCHEMA-02 | WebSite JSON-LD schema present on homepage | Generate via Schema.NET `WebSite` class. Properties: name, url, inLanguage. No SearchAction needed (Google retired sitelinks search box November 2024). |
| SCHEMA-03 | WebPage JSON-LD schema present on homepage | Generate via Schema.NET `WebPage` class. Properties: name, description, url, inLanguage, isPartOf (WebSite). |
| SCHEMA-04 | Service JSON-LD schema describing the contract generation tool | Generate via Schema.NET `Service` class. Properties: name, description, provider (Organization), serviceType, areaServed, url. |

</phase_requirements>

## Standard Stack

### Core
| Library | Version | Purpose | Why Standard |
|---------|---------|---------|--------------|
| Schema.NET | 13.0.0 | Type-safe JSON-LD generation for Organization, WebSite, WebPage, Service schemas | Standard .NET library for schema.org structured data. Uses System.Text.Json. 1.8M+ NuGet downloads. Supports .NET 6+/netstandard2.0, fully compatible with .NET 9. Provides `ToHtmlEscapedString()` for XSS-safe HTML embedding. |
| `<HeadContent>` | .NET 9 built-in | Inject meta tags, canonical URL, OG tags into `<head>` | Built-in Blazor component. Already used in Phase 1 for noindex tags. `<HeadOutlet/>` in App.razor collects all output. |
| `<PageTitle>` | .NET 9 built-in | Set homepage title via resource string | Already used on Home.razor line 4. Just needs updated resource string value. |
| MarkupString | .NET 9 built-in | Render raw HTML (JSON-LD script tags) bypassing RZ9992 | Built-in .NET type for rendering unescaped HTML in Blazor. Standard workaround for JSON-LD script tags documented in aspnetcore#37230. |

### Supporting
| Library | Version | Purpose | When to Use |
|---------|---------|---------|-------------|
| System.Text.Json | 9.0.x (built-in) | JSON serialization backing Schema.NET | Automatically used by Schema.NET. Already part of .NET 9 runtime. |

### Alternatives Considered
| Instead of | Could Use | Tradeoff |
|------------|-----------|----------|
| Schema.NET for JSON-LD | Hand-written JSON strings | Schema.NET provides type safety, auto-escaping, schema.org vocabulary correctness. Hand-written JSON is error-prone and lacks compile-time checks. |
| Schema.NET for JSON-LD | SeoTags NuGet package | SeoTags wraps multiple SEO concerns but is less popular (12K downloads) and adds unnecessary abstraction for this focused task. |
| MarkupString for script tags | Static JSON-LD in App.razor `<head>` section | Placing directly in App.razor head avoids RZ9992 entirely, but mixes concerns. Using MarkupString keeps structured data co-located with the page it describes and follows the established HeadContent pattern. |
| Inline HeadContent | Shared SeoMetadata.razor component | For a single page (homepage only), inline is simpler and matches the Phase 1 pattern. A shared component adds abstraction that is premature with only one indexed page. |

**Installation:**
```bash
dotnet add package Schema.NET --version 13.0.0
```

## Architecture Patterns

### Files to Modify
```
AutoDokas.csproj            # Add Schema.NET 13.0.0 package reference

Resources/
  Text.resx                 # Update HomeTitle value (currently "Pagrindinis")
  Text.Designer.cs          # Auto-regenerated from Text.resx

Components/Pages/
  Home.razor                # Add HeadContent with meta description, canonical, OG tags, JSON-LD
```

### Pattern 1: Meta Description, Canonical URL, and Open Graph Tags via HeadContent
**What:** A single `<HeadContent>` block in Home.razor containing all meta tags, the canonical link, and Open Graph meta properties.
**When to use:** Homepage page-specific SEO metadata.
**Example:**
```razor
@page "/"
@using AutoDokas.Resources

<PageTitle>@Text.HomeTitle</PageTitle>

<HeadContent>
    <meta name="description" content="Automobilio pirkimo pardavimo sutartis internetu. Užpildykite, pasirašykite ir atsisiųskite — greitai, saugiai, be spausdintuvo." />
    <link rel="canonical" href="https://autodokas.lt/" />
    <meta property="og:title" content="Automobilio pirkimo pardavimo sutartis internetu | AutoDokas" />
    <meta property="og:description" content="Automobilio pirkimo pardavimo sutartis internetu. Užpildykite, pasirašykite ir atsisiųskite — greitai, saugiai, be spausdintuvo." />
    <meta property="og:url" content="https://autodokas.lt/" />
    <meta property="og:type" content="website" />
    <meta property="og:site_name" content="AutoDokas" />
</HeadContent>
```
Source: [Microsoft Learn - Control head content in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content?view=aspnetcore-9.0)

**Key detail:** Multiple elements can be placed inside a single `<HeadContent>` block. They all get rendered into the `<head>` via `<HeadOutlet/>` in App.razor. The homepage uses static SSR (no `@rendermode` directive), so all HeadContent is rendered in the initial HTML response -- fully visible to crawlers.

### Pattern 2: JSON-LD Structured Data via MarkupString
**What:** Use Schema.NET to generate JSON-LD, then render it via `MarkupString` cast to bypass Blazor's RZ9992 script tag restriction.
**When to use:** Any JSON-LD structured data in Blazor components.
**Example:**
```razor
@using Schema.NET

@((MarkupString)_jsonLd)

@code {
    private string _jsonLd = string.Empty;

    protected override void OnInitialized()
    {
        var organization = new Organization
        {
            Name = "AutoDokas",
            Url = new Uri("https://autodokas.lt"),
        };
        _jsonLd = $"<script type=\"application/ld+json\">{organization.ToHtmlEscapedString()}</script>";
    }
}
```
Source: [GitHub - dotnet/aspnetcore#37230](https://github.com/dotnet/aspnetcore/issues/37230), [Schema.NET GitHub](https://github.com/RehanSaeed/Schema.NET)

**Critical detail:** Use `ToHtmlEscapedString()` not `ToString()` to prevent XSS when embedding Schema.NET output in HTML script tags. The `ToHtmlEscapedString()` method escapes HTML special characters in string values.

**Placement recommendation:** JSON-LD scripts should NOT go inside `<HeadContent>` because `MarkupString` + `HeadContent` combination has been reported to have inconsistent behavior. Instead, place the `@((MarkupString)_jsonLd)` directly in the page body near the top of Home.razor. Google explicitly states that JSON-LD can appear anywhere in the page body -- it does not need to be in `<head>`. This is simpler and more reliable than trying to combine MarkupString with HeadContent.

### Pattern 3: Schema.NET Object Creation for All Four Schemas
**What:** Create Organization, WebSite, WebPage, and Service JSON-LD objects using Schema.NET's strongly-typed classes.
**Example (all four schemas):**
```csharp
// Organization
var organization = new Organization
{
    Name = "AutoDokas",
    Url = new Uri("https://autodokas.lt"),
    // Logo can be added when an asset is available
};

// WebSite
var webSite = new WebSite
{
    Name = "AutoDokas",
    Url = new Uri("https://autodokas.lt"),
    InLanguage = "lt",
};

// WebPage
var webPage = new WebPage
{
    Name = "Automobilio pirkimo pardavimo sutartis internetu | AutoDokas",
    Description = "Automobilio pirkimo pardavimo sutartis internetu. Užpildykite, pasirašykite ir atsisiųskite — greitai, saugiai, be spausdintuvo.",
    Url = new Uri("https://autodokas.lt/"),
    InLanguage = "lt",
    IsPartOf = webSite,
};

// Service
var service = new Service
{
    Name = "Automobilio pirkimo pardavimo sutarties sudarymas internetu",
    Description = "Internetinė paslauga automobilio pirkimo pardavimo sutarčiai sudaryti, pasirašyti ir atsisiųsti PDF formatu.",
    Url = new Uri("https://autodokas.lt/"),
    Provider = organization,
    ServiceType = "Automobilio pirkimo pardavimo sutarties sudarymas",
    AreaServed = "Lietuva",
};
```
Source: [Schema.NET GitHub README](https://github.com/RehanSaeed/Schema.NET), [schema.org/Service](https://schema.org/Service), [schema.org/WebPage](https://schema.org/WebPage)

### Pattern 4: Resource String Update for Page Title
**What:** Update the `HomeTitle` resource string in Text.resx to contain the keyword-optimized title.
**Example:**
```xml
<data name="HomeTitle" xml:space="preserve">
    <value>Automobilio pirkimo pardavimo sutartis internetu | AutoDokas</value>
    <comment/>
</data>
```
**Key detail:** The `<PageTitle>@Text.HomeTitle</PageTitle>` in Home.razor already references this resource string, so only the .resx value needs changing. After editing Text.resx, the `Text.Designer.cs` file will be regenerated automatically on build.

### Anti-Patterns to Avoid
- **Putting JSON-LD in MarkupString inside HeadContent:** While technically possible, the combination of `MarkupString` within `<HeadContent>` has reported edge cases with rendering. Place JSON-LD `MarkupString` directly in the page body instead. Google parses JSON-LD from the body equally well as from the head.
- **Using separate `<script>` tags for each schema:** Combine all schemas into a single `<script type="application/ld+json">` block using `@graph` array, or use separate script tags -- both are valid, but separate tags are simpler when generating with Schema.NET since each object serializes independently.
- **Hardcoding the canonical URL without trailing slash:** The homepage canonical should always include the trailing slash (`https://autodokas.lt/` not `https://autodokas.lt`). The trailing slash is standard for domain-root URLs.
- **Setting og:title different from the page title:** The og:title should match or closely mirror the `<title>` tag to maintain consistency between SERP and social previews.
- **Using relative URLs in canonical or OG tags:** Always use absolute URLs with full protocol (https://) for canonical, og:url, and structured data URLs.

## Don't Hand-Roll

| Problem | Don't Build | Use Instead | Why |
|---------|-------------|-------------|-----|
| JSON-LD generation | Manual JSON string concatenation | Schema.NET 13.0.0 | Type safety, correct schema.org vocabulary, automatic JSON serialization, HTML escaping via `ToHtmlEscapedString()`. Manual JSON is error-prone (missing commas, wrong property names, no schema validation). |
| HTML head meta injection | Custom JavaScript to inject meta tags at runtime | Blazor `<HeadContent>` | JS-injected tags are invisible during SSR prerendering. Crawlers only see the initial HTML response. |
| Open Graph tag validation | Manual review of OG tag output | Facebook Sharing Debugger or opengraph.xyz | Tools verify actual crawler perception, catch issues invisible in source code review (e.g., encoding problems, missing required properties). |
| Schema validation | Manual JSON-LD review | Google Rich Results Test or Schema Markup Validator | Automated validation catches structural errors, missing required properties, and deprecated types. |

**Key insight:** Schema.NET eliminates the entire class of JSON-LD syntax and vocabulary errors. A typo in a hand-written `"@type": "Organziation"` would silently fail -- Schema.NET makes this a compile-time error.

## Common Pitfalls

### Pitfall 1: RZ9992 Compiler Error for Script Tags in Blazor Components
**What goes wrong:** Adding `<script type="application/ld+json">` directly in a .razor file triggers Blazor compiler error RZ9992: "Script tags should not be placed inside components."
**Why it happens:** Blazor's Razor compiler blocks all `<script>` tags in components to prevent unsafe JavaScript injection patterns.
**How to avoid:** Cast the entire script tag string to `MarkupString`: `@((MarkupString)"<script type=\"application/ld+json\">...</script>")`. Build the string in `@code` block and render with `@((MarkupString)_jsonLd)`.
**Warning signs:** Build failure with RZ9992 error.

### Pitfall 2: XSS via ToString() Instead of ToHtmlEscapedString()
**What goes wrong:** Using Schema.NET's `ToString()` method instead of `ToHtmlEscapedString()` can expose the page to cross-site scripting if any schema property values contain user-controllable data.
**Why it happens:** `ToString()` outputs raw JSON without HTML escaping. If a property value contains `</script>`, it would break out of the JSON-LD script tag.
**How to avoid:** Always use `ToHtmlEscapedString()` when embedding Schema.NET output in HTML `<script>` tags. For this phase, all values are hardcoded constants, so the risk is theoretical -- but using `ToHtmlEscapedString()` is the correct habit.
**Warning signs:** Schema.NET GitHub README explicitly warns about this.

### Pitfall 3: Meta Description Truncation in SERPs
**What goes wrong:** Google truncates meta descriptions longer than ~155-160 characters, showing "..." which looks unprofessional.
**Why it happens:** Writing too much text in the description, trying to include too many keywords.
**How to avoid:** Keep description under 155 characters for safety margin. Count characters carefully. Focus on one compelling sentence with the primary keyword.
**Warning signs:** Testing with SERP preview tools shows truncation.

### Pitfall 4: Duplicate Title Tag from PageTitle + HeadContent
**What goes wrong:** If both `<PageTitle>` and a manual `<title>` tag in `<HeadContent>` are used, two title tags appear in the HTML head.
**Why it happens:** `<PageTitle>` is the Blazor way to set `<title>`. Adding another `<title>` via HeadContent creates a duplicate.
**How to avoid:** Use only `<PageTitle>` for the title. Use `<HeadContent>` for meta tags, canonical, and OG tags only. This is already the pattern in the codebase.
**Warning signs:** View page source shows two `<title>` elements.

### Pitfall 5: MarkupString Inside HeadContent Edge Cases
**What goes wrong:** Combining `MarkupString` rendering inside `<HeadContent>` can have inconsistent behavior across Blazor render modes because `HeadContent` expects standard Razor markup, not raw HTML strings.
**Why it happens:** `HeadContent` processes child content differently than normal component body. MarkupString is injected as raw HTML which may not be properly captured by the HeadOutlet.
**How to avoid:** Place JSON-LD `MarkupString` output directly in the page body, not inside `<HeadContent>`. Google supports JSON-LD in the body equally well. Keep `<HeadContent>` for standard meta/link elements only.
**Warning signs:** JSON-LD appears in page body but not in `<head>` section, or vice versa.

### Pitfall 6: Google Retired Sitelinks Search Box (November 2024)
**What goes wrong:** Adding a `SearchAction` to the WebSite schema for a sitelinks search box feature that no longer exists.
**Why it happens:** Many SEO guides and examples still include SearchAction for WebSite schemas, but Google retired this feature in November 2024.
**How to avoid:** Omit `potentialAction`/`SearchAction` from the WebSite schema. It adds unnecessary complexity with no benefit. The site has no internal search functionality anyway.
**Warning signs:** Outdated SEO guides recommending SearchAction for WebSite.

## Code Examples

Verified patterns from official sources:

### Complete Home.razor with SEO Tags
```razor
@page "/"
@using AutoDokas.Resources
@using Schema.NET

<PageTitle>@Text.HomeTitle</PageTitle>

<HeadContent>
    <meta name="description" content="Automobilio pirkimo pardavimo sutartis internetu. Užpildykite, pasirašykite ir atsisiųskite — greitai, saugiai, be spausdintuvo." />
    <link rel="canonical" href="https://autodokas.lt/" />
    <meta property="og:title" content="Automobilio pirkimo pardavimo sutartis internetu | AutoDokas" />
    <meta property="og:description" content="Automobilio pirkimo pardavimo sutartis internetu. Užpildykite, pasirašykite ir atsisiųskite — greitai, saugiai, be spausdintuvo." />
    <meta property="og:url" content="https://autodokas.lt/" />
    <meta property="og:type" content="website" />
    <meta property="og:site_name" content="AutoDokas" />
</HeadContent>

@((MarkupString)_jsonLd)

<!-- Hero Section -->
<section class="hero">
    <!-- ... existing hero content ... -->
</section>

<!-- ... rest of existing page content ... -->

@code {
    private string _jsonLd = string.Empty;

    protected override void OnInitialized()
    {
        var organization = new Organization
        {
            Name = "AutoDokas",
            Url = new Uri("https://autodokas.lt"),
        };

        var webSite = new WebSite
        {
            Name = "AutoDokas",
            Url = new Uri("https://autodokas.lt"),
            InLanguage = "lt",
        };

        var webPage = new WebPage
        {
            Name = "Automobilio pirkimo pardavimo sutartis internetu | AutoDokas",
            Description = "Automobilio pirkimo pardavimo sutartis internetu. Užpildykite, pasirašykite ir atsisiųskite — greitai, saugiai, be spausdintuvo.",
            Url = new Uri("https://autodokas.lt/"),
            InLanguage = "lt",
            IsPartOf = webSite,
        };

        var service = new Service
        {
            Name = "Automobilio pirkimo pardavimo sutarties sudarymas internetu",
            Description = "Internetinė paslauga automobilio pirkimo pardavimo sutarčiai sudaryti, pasirašyti ir atsisiųsti PDF formatu.",
            Url = new Uri("https://autodokas.lt/"),
            Provider = organization,
            ServiceType = "Automobilio pirkimo pardavimo sutarties sudarymas",
            AreaServed = "Lietuva",
        };

        var schemas = new ISchemaType[] { organization, webSite, webPage, service };
        var jsonParts = schemas.Select(s => s.ToHtmlEscapedString());
        _jsonLd = string.Join("\n", jsonParts.Select(j =>
            $"<script type=\"application/ld+json\">{j}</script>"));
    }
}
```

### Updated Text.resx Entry for HomeTitle
```xml
<data name="HomeTitle" xml:space="preserve">
    <value>Automobilio pirkimo pardavimo sutartis internetu | AutoDokas</value>
    <comment/>
</data>
```

### NuGet Package Addition
```xml
<!-- In AutoDokas.csproj -->
<PackageReference Include="Schema.NET" Version="13.0.0" />
```

## Keyword Research Findings

### Primary Keyword Analysis
**Term:** "automobilio pirkimo pardavimo sutartis" (vehicle purchase-sale contract)
**Confidence:** HIGH -- verified across all top-ranking Lithuanian competitors

**Evidence from competitor title tags (SERP analysis):**
| Competitor | Title Tag | Keyword Usage |
|------------|-----------|---------------|
| autosutartis.lt | "Automobilio pirkimo-pardavimo sutartis internetu \| Autosutartis.lt" | Exact match + "internetu" + brand |
| ledauto.lt | "Automobilio pirkimo-pardavimo sutartis" | Exact match |
| reidasofficial.lt | "Automobilio pirkimo pardavimo sutartis 2026" | Exact match + year |
| autohubas.lt | "Automobilio pirkimo-pardavimo sutartis \| Autohubas.lt" | Exact match + brand |
| regitra.lt | "Pirkimo-pardavimo sutartis" | Shorter variant (government site) |
| esablonai.lt | "Automobilio pirkimo - pardavimo sutartis sablonas forma pavyzdys" | Exact match + modifiers |

**Key observations:**
1. ALL top competitors use the exact phrase "automobilio pirkimo pardavimo sutartis" or its hyphenated form "pirkimo-pardavimo"
2. The direct competitor autosutartis.lt adds "internetu" (online) to differentiate -- matches AutoDokas's positioning
3. Format "[Primary keyword] | [Brand]" is the dominant pattern among competitors
4. The hero heading already uses "Automobilio pirkimo pardavimo sutartis internetu" -- aligning title with h1 is an SEO best practice

### Recommended Title
**"Automobilio pirkimo pardavimo sutartis internetu | AutoDokas"**
- Contains primary keyword
- Adds "internetu" (online) as differentiator matching service positioning
- Brand at end following competitor convention
- ~58 characters -- well within the 60-character display limit
- Aligns with existing h1 heading text

### Recommended Meta Description
**"Automobilio pirkimo pardavimo sutartis internetu. Užpildykite, pasirašykite ir atsisiųskite -- greitai, saugiai, be spausdintuvo."**
- Character count: ~128 characters -- safely under 155-character limit
- Contains primary keyword in first sentence
- Communicates key benefits: fill in, sign, download (the three steps)
- Differentiators: fast, safe, no printer needed
- Reads as a natural, compelling snippet in Lithuanian

### Hyphenation Note
**"pirkimo pardavimo" vs "pirkimo-pardavimo":** Both forms appear in search results. Google treats hyphens as word separators, so both versions match the same queries. The non-hyphenated form "pirkimo pardavimo" is used in the existing hero heading (`@Text.HeroTitle`), so the title should match. Competitor autosutartis.lt uses the hyphenated form. Either works for SEO -- consistency with the existing codebase (no hyphen) is recommended.

## State of the Art

| Old Approach | Current Approach | When Changed | Impact |
|--------------|------------------|--------------|--------|
| WebSite schema with SearchAction for sitelinks search box | WebSite schema without SearchAction | November 2024 (Google retired feature) | Do NOT add SearchAction to WebSite schema -- the feature no longer exists |
| Microdata or RDFa embedded in HTML | JSON-LD in script tags | Google preference since ~2017 | JSON-LD is Google's recommended format for structured data |
| Hand-written JSON-LD strings | Schema.NET type-safe generation | Schema.NET available since 2017 | Compile-time validation, auto-escaping, correct vocabulary |
| Meta keywords tag | Not used by Google | Since 2009 | Do not add `<meta name="keywords">` -- Google has ignored it since 2009 |
| Multiple separate JSON-LD scripts per page | Either multiple scripts or single @graph | Both remain valid | Using separate scripts per schema type is simpler with Schema.NET |

**Deprecated/outdated:**
- `<meta name="keywords">`: Ignored by Google since 2009. Not needed.
- SearchAction in WebSite schema for sitelinks search box: Google retired in November 2024.
- Microdata format for structured data: JSON-LD is the Google-preferred format.

## Open Questions

1. **Schema.NET `ISchemaType` interface for combining schemas**
   - What we know: Each Schema.NET object has `ToString()` and `ToHtmlEscapedString()`. Each serializes to a complete JSON-LD object with `@context`.
   - What's unclear: Whether Schema.NET supports a built-in way to combine multiple schemas into a single `@graph` array in one script tag.
   - Recommendation: Use separate `<script type="application/ld+json">` tags for each schema (Organization, WebSite, WebPage, Service). Google supports multiple JSON-LD blocks on a page equally well as a single @graph block. Separate tags are simpler with Schema.NET.

2. **Organization schema properties with limited business info**
   - What we know: User did not provide business address, phone, email, or social media links for the Organization schema.
   - What's unclear: Whether a minimal Organization schema (just name + url) is sufficient for Google.
   - Recommendation: Use minimal Organization schema with `name` and `url`. A minimal but valid schema is better than no schema. Properties can be enriched in future phases when business info is available.

3. **Text.Designer.cs auto-regeneration**
   - What we know: The `.resx` file has `<Generator>PublicResXFileCodeGenerator</Generator>` configuration, so `Text.Designer.cs` should regenerate on build.
   - What's unclear: Whether the build pipeline always auto-regenerates, or if manual regeneration is needed.
   - Recommendation: After updating `Text.resx`, run `dotnet build` to verify `Text.Designer.cs` is updated. If not, regenerate manually with `dotnet tool run resgen` or by right-clicking in Visual Studio.

## Sources

### Primary (HIGH confidence)
- [Microsoft Learn - Control head content in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content?view=aspnetcore-9.0) - HeadContent component usage, PageTitle component
- [Schema.NET GitHub Repository](https://github.com/RehanSaeed/Schema.NET) - API usage, ToHtmlEscapedString() method, version info
- [Schema.NET NuGet Package](https://www.nuget.org/packages/Schema.NET) - v13.0.0, .NET 6+/netstandard2.0, System.Text.Json dependency
- [GitHub - dotnet/aspnetcore#37230](https://github.com/dotnet/aspnetcore/issues/37230) - RZ9992 workaround with MarkupString, issue status (closed as not planned)
- [schema.org/Service](https://schema.org/Service) - Service schema properties and example
- [schema.org/WebPage](https://schema.org/WebPage) - WebPage schema properties
- [The Open Graph Protocol](https://ogp.me/) - OG tag specifications and required properties
- Codebase analysis - Home.razor, App.razor, Text.resx, AutoDokas.csproj directly read and verified

### Secondary (MEDIUM confidence)
- [Semrush - Canonical URL Guide](https://www.semrush.com/blog/canonical-url-guide/) - Self-referential canonical best practices, trailing slash
- [SEO-Wiki - Self-referencing Canonicals](https://www.seo-day.de/wiki/technisches-seo/crawling-indexierung/canonical-tags/self-referencing.php?lang=en) - Homepage trailing slash standard
- [jsonld.com - Organization Example](https://jsonld.com/organization/) - Organization JSON-LD property reference
- [jsonld.com - WebPage Example](https://jsonld.com/web-page/) - WebPage JSON-LD property reference
- [Rehansaeed.com - Structured Data Using Schema.NET](https://rehansaeed.com/structured-data-using-schema-net/) - Schema.NET usage patterns
- Competitor analysis: autosutartis.lt homepage (title, meta description, JSON-LD schemas verified via WebFetch)

### Tertiary (LOW confidence)
- Google Trends keyword volume data was not directly accessible (HTTP 429 rate limit). Keyword recommendations are based on competitor SERP analysis instead, which is arguably more reliable for this specific Lithuanian niche.

## Metadata

**Confidence breakdown:**
- Standard stack: HIGH - Schema.NET 13.0.0 verified on NuGet (supports .NET 6+, uses System.Text.Json, compatible with .NET 9). MarkupString workaround for RZ9992 confirmed as standard pattern via official GitHub issue. HeadContent used in Phase 1 already.
- Architecture: HIGH - All target files read (Home.razor, App.razor, Text.resx, AutoDokas.csproj). Existing patterns (HeadContent for noindex, PageTitle for titles, resource strings) are directly reusable. Static SSR on homepage ensures crawler visibility.
- Pitfalls: HIGH - RZ9992 workaround well-documented. XSS risk with ToString() vs ToHtmlEscapedString() documented in Schema.NET README. MarkupString + HeadContent edge case documented. Keyword research cross-verified against 6+ competitor title tags.
- Keyword research: MEDIUM - Based on competitor SERP title analysis (6+ sites) rather than direct Google Trends volume data. Competitor consensus on "automobilio pirkimo pardavimo sutartis" is strong evidence.

**Research date:** 2026-03-03
**Valid until:** 2026-04-03 (stable domain -- schema.org specs, OG protocol, and Blazor HeadContent change infrequently)
