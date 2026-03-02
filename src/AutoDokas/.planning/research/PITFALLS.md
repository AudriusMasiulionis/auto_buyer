# Domain Pitfalls: Blazor Server SEO Optimization

**Domain:** Technical SEO for Blazor Server (.NET 8) Lithuanian vehicle contract tool
**Researched:** 2026-03-02
**Confidence:** HIGH (verified against codebase, Microsoft docs, GitHub issues, and competitor analysis)

---

## Critical Pitfalls

Mistakes that cause complete SEO failure or require significant rework.

---

### Pitfall 1: robots.txt Blocks All Crawling (ACTIVE IN CODEBASE)

**What goes wrong:** The current `robots.txt` uses `Allow: /$` + `Disallow: /` which only permits crawling of the exact root URL (`/`) and `sitemap.xml`. Every other page -- `/contract`, `/terms-and-conditions`, `/privacy-policy` -- is blocked from all search engine crawlers. Google cannot discover or index any page except the homepage.

**Why it happens:** The original `robots.txt` was likely set to restrictive defaults during development and never updated for production. The `Allow: /$` directive with trailing `$` is a Google-specific regex extension meaning "only the exact root path." Combined with `Disallow: /`, it creates a near-total block.

**Consequences:**
- Google Search Console will show 0 indexed pages (except possibly `/`)
- The `/contract` page -- the entire product -- is invisible to search engines
- Legal pages (`/terms-and-conditions`, `/privacy-policy`) are blocked, reducing site trust signals
- Sitemap references to blocked URLs create conflicting signals

**Prevention:**
```
User-agent: *
Allow: /

Disallow: /contract/
Disallow: /buyer/
Disallow: /BuyerNotificationSent
Disallow: /ContractCompleted
Disallow: /contract/download/

Sitemap: https://autodokas.lt/sitemap.xml
```

Allow all public pages. Disallow only parameterized/private routes that contain user data.

**Detection:**
- Google Search Console > Indexing > Pages > "Blocked by robots.txt"
- Test with `curl -A Googlebot https://autodokas.lt/robots.txt`
- Use Google's robots.txt Tester in Search Console

**Phase:** Must be fixed FIRST, before any other SEO work. Nothing else matters if crawlers cannot access pages.

---

### Pitfall 2: `<html lang="en">` on a Lithuanian-Only Site (ACTIVE IN CODEBASE)

**What goes wrong:** The `App.razor` root component hardcodes `<html lang="en">` even though the entire site content, cookie consent, legal pages, and target audience are Lithuanian. The `lang` attribute is set to English.

**Why it happens:** The Blazor project template defaults to `lang="en"`. The developer added Lithuanian localization infrastructure (`RequestLocalizationOptions` with `"lt"` as default) but never updated the HTML root element.

**Consequences:**
- Screen readers announce the page in English, creating an accessibility violation
- Bing uses the `lang` attribute for language determination and may serve the page to English-speaking audiences
- Google primarily uses visible content for language detection (so impact on Google ranking is minimal) but the mismatch is a quality signal issue
- SEO audit tools (Lighthouse, Ahrefs, Screaming Frog) will flag this as an error, creating noise in SEO reports
- Browsers may offer to "translate this page" from English, confusing Lithuanian users

**Prevention:** Change `App.razor` line 3 from `<html lang="en">` to `<html lang="lt">`. This is a one-line fix but easy to overlook because it is in the root template, not a page component.

**Detection:**
- View page source, check `<html>` tag
- Lighthouse accessibility audit flags `lang` mismatch
- Browser "translate this page" prompt appearing for Lithuanian users

**Phase:** Fix alongside robots.txt in the first phase. Trivial to implement but high-visibility impact.

---

### Pitfall 3: HeadContent Meta Tags Not Rendered During Prerender Phase

**What goes wrong:** Developers add `<HeadContent>` with meta descriptions and Open Graph tags to interactive Blazor components, but the tags are populated using data fetched in `OnInitializedAsync`. If the data is not available during the static prerender phase (the first of the two renders), search engine crawlers see empty or missing meta tags.

**Why it happens:** In .NET 8 Blazor, components with `@rendermode InteractiveServer` render twice: first as static HTML (prerender), then again interactively via SignalR. Meta tag content must be available during the FIRST render for crawlers to see it. If meta content depends on async data loading, it may not be ready during prerender.

**Consequences:**
- Google sees empty `<meta name="description">` tags or no tags at all
- Open Graph tags are missing when pages are shared on social media
- Title tags may show loading states or defaults instead of actual content
- Google Search Console may report "missing meta description" for pages that appear correct in browser DevTools

