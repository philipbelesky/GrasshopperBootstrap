## Grasshopper Bootstrap

[![Build Action](https://github.com/philipbelesky/GrasshopperBootstrap/workflows/Build%20Grasshopper%20Plugin/badge.svg)](https://github.com/philipbelesky/GrasshopperBootstrap/actions/workflows/dotnet-grasshopper.yml)
[![Maintainability](https://api.codeclimate.com/v1/badges/20e0e2fd92a1951ccb20/maintainability)](https://codeclimate.com/github/philipbelesky/GrasshopperBootstrap/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/20e0e2fd92a1951ccb20/test_coverage)](https://codeclimate.com/github/philipbelesky/GrasshopperBootstrap/test_coverage)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/6a5919298be744a2bc1018bd9e0ec1c2)](https://www.codacy.com/manual/philipbelesky/GrasshopperBootstrap?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=philipbelesky/GrasshopperBootstrap&amp;utm_campaign=Badge_Grade)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A slightly opinionated starter template for Grasshopper plugin development. Comes with a variety of templates and tools to help development. Most are unnecessary for most projects. However, most are easier to remove from a project than to add in.

Many of these features assume you are working on Github, using Visual Studio 2017/2019, and developing for Rhinoceros 6+ or higher.

### Features

- [X] Grasshopper/RhinoCommon bundled using their NuGet packages
  - *Ensures reference paths are maintained across local environments and can be controlled independently of the installed applications.*
- [X] Setup release builds to produce a single `gha` that includes all other dependent `dlls`
  - *ILRepack is used over ILMerge or Fody for better macOS compatibility. Refer to [this repo](https://github.com/ravibpatel/ILRepack.Lib.MSBuild.Task) for its documentation.*
  - *The sample code is setup to bundle Newtonsoft.Json as an example. Be sure to remove this and its reference in `ILRepack.targets` when starting your project!*
- [X] Continuous Integration using GitHub Actions
  - *See the steps in `.github/workflows/dotnet-grasshopper.xml`; currently the action will build the application and then upload the gha file from the Release build as an artefact. Build or linting failures will trigger a fail.*
- [X] Cross-compatibility between macOS and Windows versions of Visual Studio
  - *`GrasshopperBootstrap.csproj` defines split build paths so that each OS will start its own version of Rhinoceros*
- [X] Unit-testing framework (sort of)
  - *The `GrasshopperBootstrap.Tests` project is setup to run its tests within a headless version of Rhinoceros 7 so that all of Rhinocommon is accessible. Note that this project needs to be run as `x64` (under the `Test` menu in Visual Studio).*
  - *That same project has a Grasshopper definition that uses [PancakeContract](https://www.food4rhino.com/app/pancakecontract) to show how to run unit tests within Grasshopper.*
- [X] A shared class for all component files to inherit
  - *Allows for shared functionality and/or easy implementation of error reporting*
- [X] Linting using FxCop, StyleCop, an `.editorconfig` with a (relatively?) sane set of defaults
- [X] GitHub Pages setup for documentation
  - *See the `docs` folder and the README there*
- [X] Icons Illustrator template with some of the original Grasshopper icons provided as references
  - *See the `assets` folder*
- [X] A bundled template for an "About" component that reports current/latest version and links to documentation
  - *See `AboutComponent.cs` and `GrasshopperBootstrapInfo.cs`*
- [X] 3DM files setup to be stored using [Git Large File Storage](https://git-lfs.github.com)
- [X] Setup to optionally debug using the WIP versions of Rhinoceros
  - *Use the Debug (WIP) build configuration if this is desirable. You may want to check the file paths in the `csproj` file match your local paths.*
- [X] Code analysis with Codacy and Code Climate
  - *These are not setup per-se; I've just added badges to this README. To use these in your projects you will need to [add](https://github.com/marketplace/codacy) [each](https://github.com/marketplace/code-climate) app via the [GitHub Marketplace](https://github.com/marketplace/code-climate)*
- [X] Message added below components showing current version when running in DEBUG
  - *This is to easily identify the currently-loaded plugin version and as a safe guard against publishing development builds*

### Roadmap (PRs welcome!)

- [ ] Real Unit testing via NodeInCode?
- [ ] Performance tests using BenchmarkDotNet?
- [ ] Error reporting using Sentry
- [ ] Scripts for easily generating Yak releases
- [ ] Automate Yak releases via GitHub actions?

### Setup

This should all work out of the box. See features notes for optional setup steps.

Comments in the code with `GrasshopperBootstrapTODO:` highlight crucial changes to make when setting up your own plugin.

The default 'spiral' component from the McNeel sample plugin is included for reference and to demonstrate some features.

### Acknowledgements

Thanks to Andrew Heumann, Matthew Nelson, and Will Pearson for their recommendations.

Thanks to Tom Makin for the unit testing code [developed in this repo](https://github.com/tmakin/RhinoCommonUnitTesting).
