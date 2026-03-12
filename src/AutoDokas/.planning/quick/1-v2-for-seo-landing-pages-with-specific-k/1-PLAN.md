---
phase: quick
plan: 1
type: execute
wave: 1
depends_on: []
files_modified:
  - Components/Pages/Landing/SutartisInternetu.razor
  - Components/Pages/Landing/TransportoPriemonesSutartis.razor
  - Components/Pages/Landing/NaudotoAutomobilioSutartis.razor
  - Components/Pages/Landing/AutomobilioPardavimoSutartis.razor
  - Components/Pages/Landing/SutartiesForma.razor
  - Resources/Text.resx
  - Resources/Text.Designer.cs
  - wwwroot/sitemap.xml
autonomous: true
requirements: [SEO-LP-01, SEO-LP-02, SEO-LP-03, SEO-LP-04, SEO-LP-05]

must_haves:
  truths:
    - "Each landing page targets a unique long-tail keyword cluster and has unique content"
    - "All 5 landing pages have proper meta title, meta description, canonical URL, and OG tags"
    - "All 5 landing pages have JSON-LD structured data (WebPage, FAQPage)"
    - "All landing pages link to /contract with clear CTA"
    - "sitemap.xml includes all 5 new landing page URLs"
    - "Google can crawl all landing pages (not blocked by robots.txt)"
  artifacts:
    - path: "Components/Pages/Landing/SutartisInternetu.razor"
      provides: "Landing page for 'automobilio sutartis internetu'"
    - path: "Components/Pages/Landing/TransportoPriemonesSutartis.razor"
      provides: "Landing page for 'transporto priemones pirkimo pardavimo sutartis'"
    - path: "Components/Pages/Landing/NaudotoAutomobilioSutartis.razor"
      provides: "Landing page for 'naudoto automobilio pirkimo sutartis'"
    - path: "Components/Pages/Landing/AutomobilioPardavimoSutartis.razor"
      provides: "Landing page for 'automobilio pardavimo sutartis'"
    - path: "Components/Pages/Landing/SutartiesForma.razor"
      provides: "Landing page for 'automobilio sutarties forma / blankas'"
  key_links:
    - from: "Each landing page"
      to: "/contract"
      via: "CTA button link"
    - from: "wwwroot/sitemap.xml"
      to: "All landing page URLs"
      via: "sitemap entries"
---

## Objective

Create 5 SEO landing pages targeting specific Lithuanian long-tail keyword variations for vehicle sale contracts. Each page has unique content, FAQ section, full SEO meta tags, JSON-LD structured data, and CTA linking to the contract form. Update sitemap.xml to include all new pages.

**Purpose:** Expand the site's indexed footprint from 1 page (homepage) to 6 pages, capturing long-tail search traffic for different ways Lithuanians search for vehicle sale contracts.

**Output:** 5 new landing page Razor components, updated resource strings, updated sitemap.xml.

## Execution Context

See workflows: execute-plan.md, summary.md

## Context

**Reference files:**
- `Components/Pages/Home.razor` -- reference for SEO pattern: HeadContent, JSON-LD, page structure
- `Resources/Text.resx` -- existing resource strings pattern
- `wwwroot/sitemap.xml` -- current sitemap with only homepage
- `wwwroot/robots.txt` -- current robots.txt

**Interfaces -- established SEO pattern from Home.razor:**

Each landing page MUST follow this exact pattern from Home.razor:

1. PageTitle via resource: `<PageTitle>@Text.{Page}Title</PageTitle>`

2. HeadContent block with:
   - meta description (unique per page, max 160 chars, keyword-rich Lithuanian)
   - canonical URL (self-referential, e.g., `https://autodokas.lt/automobilio-sutartis-internetu`)
   - og:title, og:description, og:url, og:type="website", og:site_name="AutoDokas"

3. JSON-LD via `@((MarkupString)_jsonLd)` rendered in page body, containing:
   - WebPage schema (Name, Description, Url, InLanguage="lt", IsPartOf=webSite)
   - FAQPage schema with 3-4 unique Q&A pairs per page
   - Uses Schema.NET library: `@using Schema.NET`
   - Uses `OneOrMany<IThing>` for FAQPage.MainEntity
   - Uses StripHtml helper for clean JSON-LD text

4. Resource strings in Text.resx for ALL user-visible Lithuanian text (no hardcoded strings in .razor)

5. CSS classes reuse existing styles from Home.razor: hero, section-heading, accordion, btn-hero, etc.

## Tasks

### Task 1: Create 5 landing page Razor components with unique content and full SEO

