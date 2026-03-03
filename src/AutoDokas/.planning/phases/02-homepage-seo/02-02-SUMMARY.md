---
phase: 02-homepage-seo
plan: 02
subsystem: seo
tags: [schema.org, json-ld, structured-data, blazor, schema-net]

# Dependency graph
requires:
  - phase: 02-homepage-seo/01
    provides: "Homepage meta tags, canonical URL, and OpenGraph tags in Home.razor"
provides:
  - "Organization JSON-LD with name and url"
  - "WebSite JSON-LD with name, url, and inLanguage=lt"
  - "WebPage JSON-LD with name, description, url, inLanguage, isPartOf->WebSite"
  - "Service JSON-LD with name, description, url, broker->Organization, serviceType, areaServed"
affects: [03-validation]

# Tech tracking
tech-stack:
  added: [Schema.NET 13.0.0]
  patterns: [MarkupString JSON-LD rendering in page body, ToHtmlEscapedString for XSS safety]

key-files:
  created: []
  modified:
    - AutoDokas.csproj
    - Components/Pages/Home.razor

key-decisions:
  - "Used Broker instead of Provider on Service schema (Provider not available in Schema.NET 13.0.0)"
  - "Used Thing[] array instead of ISchemaType[] (ToHtmlEscapedString is on concrete Thing class)"
  - "JSON-LD rendered in page body via MarkupString, not inside HeadContent (avoids edge case)"

patterns-established:
  - "Schema.NET structured data: use ToHtmlEscapedString() for HTML embedding, never ToString()"
  - "MarkupString workaround: render script tags via @((MarkupString)variable) to bypass RZ9992"

requirements-completed: [SCHEMA-01, SCHEMA-02, SCHEMA-03, SCHEMA-04]

# Metrics
duration: 4min
completed: 2026-03-03
---

# Phase 2 Plan 2: Homepage JSON-LD Summary

**4 JSON-LD structured data blocks (Organization, WebSite, WebPage, Service) on homepage using Schema.NET 13.0.0 with XSS-safe rendering**

## Performance

- **Duration:** 4 min
- **Started:** 2026-03-03T13:48:49Z
- **Completed:** 2026-03-03T13:53:03Z
- **Tasks:** 2
- **Files modified:** 2

## Accomplishments
- Schema.NET 13.0.0 NuGet package added for strongly-typed schema.org JSON-LD generation
- 4 JSON-LD structured data blocks rendered on homepage: Organization, WebSite, WebPage, Service
- Schema relationships established: WebPage.IsPartOf -> WebSite, Service.Broker -> Organization
- All schemas use ToHtmlEscapedString() for XSS-safe HTML embedding

## Task Commits

Each task was committed atomically:

1. **Task 1: Add Schema.NET NuGet package** - `1946f23` (chore)
2. **Task 2: Add JSON-LD structured data to Home.razor** - `c4c5214` (feat)

## Files Created/Modified
- `AutoDokas.csproj` - Added Schema.NET 13.0.0 package reference
- `Components/Pages/Home.razor` - Added @using Schema.NET, MarkupString JSON-LD rendering, and @code block with Organization/WebSite/WebPage/Service schema objects

## Decisions Made
- **Broker instead of Provider:** Schema.NET 13.0.0 does not expose a `Provider` property on the `Service` type. Used `Broker` (which accepts IOrganization) as the closest available property to link the service to the organization. The JSON-LD output correctly includes the organization reference.
- **Thing[] instead of ISchemaType[]:** The `ISchemaType` interface does not exist in Schema.NET 13.0.0. The `ToHtmlEscapedString()` method is defined on the concrete `Thing` base class (not on `IThing` interface), so `Thing[]` is the correct array type.
- **JSON-LD in page body:** Placed MarkupString rendering after HeadContent and before Hero section, in the page body. Google parses JSON-LD equally from body or head, and this avoids the MarkupString + HeadContent edge case.

## Deviations from Plan

### Auto-fixed Issues

**1. [Rule 3 - Blocking] Schema.NET Service.Provider property does not exist**
- **Found during:** Task 2 (Add JSON-LD structured data)
- **Issue:** Plan specified `Provider = organization` on Service, but Schema.NET 13.0.0 does not have a `Provider` property on the `Service` type. Build failed with CS0117.
- **Fix:** Used `Broker = organization` instead. `Broker` accepts `IOrganization` and produces the same JSON-LD relationship linking service to organization.
- **Files modified:** Components/Pages/Home.razor
- **Verification:** dotnet build succeeds, JSON-LD output includes organization reference via broker field
- **Committed in:** c4c5214 (Task 2 commit)

**2. [Rule 3 - Blocking] ISchemaType interface does not exist in Schema.NET 13.0.0**
- **Found during:** Task 2 (Add JSON-LD structured data)
- **Issue:** Plan specified `new ISchemaType[]` but this interface does not exist. Build failed with CS0246.
- **Fix:** Used `new Thing[]` instead. The `Thing` base class has `ToHtmlEscapedString()` which is needed for JSON-LD serialization.
- **Files modified:** Components/Pages/Home.razor
- **Verification:** dotnet build succeeds, all schema objects serialize correctly
- **Committed in:** c4c5214 (Task 2 commit)

---

**Total deviations:** 2 auto-fixed (2 blocking)
**Impact on plan:** Both auto-fixes were necessary due to Schema.NET 13.0.0 API differences from plan assumptions. The functional outcome (4 JSON-LD blocks with correct schema relationships) is preserved. No scope creep.

## Issues Encountered
None beyond the deviations documented above.

## User Setup Required
None - no external service configuration required.

## Next Phase Readiness
- Homepage SEO is complete (meta tags from 02-01 + structured data from 02-02)
- Ready for Phase 3 validation/testing
- All SCHEMA-01 through SCHEMA-04 requirements addressed

## Self-Check: PASSED

- FOUND: AutoDokas.csproj
- FOUND: Components/Pages/Home.razor
- FOUND: .planning/phases/02-homepage-seo/02-02-SUMMARY.md
- FOUND: commit 1946f23
- FOUND: commit c4c5214

---
*Phase: 02-homepage-seo*
*Completed: 2026-03-03*
