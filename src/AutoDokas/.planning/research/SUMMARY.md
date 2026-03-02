# Project Research Summary

**Project:** AutoDokas SEO Optimization
**Domain:** Technical SEO for Blazor Server (.NET 9) — Lithuanian vehicle sale contract tool
**Researched:** 2026-03-02
**Confidence:** HIGH

## Executive Summary

AutoDokas (autodokas.lt) is a Lithuanian-language vehicle sale contract generator built on Blazor Server (.NET 9). The site has exactly 4 public pages and already uses static SSR for the homepage and legal pages, which is the ideal SEO architecture for Blazor — crawlers receive fully-rendered HTML including all `<head>` content without any JavaScript execution. The core technical foundation is sound, but the site is currently invisible to search engines due to two active blockers: a robots.txt that prevents crawling of every page except the homepage, and a sitemap that only lists the homepage. These two defects make every other SEO investment ineffective until fixed.

The recommended approach is deliberately minimal: fix the two critical blockers first (robots.txt + sitemap), then add missing metadata (title tags, meta descriptions, canonical URLs, `lang` attribute) using Blazor's built-in `<HeadContent>` component via a single reusable `SeoMetadata.razor` component. The only external dependency needed is Schema.NET (v13.0.0) for type-safe JSON-LD structured data. No SEO framework or heavyweight package is justified for a 4-page site. The entire implementation is 4 phases of low-to-medium complexity work.

The primary competitive risk is autosutartis.lt, which already has Organization, WebSite, and FAQPage schemas plus keyword-optimized titles. AutoDokas should target transactional search intent ("automobilio pirkimo pardavimo sutartis internetu", "pildyti", "nemokamai") rather than informational intent where government sites (regitra.lt) have authority advantage. Fixing the robots.txt alone would be a more significant improvement than everything a competitor has done on structured data.

## Key Findings

### Recommended Stack

The entire SEO implementation requires only one NuGet package (`Schema.NET` v13.0.0) and uses .NET 9 built-in capabilities for everything else. Blazor's `<HeadContent>` + `<HeadOutlet>` pipeline renders meta tags server-side during the initial HTTP response, making them crawler-visible without any prerendering workaround. The one known framework quirk is RZ9992: Blazor blocks raw `<script>` tags in components, which requires a `MarkupString` cast to embed JSON-LD inside `<HeadContent>`.

**Core technologies:**
- `<HeadContent>` (.NET 9 built-in): Injects per-page meta tags, canonical URLs, Open Graph tags, and JSON-LD into `<head>` — native SSR-compatible, no packages needed
- `<PageTitle>` (.NET 9 built-in): Per-page title tag — already in use, needs keyword-optimized content
- ASP.NET Core Minimal API (built-in): Replace broken static robots.txt and incomplete sitemap.xml with code-defined endpoints
- Schema.NET (v13.0.0, NuGet): Strongly-typed C# classes for all schema.org types with XSS-safe `ToHtmlEscapedString()` — 3.3M downloads, .NET Standard 2.0 compatible

**What to avoid:** SeoTags NuGet (MVC-only), Toolbelt.Blazor.HeadElement (pre-.NET 6 workaround, now redundant), Sidio.Sitemap.Blazor (overkill for 4 URLs), and any package that injects meta tags via JavaScript interop.

### Expected Features

The feature dependency graph is clear: fixing robots.txt is the critical path. Every other SEO feature — meta descriptions, canonical URLs, structured data, sitemap expansion — is ineffective while crawlers are blocked.

**Must have (table stakes — P1):**
- Fix robots.txt — currently blocks all pages except `/`; this is the single biggest SEO blocker
- Fix `<html lang="en">` to `lang="lt"` — accessibility violation and language classification error
- Expand sitemap.xml — only lists homepage; 3 public pages missing
- Keyword-optimized title tags — homepage currently uses "Pagrindinis" (generic Lithuanian for "Home"), no keyword value
- Meta descriptions — no descriptions exist on any page; needed for SERP snippets
- Canonical URLs — no canonicals exist; required to prevent duplicate content from query string variants

**Should have (competitive edge — P2):**
- JSON-LD Organization + WebSite schemas — competitor autosutartis.lt already has these; Google Knowledge Graph signal
- JSON-LD WebPage per page — helps Google understand page purpose and relationships
- Open Graph tags — required for non-broken social media link previews; Facebook dominant in Lithuania
- Complete web manifest — currently has empty `name` and `short_name` fields
- Semantic HTML audit — heading hierarchy and image alt text review

