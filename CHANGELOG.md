# Changelog

All notable changes to Replicator will be documented in this file.

>The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

### Added

- New sequential pool type
- New burst pool type

### Changed

- Refactored VariantPool into an abstract class that can be used as a base for other multi-pool strategies
- RandomPool is now a subtype of VariantPool
- All pool types are now grouped together in the asset creation menu
- All pool types are now able to be created at runtime, through a Create factory method.

## [0.3.0-alpha]

### Added

- Pools can be individually configured to hide/show despawned objects in the hierarchy (default: hide)
- Particle system cleanup now allows configuration of clear behaviour
- The behaviour to use when growing a pool is now configurable

### Changed

- Updated the Multi-Object pool's editor for better usability
- Replicator is now targeting Unity 2019.3
- Refactoring clean-ups
- Pools' default behaviour has been changed to pre-load their full capacity. This can be disabled by editing their Pre Load option

### Fixed

- [#3] - Multi-pools where the underlying base variant is not available to spawn will now allow other variants to spawn

## [0.2.0-alpha]

### Added

- Scripts for cleanup of ParticleSystems when they are recyled & to play when spawned if they are set to Play On Awake.
- Added a 'Variant Pool' capable of spawning one of a number of predefined variants.

### Changed

- The preload configuration option is now numeric, allowing less than the capacity of an Object Pool to be pre-loaded
- Object Pool capacity no longer accepts negative numbers
- Tidied up some tooltips
- Spawn / Despawn no longer sets HideFlags, but only sets/unsets the appropriate bit, to play better with client code

## [0.1-alpha]

### Added

- Core object pooling system
- Interfaces for recieving callbacks on spawn / recycle
- Extension methods for mimicking Unity's Instantiate / Destroy calls

[0.3.0-alpha]: https://github.com/ettmetal/Replicator/compare/0.1-alpha...0.3.0-alpha
[0.2.0-alpha]: https://github.com/ettmetal/Replicator/compare/0.1-alpha...0.2.0-alpha
[0.1-alpha]: https://github.com/ettmetal/Replicator/compare/a2010e58963b3f15a45031087ad54d5d1ac82bc0...0.1-alpha

[#3]: https://github.com/ettmetal/Replicator/issues/3