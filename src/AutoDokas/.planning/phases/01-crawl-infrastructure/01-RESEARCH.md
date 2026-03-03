# Phase 1: Crawl Infrastructure - Research

**Researched:** 2026-03-03
**Domain:** Blazor Server SEO crawl infrastructure (robots.txt, sitemap, noindex, lang, manifest)
**Confidence:** HIGH

## Summary

Phase 1 addresses five concrete crawl infrastructure problems that prevent Google from discovering and correctly classifying autodokas.lt. The current `robots.txt` uses `Disallow: /` which blocks all crawling except the exact homepage and sitemap.xml. The HTML lang attribute is set to "en" instead of "lt". The sitemap.xml is missing a `lastmod` date. The web manifest has empty `name`, `short_name`, and `description` fields. No pages have `noindex` meta tags to prevent indexing of contract form and legal pages.

All five fixes involve editing static files or adding small Blazor `<HeadContent>` blocks to existing `.razor` pages. No new NuGet packages are needed. The entire phase uses .NET 9 built-in capabilities. The `<HeadOutlet/>` component in `App.razor` already collects `<HeadContent>` output from child components, so adding noindex meta tags is a matter of adding `<HeadContent><meta name="robots" content="noindex" /></HeadContent>` to each page that should not be indexed.

**Primary recommendation:** Fix robots.txt first (it is the critical blocker), then apply all other changes. Use inline `<HeadContent>` per page for noindex tags -- a shared component is unnecessary for a single meta tag.

<user_constraints>

## User Constraints (from CONTEXT.md)