**Defer to v2+:**
- JSON-LD Service schema — more complex, defer until basic structured data is validated
- FAQ Schema + content — requires content creation (out of scope); revisit after content strategy is decided
- Core Web Vitals optimization — monitor first; Blazor SSR is generally good for LCP
- Long-tail keyword optimization — defer until Search Console shows which queries bring impressions

### Architecture Approach

The architecture uses two new reusable Razor components (`SeoMetadata.razor` and `StructuredData.razor`) placed in a dedicated `Components/Seo/` folder, with no service layer needed for a 4-page static site. Each page passes its own title, description, canonical URL, and structured data as parameters. The sitemap is replaced with a Minimal API endpoint; robots.txt remains a static file but with corrected content. No DI services, no abstractions beyond the two shared components.

**Major components:**
1. `SeoMetadata.razor` — accepts Title, Description, CanonicalUrl, OgImage, Robots parameters; emits meta/OG tags via `<HeadContent>` into `<HeadOutlet>` in App.razor
2. `StructuredData.razor` — accepts a C# object; serializes to JSON-LD via `MarkupString` workaround inside `<HeadContent>`; used for Organization, WebSite, WebPage, Service schemas
3. Minimal API sitemap endpoint in `Program.cs` — replaces static `wwwroot/sitemap.xml`; returns XML for all 4 public URLs with `lastmod` dates
4. `App.razor` (modified) — fix `lang="lt"` and optionally add site-wide Organization/WebSite JSON-LD directly in `<head>` (avoids RZ9992 for global schemas)
5. Static files (`robots.txt`, `site.webmanifest`) — content fixes only, no code changes

### Critical Pitfalls

1. **robots.txt blocks all crawling (ACTIVE BUG)** — The current file uses `Allow: /$` + `Disallow: /` which permits only the exact root URL. Fix immediately by replacing with explicit Disallow rules for only private/parameterized routes (`/contract/`, `/buyer/`, `/BuyerNotificationSent`, `/ContractCompleted`). Nothing else is worth doing until this is fixed.

2. **RZ9992: JSON-LD script tags blocked in components** — Blazor raises a compile error when embedding `<script>` tags in Razor components. Use `@((MarkupString)$"<script type=\"application/ld+json\">{jsonLd}</script>")` inside `<HeadContent>`, or place site-wide schemas directly in the `<head>` section of `App.razor` (which is not a component and allows script tags).

3. **HeadContent meta tags missing during prerender if they depend on async data** — For pages using `@rendermode InteractiveServer` (i.e., `/contract`), meta tags in `<HeadContent>` must use hardcoded values or resource strings — never values fetched in `OnInitializedAsync`. The contract page's title uses `Text.NewContract` which is a resource string available at prerender time; this is correct.

4. **Generic title tag "Pagrindinis" (ACTIVE BUG)** — The homepage title is the Lithuanian equivalent of "Home", with no keywords or brand. Must be changed to a keyword-rich format matching competitor pattern: "Automobilio pirkimo pardavimo sutartis internetu | AutoDokas.lt". This change is in the resource string file, not in Razor markup.

5. **Competing against government/authority sites on wrong keyword** — regitra.lt and VVTAT dominate informational intent for "automobilio sutartis". AutoDokas should target transactional variants: "automobilio pirkimo pardavimo sutartis internetu", "pildyti", "nemokamai". Structured data (Service schema) further differentiates a tool from an informational page.

## Implications for Roadmap

Based on research, the ARCHITECTURE.md build order is the correct phase structure. Dependencies are clear and well-understood. Suggested phase structure:

### Phase 1: Crawl Infrastructure Fixes
**Rationale:** The robots.txt and sitemap bugs are blockers — all downstream SEO work is wasted while crawlers cannot reach pages. These fixes are independent of each other and have no code dependencies. Fix everything in `wwwroot/` and infrastructure in a single pass.
**Delivers:** Google can crawl and discover all 4 public pages. Sitemap is complete and consistent with robots.txt. Site manifest is complete.
**Addresses:** Fix robots.txt (P1), Expand sitemap.xml (P1), Fix `<html lang="lt">` (P1), Fix web manifest (P2)
**Avoids:** Pitfall 1 (robots.txt blocker), Pitfall 5 (sitemap missing pages), Pitfall 2 (html lang mismatch), Pitfall 9 (incomplete manifest), Pitfall 11 (forgetting to submit sitemap post-deploy)