**Type:** auto

**Files:**
- Components/Pages/Landing/SutartisInternetu.razor
- Components/Pages/Landing/TransportoPriemonesSutartis.razor
- Components/Pages/Landing/NaudotoAutomobilioSutartis.razor
- Components/Pages/Landing/AutomobilioPardavimoSutartis.razor
- Components/Pages/Landing/SutartiesForma.razor
- Resources/Text.resx
- Resources/Text.Designer.cs

**Action:**

Create 5 landing pages in `Components/Pages/Landing/` directory. Each page follows the Home.razor SEO pattern but with UNIQUE content (not duplicated from homepage).

**Page structure for each (simpler than homepage -- focused landing page):**
1. HeadContent with unique meta title, description, canonical, OG tags
2. JSON-LD: WebPage + FAQPage schemas (3-4 FAQ pairs each)
3. Hero section with keyword-rich h1 and subtitle
4. Content section (2-3 paragraphs of unique, keyword-optimized Lithuanian text)
5. FAQ accordion section (3-4 unique Q&A pairs, different from homepage FAQs)
6. CTA section linking to `/contract`

**Landing Page 1: `/automobilio-sutartis-internetu`**
- Target keyword: "automobilio sutartis internetu"
- Angle: Emphasize the ONLINE aspect -- no need to go anywhere, digital convenience
- h1: "Automobilio sutartis internetu -- sudarykite be spausdintuvo"
- Content focus: Why online is better than paper, how digital signing works, advantages of AutoDokas
- FAQ: Focus on online process questions (Is it legal online? How does digital signing work? Do I need to print? Can I do it from my phone?)

**Landing Page 2: `/transporto-priemones-pirkimo-pardavimo-sutartis`**
- Target keyword: "transporto priemones pirkimo pardavimo sutartis"
- Angle: FORMAL/official -- use the formal "transporto priemone" term throughout, emphasize legal compliance
- h1: "Transporto priemones pirkimo pardavimo sutartis"
- Content focus: Legal requirements for vehicle transfer in Lithuania, what makes a contract valid, formal language
- FAQ: Focus on legal/formal questions (What legal requirements? Is it valid for Regitra? What documents do I need? What about commercial vehicles?)

**Landing Page 3: `/naudoto-automobilio-pirkimo-sutartis`**
- Target keyword: "naudoto automobilio pirkimo sutartis"
- Angle: USED CAR specific -- buying a used car, what to check, common concerns
- h1: "Naudoto automobilio pirkimo sutartis"
- Content focus: Used car buying process, what defects to document, mileage verification, buyer protections
- FAQ: Focus on used car concerns (What defects to declare? How to verify mileage? What if car has hidden damage? What about technical inspection?)

**Landing Page 4: `/automobilio-pardavimo-sutartis`**
- Target keyword: "automobilio pardavimo sutartis"
- Angle: SELLER-focused -- selling your car, seller's responsibilities
- h1: "Automobilio pardavimo sutartis -- parduokite greitai ir saugiai"
- Content focus: Seller's process, what seller needs to prepare, how to hand over the car properly
- FAQ: Focus on seller questions (What seller must disclose? How to transfer insurance? What about SDK? When to deregister?)

