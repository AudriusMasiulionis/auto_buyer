# Phase 3: Validation and Submission - Research

**Researched:** 2026-03-03
**Domain:** SEO validation tooling (Lighthouse CLI, Google Rich Results Test, Schema Markup Validator)
**Confidence:** HIGH

## Summary

Phase 3 validates the SEO work from Phases 1 and 2 using two external tools: Google Rich Results Test (manual browser test for structured data) and Lighthouse CLI (automated SEO audit score). Both tools require the site to be deployed and publicly accessible at https://autodokas.lt/ before they can run. The user handles VALID-01 (sitemap submission) and VALID-02 (URL Inspection reindexing) manually outside this phase.

The Lighthouse CLI (`npx lighthouse`) runs as a one-off command requiring Node.js 22+ and Chrome/Chromium installed locally. The development machine has Node v24.13.1 and Chrome at the standard macOS path, so no installation is needed. Lighthouse 13.0.3 (latest as of 2026-02-11) has 10 weighted SEO audits -- all pass/fail, equally weighted except `is-crawlable` which carries 4x weight. Based on analysis of the existing codebase, the homepage should pass all automated SEO audits (title, meta description, canonical, viewport, crawlable status, valid robots.txt, alt attributes, crawlable anchors, link text, HTTP status). The target of 90+ is achievable -- failing 1 of the 10 standard audits still yields ~90.

The Rich Results Test only validates schema types Google supports for rich results display. Of the four schema types on the homepage (Organization, WebSite, WebPage, Service), only Organization is eligible for Google rich results. WebSite, WebPage, and Service are valid schema.org types but are NOT in Google's rich results gallery. The Rich Results Test will validate Organization but may not detect or validate the other three. For full structured data validation (including the Broker deviation on Service), the Schema Markup Validator at validator.schema.org should be used as a complementary tool.

**Primary recommendation:** Deploy first, then run Lighthouse CLI for automated SEO score, Rich Results Test for Organization schema, and Schema Markup Validator for complete structured data validation including the Service.Broker deviation.

<user_constraints>
## User Constraints (from CONTEXT.md)

### Locked Decisions
- Only plan VALID-03 (Rich Results Test) and VALID-04 (Lighthouse SEO audit)
- VALID-01 (sitemap submission to Search Console) and VALID-02 (URL Inspection reindexing) are skipped -- user handles these manually
- This keeps the phase fully automatable
- If Lighthouse scores below 90 or Rich Results Test finds errors, fix the issues within this phase
- No separate gap-closure phase -- this is the last phase, fixes happen inline
- Plans should include fix tasks alongside validation tasks
- Site needs deployment before external validators can run
- Phase plan should include a deployment checkpoint or step before running Rich Results Test or Lighthouse against production URL
- Changes from Phase 1 and 2 are committed but not yet live on autodokas.lt
- One-off CLI run: `npx lighthouse https://autodokas.lt --output=json`
- No package.json or permanent Node tooling added to the .NET project
- Results saved to .planning/ for evidence

### Claude's Discretion
- Exact Lighthouse CLI flags and output format
- How to validate Rich Results (API vs manual browser test)
- Order of validation steps
- What constitutes a "fix" vs "needs separate investigation"

### Deferred Ideas (OUT OF SCOPE)
None -- discussion stayed within phase scope.
</user_constraints>

<phase_requirements>
## Phase Requirements

| ID | Description | Research Support |
|----|-------------|-----------------|
| VALID-01 | Updated sitemap submitted to Google Search Console | OUT OF SCOPE -- user handles manually. No plan tasks needed. |
| VALID-02 | Homepage reindexing requested via Search Console URL Inspection | OUT OF SCOPE -- user handles manually. No plan tasks needed. |
| VALID-03 | Structured data validated with Google Rich Results Test (no errors) | Rich Results Test at https://search.google.com/test/rich-results validates only Google-supported rich result types. Organization is the only eligible type among the 4 schemas. Use Schema Markup Validator (validator.schema.org) as complementary tool for full validation of all 4 types including Service.Broker deviation. Both are browser-based manual tests against the live production URL. |
| VALID-04 | Lighthouse SEO audit score 90+ (fix any issues found) | `npx lighthouse https://autodokas.lt --only-categories=seo --output=json --output-path=.planning/phases/03-validation-and-submission/lighthouse-report.json --chrome-flags="--headless=new"` -- Lighthouse 13.0.3 has 10 weighted SEO audits. Based on codebase analysis, all should pass. Fix-forward strategy if score < 90. |
</phase_requirements>

