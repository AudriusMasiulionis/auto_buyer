# Architecture Research: Blazor Server SEO Infrastructure

**Domain:** Technical SEO for Blazor Server application
**Researched:** 2026-03-02
**Confidence:** HIGH

## System Overview

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Blazor Rendering Pipeline                         │
│                                                                      │
│  ┌──────────────┐   ┌──────────────┐   ┌──────────────────────┐     │
│  │  PageTitle   │   │ HeadContent  │   │ Structured Data      │     │
│  │  Component   │   │ Component    │   │ (JSON-LD via         │     │
│  │  (per page)  │   │ (per page)   │   │  HeadContent)        │     │
│  └──────┬───────┘   └──────┬───────┘   └──────────┬───────────┘     │
│         │                  │                       │                  │
│         └──────────────────┼───────────────────────┘                  │
│                            │                                         │
│                    ┌───────▼────────┐                                │
│                    │  HeadOutlet    │  (in App.razor <head>)          │
│                    │  Component    │  Renders all head content        │
│                    └───────┬────────┘                                │
│                            │                                         │
├────────────────────────────┼─────────────────────────────────────────┤
│              SSR / Prerendering Layer                                │
│                            │                                         │
│  First render (static HTML) includes all <head> tags                │
│  Crawlers see complete HTML with meta tags, JSON-LD, titles         │
│  Second render (SignalR) makes page interactive                     │
│                            │                                         │
├────────────────────────────┼─────────────────────────────────────────┤
│              ASP.NET Core Middleware Pipeline                        │
│                                                                      │
│  ┌──────────────┐   ┌──────────────┐   ┌──────────────────────┐     │
│  │ robots.txt   │   │ sitemap.xml  │   │ Static Files         │     │
│  │ (static file │   │ (Minimal API │   │ Middleware            │     │
│  │  in wwwroot) │   │  endpoint)   │   │                      │     │
│  └──────────────┘   └──────────────┘   └──────────────────────┘     │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### How It Works (Current App Context)

The AutoDokas app is a .NET 9 Blazor Server application. It uses `MapRazorComponents<App>().AddInteractiveServerRenderMode()` in `Program.cs`. The `App.razor` component contains the full HTML document shell including `<HeadOutlet/>` in the `<head>` section. Pages use `@rendermode InteractiveServer` with prerendering enabled by default.

**Critical SEO advantage of this setup:** Blazor Server with prerendering renders the page statically on the first request. Crawlers (Google, Bing) receive fully-rendered HTML including all `<head>` tags from `PageTitle` and `HeadContent` components. They never establish a SignalR connection -- they just see the static HTML. This means SEO tags injected via `HeadContent` are crawler-visible without any special configuration.

## Component Responsibilities

| Component | Responsibility | Typical Implementation |
|-----------|----------------|------------------------|
| **SeoMetadata (new)** | Reusable component that emits meta description, canonical URL, Open Graph tags, and robots directive into `<head>` for each page | Razor component using `HeadContent`; accepts parameters for title, description, canonical URL, OG image |
| **StructuredData (new)** | Renders JSON-LD `<script>` blocks into the page for search engine rich results | Razor component using `HeadContent`; accepts a typed C# object and serializes to JSON-LD |
| **PageTitle (existing)** | Sets the `<title>` element | Built-in Blazor component, already used on all pages |
| **HeadOutlet (existing)** | Collects and renders all `PageTitle` and `HeadContent` output into the `<head>` | Built-in Blazor component, already in `App.razor` |
| **robots.txt (existing)** | Controls crawler access to paths | Static file in `wwwroot/`; needs content fix only |
| **sitemap.xml (replace)** | Lists all indexable URLs with lastmod dates | Replace static file with Minimal API endpoint in `Program.cs` |
| **site.webmanifest (existing)** | PWA manifest with app name/description | Static file in `wwwroot/`; needs `name` and `description` fields populated |
| **App.razor (existing)** | HTML document shell; contains `<html lang>`, `<head>`, `<HeadOutlet/>` | Needs `lang="lt"` fix (currently `lang="en"`) |

## Recommended Project Structure

