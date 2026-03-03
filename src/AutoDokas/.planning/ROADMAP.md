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
- [ ] 01-01-PLAN.md — Fix robots.txt, sitemap.xml, site.webmanifest, and HTML lang attribute
- [ ] 01-02-PLAN.md — Add noindex meta tags to 9 non-content pages

### Phase 2: Homepage SEO
**Goal**: The homepage is fully optimized for Lithuanian vehicle sale contract search queries
**Depends on**: Phase 1
**Requirements**: META-01, META-02, META-03, META-04, SCHEMA-01, SCHEMA-02, SCHEMA-03, SCHEMA-04
**Success Criteria** (what must be TRUE):
  1. Homepage title contains researched Lithuanian keywords for vehicle sale contracts (not generic "Pagrindinis")
  2. Homepage meta description appears as a compelling SERP snippet (max 160 chars, Lithuanian, keyword-rich)
  3. Homepage has a self-referential canonical URL and Open Graph tags (og:title, og:description, og:url, og:type, og:site_name)
  4. Homepage serves Organization, WebSite, WebPage, and Service JSON-LD schemas that parse without errors
**Plans**: TBD

Plans:
- [ ] 02-01: TBD
- [ ] 02-02: TBD

### Phase 3: Validation and Submission
**Goal**: All SEO changes are verified correct and submitted to Google for indexing
**Depends on**: Phase 2
**Requirements**: VALID-01, VALID-02, VALID-03, VALID-04
**Success Criteria** (what must be TRUE):
  1. Updated sitemap.xml is submitted in Google Search Console and accepted without errors
  2. Homepage URL Inspection in Search Console shows the page is indexable with correct meta tags
  3. Google Rich Results Test returns zero errors for all structured data on the homepage
  4. Lighthouse SEO audit scores 90 or higher
**Plans**: TBD

Plans:
- [ ] 03-01: TBD

## Progress

**Execution Order:**
Phases execute in numeric order: 1 -> 2 -> 3

| Phase | Plans Complete | Status | Completed |
|-------|----------------|--------|-----------|
| 1. Crawl Infrastructure | 0/2 | Not started | - |
| 2. Homepage SEO | 0/? | Not started | - |
| 3. Validation and Submission | 0/? | Not started | - |
