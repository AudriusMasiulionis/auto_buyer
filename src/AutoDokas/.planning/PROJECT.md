# AutoDokas SEO Optimization

## What This Is

SEO optimization project for autodokas.lt — a Lithuanian vehicle sale contract tool (Blazor Server). The app works but has zero organic discovery. People searching for "automobilio pirkimo pardavimo sutartis" find government sites and PDF templates instead. This project fixes the technical SEO gaps so search engines can properly index and rank the site.

## Core Value

Fix the critical SEO blockers (robots.txt, meta tags, structured data) so that autodokas.lt appears in Lithuanian search results for vehicle sale contract queries and drives steady organic traffic.

## Requirements

### Validated

- ✓ Working vehicle sale contract tool — existing
- ✓ Landing page with keyword-rich headline ("Automobilio pirkimo pardavimo sutartis internetu") — existing
- ✓ Google Search Console verified — existing
- ✓ Google Analytics with Consent Mode v2 — existing
- ✓ Bilingual support (LT/EN) — existing
- ✓ HTTPS, HSTS, security headers — existing
- ✓ Favicons and PWA icons — existing
- ✓ Cookie consent system — existing

### Active

- [ ] Fix robots.txt to allow crawlers to index all public pages
- [ ] Expand sitemap.xml to include all public pages with lastmod
- [ ] Add meta descriptions to all pages (Lithuanian, keyword-optimized)
- [ ] Add structured data (JSON-LD) — Organization, Service, WebPage schemas
- [ ] Add Open Graph tags for social media sharing
- [ ] Add canonical URLs to all pages
- [ ] Add hreflang tags for LT/EN language variants
- [ ] Complete web manifest (name, short_name, description)

### Out of Scope

- New content pages or blog — user wants optimization only, not content creation
- Link building or off-page SEO — focus is on technical on-page SEO
- Paid advertising (Google Ads) — organic traffic is the goal
- English-language SEO targeting — Lithuanian market only
- UI/UX redesign — the app works, just needs discoverability

## Context

- Domain: autodokas.lt ("auto" + "dokas" = vehicle expert)
- Primary keyword: "automobilio pirkimo pardavimo sutartis" (vehicle purchase-sale contract)
- Competition: Government/registry sites (Regitra) rank for the main term
- Tech: Blazor Server (.NET) — server-side rendered, crawlable
- Current robots.txt is critically broken — blocks all paths except `/` and `/sitemap.xml`
- Sitemap only lists homepage — contract page and legal pages missing
- Pages use Blazor's `<PageTitle>` and `<HeadOutlet/>` — meta tags can be added via `<HeadContent>`
- Localization uses query string and cookie culture providers, default is Lithuanian (lt)
- Public routable pages: `/`, `/contract`, `/terms-and-conditions`, `/privacy-policy`
- Non-indexable pages (behind user actions): `/contract/{id}`, `/buyer/{id}`, `/contract/download/{id}`, `/BuyerNotificationSent`, `/ContractCompleted`

## Constraints

- **Tech stack**: Blazor Server (.NET) — all changes must work within this framework
- **Scope**: Technical on-page SEO only — no new pages, no content marketing
- **Market**: Lithuanian language and market only
- **Existing structure**: Two main pages (landing + contract form) plus legal pages — optimize what exists

## Key Decisions

| Decision | Rationale | Outcome |
|----------|-----------|---------|
| SEO optimization only, no content strategy | User wants to fix technical gaps first, not build a content engine | — Pending |
| Lithuanian market focus | Primary audience is Lithuanian vehicle buyers/sellers | — Pending |
| Target steady organic traffic over single keyword ranking | Broader long-tail capture is more sustainable than chasing one term | — Pending |

---
*Last updated: 2026-03-02 after initialization*
