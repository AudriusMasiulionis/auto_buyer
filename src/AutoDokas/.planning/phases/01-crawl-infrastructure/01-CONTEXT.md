# Phase 1: Crawl Infrastructure - Context

**Gathered:** 2026-03-03
**Status:** Ready for planning

<domain>
## Phase Boundary

Fix all crawl blockers so Google can discover and correctly classify autodokas.lt. Only the homepage (/) should be indexed. All other pages (contract form, legal, app routes, test pages) should be blocked or noindexed. Fix the lang attribute and complete the web manifest.

</domain>

<decisions>
## Implementation Decisions

### robots.txt Strategy
- Allow all paths by default (remove `Disallow: /`), rely on noindex meta tags for non-content pages
- Specifically block test pages (/email-test, /contract-test) via Disallow — these shouldn't be public
- Block app routes (/contract/*, /buyer/*, /BuyerNotificationSent, /ContractCompleted) via Disallow as defense-in-depth
- Keep sitemap reference: `Sitemap: https://autodokas.lt/sitemap.xml`

### noindex Implementation
- Add `<meta name="robots" content="noindex">` to every non-indexed page
- Pages that get noindex: /contract (empty form), /terms-and-conditions, /privacy-policy, /Error, /BuyerNotificationSent, /ContractCompleted, /contract/{id}, /buyer/{id}, /contract/download/{id}
- Test pages (/email-test, /contract-test) also get noindex as additional protection

### Web Manifest
- name: "AutoDokas"
- short_name: "AutoDokas"
- description: "Automobilio pirkimo pardavimo sutartis internetu" (matches primary keyword)

### Sitemap
- Keep as static file at wwwroot/sitemap.xml (only 1 indexed URL — homepage)
- Update to include lastmod date
- Remove any non-homepage URLs

### Lang Attribute
- Change `<html lang="en">` to `<html lang="lt">` in App.razor (line 3)

### Claude's Discretion
- Exact robots.txt formatting and ordering of rules
- Whether to use a shared Blazor component for noindex or inline HeadContent per page
- lastmod date format and value in sitemap.xml

</decisions>

<code_context>
## Existing Code Insights

### Reusable Assets
- `<HeadOutlet/>` in App.razor: Already renders dynamic head content — noindex tags via `<HeadContent>` will work
- `<PageTitle>` component: Already used on each page for titles
- Blazor's `<HeadContent>` component: Can inject any meta tags into `<head>`

### Established Patterns
- Static SSR for public pages (Home, TermsAndConditions, PrivacyPolicy): Meta tags render in initial HTML, crawlers see them
- InteractiveServer for contract pages: Meta tags still render at prerender time

### Integration Points
- `wwwroot/robots.txt` — static file, direct edit
- `wwwroot/sitemap.xml` — static file, direct edit
- `wwwroot/site.webmanifest` — static file, direct edit (JSON)
- `Components/App.razor` line 3 — `<html lang="en">` → `<html lang="lt">`
- Each `.razor` page file — add `<HeadContent>` with noindex meta tag
- Pages needing noindex: Contract.razor, BuyerNotificationSent.razor, ContractCompleted.razor, ContractDownload.razor, TermsAndConditions.razor, PrivacyPolicy.razor, Error.razor, EmailSendTestPage.razor, ContractPdfTemplatePage.razor

</code_context>

<specifics>
## Specific Ideas

No specific requirements — straightforward infrastructure fixes following SEO best practices.

</specifics>

<deferred>
## Deferred Ideas

None — discussion stayed within phase scope.

</deferred>

---

*Phase: 01-crawl-infrastructure*
*Context gathered: 2026-03-03*