```
Components/
├── Seo/                        # NEW: SEO components
│   ├── SeoMetadata.razor       # Meta description, canonical, OG tags, robots
│   └── StructuredData.razor    # JSON-LD structured data renderer
├── Pages/
│   ├── Home.razor              # Uses SeoMetadata + StructuredData
│   ├── Contract/
│   │   └── Contract.razor      # Uses SeoMetadata (noindex for /contract/{id})
│   └── Legal/
│       ├── TermsAndConditions.razor  # Uses SeoMetadata
│       └── PrivacyPolicy.razor       # Uses SeoMetadata
├── Layout/
│   └── MainLayout.razor        # No SEO changes needed
└── App.razor                   # Fix lang="lt", keep HeadOutlet

wwwroot/
├── robots.txt                  # Fix: Allow all public paths
└── site.webmanifest            # Fix: Add name, short_name, description

Services/
└── (none needed)               # No service layer for this scope
```

### Structure Rationale

- **Components/Seo/:** Dedicated folder for SEO-specific reusable components. Keeps SEO concerns separated from page logic. Only two files needed for this project's scope.
- **No service layer:** For a site with 4 static routes and no dynamic SEO data from a database, a service layer (PageMetaDataService, JsonLdService) adds unnecessary abstraction. Page-level parameters on the `SeoMetadata` component are simpler and sufficient. If the site grows to 20+ pages or pulls SEO data from a CMS, then introduce a service.
- **Sitemap as Minimal API:** The sitemap should be a Minimal API endpoint rather than a static XML file. This allows adding `lastmod` dates and ensures it stays in sync with actual routes without manual updates.

## Architectural Patterns

### Pattern 1: Per-Page HeadContent via Reusable Component

**What:** A single `SeoMetadata.razor` component that each page includes, passing its specific title, description, canonical URL, and Open Graph data as parameters. The component uses `<HeadContent>` internally to inject tags into the `<head>`.

**When to use:** Always. Every indexable page should include this component.

**Trade-offs:** Simple and explicit (each page declares its own SEO data), but requires updating each page file when adding the component. For 4 pages, this is trivial.

**Example:**

```razor
@* Components/Seo/SeoMetadata.razor *@

<HeadContent>
    <meta name="description" content="@Description" />
    <link rel="canonical" href="@CanonicalUrl" />
    @if (!string.IsNullOrEmpty(Robots))
    {
        <meta name="robots" content="@Robots" />
    }

    @* Open Graph *@
    <meta property="og:title" content="@Title" />
    <meta property="og:description" content="@Description" />
    <meta property="og:url" content="@CanonicalUrl" />
    <meta property="og:type" content="website" />
    <meta property="og:locale" content="lt_LT" />
    <meta property="og:site_name" content="AutoDokas" />
    @if (!string.IsNullOrEmpty(OgImage))
    {
        <meta property="og:image" content="@OgImage" />
    }
</HeadContent>

@code {
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public string Description { get; set; } = "";
    [Parameter] public string CanonicalUrl { get; set; } = "";
    [Parameter] public string? OgImage { get; set; }
    [Parameter] public string? Robots { get; set; }
}
```

**Usage in a page:**

```razor
@page "/"

<PageTitle>Automobilio pirkimo pardavimo sutartis internetu | AutoDokas</PageTitle>

<SeoMetadata
    Title="Automobilio pirkimo pardavimo sutartis internetu | AutoDokas"
    Description="Nemokamai sukurkite automobilio pirkimo pardavimo sutarti internetu. Teisiškai galiojanti skaitmeninė sutartis per kelias minutes."
    CanonicalUrl="https://autodokas.lt/"
    OgImage="https://autodokas.lt/img/og-preview.png" />

<StructuredData Value="@_organizationSchema" />
<StructuredData Value="@_serviceSchema" />

@* ... rest of page content ... *@
```

### Pattern 2: JSON-LD Structured Data via HeadContent

**What:** A `StructuredData.razor` component that accepts a C# object (anonymous or typed), serializes it to JSON, and renders it as a `<script type="application/ld+json">` block inside `<HeadContent>`.

**When to use:** On pages where you want rich search results (homepage: Organization + Service schema; contract page: WebPage schema).

