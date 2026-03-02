# Feature Research: SEO Optimization for autodokas.lt

**Domain:** Technical SEO for a Lithuanian vehicle sale contract tool (Blazor Server)
**Researched:** 2026-03-02
**Confidence:** HIGH (features are well-established SEO fundamentals, verified across multiple sources)

## Feature Landscape

### Table Stakes (Google Won't Rank You Without These)

Features that are non-negotiable for basic search visibility. Missing any of these means Google either cannot crawl the site properly or will not display it favorably in SERPs.

| Feature | Why Expected | Complexity | Notes |
|---------|--------------|------------|-------|
| **Fix robots.txt** | Current file blocks all paths except `/` and `/sitemap.xml`. Google literally cannot crawl `/contract`, `/terms-and-conditions`, or `/privacy-policy`. This is the single biggest blocker. | LOW | Change `Disallow: /` to allow public pages. Keep disallowing dynamic contract pages (`/contract/{id}`, `/buyer/{id}`, `/contract/download/{id}`, `/BuyerNotificationSent`, `/ContractCompleted`). |
| **Expand sitemap.xml** | Sitemap only lists homepage. Missing 3 indexable pages. Google uses sitemaps for discovery and to understand site structure. | LOW | Add `/contract`, `/terms-and-conditions`, `/privacy-policy` with `<lastmod>` dates. Only 4 URLs total. |
| **Unique, keyword-optimized title tags** | Current homepage title is generic "Pagrindinis" (Home). The primary keyword "automobilio pirkimo pardavimo sutartis" must appear in the homepage title. Google weighs title tags heavily. Competitor autosutartis.lt uses "Automobilio pirkimo-pardavimo sutartis internetu \| Autosutartis.lt". | LOW | Target 50-60 characters. Front-load the primary keyword. Each page needs a unique title. Homepage: "Automobilio pirkimo pardavimo sutartis internetu \| AutoDokas.lt". Contract: "Nauja sutartis - Automobilio pirkimo pardavimo forma \| AutoDokas.lt". |
| **Meta descriptions on all pages** | No meta descriptions exist on any page. Google uses these for SERP snippets. Without them, Google auto-generates snippets which are often suboptimal. Meta descriptions indirectly influence rankings via CTR. | LOW | 150-160 characters each. Include primary keyword naturally. Add a call to action ("Uzpildykite nemokamai" / "Fill out for free"). Use `<HeadContent>` component. |
| **Fix `<html lang="en">` to `lang="lt"`** | App.razor declares `lang="en"` but all content is Lithuanian. This mismatch confuses Bing (which uses lang attribute for language detection) and assistive technologies. Google detects language algorithmically but this is still wrong. | LOW | Single character change in App.razor. Critical correctness fix. |
| **Canonical URLs on all pages** | No canonical tags exist. Without them, Google may index URL variants (trailing slashes, query params) as duplicate content, diluting ranking signals. Small sites are especially vulnerable because they have fewer pages to absorb duplicate content damage. | LOW | Add `<link rel="canonical" href="https://autodokas.lt/[path]" />` via `<HeadContent>` on each page. Self-referencing canonicals are the standard practice. |

### Differentiators (Competitive Advantage in Lithuanian SERPs)

Features that go beyond basics. These help autodokas.lt compete against established sites like regitra.lt, autosutartis.lt, and esablonai.lt for Lithuanian vehicle contract searches.

