# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).
This project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.5.1] - 2024-05-20

### Fixed

- Fixed an issue with additional lights on Forward+

## [3.5.0] - 2024-01-10

### Added

- Added support for LOD Crossfade

## [3.4.4] - 2023-11-20

### Fixed

- Fixed an issue with Rendering Light Layers.

## [3.4.3] - 2023-11-02

### Fixed

- Fixed an issue with Height Map sampling.

## [3.4.2] - 2023-11-01

### Changed

- Adjusted some usage of macros and renamed some functions.

## [3.4.1] - 2023-10-09

### Fixed

- Fixed instances of potential div by 0 errors.

## [3.4.0] - 2023-09-25

### Added

- Added a toggle for the Vertex Color Surface 2 input.

## [3.3.1] - 2023-09-21

### Fixed

- Fixed an issue with Spot and Point Light support

## [3.3.0] - 2023-09-21

### Added

- Added support for Vertex Painting

## [3.2.0] - 2023-09-20

### Added

- Added support for Light Cookies for Additional and Main Lights

### Changed

- Changed name of Metalness Map Exposure and Roughness Map Exposure to simplify editor. These are now simply "Exposure" options related to the parent.

## [3.1.0] - 2023-09-06

This version is compatible with Unity 2022.3.0f1+.

### Added

- When you add a roughness or metalness map, the editor hides the raw value and shows an exposure option instead. This option lets you adjust the exposure of the corresponding map.

## [3.0.2] - 2023-08-29

This version is compatible with Unity 2022.3.0f1+.

### Fixed

- Fixed missing additional light shadows.
- Fixed additional light support for Forward+.

## [3.0.1] - 2023-08-24

This version is compatible with Unity 2022.3.0f1+.

### Fixed

- Fixed support for ASTC normal map encoding

## [3.0.0] - 2023-08-23

This version is compatible with Unity 2022.3.0f1+.

### Added

- Added support for Forward+ rendering.

### Changed

- Changed Glossy Reflections system to leverage Unity's default system, which natively supports Forward, Forward+, and Deferred along with Blended Reflection Maps.
- Changed Additional lights sytem to support Forward+ rendering.

## [2.3.0] - 2023-07-21

### Added

- Added support for Height Maps.
- Added support for Subsurface Scattering.

## [2.2.0] - 2023-06-20

### Added

- Added support for baked shadows.

## [2.1.0] - 2023-06-06

### Added

- Added support for Transparent materials, including Alpha, Premultiply, Additive, and Multiply blend modes
- Added support for Alpha Clip materials
- Added support for sorting priority
- Added support for fog

## [2.0.0] - 2023-05-19

### Changed

- Switched to Package variant.

### Added

- Added support for Lightmaps.

## [1.0.3] - 2023-04-10

### Fixed

- Fixed an issue with the shadow caster pass.

## [1.0.2] - 2023-03-17

### Changed

- Changed the DepthNormalsOnly and DepthOnly passes to operate in a cleaner manner.

## [1.0.1] - 2023-03-16

### Fixed

- Fixed an issue causing the shader to fail to render correctly on VR headsets.
- Fixed an issue with missing materials in demo scene

## [1.0.0] - 2023-02-09

Initial release