## Standard Stack

### Core
| Tool | Version | Purpose | Why Standard |
|------|---------|---------|--------------|
| Lighthouse CLI | 13.0.3 | Automated SEO audit scoring | Google's official web audit tool; npx means zero install footprint |
| Google Rich Results Test | N/A (web tool) | Validate structured data for rich results eligibility | Google's own validation -- the authoritative source for rich result eligibility |
| Schema Markup Validator | N/A (web tool) | Validate all schema.org types including non-rich-result types | Official schema.org tool; validates types RRT ignores (WebSite, WebPage, Service) |

### Supporting
| Tool | Version | Purpose | When to Use |
|------|---------|---------|-------------|
| Node.js | 22+ (machine has 24.13.1) | Runtime for Lighthouse CLI | Required by Lighthouse; already installed |
| Chrome | Latest (macOS) | Browser engine Lighthouse drives | Required by Lighthouse; installed at /Applications/Google Chrome.app |

### Alternatives Considered
| Instead of | Could Use | Tradeoff |
|------------|-----------|----------|
| Rich Results Test (browser) | Rich Results Test API | No public API exists; browser test is the only option |
| Schema Markup Validator | JSON-LD Playground | JSON-LD Playground validates syntax only, not schema.org compliance |
| npx lighthouse | Chrome DevTools Lighthouse tab | CLI produces machine-readable JSON for evidence archival |

**Installation:**
```bash
# No installation needed -- npx downloads lighthouse on demand
# Verify prerequisites:
node --version  # Must be >= 22
ls "/Applications/Google Chrome.app"  # Chrome must be installed
```

## Architecture Patterns

### Validation Workflow Order
```
1. DEPLOY     -- Push Phase 1+2 changes to production (user action)
2. VERIFY     -- Confirm deployment live (curl/browser check)
3. LIGHTHOUSE -- Run CLI, save JSON report, extract SEO score
4. RRT        -- Manual browser test at Rich Results Test URL
5. SMV        -- Manual browser test at Schema Markup Validator
6. FIX        -- If any failures, fix inline and re-validate
7. EVIDENCE   -- Save reports/screenshots to .planning/
```

### Pattern 1: Lighthouse One-Off CLI Run
**What:** Run Lighthouse as a single npx command with no project-level Node tooling
**When to use:** One-time validation of a deployed site
**Example:**
```bash
# SEO-only audit with JSON output for evidence
npx lighthouse https://autodokas.lt \
  --only-categories=seo \
  --output=json \
  --output-path=.planning/phases/03-validation-and-submission/lighthouse-report.json \
  --chrome-flags="--headless=new"

# Also generate HTML report for human review
npx lighthouse https://autodokas.lt \
  --only-categories=seo \
  --output=html \
  --output-path=.planning/phases/03-validation-and-submission/lighthouse-report.html \
  --chrome-flags="--headless=new"
```

### Pattern 2: Extract SEO Score from JSON Report
**What:** Parse the JSON output to programmatically check the SEO score
**Example:**
```bash
# Extract SEO score from Lighthouse JSON report
node -e "
  const r = require('./.planning/phases/03-validation-and-submission/lighthouse-report.json');
  const score = r.categories.seo.score * 100;
  console.log('SEO Score:', score);
  const audits = r.categories.seo.auditRefs
    .filter(a => a.weight > 0)
    .map(a => ({
      id: a.id,
      score: r.audits[a.id].score,
      title: r.audits[a.id].title
    }));
  audits.forEach(a => console.log(a.score === 1 ? 'PASS' : 'FAIL', a.title));
"
```