| Feature | Value Proposition | Complexity | Notes |
|---------|-------------------|------------|-------|
| **JSON-LD Structured Data: Organization** | Establishes autodokas.lt as a recognized entity in Google's Knowledge Graph. Competitor autosutartis.lt already has Organization schema. Helps Google associate the site with "MB Kibernetinis Dangus" and display rich organizational info. | LOW | Add to App.razor or MainLayout.razor. Include name, url, contactPoint (email), logo. Google recommends JSON-LD as the preferred format. |
| **JSON-LD Structured Data: WebSite** | Declares the site as a WebSite entity with name and URL. Competitor autosutartis.lt already uses this. Helps Google understand site identity. | LOW | Straightforward schema. Combine with Organization in a single `<script type="application/ld+json">` block or use separate blocks. |
| **JSON-LD Structured Data: WebPage per page** | Marks each page with its type (WebPage for general, AboutPage for legal). Enables Google to better understand page purpose and relationships. | LOW | Add to each page's `<HeadContent>`. Include name, description, url, inLanguage: "lt". |
| **JSON-LD Structured Data: Service** | Describes the contract generation as a formal Service offering. No competitors do this well. Helps Google understand what autodokas.lt actually does. Could trigger rich results for service-related queries. | MEDIUM | Use schema.org/Service with serviceType, provider (Organization), areaServed ("LT"), availableChannel (website). Must accurately reflect page content per Google's guidelines. |
| **Open Graph tags (og:title, og:description, og:image, og:url)** | Controls how the site appears when shared on Facebook, LinkedIn, and messaging apps. Lithuanian users share links on Facebook heavily. A well-formatted share card builds trust and drives clicks. Without OG tags, social platforms guess poorly. | LOW | Add to each page via `<HeadContent>`. Requires an OG image (1200x630px recommended). The hero car image could be adapted. og:locale should be "lt_LT". |
| **FAQ Schema for landing page** | Competitor autosutartis.lt uses FAQPage schema with 5 Q&A pairs. While Google restricted FAQ rich results to authoritative sites in 2023, the structured data still helps with featured snippets and AI search engines. Provides clear answers to common queries like "Ar sutartis turi juridine galia?" (Does the contract have legal validity?). | MEDIUM | Requires writing 4-6 Lithuanian FAQ pairs. Content must already exist on the page (Google requires parity between markup and visible content). Since project scope excludes content creation, this feature depends on whether existing landing page text can be structured as Q&A. Flag: this borders on content creation which is out of scope. |
| **Complete web manifest** | Current manifest has empty name and short_name fields. While not directly an SEO factor, a complete manifest improves "installability" signals and can influence how the site appears in app-related searches. More importantly, it's a completeness/professionalism signal. | LOW | Fill in name: "AutoDokas", short_name: "AutoDokas", description in Lithuanian, start_url, scope. |
| **Semantic HTML improvements** | Using proper heading hierarchy (h1, h2, h3) and semantic elements (article, section, nav, main, footer) helps Google parse page structure. Current pages use semantic elements reasonably well but some improvements possible (e.g., hero image alt text is empty `alt=""`). | LOW | Audit heading hierarchy. Add descriptive alt text to images. Ensure single h1 per page. |

### Anti-Features (Things to Deliberately NOT Build)

Features that seem beneficial but would be harmful, wasteful, or violate Google's guidelines in this context.

| Feature | Why Requested | Why Problematic | Alternative |
|---------|---------------|-----------------|-------------|
| **Keyword stuffing in meta tags** | "More keywords = higher ranking" intuition | Google explicitly penalizes keyword-stuffed titles and descriptions. Lithuanian is grammatically rich, so unnatural keyword repetition is obvious. Natural Lithuanian with case variations is better. | Write naturally in Lithuanian. The primary keyword "automobilio pirkimo pardavimo sutartis" and its grammatical variants will appear organically. |
| **Hidden text / invisible content** | Temptation to add keyword-rich text hidden via CSS | Google's structured data guidelines require that JSON-LD content match visible page content. Hidden text is a spam signal that can trigger manual penalties. | Ensure all structured data accurately reflects visible content. If content doesn't exist on the page, don't mark it up. |
| **Hreflang tags** | "Should add for international SEO" | The site is Lithuanian-only. Hreflang is for multi-language/multi-region sites. Adding hreflang for a single-language site is unnecessary complexity that could create errors. Google detects Lithuanian content algorithmically. | Set `lang="lt"` on the html element. That's sufficient for a single-language site. |
| **Blog / content pages** | "More pages = more ranking opportunities" | Explicitly out of scope per PROJECT.md. Content marketing requires ongoing effort that diverts from the technical SEO goal. A 4-page tool site can rank well for its niche with proper technical SEO. | Focus on optimizing existing 4 pages perfectly. Consider content strategy as a separate future initiative. |
| **Excessive structured data types** | "Add every schema type" | Google warns against marking up content not visible to users. Adding schemas for content that doesn't exist (e.g., Review, Product, Event) violates guidelines and can disqualify from rich results entirely. | Use only Organization, WebSite, WebPage, and Service schemas that accurately reflect actual page content. |
| **Dynamic server-side rendered sitemap** | "Generate sitemap from routes automatically" | Overkill for 4 static URLs. Adds unnecessary complexity, build-time dependency, and a potential point of failure. Static XML is simpler and more reliable. | Keep a hand-maintained static sitemap.xml. With only 4 URLs, maintenance is trivial. |
| **Meta keywords tag** | "Add keywords meta tag for SEO" | Google has officially stated it does not use the meta keywords tag for ranking. It has been ignored since 2009. Adding it provides zero SEO value and reveals your keyword strategy to competitors. | Skip entirely. Invest effort in title tags and meta descriptions instead. |
| **Aggressive AI bot blocking** | "Block GPTBot, CCBot, etc. in robots.txt" | While some sites block AI crawlers for content protection, autodokas.lt's content is a tool, not articles. Blocking AI bots provides no benefit and could reduce visibility in AI-powered search results (like Google's AI Overviews). | Allow all standard crawlers. The site is a tool, not a content publisher. Being indexed by AI systems increases discovery. |

