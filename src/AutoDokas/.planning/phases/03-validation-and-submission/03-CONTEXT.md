# Phase 3: Validation and Submission - Context

**Gathered:** 2026-03-03
**Status:** Ready for planning

<domain>
## Phase Boundary

Verify all SEO changes from Phases 1 and 2 are working correctly, fix any issues found, and achieve a Lighthouse SEO score of 90+. Only VALID-03 (Rich Results Test) and VALID-04 (Lighthouse audit) are in scope — VALID-01 and VALID-02 (Search Console submission/reindexing) are handled manually by the user outside this phase.

</domain>

<decisions>
## Implementation Decisions

### Scope — Automated Validations Only
- Only plan VALID-03 (Rich Results Test) and VALID-04 (Lighthouse SEO audit)
- VALID-01 (sitemap submission to Search Console) and VALID-02 (URL Inspection reindexing) are skipped — user handles these manually
- This keeps the phase fully automatable

### Fix-Forward Strategy
- If Lighthouse scores below 90 or Rich Results Test finds errors, fix the issues within this phase
- No separate gap-closure phase — this is the last phase, fixes happen inline
- Plans should include fix tasks alongside validation tasks

### Deployment Prerequisite
- Site needs deployment before external validators can run
- Phase plan should include a deployment checkpoint or step before running Rich Results Test or Lighthouse against production URL
- Changes from Phase 1 and 2 are committed but not yet live on autodokas.lt

### Lighthouse Execution
- One-off CLI run: `npx lighthouse https://autodokas.lt --output=json`
- No package.json or permanent Node tooling added to the .NET project
- Results saved to .planning/ for evidence

### Claude's Discretion
- Exact Lighthouse CLI flags and output format
- How to validate Rich Results (API vs manual browser test)
- Order of validation steps
- What constitutes a "fix" vs "needs separate investigation"

</decisions>

<code_context>
## Existing Code Insights

### Reusable Assets
- No existing validation tooling — this is a .NET Blazor project with no Node/npm setup
- Phase 2 VERIFICATION.md already identified 4 human verification items that overlap with this phase's scope

### Established Patterns
- Static files (robots.txt, sitemap.xml) served directly from wwwroot/
- HeadContent → HeadOutlet rendering pipeline confirmed working in Phase 1 and 2
- Schema.NET 13.0.0 JSON-LD rendering via MarkupString in page body

### Integration Points
- Production URL: https://autodokas.lt/
- robots.txt: https://autodokas.lt/robots.txt
- sitemap.xml: https://autodokas.lt/sitemap.xml
- All SEO artifacts are server-rendered (SSR) — external validators will see them in the initial HTML response

</code_context>

<specifics>
## Specific Ideas

- Service.Broker deviation from Phase 2 needs explicit confirmation via Rich Results Test (Schema.NET 13.0.0 doesn't have Provider property — Broker was used instead)
- Lighthouse should focus on SEO category score specifically (not Performance, Accessibility, etc.)

</specifics>

<deferred>
## Deferred Ideas

None — discussion stayed within phase scope.

</deferred>

---

*Phase: 03-validation-and-submission*
*Context gathered: 2026-03-03*