**Prevention:**
- For the homepage (`Home.razor`): This page does NOT use `@rendermode InteractiveServer`, so it renders as static SSR. `<HeadContent>` with hardcoded Lithuanian meta tags will work correctly because static SSR generates full HTML on first response.
- For the contract page (`Contract.razor`): This page uses `@rendermode InteractiveServer` with `@attribute [StreamRendering]`. The `<PageTitle>` uses `Text.NewContract` which is a resource string available at prerender time -- this works. Any additional `<HeadContent>` meta tags should use resource strings or hardcoded values, NOT values fetched from a database or API.
- General rule: Never use `OnInitializedAsync` data in `<HeadContent>` for pages that need SEO indexing.

**Detection:**
- `curl -s https://autodokas.lt/contract | grep '<meta name="description"'` -- check if meta is in raw HTML response
- Google Search Console > URL Inspection > View Crawled Page
- Compare browser DevTools source vs. `View Page Source` (right-click)

**Phase:** Must be understood before adding meta tags. Implement meta tags using static resource strings in the HeadContent phase.

---

### Pitfall 4: JSON-LD Structured Data Blocked by Blazor Script Restriction (RZ9992)

**What goes wrong:** Blazor raises compile error RZ9992 ("Script tags should not be placed inside components") when you try to embed `<script type="application/ld+json">` inside a Razor component. Developers either give up on structured data or place it incorrectly.

**Why it happens:** Blazor prevents all `<script>` tags in components because the framework cannot dynamically update scripts. This restriction applies even to non-executable scripts like JSON-LD, which is purely declarative structured data for search engines.

**Consequences:**
- No structured data in search results (no rich snippets, no Organization knowledge panel)
- Competitors like autosutartis.lt implement FAQPage, Organization, and WebSite schemas and get enhanced search listings
- Missing structured data is a competitive disadvantage in the Lithuanian search market where few competitors exist

**Prevention:** Two approaches (use the first one):

**Approach A -- MarkupString workaround (recommended for per-page JSON-LD):**
```csharp
<HeadContent>
    @((MarkupString)@"<script type=""application/ld+json"">
    {
        ""@context"": ""https://schema.org"",
        ""@type"": ""Organization"",
        ""name"": ""AutoDokas"",
        ""url"": ""https://autodokas.lt""
    }
    </script>")
</HeadContent>
```

**Approach B -- Place in App.razor directly (for site-wide schemas):**
Since `App.razor` is not a typical Blazor component but the root HTML template, you can place `<script type="application/ld+json">` directly in the `<head>` section before `<HeadOutlet/>`. This avoids RZ9992 entirely for site-wide schemas (Organization, WebSite).

**Detection:**
- Build fails with RZ9992 if you use raw script tags in components
- Google Rich Results Test: `https://search.google.com/test/rich-results`
- Schema.org Validator

**Phase:** Structured data phase. Know the workaround before attempting implementation.

---

## Moderate Pitfalls

---

### Pitfall 5: Sitemap Missing Public Pages

**What goes wrong:** The current `sitemap.xml` only lists the homepage (`https://autodokas.lt/`). The `/contract`, `/terms-and-conditions`, and `/privacy-policy` pages are absent. Search engines treat the sitemap as a hint for which URLs are important -- missing URLs signal they are unimportant.

**Prevention:**
```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://autodokas.lt/</loc>
    <lastmod>2026-03-02</lastmod>
    <priority>1.0</priority>
  </url>
  <url>
    <loc>https://autodokas.lt/contract</loc>
    <lastmod>2026-03-02</lastmod>
    <priority>0.8</priority>
  </url>
  <url>
    <loc>https://autodokas.lt/terms-and-conditions</loc>
    <lastmod>2026-03-02</lastmod>
    <priority>0.3</priority>
  </url>
  <url>
    <loc>https://autodokas.lt/privacy-policy</loc>
    <lastmod>2026-03-02</lastmod>
    <priority>0.3</priority>
  </url>
</urlset>
```

Do NOT include parameterized routes (`/contract/{id}`, `/buyer/{id}`, `/contract/download/{id}`) in the sitemap. These are user-specific and should not be indexed.

**Detection:**
- Google Search Console > Sitemaps > check submitted vs. indexed count
- Manual review: `curl https://autodokas.lt/sitemap.xml`

**Phase:** Fix with robots.txt in the first phase (foundational crawl infrastructure).