## Feature Dependencies

```
[Fix robots.txt]
    └──enables──> [Google can crawl all pages]
                       └──enables──> [Meta descriptions matter]
                       └──enables──> [Canonical URLs are effective]
                       └──enables──> [Structured data gets indexed]
                       └──enables──> [Sitemap expansion is meaningful]

[Fix html lang="lt"]
    └──independent──> (no dependencies, immediate correctness fix)

[Expand sitemap.xml]
    └──requires──> [Fix robots.txt] (no point listing pages in sitemap if robots.txt blocks them)

[Meta descriptions]
    └──requires──> [Fix robots.txt] (descriptions only matter if pages are crawlable)
    └──enhances──> [Title tag optimization] (title + description together form the SERP snippet)

[Title tag optimization]
    └──requires──> [Fix robots.txt] (titles only display in SERPs if pages are indexed)

[Canonical URLs]
    └──requires──> [Fix robots.txt] (canonicals only matter for crawled pages)

[JSON-LD Organization + WebSite]
    └──requires──> [Fix robots.txt] (structured data must be on crawlable pages)
    └──enhances──> [JSON-LD WebPage] (Organization is referenced by WebPage via publisher)

[JSON-LD WebPage per page]
    └──requires──> [JSON-LD Organization] (references Organization as publisher)
    └──requires──> [Meta descriptions] (WebPage description should match meta description)

[JSON-LD Service]
    └──requires──> [JSON-LD Organization] (Service references Organization as provider)
    └──requires──> [JSON-LD WebPage] (Service is part of a WebPage)

[Open Graph tags]
    └──independent──> (works regardless of robots.txt; social crawlers ignore robots.txt)
    └──enhances──> [Meta descriptions] (og:description often mirrors meta description)

[FAQ Schema]
    └──requires──> [Visible FAQ content on page] (Google requires content parity)
    └──conflicts──> [Out-of-scope content creation] (writing FAQs is content work)

[Complete web manifest]
    └──independent──> (no SEO dependencies)
```

### Dependency Notes

- **Fix robots.txt is the critical path**: Every other SEO feature is ineffective if Google cannot crawl the pages. This is the absolute first thing to implement.
- **Sitemap requires robots.txt**: Listing pages in the sitemap while robots.txt blocks them creates a contradiction that confuses crawlers.
- **Structured data builds incrementally**: Organization/WebSite first, then WebPage references Organization, then Service references both.
- **FAQ Schema conflicts with scope**: Writing FAQ content is content creation, which PROJECT.md explicitly excludes. This feature should be flagged as a potential future addition only if FAQ content already exists on the landing page (it does not currently).
- **Open Graph is independent**: Social media crawlers (Facebook, LinkedIn) do not respect robots.txt, so OG tags work regardless. However, they complement the rest of the SEO work.

## MVP Definition

### Launch With (v1) -- Critical Path

Minimum viable SEO -- what's needed to unblock Google indexing and appear in Lithuanian SERPs.

