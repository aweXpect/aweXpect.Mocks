# aweXpect.Mocks

[![Nuget](https://img.shields.io/nuget/v/aweXpect.Mocks)](https://www.nuget.org/packages/aweXpect.Mocks)
[![Build](https://github.com/aweXpect/aweXpect.Mocks/actions/workflows/build.yml/badge.svg)](https://github.com/aweXpect/aweXpect.Mocks/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=aweXpect_aweXpect.Mocks&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=aweXpect_aweXpect.Mocks)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=aweXpect_aweXpect.Mocks&metric=coverage)](https://sonarcloud.io/summary/overall?id=aweXpect_aweXpect.Mocks)
[![Mutation testing badge](https://img.shields.io/endpoint?style=flat&url=https%3A%2F%2Fbadge-api.stryker-mutator.io%2Fgithub.com%2FaweXpect%2FaweXpect.Mocks%2Fmain)](https://dashboard.stryker-mutator.io/reports/github.com/aweXpect/aweXpect.Mocks/main)

Template for extension projects for [aweXpect](https://github.com/aweXpect/aweXpect).

## Steps after creating a new project from this Template:

- Enable Stryker Mutator
	- Enable the repository in the [Stryker Mutator Dashboard](https://dashboard.stryker-mutator.io/repos/aweXpect)
	- Add the API Key as `STRYKER_DASHBOARD_API_KEY` repository secret
- Take over settings from Mocks project
	- General Settings
	- Protect the `main` branch
	- Create a "production" environment and add the `NUGET_API_KEY` secret
- Adapt the copyright and project information in Source/Directory.Build.props
- Adapt the README.md

