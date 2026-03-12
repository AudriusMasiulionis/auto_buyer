# Roadmap: AutoDokas SEO Optimization

## Overview

AutoDokas is invisible to search engines due to a broken robots.txt and missing metadata. This roadmap fixes the three layers of that problem in dependency order: first unblock crawlers so Google can reach the site, then optimize the homepage with meta tags and structured data so Google understands what the site offers, then validate everything and submit to Search Console so changes take effect. Only the homepage is indexed -- all other pages are noindex.

## Phases

**Phase Numbering:**
- Integer phases (1, 2, 3): Planned milestone work
- Decimal phases (2.1, 2.2): Urgent insertions (marked with INSERTED)

Decimal phases appear between their surrounding integers in numeric order.

- [ ] **Phase 1: Crawl Infrastructure** - Unblock search engine crawlers and fix site-level configuration
- [ ] **Phase 2: Homepage SEO** - Optimize homepage with meta tags, Open Graph, and structured data
- [ ] **Phase 3: Validation and Submission** - Verify implementation and submit to Google Search Console

## Phase Details

### Phase 1: Crawl Infrastructure
**Goal**: Search engines can discover and correctly classify the site
**Depends on**: Nothing (first phase)
**Requirements**: CRAWL-01, CRAWL-02, CRAWL-03, CRAWL-04, CRAWL-05
**Success Criteria** (what must be TRUE):
  1. Googlebot can crawl the homepage and sitemap.xml (robots.txt allows access, blocks only private routes)
  2. sitemap.xml returns the homepage URL with a valid lastmod date
  3. The HTML document declares `lang="lt"` (not "en")
  4. Contract form pages, legal pages, and app-internal pages serve a noindex meta tag
  5. Web manifest has complete name, short_name, and description fields
**Plans**: 2 plans

Plans:
- [x] 01-01-PLAN.md — Fix robots.txt, sitemap.xml, site.webmanifest, and HTML lang attribute
- [x] 01-02-PLAN.md — Add noindex meta tags to 9 non-content pages

### Phase 2: Homepage SEO
**Goal**: The homepage is fully optimized for Lithuanian vehicle sale contract search queries
**Depends on**: Phase 1
**Requirements**: META-01, META-02, META-03, META-04, SCHEMA-01, SCHEMA-02, SCHEMA-03, SCHEMA-04
**Success Criteria** (what must be TRUE):
  1. Homepage title contains researched Lithuanian keywords for vehicle sale contracts (not generic "Pagrindinis")
  2. Homepage meta description appears as a compelling SERP snippet (max 160 chars, Lithuanian, keyword-rich)
  3. Homepage has a self-referential canonical URL and Open Graph tags (og:title, og:description, og:url, og:type, og:site_name)
  4. Homepage serves Organization, WebSite, WebPage, and Service JSON-LD schemas that parse without errors
**Plans**: 2 plans

Plans:
- [x] 02-01-PLAN.md — Add keyword-optimized title, meta description, canonical URL, and Open Graph tags
- [x] 02-02-PLAN.md — Add JSON-LD structured data (Organization, WebSite, WebPage, Service) via Schema.NET

### Phase 02.1: Homepage Content Optimization (INSERTED)

**Goal:** The homepage has rich, keyword-optimized content with proper heading hierarchy, new content sections (service description, stats, FAQ), and FAQPage structured data
**Depends on:** Phase 2
**Requirements**: CONTENT-01, CONTENT-02, CONTENT-03, CONTENT-04, CONTENT-05, CONTENT-06
**Success Criteria** (what must be TRUE):
  1. Homepage heading hierarchy flows h1 -> h2 -> h3 (no h5 content headings)
  2. All section headings contain Lithuanian vehicle sale contract keywords (not generic text)
  3. Hero subtitle and all section body text expanded to 2-3 sentences with keyword variations
  4. Service description section appears between hero and How It Works
  5. Stats counter section displays real contract count from database
  6. FAQ section displays 5-8 fully expanded questions with keyword-rich content and internal links
  7. FAQPage JSON-LD structured data renders alongside existing schemas without errors
**Plans**: 2 plans

Plans:
- [x] 02.1-01-PLAN.md — Heading fixes, expanded copy, service description section, stats counter section
- [x] 02.1-02-PLAN.md — FAQ section with 5-8 Q&A pairs and FAQPage JSON-LD structured data

### Phase 3: Validation and Submission
**Goal**: All SEO changes are verified correct and submitted to Google for indexing
**Depends on**: Phase 2
**Requirements**: VALID-01, VALID-02, VALID-03, VALID-04
**Success Criteria** (what must be TRUE):
  1. Updated sitemap.xml is submitted in Google Search Console and accepted without errors
  2. Homepage URL Inspection in Search Console shows the page is indexable with correct meta tags
  3. Google Rich Results Test returns zero errors for all structured data on the homepage
  4. Lighthouse SEO audit scores 90 or higher
**Plans**: 3 plans

Plans:
- [ ] 03-01-PLAN.md — Deployment verification, Lighthouse SEO audit (90+ target), and Search Console submission documentation
- [ ] 03-02-PLAN.md — Structured data validation via Rich Results Test and Schema Markup Validator
- [ ] 03-03-PLAN.md — Cross-phase content and schema integration verification (all 5 JSON-LD schemas, heading hierarchy, FAQ content)

## Progress

**Execution Order:**
Phases execute in numeric order: 1 -> 2 -> 2.1 -> 3

| Phase | Plans Complete | Status | Completed |
|-------|----------------|--------|-----------|
| 1. Crawl Infrastructure | 2/2 | Complete | 2026-03-03 |
| 2. Homepage SEO | 2/2 | Complete | 2026-03-03 |
| 2.1 Homepage Content Optimization | 2/2 | Complete | 2026-03-03 |
| 3. Validation and Submission | 0/3 | Not started | - |