- [x] **Fix robots.txt** -- Unblocks everything. Without this, nothing else matters. (~5 min change)
- [x] **Fix `<html lang="lt">`** -- Correctness fix. Single character change in App.razor. (~1 min)
- [x] **Expand sitemap.xml** -- Add 3 missing public pages with lastmod. (~10 min)
- [x] **Optimize title tags** -- Replace generic "Pagrindinis" with keyword-rich Lithuanian titles on all 4 pages. (~30 min)
- [x] **Add meta descriptions** -- Write 150-160 character Lithuanian descriptions for all 4 pages. (~30 min)
- [x] **Add canonical URLs** -- Self-referencing canonical tags on all 4 pages. (~15 min)

### Add After Validation (v1.x)

Features to add once pages are indexed and appearing in Google Search Console data.

- [ ] **JSON-LD Organization + WebSite** -- Add after confirming pages are indexed. Trigger: pages appear in Search Console.
- [ ] **JSON-LD WebPage per page** -- Add after Organization schema is validated.
- [ ] **Open Graph tags** -- Add when social sharing becomes a traffic source. Trigger: any social referral traffic in Analytics, or when user wants to promote on social media.
- [ ] **Complete web manifest** -- Add alongside other metadata work. Low effort, low urgency.
- [ ] **Semantic HTML audit** -- Review heading hierarchy and alt text after core SEO is in place.

### Future Consideration (v2+)

Features to defer until after initial SEO results are measured (3-6 months post-launch).

- [ ] **JSON-LD Service schema** -- Defer until basic structured data is indexed and validated. More complex to implement correctly.
- [ ] **FAQ Schema + FAQ content** -- Defer because it requires writing new content (out of current scope). Revisit if user decides to add content strategy.
- [ ] **Additional long-tail keyword optimization** -- Defer until Search Console shows which queries bring impressions. Then optimize existing content for actual search terms.
- [ ] **Performance optimization for Core Web Vitals** -- Defer unless Lighthouse/PageSpeed scores reveal problems. Blazor Server pre-renders HTML which is generally good for LCP.

## Feature Prioritization Matrix

| Feature | User Value | Implementation Cost | Priority |
|---------|------------|---------------------|----------|
| Fix robots.txt | HIGH (unblocks all crawling) | LOW (text file edit) | **P1** |
| Fix html lang="lt" | HIGH (correctness) | LOW (1 character) | **P1** |
| Expand sitemap.xml | HIGH (page discovery) | LOW (add 3 URLs) | **P1** |
| Title tag optimization | HIGH (SERP display + ranking) | LOW (4 strings) | **P1** |
| Meta descriptions | HIGH (CTR from SERPs) | LOW (4 strings) | **P1** |
| Canonical URLs | MEDIUM (duplicate prevention) | LOW (4 link tags) | **P1** |
| JSON-LD Organization + WebSite | MEDIUM (Knowledge Graph, rich results) | LOW (JSON block) | **P2** |
| JSON-LD WebPage per page | MEDIUM (page understanding) | LOW (JSON per page) | **P2** |
| Open Graph tags | MEDIUM (social sharing) | LOW (meta tags + image) | **P2** |
| Complete web manifest | LOW (completeness) | LOW (JSON edit) | **P2** |
| Semantic HTML audit | LOW (marginal ranking benefit) | LOW (audit + fixes) | **P2** |
| JSON-LD Service | MEDIUM (rich results potential) | MEDIUM (schema design) | **P3** |
| FAQ Schema + content | MEDIUM (featured snippets) | HIGH (requires content creation) | **P3** |
| Core Web Vitals optimization | LOW-MEDIUM (tiebreaker signal) | MEDIUM-HIGH (depends on issues) | **P3** |

**Priority key:**
- **P1**: Must have for launch -- without these, Google cannot properly crawl or display the site
- **P2**: Should have -- adds competitive edge in Lithuanian SERPs, implement in second pass
- **P3**: Nice to have -- defer until SEO results are measurable

## Competitor Feature Analysis