### Phase 2: SEO Components
**Rationale:** Create the two reusable Razor components before applying them to any page. This is a one-time setup that all page integration work (Phase 3) depends on. No page files need changing in this phase.
**Delivers:** `SeoMetadata.razor` and `StructuredData.razor` components, fully built and tested in isolation.
**Uses:** `<HeadContent>` built-in, `MarkupString` workaround for JSON-LD (RZ9992 fix), Schema.NET v13.0.0 for typed JSON-LD
**Avoids:** Pitfall 3 (async data in HeadContent — enforce static values at component design time), Pitfall 4 (RZ9992 — baked into StructuredData.razor)

### Phase 3: Page Meta Tags and Titles
**Rationale:** With components in place, apply SEO metadata to all 4 public pages. This is the bulk of the work — updating title tags, adding meta descriptions, canonical URLs, and Open Graph tags per page. Must use resource strings, not async data.
**Delivers:** All 4 public pages have unique, keyword-optimized titles; meta descriptions for SERP snippets; canonical URLs preventing duplicate content; Open Graph tags for social sharing.
**Addresses:** Title tag optimization (P1), Meta descriptions (P1), Canonical URLs (P1), Open Graph tags (P2), noindex on private routes (Pitfall 12)
**Avoids:** Pitfall 3 (async data in HeadContent), Pitfall 6 (missing canonicals), Pitfall 7 (generic titles), Pitfall 8 (missing OG tags), Pitfall 13 (Lithuanian keyword case variants), Pitfall 14 (informational vs. transactional intent)

### Phase 4: Structured Data (JSON-LD)
**Rationale:** Build on validated page meta tags with structured data. Organization and WebSite schemas go site-wide (in App.razor head or MainLayout). WebPage schema per page references Organization. Service schema on homepage is the highest-value differentiator vs. competitors.
**Delivers:** Google Knowledge Graph entity for AutoDokas; rich result eligibility; competitive parity with autosutartis.lt on structured data.
**Uses:** Schema.NET v13.0.0 for type-safe Organization, WebSite, WebPage, Service schema generation
**Addresses:** JSON-LD Organization + WebSite (P2), JSON-LD WebPage per page (P2), JSON-LD Service on homepage (P3)
**Avoids:** Pitfall 4 (RZ9992 — use MarkupString workaround), anti-feature of excessive schema types not reflected in page content

### Phase 5: Validation and Search Console Submission
**Rationale:** SEO changes have delayed effect. Active submission to Google Search Console accelerates indexing. Validation with Rich Results Test and URL Inspection confirms implementation correctness before waiting for organic crawling.
**Delivers:** Sitemap submitted, all public URLs requested for indexing, JSON-LD validated via Rich Results Test and Schema.org Validator, Lighthouse SEO score 90+ confirmed.
**Addresses:** Pitfall 11 (not submitting sitemap after robots.txt fix)
**Tools:** Google Search Console, Google Rich Results Test, Schema Markup Validator, Google Lighthouse

### Phase Ordering Rationale

- Phase 1 must come before everything because the robots.txt bug is a hard blocker for all SEO value
- Phase 2 must precede Phase 3 to avoid duplicating component logic across 4 page files
- Phase 3 must precede Phase 4 because WebPage and Service schemas reference Organization and must match visible meta description content (Google requires parity)
- Phase 5 is a post-deployment step that depends on all previous phases being deployed
- Phases 1 and 4 can be partially parallelized (sitemap endpoint in Program.cs is independent of Razor component work), but Phase 1 crawl fixes should be deployed first

### Research Flags

Phases with standard patterns (no additional research needed):
- **Phase 1:** robots.txt and sitemap formats are industry-standard; exact content is already specified in PITFALLS.md
- **Phase 2:** HeadContent and MarkupString patterns verified against official Microsoft docs and dotnet/aspnetcore GitHub issues
- **Phase 3:** Meta tags, canonical URLs, and Open Graph tag implementation are well-documented; Lithuanian keyword strategy is covered in PITFALLS.md
- **Phase 5:** Google Search Console submission process is standard; no coding involved