**Trade-offs:** Using anonymous objects keeps things simple but loses type safety. For this project's 3-4 schemas, anonymous objects are fine. If schema complexity grows, consider the `Schema.NET` NuGet package for strongly-typed schema.org classes.

**Example:**

```razor
@* Components/Seo/StructuredData.razor *@
@using System.Text.Json

<HeadContent>
    <script type="application/ld+json">@((MarkupString)JsonSerializer.Serialize(Value, _jsonOptions))</script>
</HeadContent>

@code {
    [Parameter, EditorRequired] public object Value { get; set; } = default!;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
```

**Usage for Organization schema:**

```csharp
private object _organizationSchema => new
{
    @context = "https://schema.org",
    @type = "Organization",
    name = "AutoDokas",
    url = "https://autodokas.lt",
    contactPoint = new
    {
        @type = "ContactPoint",
        email = "contact@autodokas.lt",
        contactType = "customer service"
    }
};
```

### Pattern 3: Sitemap as Minimal API Endpoint

**What:** Replace the static `wwwroot/sitemap.xml` with a Minimal API endpoint that generates XML dynamically. This keeps the sitemap in sync with actual routes and allows setting accurate `lastmod` dates.

**When to use:** Always preferred over static sitemap files for maintainability, even for small sites.

**Trade-offs:** Slightly more code than a static file, but prevents the sitemap from going stale. For 4 URLs, the complexity is minimal.

**Example:**

```csharp
// In Program.cs or a SitemapEndpoints.cs extension class
app.MapGet("/sitemap.xml", () =>
{
    var urls = new[]
    {
        new { Loc = "https://autodokas.lt/", Priority = "1.0", LastMod = "2026-03-01" },
        new { Loc = "https://autodokas.lt/contract", Priority = "0.8", LastMod = "2026-03-01" },
        new { Loc = "https://autodokas.lt/terms-and-conditions", Priority = "0.3", LastMod = "2026-02-01" },
        new { Loc = "https://autodokas.lt/privacy-policy", Priority = "0.3", LastMod = "2026-02-01" },
    };

    var xml = $"""
        <?xml version="1.0" encoding="UTF-8"?>
        <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
        {string.Join("\n", urls.Select(u => $"""
          <url>
            <loc>{u.Loc}</loc>
            <lastmod>{u.LastMod}</lastmod>
            <priority>{u.Priority}</priority>
          </url>
        """))}
        </urlset>
        """;

    return Results.Content(xml, "application/xml");
}).ExcludeFromDescription();
```

**Important:** The static `wwwroot/sitemap.xml` file must be deleted when the endpoint is added, otherwise `UseStaticFiles()` will serve the static file before the endpoint is reached. Alternatively, the endpoint can be mapped before `UseStaticFiles()`.

## Data Flow

### Meta Tags: Page Definition to HTML Head

```
Page Component (e.g., Home.razor)
    │
    ├── <PageTitle>...</PageTitle>
    │       │
    │       └──→ HeadOutlet renders <title> in <head>
    │
    ├── <SeoMetadata Title="..." Description="..." ... />
    │       │
    │       └──→ SeoMetadata.razor internally uses <HeadContent>
    │               │
    │               └──→ HeadOutlet renders <meta>, <link>, <meta property="og:..."> in <head>
    │
    └── <StructuredData Value="@schema" />
            │
            └──→ StructuredData.razor internally uses <HeadContent>
                    │
                    └──→ HeadOutlet renders <script type="application/ld+json"> in <head>
```

### Crawler Request Flow

```
Google/Bing Crawler
    │
    ├── GET /robots.txt
    │       └── Static file served by UseStaticFiles() middleware
    │           Returns: Allow all public paths, Sitemap URL
    │
    ├── GET /sitemap.xml
    │       └── Minimal API endpoint returns XML with all 4 public URLs
    │
    ├── GET / (homepage)
    │       └── Blazor SSR prerender:
    │           1. Server renders App.razor → MainLayout → Home.razor
    │           2. HeadOutlet collects PageTitle + all HeadContent from SeoMetadata + StructuredData
    │           3. Returns complete HTML with <head> containing:
    │              - <title>Automobilio pirkimo...</title>
    │              - <meta name="description" content="...">
    │              - <link rel="canonical" href="https://autodokas.lt/">
    │              - <meta property="og:title" content="...">
    │              - <script type="application/ld+json">{...}</script>
    │           4. Crawler parses static HTML; never initiates SignalR
    │
    └── GET /contract (public contract page)
            └── Same SSR prerender flow, different meta content
```

