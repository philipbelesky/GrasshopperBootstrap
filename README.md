## Grasshopper Bootstrap

![Build Action](https://github.com/philipbelesky/GrasshopperBootstrap/workflows/Build%20Grasshopper%20Plugin/badge.svg)
[![Maintainability](https://api.codeclimate.com/v1/badges/20e0e2fd92a1951ccb20/maintainability)](https://codeclimate.com/github/philipbelesky/GrasshopperBootstrap/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/20e0e2fd92a1951ccb20/test_coverage)](https://codeclimate.com/github/philipbelesky/GrasshopperBootstrap/test_coverage)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/6a5919298be744a2bc1018bd9e0ec1c2)](https://www.codacy.com/manual/philipbelesky/GrasshopperBootstrap?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=philipbelesky/GrasshopperBootstrap&amp;utm_campaign=Badge_Grade)

A slightly opinionated starter template for Grasshopper plugin development with a variety of development aids. Most of these are unnecessary, but most are easier to remove than to add.

Most of these features assume you are working on Github, using Visual Studio 2017 or higher, and developer for Rhinoceros 6 or higher.

### Features

- [X] Grasshopper/RhinoCommon bundled with Nuget
  - *Ensures reference paths are maintained across local environments*
- [X] Setup release builds to do to produce a `gha` that including all dependencies
    - ILRepack is used over ILMerge or Fody for better macOS compatibility. Reference to [this repo](https://github.com/ravibpatel/ILRepack.Lib.MSBuild.Task) for its documentation.
    - The sample code is setup to bundle Newtonsoft.Json as an example. Be sure to remove this and its reference in `ILRepack.targets` when starting your project!
- [X] CD/CI using GitHub Actions
  - *See the steps in .github/workflows; currently the action will build the application and then upload the gha file from the Release build as an artefact. Build or linting failures will trigger a failed check.*
- [X] Cross-compatibility between MacOS and Windows versions of Visual Studio
  - *`GrasshopperBootstrap.csproj` defines split build paths so that each OS will start its own version of Rhinoceros*
- [X] Unit-test frameworks (sort of)
  - The GrasshopperBootstrap.Tests project is setup to run its tests within a headless versions of Rhinoceros 7 so that all of Rhinocommon is accessible. Note that it needs to be run as `x64` (under the `Test` menu in Visual Studio).
  - That same project has a Grasshopper definition that uses [PancakeContract](https://www.food4rhino.com/app/pancakecontract) to run unit tests within Grasshopper.
- [X] Separate out a `release` folder for non-debug builds
  - *Makes working with Yak and bundled assemblies easier*
- [X] A shared class for all component files to inherent
  - *Allows for shared functionality and/or easy implementation of error reporting*
- [X] Linting using FxCop, StyleCop, and a (relatively?) sane set of defaults
- [X] GitHub Pages setup for documentation
  - *See the `docs` folder and the README there*
- [X] Icons Illustrator template with original Grasshopper icons as references
  - *See the `assets` folder*
- [X] A bundled template for an "About" component that reports current/latest version and links to documentation
  - *See `AboutComponent.cs` and `GrasshopperBootstrapInfo.cs`
- [X] 3DM files setup to be stored using [Git Large File Storage](https://git-lfs.github.com)
- [X] Setup to optionally debug using the WIP versions of Rhinoceros
  - *Use the Debug (WIP) build configuration if this is desirable. You may want to check the file paths in the `csproj` file match your local paths.*
- [X] Code analysis with Codacy and Code Climate
  - *These are not setup per-se; I've just added badges to this README. To use on your projects you will need to [add](https://github.com/marketplace/codacy) [each](https://github.com/marketplace/code-climate) app via the [GitHub Marketplace](https://github.com/marketplace/code-climate)*

### Roadmap (PRs welcome!)

- [ ] Real Unit testing via NodeInCode?
- [ ] Performance tests using BenchmarkDotNet?
- [ ] Error reporting using Sentry
- [ ] Scripts for easily generating Yak releases
- [ ] Automate Yak releases via GitHub actions?

### Setup

This should all work out of the box. See features notes for optional setup steps.

Comments with `GrasshopperBootstrapTODO:` in the code highlight crucial aspects to change in setting up your own plugin.

The default 'spiral' component from the McNeel sample plugin is included for reference and to demonstrate some features.

### Acknowledgements

Thanks to Andrew Heumann, Matthew Nelson, and Will Pearson for their recommendations.