**Landing Page 5: `/automobilio-sutarties-forma`**
- Target keyword: "automobilio pirkimo pardavimo sutarties forma" + "blankas"
- Angle: FORM/TEMPLATE seekers -- people looking for a blank form or template
- h1: "Automobilio pirkimo pardavimo sutarties forma"
- Content focus: Why use AutoDokas instead of a blank PDF form, advantages of guided form filling vs blank template, what fields the form includes
- FAQ: Focus on form questions (Where to get the form? What's the difference between blank and AutoDokas? Is AutoDokas form standard? What fields does it include?)

**Resource strings:** Add all text to `Resources/Text.resx` with naming convention:
- `Lp{N}Title` for page titles (N=1-5)
- `Lp{N}MetaDescription` for meta descriptions
- `Lp{N}HeroTitle` for h1 headings
- `Lp{N}HeroSubtitle` for hero subtitles
- `Lp{N}Content1`, `Lp{N}Content2`, `Lp{N}Content3` for content paragraphs
- `Lp{N}ContentHeading` for the content section h2
- `Lp{N}FaqHeading` for FAQ section heading
- `Lp{N}Faq{M}Question`, `Lp{N}Faq{M}Answer` for FAQ pairs (M=1-4)
- `Lp{N}CtaTitle`, `Lp{N}CtaSubtitle` for CTA section

After adding to Text.resx, rebuild the project with `dotnet build` so that `Text.Designer.cs` is auto-regenerated.

**Each page's @code block must:**
- Use `@using Schema.NET` and `@using AutoDokas.Resources`
- Build WebPage + FAQPage JSON-LD schemas
- Include the StripHtml helper method
- Reference the WebSite schema inline (same pattern as Home.razor)

**CSS:** Reuse existing styles (hero, section-heading, accordion, btn-hero, bottom-cta, etc.) -- these are already defined in app.css and work for landing pages. Use the same section classes and layout patterns as Home.razor. The landing pages should look visually consistent with the homepage but with different content.

**IMPORTANT content uniqueness rules:**
- Each page's h1 MUST contain the target keyword for that page
- Each page's meta description MUST be unique and contain the target keyword
- Each page's content paragraphs MUST be unique (not copy-pasted from homepage or other landing pages)
- Each page's FAQ pairs MUST be different from homepage FAQs and from each other
- Internal links: Each page links to /contract (CTA) and can cross-link to homepage
- Do NOT duplicate homepage FAQ questions on landing pages

**Lithuanian language quality:**
- All text must be grammatically correct Lithuanian
- Use proper Lithuanian diacritics (a with ogonek, e with ogonek, etc.)
- Use em dashes (--) not hyphens for text breaks
- Keep meta descriptions under 160 characters
- Keep page titles under 60 characters

**Verify:**
```
cd /Users/audriusmasiulionis/Development/auto_buyer/src/AutoDokas && dotnet build --no-restore 2>&1 | tail -5
```

**Done:**
- 5 landing page .razor files exist in Components/Pages/Landing/
- Each has unique @page route, PageTitle, HeadContent (meta+OG+canonical), JSON-LD (WebPage+FAQPage)
- Each has hero section with keyword h1, content section, FAQ accordion, CTA linking to /contract
- All Lithuanian text is in Text.resx resources (no hardcoded strings in .razor files)
- Project compiles without errors

---

### Task 2: Update sitemap.xml and verify robots.txt allows all landing pages

**Type:** auto

**Files:**
- wwwroot/sitemap.xml

**Action:**

**sitemap.xml:** Add all 5 new landing page URLs with today's date (2026-03-12) as lastmod and priority 0.8 (homepage is 1.0). The URLs are:
- `https://autodokas.lt/automobilio-sutartis-internetu`
- `https://autodokas.lt/transporto-priemones-pirkimo-pardavimo-sutartis`
- `https://autodokas.lt/naudoto-automobilio-pirkimo-sutartis`
- `https://autodokas.lt/automobilio-pardavimo-sutartis`
- `https://autodokas.lt/automobilio-sutarties-forma`

**robots.txt:** Verify the current robots.txt does NOT block these landing page URLs. The current `Disallow: /contract` rule uses prefix matching and blocks `/contract*` paths. The new landing pages use different URL prefixes so they are NOT blocked. No changes needed to robots.txt unless a conflict is found.

**Verify:**
```
cd /Users/audriusmasiulionis/Development/auto_buyer/src/AutoDokas && python3 -c "
import xml.etree.ElementTree as ET
tree = ET.parse('wwwroot/sitemap.xml')
ns = {'s': 'http://www.sitemaps.org/schemas/sitemap/0.9'}
urls = tree.findall('.//s:loc', ns)
print(f'{len(urls)} URLs in sitemap')
for u in urls:
    print(u.text)
assert len(urls) == 6, f'Expected 6 URLs, got {len(urls)}'
"
```

**Done:**
- sitemap.xml contains 6 URLs (1 homepage + 5 landing pages)
- sitemap.xml is valid XML
- robots.txt does not block any landing page URL
- All URLs use https://autodokas.lt/ prefix

## Verification

After both tasks:
1. `dotnet build` compiles without errors
2. sitemap.xml contains 6 valid URLs
3. Each landing page has: unique @page route, PageTitle, HeadContent, JSON-LD, hero, content, FAQ, CTA
4. No content duplication between landing pages or with homepage

## Success Criteria

- 5 new landing pages accessible at their respective URLs
- Each page targets a distinct keyword cluster with unique Lithuanian content
- All pages have full SEO: meta tags, OG tags, canonical URL, WebPage + FAQPage JSON-LD
- All pages link to /contract with CTA button
- sitemap.xml lists all 6 pages (homepage + 5 landing pages)
- Project builds without errors

## Output

After completion, create `.planning/quick/1-v2-for-seo-landing-pages-with-specific-k/1-SUMMARY.md`