Phases that may benefit from deeper research during planning:
- **Phase 4 (Service schema):** The Service schema design for a "contract generation tool" has no direct examples in Google's rich results documentation. Schema.NET provides the type; the property values need validation against Google's structured data policies. Recommend testing against Rich Results Test before finalizing schema design.

## Confidence Assessment

| Area | Confidence | Notes |
|------|------------|-------|
| Stack | HIGH | All technologies verified against official Microsoft docs (learn.microsoft.com), official GitHub issues (dotnet/aspnetcore), and NuGet download statistics. One package (Schema.NET) with 3.3M downloads and .NET Standard 2.0 compatibility. |
| Features | HIGH | Feature priorities derived from Google's official SEO documentation plus competitor analysis (autosutartis.lt directly inspected). Active bugs (robots.txt, html lang) confirmed by codebase inspection. |
| Architecture | HIGH | Build order and component design verified against official Blazor docs and official GitHub issue confirmations. HeadContent SSR behavior confirmed in dotnet/aspnetcore#54300. RZ9992 workaround confirmed in dotnet/aspnetcore#37230. |
| Pitfalls | HIGH | Critical pitfalls verified against codebase (robots.txt and html lang bugs confirmed as active). RZ9992 is a known compile error. Prerender timing issue documented in official GitHub issues. |

**Overall confidence:** HIGH

### Gaps to Address

- **OG image asset:** Open Graph requires a 1200x630px image (`og-image.png`). Research confirms the requirement but cannot determine whether an existing asset can be adapted or a new one is needed. Flagged as design work outside the technical SEO scope.
- **Service schema property values:** Schema.NET provides the `Service` type, but the correct property values (serviceType, areaServed, availableChannel) for a contract generation tool are not directly specified in Google's rich results documentation. Validate against Rich Results Test after initial implementation.
- **FAQ schema future decision:** Writing FAQ content is out of scope per PROJECT.md, but autosutartis.lt uses FAQPage schema to advantage. If content strategy changes, FAQ schema implementation is straightforward using the MarkupString pattern established in Phase 4. No immediate action needed.
- **Title resource string location:** The generic "Pagrindinis" title comes from a Lithuanian resource string file. The file path was not identified in research. Must locate the correct `.resx` file during Phase 3 implementation.

## Sources

### Primary (HIGH confidence)
- [Microsoft Learn: Control head content in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content) — HeadContent, HeadOutlet, PageTitle behavior
- [Microsoft Learn: Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes) — Prerendering behavior, SSR confirmation
- [dotnet/aspnetcore Issue #37230](https://github.com/dotnet/aspnetcore/issues/37230) — RZ9992 MarkupString workaround for JSON-LD
- [dotnet/aspnetcore Issue #54300](https://github.com/dotnet/aspnetcore/issues/54300) — HeadContent works only during static SSR prerendering
- [Schema.NET NuGet](https://www.nuget.org/packages/Schema.NET) — v13.0.0, 3.3M downloads, .NET Standard 2.0
- [Google: Structured data general guidelines](https://developers.google.com/search/docs/appearance/structured-data/sd-policies) — JSON-LD content parity requirements
- [Google: Consolidate duplicate URLs](https://developers.google.com/search/docs/crawling-indexing/consolidate-duplicate-urls) — Canonical URL requirements

### Secondary (MEDIUM confidence)
- [autosutartis.lt](https://autosutartis.lt/) — Direct competitor; confirmed Organization, WebSite, FAQPage schemas in use
- [Sidio.Sitemap.Blazor GitHub](https://github.com/marthijn/Sidio.Sitemap.Blazor) — Documented as future option for 20+ page scenarios
- [Search Engine Land: FAQ schema](https://searchengineland.com/faq-schema-rise-fall-seo-today-463993) — FAQ rich results restricted to authoritative sites since 2023
- [RankTracker: Lithuanian local SEO](https://www.ranktracker.com/blog/a-complete-guide-for-doing-local-seo-in-lithuania/) — Lithuanian search behavior patterns

### Tertiary (community sources)
- [GhostlyInc: Blazor SEO Meta Data Component](https://ghostlyinc.com/en-US/blazor-seo-meta-data-component/) — Component pattern, aligns with official docs
- [Dateo Software: Blazor robots.txt analysis](https://dateo-software.de/blog/blazor-robots) — Real-world Blazor SEO patterns

---
*Research completed: 2026-03-02*
*Ready for roadmap: yes*
