# Contributing

## Quick start
1. Install Unity **6000.2.5f1** with URP and Input System package.
2. Clone repo. Open in Unity Hub.
3. Read `docs/GDD.md` and `docs/TDD.md`.
4. Pick a `good first issue` or open a proposal.

## Guidelines
- C# 10, one class per file, namespaces `Game.*`.
- No `FindObjectOfType`; prefer injection or simple Service Locator.
- Keep GC alloc/frame ~0 in steady state.
- Add unit tests for new guards/transitions.