---

### Pitfall 6: Missing Canonical URLs Causing Duplicate Content Risk

**What goes wrong:** Without `<link rel="canonical">` tags, search engines may index multiple URL variants of the same page. For Blazor Server apps, this is especially relevant because query strings (e.g., `?culture=lt`) or trailing slashes can create duplicate URLs.

**Why it happens:** Blazor does not add canonical URLs automatically. The localization system uses `QueryStringRequestCultureProvider` and `CookieRequestCultureProvider`, meaning URLs like `/?culture=lt` and `/` could both be crawled.

**Prevention:** Add canonical URLs to every public page via `<HeadContent>`:
```razor
<HeadContent>
    <link rel="canonical" href="https://autodokas.lt/" />
</HeadContent>
```

Use absolute URLs (with `https://autodokas.lt` prefix), not relative paths. Each page must have its own canonical pointing to itself.

**Detection:**
- Google Search Console > Pages > "Duplicate without user-selected canonical"
- View page source and search for `rel="canonical"`

**Phase:** Meta tags phase. Implement alongside meta descriptions and Open Graph tags.

---

### Pitfall 7: Page Title Too Generic for SEO

**What goes wrong:** The homepage `<PageTitle>` is set to `@Text.HomeTitle` which resolves to `"Pagrindinis"` (meaning "Main" or "Home" in Lithuanian). This is a useless title for SEO -- it contains no keywords, no brand name, and no indication of what the page offers.

**Why it happens:** The title was set for in-app navigation purposes, not SEO. The actual keyword-rich content (`"Automobilio pirkimo pardavimo sutartis internetu"`) exists only in the H1 hero heading, not the title tag.

**Consequences:**
- Google uses the title tag as the primary ranking signal for on-page relevance
- A title of "Pagrindinis" competes with every Lithuanian website's homepage
- The title appears in search results snippets -- users see "Pagrindinis" and have no reason to click
- Competitor autosutartis.lt uses "Automobilio pirkimo-pardavimo sutartis internetu | Autosutartis.lt" as their title

**Prevention:** Change `Text.HomeTitle` resource to a keyword-optimized title:
```
"Automobilio pirkimo pardavimo sutartis internetu | AutoDokas.lt"
```

Similarly, `Text.NewContract` ("Nauja sutartis") is the title for `/contract`. Consider:
```
"Automobilio pirkimo pardavimo sutartis - Pildyti | AutoDokas.lt"
```

Keep titles under 60 characters. Include primary keyword + brand name.

**Detection:**
- Google Search Console > Performance > check what titles appear in search results
- `<title>` tag in View Page Source

**Phase:** Meta tags phase. Must be updated alongside meta descriptions.

---

### Pitfall 8: Open Graph Image Missing or Unconfigured

**What goes wrong:** Without Open Graph meta tags, sharing autodokas.lt on Facebook, LinkedIn, or messaging apps shows a generic link with no image, title, or description. This kills social referral traffic.

**Why it happens:** Blazor does not provide Open Graph infrastructure. It must be manually added via `<HeadContent>`.

**Prevention:** Add to each public page:
```razor
<HeadContent>
    <meta property="og:title" content="Automobilio pirkimo pardavimo sutartis internetu" />
    <meta property="og:description" content="Nemokamai sukurkite automobilio pirkimo-pardavimo sutarti internetu." />
    <meta property="og:image" content="https://autodokas.lt/img/og-image.png" />
    <meta property="og:url" content="https://autodokas.lt/" />
    <meta property="og:type" content="website" />
    <meta property="og:locale" content="lt_LT" />
</HeadContent>
```

Create a 1200x630px OG image with the AutoDokas brand and a car visual.

**Detection:**
- Facebook Sharing Debugger: `https://developers.facebook.com/tools/debug/`
- LinkedIn Post Inspector
- Share the URL in a messaging app and check the preview

**Phase:** Meta tags phase, but OG image creation may need design work.

---

### Pitfall 9: Web Manifest Incomplete (Empty Name/Short Name)

**What goes wrong:** The current `site.webmanifest` has empty `name` and `short_name` fields. While this primarily affects PWA install prompts, Google also uses web manifest data as a trust signal. An empty manifest looks like an unfinished site.

**Prevention:** Update `site.webmanifest`:
```json
{
    "name": "AutoDokas - Automobilio sutartis internetu",
    "short_name": "AutoDokas",
    "description": "Automobilio pirkimo pardavimo sutartis internetu",
    "icons": [...],
    "theme_color": "#ffffff",
    "background_color": "#ffffff",
    "display": "standalone",
    "start_url": "/",
    "lang": "lt"
}
```

