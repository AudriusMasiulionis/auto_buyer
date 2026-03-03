# Phase 2: Homepage SEO - Context

**Gathered:** 2026-03-03
**Status:** Ready for planning

<domain>
## Phase Boundary

Optimize the homepage with keyword-rich title, meta description, canonical URL, Open Graph tags, and JSON-LD structured data (Organization, WebSite, WebPage, Service). All changes target Home.razor and App.razor only. Keyword research from Google Trends should inform title and meta description choices.

</domain>

<decisions>
## Implementation Decisions

### Page Title
- Replace current "Pagrindinis" (Home) with keyword-optimized Lithuanian title
- Research Google Trends for "automobilio pirkimo pardavimo sutartis" and related terms during planning
- Format: "[Primary keyword] | AutoDokas" (brand at end)

### Meta Description
- Lithuanian, max 160 chars, keyword-rich
- Should read as a compelling SERP snippet that makes users click
- Mention the key benefit: online, no printer needed, digital signing

### Canonical URL
- Self-referential canonical on homepage: https://autodokas.lt/

### Open Graph Tags
- og:title, og:description, og:url, og:type (website), og:site_name (AutoDokas)
- No OG image for v1 — deferred to v2 (needs design work, flagged in STATE.md as blocker)

### Structured Data (JSON-LD)
- Organization schema: AutoDokas brand info
- WebSite schema: site-level info with search action if applicable
- WebPage schema: homepage-specific metadata
- Service schema: describe the vehicle sale contract generation tool
- Use Schema.NET NuGet package (v13.0.0) for type-safe JSON-LD generation
- Use MarkupString workaround for Blazor RZ9992 (script tags in components)

### Claude's Discretion
- Exact keyword phrasing in title and description (informed by Google Trends research)
- JSON-LD property values for Organization (no specific business info provided — use domain info)
- Whether to place structured data in Home.razor or App.razor
- SeoMetadata reusable component design vs inline HeadContent

</decisions>

<code_context>
## Existing Code Insights

### Reusable Assets
- `<HeadOutlet/>` in App.razor line 36: Renders all HeadContent into HTML head
- `<PageTitle>` on Home.razor line 4: Currently uses `@Text.HomeTitle` resource string
- Resource strings in `AutoDokas.Resources.Text`: Localized strings for all page content
- Hero section already has keyword-rich h1: `@Text.HeroTitle` = "Automobilio pirkimo pardavimo sutartis internetu"

### Established Patterns
- Phase 1 used inline `<HeadContent>` per page for noindex tags
- Static SSR on homepage — HeadContent renders in initial HTML (crawler-friendly)
- Resource strings via `@Text.*` pattern for all user-visible text

### Integration Points
- `Components/Pages/Home.razor` — add HeadContent with meta description, canonical, OG tags
- `Components/App.razor` — potential location for site-wide structured data (Organization, WebSite)
- `Resources/Text.resx` (or similar) — update HomeTitle resource string for SEO title
- NuGet: Schema.NET 13.0.0 needs to be added to project

</code_context>

<specifics>
## Specific Ideas

- Primary keyword: "automobilio pirkimo pardavimo sutartis" (vehicle purchase-sale contract)
- Domain brand: AutoDokas.lt
- Competitor autosutartis.lt already has keyword-rich titles and JSON-LD schemas — need to match or exceed
- Hero heading already contains the primary keyword — title tag should align with it

</specifics>

<deferred>
## Deferred Ideas

- OG image asset (1200x630px) — needs design work, deferred to v2 (ESEO-03)
- FAQ schema — requires content creation, deferred to v2 (CONT-03)

</deferred>

---

*Phase: 02-homepage-seo*
*Context gathered: 2026-03-03*