### Pattern 3: Deployment Verification
**What:** Confirm the production site is serving updated content before running validators
**When to use:** After deployment, before any external validation
**Example:**
```bash
# Verify robots.txt is served correctly
curl -s https://autodokas.lt/robots.txt | head -5

# Verify sitemap.xml is served
curl -s https://autodokas.lt/sitemap.xml | head -5

# Verify homepage returns 200 and contains expected meta tags
curl -s -o /dev/null -w "%{http_code}" https://autodokas.lt/

# Verify JSON-LD is in the page source
curl -s https://autodokas.lt/ | grep -c "application/ld+json"
```

### Anti-Patterns to Avoid
- **Running validators before deployment:** Lighthouse and Rich Results Test need the live production URL. Running against localhost or a non-deployed URL will not validate the real deployment.
- **Treating Rich Results Test as comprehensive schema validator:** It only checks Google-supported rich result types. Organization passes, but WebSite/WebPage/Service will not appear. This is expected behavior, not an error.
- **Installing lighthouse globally or adding package.json:** The user explicitly wants no permanent Node tooling. Use `npx` which downloads on demand and does not persist.
- **Chasing 100 SEO score:** 90+ is the target. Some audits (like hreflang) may not apply to a single-language Lithuanian site. N/A audits still pass.

## Don't Hand-Roll

| Problem | Don't Build | Use Instead | Why |
|---------|-------------|-------------|-----|
| SEO audit scoring | Custom crawl-and-check script | `npx lighthouse --only-categories=seo` | Lighthouse checks 10+ factors with proper weighting; keeping up with Google's changes is impractical |
| Structured data validation | Manual JSON-LD parsing | Rich Results Test + Schema Markup Validator | These tools validate against Google's actual parsing logic and the full schema.org vocabulary |
| JSON report parsing | Complex report analyzer | `node -e` one-liner to extract score | The JSON structure is stable and well-documented; a one-liner suffices |

**Key insight:** This phase is about running existing validation tools and interpreting their output, not building custom validation infrastructure.

## Common Pitfalls

### Pitfall 1: Forgotten Deployment
**What goes wrong:** Running Lighthouse or Rich Results Test against https://autodokas.lt/ when Phase 1+2 changes are only in git, not deployed
**Why it happens:** All code changes are committed but deployment is a separate step the user manages
**How to avoid:** Include an explicit deployment checkpoint as the first step. Verify with curl that the production URL returns updated content (e.g., check for the new meta description in HTML source)
**Warning signs:** Lighthouse shows missing meta description or old title; Rich Results Test shows no structured data

### Pitfall 2: Rich Results Test Shows "No Rich Results Detected"
**What goes wrong:** The test reports no eligible items, causing alarm
**Why it happens:** Service, WebSite, and WebPage are not Google rich-result-eligible types. Only Organization is eligible. If Organization validates correctly, this is a SUCCESS even if the tool says "no rich results" for the other schemas
**How to avoid:** Use Schema Markup Validator (validator.schema.org) in parallel to confirm all 4 schema types are syntactically correct. Document that 3 of 4 types are intentionally non-rich-result but still valid SEO signals
**Warning signs:** Panic about "no rich results" when the Organization schema does validate

### Pitfall 3: Service.Broker Not Recognized
**What goes wrong:** Schema Markup Validator flags the Broker property on Service as unusual
**Why it happens:** Phase 2 used `Broker` instead of `Provider` because Schema.NET 13.0.0 doesn't expose Provider on the Service class. Broker is a valid schema.org property but less commonly used for this purpose
**How to avoid:** Explicitly check that the Broker property appears in the JSON-LD output and that the validator accepts it without errors. If the validator flags a warning (not error), document it as acceptable
**Warning signs:** Validator warnings about Broker on Service type

### Pitfall 4: Lighthouse Chrome Launch Failure
**What goes wrong:** `npx lighthouse` fails to find or launch Chrome
**Why it happens:** Lighthouse needs Chrome/Chromium installed locally. On macOS, the standard path is `/Applications/Google Chrome.app` which is present on this machine
**How to avoid:** If Chrome is not found, use `--chrome-flags="--headless=new"` and optionally specify the Chrome path with `CHROME_PATH` environment variable
**Warning signs:** Error message about "Chrome not found" or "Unable to connect"