**Phase:** Foundation phase, alongside robots.txt and sitemap fixes.

---

## Minor Pitfalls

---

### Pitfall 10: Blazor.web.js Blocking Time Hurting Core Web Vitals

**What goes wrong:** The `blazor.web.js` framework script contributes to Total Blocking Time (TBT), which affects the Interaction to Next Paint (INP) Core Web Vital. Google uses Core Web Vitals as a ranking signal. For pages that do not need interactivity (like the homepage), this JavaScript overhead is wasteful.

**Why it happens:** Blazor loads `_framework/blazor.web.js` on every page regardless of whether the page uses interactive features. The homepage (`Home.razor`) is static SSR and does not need SignalR, but still loads the full framework JS.

**Prevention:** This is a known architectural limitation of Blazor (tracked for improvement post-.NET 10). Mitigations:
- Ensure the `blazor.web.js` script is loaded with the default placement (end of `<body>` -- which it already is)
- Use static SSR for all SEO-critical pages (homepage already does this correctly)
- Do not add `@rendermode InteractiveServer` to the homepage or legal pages
- Monitor Core Web Vitals in Google Search Console and PageSpeed Insights

**Phase:** Ongoing monitoring. No immediate fix possible, but avoid making it worse.

---

### Pitfall 11: Not Submitting Sitemap After robots.txt Fix

**What goes wrong:** After fixing robots.txt to allow crawling, developers expect Google to immediately start indexing. In reality, it can take days to weeks for Google to re-crawl, especially for small sites with no backlinks.

**Prevention:**
1. Fix robots.txt and sitemap.xml
2. Submit updated sitemap in Google Search Console > Sitemaps
3. Use URL Inspection tool to request indexing for each public page individually
4. Monitor the "Indexing" report in Search Console daily for the first two weeks

**Phase:** Post-deployment validation. Must be done after every SEO change.

---

### Pitfall 12: Forgetting `noindex` on Private/Dynamic Routes

**What goes wrong:** Even with correct robots.txt, if a parameterized route like `/contract/{guid}` somehow gets linked externally, Google may attempt to index it. Since these pages contain user personal data (names, addresses, vehicle info), this is both an SEO and privacy concern.

**Prevention:** Add `<HeadContent>` with noindex to Contract.razor when showing user-specific content:
```razor
@if (ContractId.HasValue)
{
    <HeadContent>
        <meta name="robots" content="noindex, nofollow" />
    </HeadContent>
}
```

The `/BuyerNotificationSent` and `/ContractCompleted` pages should also include noindex meta tags.

**Detection:**
- Google Search Console > Pages > check for unexpected indexed URLs
- `site:autodokas.lt` in Google Search to see what is indexed

**Phase:** Meta tags phase. Implement as a safety net alongside the robots.txt disallow rules.

---

## Lithuanian Market-Specific Pitfalls

---

### Pitfall 13: Ignoring Google.lt Search Behavior Differences

**What goes wrong:** Developers optimize for generic SEO best practices without considering Lithuanian search patterns. Lithuanian users often search with inflected forms (different grammatical cases) of the keyword.

**Why it matters:** The primary keyword "automobilio pirkimo pardavimo sutartis" can be searched as:
- "automobilio pirkimo pardavimo sutartis" (nominative -- searching for the contract itself)
- "automobilio pirkimo pardavimo sutarti" (accusative -- "I want to fill out the contract")
- "automobilio sutartis internetu" (shortened form)
- "auto sutartis" (colloquial abbreviation)
- "masinos pirkimo pardavimo sutartis" (using "masina" instead of "automobilis")

**Prevention:**
- Use the primary keyword in the title tag and H1
- Include natural variations in the meta description and page body text
- Do NOT keyword-stuff -- Lithuanian grammar makes it obvious when text is artificially optimized
- The existing hero text ("Automobilio pirkimo pardavimo sutartis internetu") is a strong keyword match, keep it as the H1

**Detection:**
- Google Search Console > Performance > Queries -- see which actual search terms drive impressions
- Compare your page's keyword usage with top-ranking competitors

**Phase:** Meta tags phase. Keyword strategy should inform title and description copywriting.

---

### Pitfall 14: Competing Against Government and Authority Sites Without Differentiating

**What goes wrong:** The Lithuanian government site (Regitra, VVTAT) ranks for vehicle contract terms because Google gives authority bias to government domains (.gov.lt, .lrv.lt). Trying to outrank them on pure keyword matching is futile.

