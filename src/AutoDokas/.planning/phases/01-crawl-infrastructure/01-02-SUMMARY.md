---
phase: 01-crawl-infrastructure
plan: 02
subsystem: infra
tags: [seo, noindex, blazor, headcontent, meta-robots]

# Dependency graph
requires:
  - phase: 01-crawl-infrastructure
    provides: "robots.txt blocking app routes from crawlers (Plan 01)"
provides:
  - "noindex meta tags on all 9 non-content pages as defense-in-depth"
  - "Only homepage remains indexable by search engines"
affects: [02-homepage-seo]

# Tech tracking
tech-stack:
  added: []
  patterns: ["HeadContent with noindex meta tag per-page for non-indexable routes"]

key-files:
  created: []
  modified:
    - Components/Pages/Contract/Contract.razor
    - Components/Pages/Contract/BuyerNotificationSent.razor
    - Components/Pages/Contract/ContractCompleted.razor
    - Components/Pages/Contract/ContractDownload.razor
    - Components/Pages/Legal/TermsAndConditions.razor
    - Components/Pages/Legal/PrivacyPolicy.razor
    - Components/Pages/Error.razor
    - Components/TestPages/EmailSendTestPage.razor
    - Components/TestPages/ContractPdfTemplatePage.razor

key-decisions:
  - "Inline HeadContent per page rather than shared component -- simpler, no coupling"
  - "noindex on test pages as defense-in-depth even though dev-only guard exists"

patterns-established:
  - "noindex pattern: <HeadContent><meta name='robots' content='noindex' /></HeadContent> after PageTitle or after last directive"

requirements-completed: [CRAWL-05]

# Metrics
duration: 1min
completed: 2026-03-03
---

# Phase 1 Plan 2: Noindex Meta Tags Summary

**Added noindex meta tags via HeadContent to all 9 non-content Blazor pages, ensuring only the homepage is indexable by search engines**

## Performance

- **Duration:** 1 min
- **Started:** 2026-03-03T11:52:14Z
- **Completed:** 2026-03-03T11:53:25Z
- **Tasks:** 2
- **Files modified:** 9

## Accomplishments
- All 4 contract/status pages (Contract, BuyerNotificationSent, ContractCompleted, ContractDownload) now serve noindex
- All 5 legal/error/test pages (TermsAndConditions, PrivacyPolicy, Error, EmailSendTestPage, ContractPdfTemplatePage) now serve noindex
- Homepage (Home.razor) confirmed NOT modified -- remains fully indexable
- Defense-in-depth layer: noindex complements robots.txt Disallow from Plan 01

## Task Commits

Each task was committed atomically:

1. **Task 1: Add noindex to contract and status pages (4 files)** - `230e8b3` (feat)
2. **Task 2: Add noindex to legal, error, and test pages (5 files)** - `6718363` (feat)

**Plan metadata:** pending (docs: complete plan)

## Files Created/Modified
- `Components/Pages/Contract/Contract.razor` - noindex for /contract, /contract/{guid}, /buyer/{guid}
- `Components/Pages/Contract/BuyerNotificationSent.razor` - noindex for /BuyerNotificationSent
- `Components/Pages/Contract/ContractCompleted.razor` - noindex for /ContractCompleted
- `Components/Pages/Contract/ContractDownload.razor` - noindex for /contract/download/{guid}
- `Components/Pages/Legal/TermsAndConditions.razor` - noindex for /terms-and-conditions
- `Components/Pages/Legal/PrivacyPolicy.razor` - noindex for /privacy-policy
- `Components/Pages/Error.razor` - noindex for /Error
- `Components/TestPages/EmailSendTestPage.razor` - noindex for /email-test
- `Components/TestPages/ContractPdfTemplatePage.razor` - noindex for /contract-test

## Decisions Made
- Used inline HeadContent per page rather than a shared component -- simpler, no coupling between pages
- Added noindex to test pages (EmailSendTestPage, ContractPdfTemplatePage) as defense-in-depth even though they have dev-only environment guards

## Deviations from Plan

None - plan executed exactly as written.

## Issues Encountered
None

## User Setup Required
None - no external service configuration required.

## Next Phase Readiness
- All non-content pages now have noindex meta tags
- Combined with robots.txt (Plan 01), crawl infrastructure for keeping non-content pages out of search indexes is complete
- Ready for Phase 2 (homepage SEO optimization)

## Self-Check: PASSED

- All 9 modified files exist on disk
- Commit 230e8b3 (Task 1) verified in git log
- Commit 6718363 (Task 2) verified in git log

---
*Phase: 01-crawl-infrastructure*
*Completed: 2026-03-03*