### Pitfall 5: Stale npx Cache
**What goes wrong:** `npx lighthouse` runs an older cached version instead of latest
**Why it happens:** npx caches packages; may use an older version if previously run
**How to avoid:** Use `npx lighthouse@latest` to ensure the latest version
**Warning signs:** Version mismatch in output header

## Code Examples

### Complete Lighthouse Validation Command
```bash
# Source: Lighthouse npm documentation + GitHub docs
# Run SEO-only audit, headless, save JSON evidence
npx lighthouse@latest https://autodokas.lt \
  --only-categories=seo \
  --output=json,html \
  --output-path=.planning/phases/03-validation-and-submission/lighthouse-report \
  --chrome-flags="--headless=new" \
  --quiet

# This produces two files:
# - lighthouse-report.report.json  (machine-readable evidence)
# - lighthouse-report.report.html  (human-readable report)
```

### Parse and Display SEO Audit Results
```bash
# Source: Lighthouse JSON output structure (GitHub docs)
node -e "
  const r = require('./.planning/phases/03-validation-and-submission/lighthouse-report.report.json');
  const seo = r.categories.seo;
  console.log('=== Lighthouse SEO Score: ' + (seo.score * 100) + ' ===');
  console.log('');
  seo.auditRefs.filter(a => a.weight > 0).forEach(ref => {
    const audit = r.audits[ref.id];
    const status = audit.score === 1 ? 'PASS' : audit.score === null ? 'N/A ' : 'FAIL';
    console.log(status + '  ' + audit.title + ' (weight: ' + ref.weight + ')');
  });
"
```

### Pre-Validation Deployment Check
```bash
# Verify production site has Phase 1+2 changes deployed
echo "=== Deployment Verification ==="

# Check HTTP status
STATUS=$(curl -s -o /dev/null -w "%{http_code}" https://autodokas.lt/)
echo "Homepage HTTP status: $STATUS"

# Check for meta description (Phase 2 addition)
curl -s https://autodokas.lt/ | grep -q 'name="description"' && echo "PASS: meta description present" || echo "FAIL: meta description missing"

# Check for JSON-LD structured data (Phase 2 addition)
COUNT=$(curl -s https://autodokas.lt/ | grep -c 'application/ld+json')
echo "JSON-LD script blocks found: $COUNT (expected: 4)"

# Check robots.txt (Phase 1)
curl -s https://autodokas.lt/robots.txt | grep -q "Disallow: /contract" && echo "PASS: robots.txt updated" || echo "FAIL: robots.txt not updated"

# Check sitemap.xml (Phase 1)
curl -s https://autodokas.lt/sitemap.xml | grep -q "lastmod" && echo "PASS: sitemap.xml has lastmod" || echo "FAIL: sitemap.xml missing lastmod"
```

## State of the Art

| Old Approach | Current Approach | When Changed | Impact |
|--------------|------------------|--------------|--------|
| Google Structured Data Testing Tool | Schema Markup Validator (validator.schema.org) + Rich Results Test | August 2021 | SDTT deprecated; validation split into two tools with different scopes |
| Lighthouse viewport/font-size in SEO | Moved to Best Practices category | Lighthouse ~12.0 (2024) | Fewer audits in SEO category but is-crawlable now weighted 4x |
| `--headless` Chrome flag | `--headless=new` flag | Chrome 112+ (2023) | New headless mode has full browser capability; old mode deprecated |
| Lighthouse 12.x | Lighthouse 13.0.3 | 2025-2026 | Requires Node 22+; refined SEO audit weights |

**Deprecated/outdated:**
- Google Structured Data Testing Tool: Replaced by Schema Markup Validator + Rich Results Test in 2021
- `--headless` without `=new`: Old headless mode is deprecated; use `--headless=new`
- Lighthouse `--chrome-flags="--headless"`: Should be `--chrome-flags="--headless=new"`

## Lighthouse SEO Audit Breakdown

The 10 weighted SEO audits in Lighthouse 13.x and their expected status for autodokas.lt:

