# Requirements: AutoDokas SEO Optimization

**Defined:** 2026-03-03
**Core Value:** Fix technical SEO blockers so autodokas.lt appears in Lithuanian search results for vehicle sale contract queries and drives steady organic traffic.

## v1 Requirements

Requirements for the SEO optimization release. Each maps to roadmap phases.

### Crawl Infrastructure

- [x] **CRAWL-01**: robots.txt allows crawling of homepage and sitemap, blocks contract form and app pages
- [x] **CRAWL-02**: sitemap.xml lists only the homepage with lastmod date
- [x] **CRAWL-03**: `<html lang="lt">` attribute correctly set (currently "en")
- [x] **CRAWL-04**: Web manifest has name, short_name, and description filled in
- [x] **CRAWL-05**: Contract form pages, legal pages, and non-content pages have noindex meta tag

### Meta Tags

- [x] **META-01**: Homepage has keyword-optimized title using researched Lithuanian search terms
- [x] **META-02**: Homepage has meta description with Lithuanian keywords (max 160 chars)
- [x] **META-03**: Homepage has self-referential canonical URL
- [x] **META-04**: Homepage has Open Graph tags (og:title, og:description, og:url, og:type, og:site_name)

### Structured Data

- [x] **SCHEMA-01**: Organization JSON-LD schema present on homepage
- [x] **SCHEMA-02**: WebSite JSON-LD schema present on homepage
- [x] **SCHEMA-03**: WebPage JSON-LD schema present on homepage
- [x] **SCHEMA-04**: Service JSON-LD schema describing the contract generation tool

### Validation

- [ ] **VALID-01**: Updated sitemap submitted to Google Search Console
- [ ] **VALID-02**: Homepage reindexing requested via Search Console URL Inspection
- [ ] **VALID-03**: Structured data validated with Google Rich Results Test (no errors)
- [ ] **VALID-04**: Lighthouse SEO audit score 90+ (fix any issues found)

## v2 Requirements

### Content Strategy

- **CONT-01**: Blog/article pages targeting long-tail Lithuanian keywords
- **CONT-02**: FAQ page with common vehicle sale contract questions
- **CONT-03**: FAQ JSON-LD schema for rich snippet eligibility

### Enhanced SEO

- **ESEO-01**: Multi-language support with hreflang tags (if app goes bilingual)
- **ESEO-02**: BreadcrumbList JSON-LD schema
- **ESEO-03**: OG image asset (1200x630px) for social sharing previews

## Out of Scope

| Feature | Reason |
|---------|--------|
| New content pages or blog | User wants optimization of existing pages only |
| Link building / off-page SEO | Focus is on technical on-page SEO |
| Paid advertising (Google Ads) | Organic traffic is the goal |
| English-language SEO | Lithuanian market only, multi-lang not implemented |
| UI/UX redesign | App works, just needs discoverability |
| Indexing contract form pages | Functional app pages with no content, should be noindexed |
| Indexing legal pages | No SEO value, noindex is appropriate |
| Keyword stuffing or hidden text | Against Google guidelines, will get penalized |
| Meta keywords tag | Ignored by Google since 2009 |

## Traceability

Which phases cover which requirements. Updated during roadmap creation.

| Requirement | Phase | Status |
|-------------|-------|--------|
| CRAWL-01 | Phase 1 | Complete |
| CRAWL-02 | Phase 1 | Complete |
| CRAWL-03 | Phase 1 | Complete |
| CRAWL-04 | Phase 1 | Complete |
| CRAWL-05 | Phase 1 | Complete |
| META-01 | Phase 2 | Complete |
| META-02 | Phase 2 | Complete |
| META-03 | Phase 2 | Complete |
| META-04 | Phase 2 | Complete |
| SCHEMA-01 | Phase 2 | Complete |
| SCHEMA-02 | Phase 2 | Complete |
| SCHEMA-03 | Phase 2 | Complete |
| SCHEMA-04 | Phase 2 | Complete |
| VALID-01 | Phase 3 | Pending |
| VALID-02 | Phase 3 | Pending |
| VALID-03 | Phase 3 | Pending |
| VALID-04 | Phase 3 | Pending |

**Coverage:**
- v1 requirements: 17 total
- Mapped to phases: 17
- Unmapped: 0

---
*Requirements defined: 2026-03-03*
*Last updated: 2026-03-03 after roadmap creation*