**Prevention:** Differentiate on the action keyword, not the informational keyword:
- Government sites rank for "automobilio pirkimo pardavimo sutartis" (informational intent -- what is it?)
- AutoDokas should target "automobilio pirkimo pardavimo sutartis internetu" (transactional intent -- fill it out online)
- Add "internetu", "pildyti", "nemokamai", "online" modifiers to titles and descriptions
- Structured data (Service schema) differentiates a tool from an informational page

**Detection:**
- Search the target keyword and analyze what types of results rank (informational vs. transactional)
- Check if your page appears in Google for transactional variants

**Phase:** Meta tags and structured data phases. Strategy should guide all SEO copywriting.

---

## Phase-Specific Warnings

| Phase Topic | Likely Pitfall | Mitigation |
|-------------|---------------|------------|
| Robots.txt & Sitemap | Pitfall 1, 5: Fix robots.txt but forget to update sitemap, or vice versa | Fix both simultaneously. Test with Google Search Console. |
| HTML Foundation | Pitfall 2, 9: Fix lang but forget web manifest, or fix manifest but not lang | Audit all root-level files (App.razor, site.webmanifest) in one pass. |
| Meta Tags & Titles | Pitfall 3, 7: Add HeadContent meta tags that depend on async data | Use only resource strings or hardcoded values in HeadContent for SEO pages. |
| Canonical & OG Tags | Pitfall 6, 8: Use relative URLs in canonical tags, forget og:locale | Always use absolute URLs starting with `https://autodokas.lt`. Set og:locale to `lt_LT`. |
| Structured Data | Pitfall 4: RZ9992 compile error stops JSON-LD implementation | Use MarkupString workaround or place in App.razor head section. |
| Private Page Protection | Pitfall 12: Fix public SEO but expose private routes | Add noindex meta + robots.txt disallow as defense in depth. |
| Post-Deployment | Pitfall 11: Expect instant results after SEO changes | Submit sitemap, request indexing, monitor daily for 2 weeks. |
| Keyword Strategy | Pitfall 13, 14: Use informational keywords instead of transactional | Target "internetu" / "pildyti" / "nemokamai" modifiers to differentiate from gov sites. |

---

## Sources

### Official Documentation (HIGH confidence)
- [ASP.NET Core Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-8.0) -- Microsoft Learn
- [Control head content in ASP.NET Core Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content?view=aspnetcore-8.0) -- Microsoft Learn
- [Prerender ASP.NET Core Razor components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/prerender?view=aspnetcore-8.0) -- Microsoft Learn
- [Managing Multi-Regional and Multilingual Sites](https://developers.google.com/search/docs/specialty/international/managing-multi-regional-sites) -- Google Search Central

### GitHub Issues (HIGH confidence)
- [Allow JSON-LD Scripts (error RZ9992) -- Issue #37230](https://github.com/dotnet/aspnetcore/issues/37230) -- dotnet/aspnetcore
- [Blazor Server .NET 8 SEO Problem head tags -- Issue #54300](https://github.com/dotnet/aspnetcore/issues/54300) -- dotnet/aspnetcore
- [SEO performance issue with Blazor Web App -- Issue #62599](https://github.com/dotnet/aspnetcore/issues/62599) -- dotnet/aspnetcore

### Community & Analysis (MEDIUM confidence)
- [Real-world Blazor websites: Robots.txt](https://dateo-software.de/blog/blazor-robots) -- Dateo Software
- [Blazor .NET 8 Server-side Rendering (SSR)](https://akifmt.github.io/dotnet/2024-01-16-blazor-.net8-server-side-rendering-ssr/) -- Akif MT
- [Google won't crawl my Blazor Server App](https://support.google.com/webmasters/thread/203122230) -- Google Search Central Community
- [Blazor / SEO / index in search engines -- Issue #44750](https://github.com/dotnet/aspnetcore/issues/44750) -- dotnet/aspnetcore

### Competitor Analysis (MEDIUM confidence)
- [autosutartis.lt](https://autosutartis.lt/) -- Direct competitor implementing FAQPage, Organization, and WebSite schemas
- [ledauto.lt](https://ledauto.lt/automobilio-pirkimo-pardavimo-sutartis-2025) -- Content competitor with keyword-optimized pages
- [reidasofficial.lt](https://reidasofficial.lt/straipsniai/automobilio-pirkimo-pardavimo-sutartis/) -- Content competitor targeting same keyword
