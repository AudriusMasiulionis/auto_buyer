---
phase: 01-crawl-infrastructure
plan: 01
subsystem: crawl-config
tags: [seo, robots, sitemap, manifest, lang]
dependency_graph:
  requires: []
  provides: [crawl-policy, sitemap-lastmod, web-manifest, lang-attribute]
  affects: [search-engine-discovery]
tech_stack:
  added: []
  patterns: [static-file-config]
key_files:
  created: []
  modified:
    - wwwroot/robots.txt
    - wwwroot/sitemap.xml
    - wwwroot/site.webmanifest
    - Components/App.razor
decisions:
  - "robots.txt uses allow-by-default with specific Disallow rules for app routes"
  - "Disallow /contract without trailing slash to block all contract sub-routes via prefix matching"
metrics:
  duration: "1 minute"
  completed: 2026-03-03
---

# Phase 1 Plan 1: Crawl Infrastructure Static Files Summary

Rewrite four site-level static files to unblock search engine crawling: robots.txt switched from Disallow-all to Allow-by-default with specific app route blocks, sitemap.xml got lastmod date, site.webmanifest populated with name/description, App.razor lang changed from en to lt.

## Tasks Completed

| # | Task | Commit | Key Files |
|---|------|--------|-----------|
| 1 | Rewrite robots.txt, sitemap.xml, site.webmanifest | 7265b81 | wwwroot/robots.txt, wwwroot/sitemap.xml, wwwroot/site.webmanifest |
| 2 | Fix HTML lang attribute to Lithuanian | fc72f8f | Components/App.razor |

## Deviations from Plan

None - plan executed exactly as written.

## Verification Results

- robots.txt contains `Allow: /` and specific `Disallow` rules (no more `Disallow: /`)
- sitemap.xml contains `<lastmod>2026-03-03</lastmod>`
- site.webmanifest contains `"name": "AutoDokas"` and description
- App.razor contains `lang="lt"`

All success criteria met.

## Self-Check: PASSED

- FOUND: commit 7265b81 (Task 1)
- FOUND: commit fc72f8f (Task 2)
- FOUND: wwwroot/robots.txt
- FOUND: wwwroot/sitemap.xml
- FOUND: wwwroot/site.webmanifest
- FOUND: Components/App.razor
- FOUND: 01-01-SUMMARY.md