### Locked Decisions
- robots.txt: Allow all paths by default (remove `Disallow: /`), rely on noindex meta tags for non-content pages. Specifically block test pages (/email-test, /contract-test) and app routes (/contract/*, /buyer/*, /BuyerNotificationSent, /ContractCompleted) via Disallow as defense-in-depth. Keep sitemap reference.
- noindex: Add `<meta name="robots" content="noindex">` to: /contract (empty form), /terms-and-conditions, /privacy-policy, /Error, /BuyerNotificationSent, /ContractCompleted, /contract/{id}, /buyer/{id}, /contract/download/{id}, /email-test, /contract-test
- Web manifest: name="AutoDokas", short_name="AutoDokas", description="Automobilio pirkimo pardavimo sutartis internetu"
- Sitemap: Keep as static file at wwwroot/sitemap.xml (only homepage URL), add lastmod date, remove any non-homepage URLs
- Lang attribute: Change `<html lang="en">` to `<html lang="lt">` in App.razor line 3

### Claude's Discretion
- Exact robots.txt formatting and ordering of rules
- Whether to use a shared Blazor component for noindex or inline HeadContent per page
- lastmod date format and value in sitemap.xml

### Deferred Ideas (OUT OF SCOPE)
None -- discussion stayed within phase scope.

</user_constraints>

<phase_requirements>

## Phase Requirements

| ID | Description | Research Support |
|----|-------------|-----------------|
| CRAWL-01 | robots.txt allows crawling of homepage and sitemap, blocks contract form and app pages | Direct file edit to wwwroot/robots.txt. Remove `Disallow: /`, add specific Disallow rules for app routes. Verified Google robots.txt spec. |
| CRAWL-02 | sitemap.xml lists only the homepage with lastmod date | Direct file edit to wwwroot/sitemap.xml. Add `<lastmod>` in W3C date format (YYYY-MM-DD). Already has only homepage URL. |
| CRAWL-03 | `<html lang="lt">` attribute correctly set | One-line change in Components/App.razor line 3: `lang="en"` to `lang="lt"`. |
| CRAWL-04 | Web manifest has name, short_name, and description filled in | Direct file edit to wwwroot/site.webmanifest. Fill empty `name`, `short_name`, add `description` field. |
| CRAWL-05 | Contract form pages, legal pages, and non-content pages have noindex meta tag | Add `<HeadContent><meta name="robots" content="noindex" /></HeadContent>` to 9 .razor page files. Uses built-in Blazor HeadContent component. |

</phase_requirements>

## Standard Stack

### Core
| Library | Version | Purpose | Why Standard |
|---------|---------|---------|--------------|
| `<HeadContent>` | .NET 9 built-in | Inject noindex meta tags into `<head>` per page | Built-in Blazor component, renders during static SSR prerendering, no packages needed. Already have `<HeadOutlet/>` in App.razor collecting output. |
| `<PageTitle>` | .NET 9 built-in | Already used on most pages for titles | Existing pattern in codebase. |

### Supporting
No additional libraries needed for Phase 1. All changes are to static files or use built-in Blazor components.

### Alternatives Considered
| Instead of | Could Use | Tradeoff |
|------------|-----------|----------|
| Inline `<HeadContent>` per page | Shared `NoIndex.razor` component | Adds a component file for a single `<meta>` tag. Not worth the abstraction for Phase 1 -- a shared `SeoMetadata.razor` component will be created in Phase 2 for meta tags, and can absorb noindex as a parameter then. |

**Installation:**
```bash
# No packages needed for Phase 1
```

## Architecture Patterns

### Files to Modify
```
wwwroot/
  robots.txt              # Rewrite: allow all, disallow app routes
  sitemap.xml             # Add lastmod date
  site.webmanifest        # Fill name, short_name, add description

Components/
  App.razor               # Line 3: lang="en" -> lang="lt"

Components/Pages/
  Contract/
    Contract.razor         # Add noindex HeadContent (3 @page routes)
    BuyerNotificationSent.razor  # Add noindex HeadContent
    ContractCompleted.razor      # Add noindex HeadContent
    ContractDownload.razor       # Add noindex HeadContent
  Legal/
    TermsAndConditions.razor     # Add noindex HeadContent
    PrivacyPolicy.razor          # Add noindex HeadContent
  Error.razor                    # Add noindex HeadContent

Components/TestPages/
  EmailSendTestPage.razor        # Add noindex HeadContent
  ContractPdfTemplatePage.razor  # Add noindex HeadContent
```

### Pattern 1: noindex via HeadContent
**What:** Each page that should not be indexed adds a `<HeadContent>` block with a robots noindex meta tag.
**When to use:** Every non-indexed page in this phase.
**Example:**
```razor
@page "/terms-and-conditions"
@using AutoDokas.Resources

<PageTitle>@Text.TermsAndConditions</PageTitle>

<HeadContent>
    <meta name="robots" content="noindex" />
</HeadContent>

<div class="container my-5">
    ...
</div>
```
Source: [Microsoft Learn - Control head content in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content?view=aspnetcore-9.0)

**Key detail:** `<HeadContent>` is available because `Microsoft.AspNetCore.Components.Web` is already imported in `_Imports.razor`. No additional `@using` statements needed.

### Pattern 2: robots.txt Structure
**What:** Allow-by-default with specific Disallow rules for app routes.
**Example:**
```
User-agent: *
Allow: /

Disallow: /contract
Disallow: /buyer/
Disallow: /BuyerNotificationSent
Disallow: /ContractCompleted
Disallow: /email-test
Disallow: /contract-test

Sitemap: https://autodokas.lt/sitemap.xml
```
Source: [Google Search Central - robots.txt](https://developers.google.com/search/docs/crawling-indexing/robots/intro)

**Critical note on `/contract` path:** Using `Disallow: /contract` (without trailing slash) blocks both `/contract` and `/contract/{guid}` and `/contract/download/{guid}` because robots.txt path matching is prefix-based. This is the desired behavior since all contract paths should be blocked.

### Pattern 3: sitemap.xml with lastmod
**What:** Static sitemap with W3C date format.
**Example:**
```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://autodokas.lt/</loc>
    <lastmod>2026-03-03</lastmod>
    <priority>1.0</priority>
  </url>
</urlset>
```
Source: [sitemaps.org Protocol](https://www.sitemaps.org/protocol.html)

**lastmod format:** Use `YYYY-MM-DD` (date-only W3C format). Google accepts this format. The date should reflect when the homepage was last meaningfully updated. Use today's date since SEO changes constitute a meaningful update.

### Pattern 4: Web Manifest
**What:** Fill in empty fields and add description.
**Example:**
```json
{
  "name": "AutoDokas",
  "short_name": "AutoDokas",
  "description": "Automobilio pirkimo pardavimo sutartis internetu",
  "icons": [
    {"src": "/android-chrome-192x192.png", "sizes": "192x192", "type": "image/png"},
    {"src": "/android-chrome-512x512.png", "sizes": "512x512", "type": "image/png"}
  ],
  "theme_color": "#ffffff",
  "background_color": "#ffffff",
  "display": "standalone"
}
```
Source: [MDN - Web app manifest](https://developer.mozilla.org/en-US/docs/Web/Progressive_web_apps/Manifest)

### Anti-Patterns to Avoid
- **Blocking crawling AND using noindex on the same page:** If a page is blocked by robots.txt, Google cannot see the noindex meta tag. The user's strategy correctly uses both as defense-in-depth (robots.txt blocks crawling of app routes, noindex prevents indexing if somehow crawled). This is fine because the goal is to prevent indexing, not to allow crawling. For these app pages, we do NOT need Google to see them at all.
- **Using `noindex` in robots.txt:** Google does not support `Noindex:` directive in robots.txt. Always use `<meta>` tag or HTTP header.
- **Setting lastmod to current date on every deploy:** Google ignores lastmod if it is not "consistently and verifiably accurate." Only update lastmod when content actually changes.

## Don't Hand-Roll

| Problem | Don't Build | Use Instead | Why |
|---------|-------------|-------------|-----|
| Head tag injection | Custom JS to inject meta tags at runtime | Blazor `<HeadContent>` component | JS-injected tags are not visible during prerendering; crawlers won't see them |
| robots.txt serving | Custom middleware to dynamically generate robots.txt | Static file in wwwroot | Only 1 URL to index, no dynamic logic needed. ASP.NET Core serves wwwroot files automatically. |
| Sitemap generation | NuGet sitemap generator package | Static XML file in wwwroot | Only 1 URL to index. A dynamic generator adds complexity with zero benefit. |

**Key insight:** Phase 1 is entirely about editing existing static files and adding small Razor markup blocks. No custom code, no packages, no middleware.

## Common Pitfalls

### Pitfall 1: robots.txt Disallow: / Blocks Everything
**What goes wrong:** The current robots.txt has `Disallow: /` which blocks all crawling except what's explicitly allowed. This means Google cannot discover pages.
**Why it happens:** Default restrictive configuration from development was never updated for production.
**How to avoid:** Replace with allow-by-default strategy. Test with Google Search Console robots.txt tester after deployment.
**Warning signs:** Google Search Console shows "Blocked by robots.txt" for all pages.

### Pitfall 2: noindex Not Visible If Page Is Blocked by robots.txt
**What goes wrong:** Adding noindex to a page that robots.txt blocks means Google never sees the noindex tag. If other sites link to the page, Google may still show it in results (with no snippet, but the URL appears).
**Why it happens:** robots.txt prevents crawling, so the crawler never reads the HTML to find the noindex tag.
**How to avoid:** The user's defense-in-depth strategy is correct: robots.txt Disallow for app routes (Google won't crawl them) PLUS noindex tags (if they somehow get crawled, e.g., via cached copies or different crawler behavior). For legal pages like /terms-and-conditions and /privacy-policy, which are NOT blocked by robots.txt, the noindex tag is the primary mechanism.
**Warning signs:** Google Search Console shows "Indexed, though blocked by robots.txt."

### Pitfall 3: HeadContent on InteractiveServer Pages
**What goes wrong:** `<HeadContent>` meta tags may not render if they depend on async data.
**Why it happens:** InteractiveServer pages prerender once as static HTML, then re-render interactively. Meta tags must be available during the first (static) render.
**How to avoid:** The noindex tag is a hardcoded string -- no async dependency. It will render correctly during prerendering on all pages, including InteractiveServer pages like Contract.razor. No risk here.
**Warning signs:** `curl -s URL | grep noindex` returns nothing even though the tag is in the .razor file.

### Pitfall 4: robots.txt Path Matching Is Prefix-Based
**What goes wrong:** `Disallow: /contract` blocks `/contract`, `/contract/123`, and `/contract/download/123` -- all of which start with `/contract`. This is the desired behavior, but could be surprising.
**Why it happens:** robots.txt uses prefix matching, not exact matching. A `$` suffix means exact match (Google extension).
**How to avoid:** Verify intended coverage of each Disallow rule. In this case, prefix matching on `/contract` is exactly what we want (blocks all contract-related routes).
**Warning signs:** Unintended pages getting blocked.

### Pitfall 5: Forgetting Pages That Need noindex
**What goes wrong:** A page is missed and gets indexed by Google with no useful content, hurting site quality signals.
**Why it happens:** Pages are spread across multiple directories and some have multiple `@page` routes.
**How to avoid:** The complete list of pages needing noindex (9 files):
  1. `Components/Pages/Contract/Contract.razor` (routes: /contract, /contract/{guid}, /buyer/{guid})
  2. `Components/Pages/Contract/BuyerNotificationSent.razor`
  3. `Components/Pages/Contract/ContractCompleted.razor`
  4. `Components/Pages/Contract/ContractDownload.razor`
  5. `Components/Pages/Legal/TermsAndConditions.razor`
  6. `Components/Pages/Legal/PrivacyPolicy.razor`
  7. `Components/Pages/Error.razor`
  8. `Components/TestPages/EmailSendTestPage.razor`
  9. `Components/TestPages/ContractPdfTemplatePage.razor`
**Warning signs:** Google Search Console shows unexpected pages being indexed.

## Code Examples

Verified patterns from official sources:

### Adding noindex to a Static SSR Page (e.g., TermsAndConditions.razor)
```razor
@page "/terms-and-conditions"
@using AutoDokas.Resources

<PageTitle>@Text.TermsAndConditions</PageTitle>

<HeadContent>
    <meta name="robots" content="noindex" />
</HeadContent>

<div class="container my-5">
    <!-- existing page content unchanged -->
</div>
```

### Adding noindex to an InteractiveServer Page (e.g., Contract.razor)
```razor
@page "/contract"
@page "/contract/{ContractId:guid}"
@page "/buyer/{ContractId:guid}"
@attribute [StreamRendering]
@rendermode InteractiveServer
@using AutoDokas.Components.Pages.Contract.Sections
@using AutoDokas.Components.Shared
@using AutoDokas.Data.Models
@using AutoDokas.Extensions

<PageTitle>@(_isContractCompleted ? Text.CompletedContract : Text.NewContract)</PageTitle>

<HeadContent>
    <meta name="robots" content="noindex" />
</HeadContent>

<div class="contract-page">
    <!-- existing page content unchanged -->
</div>
```

### Adding noindex to a Page Without Existing PageTitle (e.g., BuyerNotificationSent.razor)
```razor
@page "/BuyerNotificationSent"
@attribute [StreamRendering]
@rendermode InteractiveServer

<HeadContent>
    <meta name="robots" content="noindex" />
</HeadContent>

<div class="status-page">
    <!-- existing page content unchanged -->
</div>
```

### Updated robots.txt
```
User-agent: *
Allow: /

Disallow: /contract
Disallow: /buyer/
Disallow: /BuyerNotificationSent
Disallow: /ContractCompleted
Disallow: /email-test
Disallow: /contract-test

Sitemap: https://autodokas.lt/sitemap.xml
```

### Updated sitemap.xml
```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://autodokas.lt/</loc>
    <lastmod>2026-03-03</lastmod>
    <priority>1.0</priority>
  </url>
</urlset>
```

### Updated site.webmanifest
```json
{
  "name": "AutoDokas",
  "short_name": "AutoDokas",
  "description": "Automobilio pirkimo pardavimo sutartis internetu",
  "icons": [
    {
      "src": "/android-chrome-192x192.png",
      "sizes": "192x192",
      "type": "image/png"
    },
    {
      "src": "/android-chrome-512x512.png",
      "sizes": "512x512",
      "type": "image/png"
    }
  ],
  "theme_color": "#ffffff",
  "background_color": "#ffffff",
  "display": "standalone"
}
```

### Updated App.razor (line 3 only)
```razor
<html lang="lt">
```

## State of the Art

| Old Approach | Current Approach | When Changed | Impact |
|--------------|------------------|--------------|--------|
| `Noindex:` in robots.txt | `<meta name="robots" content="noindex">` in HTML | Google never officially supported Noindex in robots.txt | Must use meta tags, not robots.txt directives |
| `<changefreq>` in sitemap.xml | Omit changefreq entirely | Google ignores changefreq | Already removed in commit 0b40a9e |
| Toolbelt.Blazor.HeadElement NuGet | Built-in `<HeadContent>` | .NET 6+ | No third-party package needed |

**Deprecated/outdated:**
- `changefreq` in sitemap.xml: Ignored by Google. Already removed from this project.
- `<meta name="keywords">`: Ignored by Google since 2009. Not needed.
- `Noindex:` directive in robots.txt: Never officially supported by Google.

## Open Questions

1. **lastmod date value**
   - What we know: Should be date of last meaningful content change. W3C date format (YYYY-MM-DD) is accepted.
   - What's unclear: Whether to use today's date (2026-03-03) or find the actual last content update date.
   - Recommendation: Use today's date (2026-03-03) since the SEO infrastructure changes constitute a meaningful update. This is a static file and should be updated whenever the homepage content changes.

2. **robots.txt: exact path matching for /contract**
   - What we know: `Disallow: /contract` blocks everything starting with `/contract` (prefix match). This covers /contract, /contract/{guid}, /contract/download/{guid}.
   - What's unclear: Nothing -- this is well-documented behavior.
   - Recommendation: Use `Disallow: /contract` without trailing slash to block all contract-related paths with a single rule.

## Sources

### Primary (HIGH confidence)
- [Google Search Central - robots.txt](https://developers.google.com/search/docs/crawling-indexing/robots/intro) - robots.txt syntax, path matching rules
- [Google Search Central - Block indexing with noindex](https://developers.google.com/search/docs/crawling-indexing/block-indexing) - noindex meta tag implementation, interaction with robots.txt
- [Microsoft Learn - Control head content in Blazor apps](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content?view=aspnetcore-9.0) - HeadContent component usage
- [sitemaps.org Protocol](https://www.sitemaps.org/protocol.html) - sitemap.xml format, lastmod W3C datetime
- [MDN - Web app manifest](https://developer.mozilla.org/en-US/docs/Web/Progressive_web_apps/Manifest) - manifest field specifications
- Codebase analysis - All 9 target .razor files read and verified, render modes confirmed

### Secondary (MEDIUM confidence)
- [Bing Webmaster Blog - lastmod importance](https://blogs.bing.com/webmaster/february-2023/The-Importance-of-Setting-the-lastmod-Tag-in-Your-Sitemap) - lastmod significance for Bing
- [Yoast - Google and Bing stress importance of lastmod](https://yoast.com/lastmod-xml-sitemaps-google-bing/) - lastmod best practices
- Project research files (.planning/research/PITFALLS.md, ARCHITECTURE.md, STACK.md) - Previously verified patterns for this codebase

### Tertiary (LOW confidence)
- None. All findings verified with primary sources.

## Metadata

**Confidence breakdown:**
- Standard stack: HIGH - Uses only .NET 9 built-in components (`<HeadContent>`), no packages needed. Verified against official Microsoft docs.
- Architecture: HIGH - All target files identified and read. Render modes confirmed (static SSR vs InteractiveServer). HeadContent works on both.
- Pitfalls: HIGH - Critical interaction between robots.txt blocking and noindex visibility verified with Google's official documentation. Defense-in-depth strategy is sound.

**Research date:** 2026-03-03
**Valid until:** 2026-04-03 (stable domain -- robots.txt, sitemap, manifest specs change very rarely)