### Key Data Flow: Why Prerendering Makes This Work

The `@rendermode InteractiveServer` directive on the Contract page includes prerendering by default in .NET 8+/.NET 9. During prerendering:

1. The Blazor runtime executes the component tree server-side
2. `PageTitle` and `HeadContent` components register their content with `HeadOutlet`
3. `HeadOutlet` renders all collected content into the `<head>` section
4. The complete HTML (including head tags) is sent as the initial response
5. Crawlers receive this fully-formed HTML and index it

The interactive SignalR connection is only established when a real user's browser loads `blazor.web.js`. Crawlers never reach this stage.

## Build Order (Dependency Graph)

The following build order respects dependencies -- each step can be independently verified before moving to the next.

```
Phase 1: Foundation (no dependencies)
├── 1a. Fix robots.txt content (Allow public paths)
├── 1b. Fix App.razor lang="lt"
└── 1c. Fix site.webmanifest (add name, description)

Phase 2: Core SEO Components (depends on nothing, enables Phase 3)
├── 2a. Create SeoMetadata.razor component
└── 2b. Create StructuredData.razor component

Phase 3: Page Integration (depends on Phase 2)
├── 3a. Add SeoMetadata to Home.razor
├── 3b. Add SeoMetadata to Contract.razor (with noindex for /contract/{id} and /buyer/{id})
├── 3c. Add SeoMetadata to TermsAndConditions.razor
├── 3d. Add SeoMetadata to PrivacyPolicy.razor
└── 3e. Add StructuredData (Organization + Service JSON-LD) to Home.razor

Phase 4: Sitemap (independent, can be done in parallel with Phase 2-3)
├── 4a. Create sitemap Minimal API endpoint
└── 4b. Delete static wwwroot/sitemap.xml
```

**Why this order:**
- Phase 1 items are independent file edits with immediate impact (unblocks crawlers)
- Phase 2 creates reusable components that Phase 3 consumes
- Phase 3 is the bulk of the work, applying SEO to each page
- Phase 4 is independent of the component work and can be parallelized

## Anti-Patterns

### Anti-Pattern 1: Over-Engineered SEO Service Layer

**What people do:** Create `IPageMetaDataService`, `ISeoService`, `IJsonLdService`, `ICanonicalUrlProvider`, register them all in DI, and wire them with cascading parameters or inject them into every page.

**Why it's wrong:** For a site with 4 static public pages and no CMS or database-driven SEO data, this adds 3-4 service files, DI registration, and abstraction layers for zero benefit. The pages themselves know their titles and descriptions at compile time.

**Do this instead:** Use a simple parameter-based component (`SeoMetadata.razor`). Each page passes its own strings. If the site grows to 20+ pages or needs CMS-driven meta data, then introduce a service.

### Anti-Pattern 2: Putting Meta Tags Directly in App.razor

**What people do:** Add static `<meta name="description">` tags directly in `App.razor`'s `<head>` section, thinking it applies globally.

**Why it's wrong:** A single meta description for all pages is an SEO anti-pattern. Every page needs its own unique description. Also, static tags in `App.razor` cannot be overridden per-page -- they coexist with `HeadContent` tags, potentially causing duplicates.

**Do this instead:** Keep `App.razor` clean (only `HeadOutlet`, viewport, charset, stylesheets). Let each page declare its own meta tags via `HeadContent` / `SeoMetadata` component.

### Anti-Pattern 3: Using JavaScript to Inject Meta Tags

**What people do:** Use JS interop (`IJSRuntime`) to dynamically set meta tags after the page loads.

**Why it's wrong:** Crawlers execute the prerendered HTML. JS-injected meta tags are not present during prerender and will not be indexed. This completely defeats the purpose.

**Do this instead:** Use Blazor's `HeadContent` component, which participates in server-side prerendering and ensures tags are in the initial HTML response.

