---
phase: quick
plan: 1
subsystem: seo-landing-pages
tags: [seo, landing-pages, json-ld, razor, lithuanian]
dependency-graph:
  requires: [Home.razor SEO pattern, Text.resx resources, app.css styles]
  provides: [5 SEO landing pages, expanded sitemap]
  affects: [sitemap.xml, search engine indexing]
tech-stack:
  added: []
  patterns: [landing page template with WebPage+FAQPage JSON-LD]
key-files:
  created:
    - Components/Pages/Landing/SutartisInternetu.razor
    - Components/Pages/Landing/TransportoPriemonesSutartis.razor
    - Components/Pages/Landing/NaudotoAutomobilioSutartis.razor
    - Components/Pages/Landing/AutomobilioPardavimoSutartis.razor
    - Components/Pages/Landing/SutartiesForma.razor
  modified:
    - Resources/Text.resx
    - Resources/Text.Designer.cs
    - wwwroot/sitemap.xml
decisions:
  - Simpler landing page structure than homepage (no hero image, no steps, no features -- focused on content + FAQ + CTA)
  - WebSite + WebPage + FAQPage JSON-LD per page (no Organization or Service schema -- those are homepage-only)
  - 4 FAQ pairs per landing page (vs 7 on homepage) for unique content without overlap
  - col-lg-8 hero layout (no image column) for text-focused landing pages
metrics:
  duration: ~5 min
  completed: 2026-03-12
---

# Quick Task 1: SEO Landing Pages Summary

5 SEO landing pages targeting Lithuanian long-tail keywords for vehicle sale contracts, each with unique content, FAQ, JSON-LD structured data, and CTA

## What Was Done

### Task 1: Create 5 landing page Razor components with unique content and full SEO
**Commit:** `d62d4b0`

Created 5 landing pages in `Components/Pages/Landing/` directory, each following the Home.razor SEO pattern:

| Page | Route | Target Keyword | Angle |
|------|-------|---------------|-------|
| SutartisInternetu.razor | /automobilio-sutartis-internetu | automobilio sutartis internetu | Online convenience |
| TransportoPriemonesSutartis.razor | /transporto-priemones-pirkimo-pardavimo-sutartis | transporto priemones pirkimo pardavimo sutartis | Formal/legal |
| NaudotoAutomobilioSutartis.razor | /naudoto-automobilio-pirkimo-sutartis | naudoto automobilio pirkimo sutartis | Used car buyer protection |
| AutomobilioPardavimoSutartis.razor | /automobilio-pardavimo-sutartis | automobilio pardavimo sutartis | Seller-focused |
| SutartiesForma.razor | /automobilio-sutarties-forma | automobilio sutarties forma/blankas | Form/template seekers |

Each page includes:
- Unique `@page` route, `PageTitle`, `HeadContent` (meta description, canonical URL, OG tags)
- JSON-LD structured data: WebPage + FAQPage schemas (3 JSON-LD blocks per page)
- Hero section with keyword-rich h1 and subtitle
- Content section with 3 unique paragraphs (h2 + service-desc)
- FAQ accordion with 4 unique Q&A pairs (different from homepage and each other)
- CTA section linking to /contract

All 80+ resource strings added to Text.resx with `Lp{N}` naming convention. No hardcoded Lithuanian text in .razor files.

### Task 2: Update sitemap.xml and verify robots.txt
**Commit:** `d5fd49f`

- Added all 5 landing page URLs to sitemap.xml with `lastmod: 2026-03-12` and `priority: 0.8`
- Total sitemap URLs: 6 (homepage + 5 landing pages)
- Verified robots.txt does not block any landing page URL (all use different path prefixes from `/contract`)

## Deviations from Plan

None -- plan executed exactly as written.

## Verification Results

- `dotnet build`: 0 errors, 6 pre-existing warnings
- Sitemap: 6 URLs, valid XML
- robots.txt: No blocking of landing page URLs
- Each page: unique route, PageTitle, HeadContent, JSON-LD, hero, content, FAQ, CTA
- No content duplication between pages or with homepage
- All Lithuanian text uses proper diacritics and em dashes

## Self-Check: PASSED

- All 5 landing page .razor files: FOUND
- SUMMARY.md: FOUND
- Commit d62d4b0: FOUND
- Commit d5fd49f: FOUND
