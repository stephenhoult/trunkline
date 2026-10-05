# Trunkline

A hand-written learning project: a small link-shortener API, built by hand so you can learn
CI/CD with trunk-based development. It runs on GitHub Actions and is deployed to Azure.

- **[SPEC.md](SPEC.md)**: what it is, the environments, the infrastructure, the rules
- **[REVIEWING.md](REVIEWING.md)**: how the ticket → build → review loop works
- **[tickets/](tickets/)**: 20 tickets, about an hour each, in order

## Focus areas

How a change gets from a short-lived branch to production when the only long-lived branch is `main`:

- PR builds and protecting `main`
- Building once and promoting the same artifact through environments
- Feature flags, smoke tests and reverts
- Database changes that don't take production down
- Deploying without stored secrets, with least privilege
- Infrastructure as code that runs in the pipeline too

The app is deliberately trivial so that the pipeline gets your attention.

## Prerequisites

| Tool | Status |
|---|---|
| .NET 10 SDK | Installed (10.0.201) |
| Azure CLI | Installed (2.84.0) |
| Terraform | Installed (1.10.5). Fine as it is, upgrade if you like |
| GitHub CLI (`gh`) | **Not installed.** `brew install gh`. Used for checking several acceptance criteria |
| GitHub account | Personal. The repo will be **public** |
| Azure account | Personal Microsoft account, free account. Created in ticket 001 |

No Docker is needed on your machine. CI uses a SQL Server service container on GitHub's runners.

## Cost

Built to cost £0 on: GitHub Free (public repo), Azure App Service F1, the Azure SQL Database free
offer, and a few pence a month of blob storage for Terraform state from ticket 019. Ticket 001
sets a budget alert before anything is created. `terraform destroy` removes everything apart
from the state storage.

## Phases

| Tickets | Phase | What you get |
|---|---|---|
| 001–003 | Foundations | Accounts, a running API, Azure infrastructure from Terraform |
| 004–008 | Pipeline | PR checks, protected `main`, build once and promote to staging then prod, per-environment config |
| 009–011 | Trunk-based | Feature flags, smoke tests, a revert drill |
| 012–015 | Database | Azure SQL, EF Core, migrations in the pipeline, expand/contract |
| 016–020 | Hardening | OIDC, per-environment identities, managed identity to SQL, Terraform in CI, reusable workflows |

## Tickets

The ticket number links to a readable web version. The markdown source is in `tickets/`.

| # | Ticket | Source |
|---|---|---|
| [001](https://claude.ai/artifact/GGngWKqHm92o9uJ7RZsCrE) | Accounts, tooling and guard rails | `tickets/001-accounts-and-tooling.md` |
| [002](https://claude.ai/artifact/N3wrB3VGGPy4j51Ar6ASFB) | Walking skeleton | `tickets/002-walking-skeleton.md` |
| [003](https://claude.ai/artifact/K6B159T5uWuTQByCoehdYu) | Infrastructure with Terraform | `tickets/003-terraform-app-service.md` |
| [004](https://claude.ai/artifact/UV3guKMZCNQH7bH6eUUm4H) | PR workflow | `tickets/004-pr-workflow.md` |
| [005](https://claude.ai/artifact/Pz25W5tUSdycK8kSu4Mqn1) | Protect `main` | `tickets/005-protect-main.md` |
| [006](https://claude.ai/artifact/1d8H9hSHUHsDXQ1mYwEHEN) | Deploy to staging | `tickets/006-deploy-to-staging.md` |
| [007](https://claude.ai/artifact/HEtT6BxgzzFt6SvjQs1cBH) | Promote the same build to production | `tickets/007-promote-to-production.md` |
| [008](https://claude.ai/artifact/FT8vLNaG7rwCGq1xS5aU5J) | Per-environment configuration | `tickets/008-per-environment-config.md` |
| [009](https://claude.ai/artifact/QC56gcSvPo8NSr1Rfd8Zgv) | Feature flags | `tickets/009-feature-flags.md` |
| [010](https://claude.ai/artifact/DD5srdsKKxfZwZRSNwk6Tx) | Smoke tests after deploy | `tickets/010-smoke-tests.md` |
| [011](https://claude.ai/artifact/QAcf25Hjdjpz8CkD8B3LXi) | Revert drill | `tickets/011-revert-drill.md` |
| [012](https://claude.ai/artifact/VqvCGhnQrrZMAUYj21txFx) | Azure SQL infrastructure | `tickets/012-azure-sql-infrastructure.md` |
| [013](https://claude.ai/artifact/K7QY4aXn1CPgMmVUgN6KTq) | EF Core persistence | `tickets/013-ef-core-persistence.md` |
| [014](https://claude.ai/artifact/LSYGxv9AXpxYrYFde7Gfwa) | Migrations in the pipeline | `tickets/014-migrations-in-the-pipeline.md` |
| [015](https://claude.ai/artifact/1KDb9QH62ccxNKcVn6W4uA) | Expand/contract | `tickets/015-expand-contract.md` |
| [016](https://claude.ai/artifact/NHpUcTFTCnYXg3i6koHYUr) | OIDC instead of a stored secret | `tickets/016-oidc.md` |
| [017](https://claude.ai/artifact/HiGzT1zJiVbPzsVXidof18) | One identity per environment | `tickets/017-per-environment-identities.md` |
| [018](https://claude.ai/artifact/TnGnd48XRiiuk6pdBJ88Ze) | Managed identity to SQL | `tickets/018-managed-identity-to-sql.md` |
| [019](https://claude.ai/artifact/Ke9jWoicQHaNPC6Xk12FZM) | Terraform in CI | `tickets/019-terraform-in-ci.md` |
| [020](https://claude.ai/artifact/8Qir1rd8Bcz5eb3MT7Dpx7) | Reusable workflows | `tickets/020-reusable-workflows.md` |

The web versions are private to you unless you share them from the page's Share menu.

## Start

Read `SPEC.md`, then `REVIEWING.md`, then `tickets/001-accounts-and-tooling.md`.