| Audit ID | What It Checks | Weight | Expected Result | Evidence |
|----------|---------------|--------|-----------------|----------|
| `is-crawlable` | No noindex on homepage, not blocked by robots.txt | ~4x | PASS | Homepage has no noindex; robots.txt allows `/` |
| `document-title` | `<title>` element exists and is not empty | 1 | PASS | `<PageTitle>@Text.HomeTitle</PageTitle>` renders keyword-rich title |
| `meta-description` | `<meta name="description">` present | 1 | PASS | Explicit meta description in HeadContent (128 chars) |
| `http-status-code` | Page returns 2xx | 1 | PASS | Blazor Server serves homepage at `/` |
| `link-text` | Links have descriptive text (not "click here") | 1 | PASS | Links use resource strings with descriptive text |
| `crawlable-anchors` | Links use `<a href>` not JS-only navigation | 1 | PASS | Standard `<a href="/contract">` tags used |
| `robots-txt` | robots.txt is syntactically valid | 1 | PASS | robots.txt follows standard format (verified in Phase 1) |
| `image-alt` | Images have alt attributes | 1 | PASS | Hero image has `alt=""` (decorative, acceptable); check other images |
| `hreflang` | Valid hreflang if present | 1 | N/A (PASS) | No hreflang tags (single-language site); audit passes when absent |
| `canonical` | Valid rel=canonical | 1 | PASS | Self-referential canonical `https://autodokas.lt/` in HeadContent |

**Expected score: 100** (all 10 audits pass). Even if 1 audit fails, score would be ~90 (meets target). The `is-crawlable` audit alone accounts for ~31% of the score due to its 4x weight -- if this one fails, the category fails.

## Open Questions

1. **Deployment method and timing**
   - What we know: Changes are committed to git but not deployed. Dockerfile exists for containerized deployment.
   - What's unclear: How the user deploys to production (Docker push? CI/CD? Manual?). Timeline for deployment.
   - Recommendation: Plan should include a "deployment checkpoint" step where the user confirms deployment is complete. The planner should NOT plan the deployment itself -- just the verification and validation after it.

2. **Image alt text on non-hero images**
   - What we know: Hero image has `alt=""` which is valid for decorative images. Lighthouse checks all images.
   - What's unclear: Whether other pages or components have images without alt text that might affect the audit if Lighthouse crawls beyond the homepage.
   - Recommendation: The Lighthouse command targets only the homepage URL, so only homepage images matter. The hero car image with `alt=""` is valid for decorative images per WCAG. This should pass.

3. **`--output` flag behavior with multiple formats**
   - What we know: `--output=json,html` should produce both formats.
   - What's unclear: Exact output filenames when using multiple formats with `--output-path`.
   - Recommendation: Test with the actual command; Lighthouse appends format extensions. If problematic, run the command twice with different `--output` values.

## Sources

### Primary (HIGH confidence)
- Lighthouse GitHub default-config.js -- complete SEO audit list with weights (is-crawlable: 93/23, all others: 1)
- Google Search Gallery (developers.google.com/search/docs/appearance/structured-data/search-gallery) -- Organization is rich-result-eligible; Service, WebSite, WebPage are NOT
- Phase 2 VERIFICATION.md -- all 10 truths verified, all 8 requirements satisfied, 4 human verification items documented

### Secondary (MEDIUM confidence)
- DebugBear Lighthouse SEO Score guide (debugbear.com/blog/lighthouse-seo-score) -- 14 audits historically, reduced in recent versions; viewport/font-size moved to Best Practices
- Lighthouse npm page -- version 13.0.3, requires Node 22+
- Google Rich Results Test (search.google.com/test/rich-results) -- manual browser-based tool, no public API

### Tertiary (LOW confidence)
- Schema Markup Validator handling of Broker property on Service -- no authoritative source confirms or denies validator behavior for this specific case; needs live testing

## Metadata

**Confidence breakdown:**
- Standard stack: HIGH -- Lighthouse CLI is well-documented, version confirmed, requirements verified on this machine
- Architecture: HIGH -- validation workflow is straightforward (deploy, verify, run tools, fix, evidence)
- Pitfalls: HIGH -- Rich Results Test scope limitation is documented by Google; deployment prerequisite is stated in CONTEXT.md; Broker deviation is documented in Phase 2 VERIFICATION.md

**Research date:** 2026-03-03
**Valid until:** 2026-04-03 (30 days -- stable tooling, no fast-moving dependencies)