| Feature | autosutartis.lt | regitra.lt | esablonai.lt | AutoDokas (Current) | AutoDokas (Target) |
|---------|----------------|------------|--------------|---------------------|-------------------|
| Keyword in title | Yes - "Automobilio pirkimo-pardavimo sutartis internetu" | Yes - government authority | Partial | No - generic "Pagrindinis" | Yes - match competitor format |
| Meta description | Not visible | Yes | Yes | None | Yes, all 4 pages |
| JSON-LD Organization | Yes | Likely | Unknown | None | Yes |
| JSON-LD WebSite | Yes | Likely | Unknown | None | Yes |
| JSON-LD FAQPage | Yes (5 Q&As) | No | No | None | Defer (content scope) |
| Open Graph | Yes (og:image) | Yes | Unknown | None | Yes |
| Canonical URLs | Unknown | Yes | Unknown | None | Yes |
| Robots.txt | Allows crawling | Allows crawling | Allows crawling | **BLOCKS crawling** | Allow public pages |
| Sitemap completeness | Unknown | Yes | Unknown | Homepage only | All 4 public pages |
| html lang | lt | lt | lt | **en** (wrong) | lt |
| Free tool | Yes | PDF download | Template download | Yes | Yes |
| SSL/HTTPS | Yes | Yes | Yes | Yes | Yes |

**Key insight:** autosutartis.lt is the most directly comparable competitor and is ahead on structured data (Organization, WebSite, FAQPage schemas) and title optimization. However, autodokas.lt's critical blocker is far more basic -- the robots.txt literally prevents Google from crawling most pages. Fixing that alone would be a massive improvement before even considering competitive parity with autosutartis.lt.

## Sources

### Official Documentation (HIGH confidence)
- [Google: How to write meta descriptions](https://developers.google.com/search/docs/appearance/snippet)
- [Google: Influencing title links](https://developers.google.com/search/docs/appearance/title-link)
- [Google: Structured data general guidelines](https://developers.google.com/search/docs/appearance/structured-data/sd-policies)
- [Google: Introduction to structured data](https://developers.google.com/search/docs/appearance/structured-data/intro-structured-data)
- [Google: Organization schema markup](https://developers.google.com/search/docs/appearance/structured-data/organization)
- [Google: Consolidate duplicate URLs (canonical)](https://developers.google.com/search/docs/crawling-indexing/consolidate-duplicate-urls)
- [Google: Canonicalization](https://developers.google.com/search/docs/crawling-indexing/canonicalization)
- [Microsoft Learn: Control head content in Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/control-head-content)
- [Schema.org: Service type](https://schema.org/Service)
- [Schema.org: LocalBusiness type](https://schema.org/LocalBusiness)

### Verified Web Sources (MEDIUM confidence)
- [Blazor SEO Meta Data Component (ghostlyinc.com)](https://ghostlyinc.com/en-US/blazor-seo-meta-data-component/)
- [Canonicalization and SEO 2026 (Search Engine Land)](https://searchengineland.com/canonicalization-seo-448161)
- [Robots.txt and SEO 2026 (Search Engine Land)](https://searchengineland.com/robots-txt-seo-453779)
- [FAQ schema rise and fall (Search Engine Land)](https://searchengineland.com/faq-schema-rise-fall-seo-today-463993)
- [Core Web Vitals importance 2026 (White Label Coders)](https://whitelabelcoders.com/blog/how-important-are-core-web-vitals-for-seo-in-2026/)
- [Lithuanian local SEO guide (RankTracker)](https://www.ranktracker.com/blog/a-complete-guide-for-doing-local-seo-in-lithuania/)
- [.lt domain benefits (UltaHost)](https://ultahost.com/blog/why-choose-a-lt-domain/)
- [Title tags and SEO 2025 (Search Engine Land)](https://searchengineland.com/title-tags-seo-everything-you-need-to-know-2025-451233)

### Competitor Analysis (MEDIUM confidence)
- [autosutartis.lt](https://autosutartis.lt/) -- Direct competitor, analyzed via WebFetch for SEO elements
- [supirkejubaze.lt](https://supirkejubaze.lt/naudinga/pirkimo-pardavimo-sutartis/) -- Content competitor
- [regitra.lt](https://www.regitra.lt/) -- Government authority, dominant for contract keyword

---
*Feature research for: AutoDokas SEO Optimization*
*Researched: 2026-03-02*