### Anti-Pattern 4: Duplicate PageTitle and HeadContent Declarations

**What people do:** Set `<PageTitle>` in the layout AND in individual pages, or put the same `<HeadContent>` meta tags in both places.

**Why it's wrong:** When multiple `PageTitle` components render, the last one wins (top-down rendering order). This can cause unexpected titles. Similarly, duplicate meta descriptions confuse crawlers.

**Do this instead:** Set `PageTitle` only in individual pages (not the layout). The `SeoMetadata` component should be the single source for meta tags on each page.

## Integration Points

### Internal Boundaries

| Boundary | Communication | Notes |
|----------|---------------|-------|
| SeoMetadata <-> HeadOutlet | Blazor component tree (HeadContent -> HeadOutlet) | Automatic via Blazor's rendering pipeline; no manual wiring needed |
| StructuredData <-> HeadOutlet | Blazor component tree (HeadContent -> HeadOutlet) | Same mechanism as SeoMetadata |
| Sitemap endpoint <-> Middleware pipeline | Minimal API registered in Program.cs | Must ensure static file middleware does not intercept `/sitemap.xml` path |
| robots.txt <-> Static files middleware | Served directly by UseStaticFiles() | Simple static file; no code integration needed |

### External Services

| Service | Integration Pattern | Notes |
|---------|---------------------|-------|
| Google Search Console | Validates robots.txt, sitemap.xml, and indexed pages | No code integration; external tool reads the endpoints |
| Google/Bing crawlers | HTTP GET requests to all public URLs | Receive prerendered HTML with full SEO metadata |
| Social media platforms | Read Open Graph meta tags when URL is shared | Tags must be in prerendered HTML (they are, via HeadContent) |

## Scaling Considerations

| Scale | Architecture Adjustments |
|-------|--------------------------|
| 4 pages (current) | Parameter-based SeoMetadata component, hardcoded sitemap entries. No services needed. |
| 10-20 pages | Still parameter-based, but consider a static dictionary of page metadata to keep page files cleaner. Sitemap endpoint reads from a shared route registry. |
| 50+ pages / CMS-driven | Introduce `IPageMetaDataService` backed by database or CMS. Sitemap generates dynamically from DB. Consider `Schema.NET` NuGet for typed JSON-LD. |

### Scaling Priorities

1. **First bottleneck:** Manual maintenance of meta data across many pages. Solve by centralizing metadata in a dictionary or config file.
2. **Second bottleneck:** Sitemap growing beyond manual management. Solve by auto-discovering routes via reflection (`@page` attributes) or database queries.

## Sources

- [Microsoft Learn: Control head content in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content) -- HIGH confidence (official docs)
- [Microsoft Learn: Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes) -- HIGH confidence (official docs, confirms prerendering is default)
- [Telerik: Controlling HTML HEAD Section in Blazor](https://www.telerik.com/blogs/blazor-basics-controlling-html-head-section-blazor) -- MEDIUM confidence (reputable source, aligns with official docs)
- [GhostlyInc: Blazor SEO Meta Data Component](https://ghostlyinc.com/en-US/blazor-seo-meta-data-component/) -- MEDIUM confidence (architecture pattern verified against official docs)
- [dotnet/aspnetcore Issue #54300: Blazor Server .NET 8 SEO head tags](https://github.com/dotnet/aspnetcore/issues/54300) -- HIGH confidence (official repo, documents known limitations)
- [dotnet/aspnetcore Issue #43128: Meta tags not detected by social media](https://github.com/dotnet/aspnetcore/issues/43128) -- HIGH confidence (official repo, confirms HeadContent works with prerendering)
- [Scott Hanselman: Dynamic robots.txt for ASP.NET Core](https://www.hanselman.com/blog/dynamically-generating-robotstxt-for-aspnet-core-sites-based-on-environment) -- MEDIUM confidence (authoritative .NET voice)
- [Khalid Abuhakmeh: Generate Sitemaps for ASP.NET Core](https://khalidabuhakmeh.com/generate-sitemaps-for-all-of-aspnet-core) -- MEDIUM confidence (reputable source, pattern verified)

---
*Architecture research for: Blazor Server SEO Infrastructure*
*Researched: 2026-03-02*
