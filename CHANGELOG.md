# Changelog

All notable changes to Replicator will be documented in this file.

>The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Scripts for cleanup of ParticleSystems when they are recyled & to play when spawned if they are set to Play On Awake.

### Changed

- The preload configuration option is now numeric, allowing less than the capacity of an Object Pool to be pre-loaded
- Object Pool capacity no longer accepts negative numbers
- Tidied up some tooltips

## [0.1-alpha]

### Added

- Core object pooling system
- Interfaces for recieving callbacks on spawn / recycle
- Extension methods for mimicking Unity's Instantiate / Destroy calls

[Unreleased]: https://github.com/ettmetal/Replicator/compare/0.1-alpha...HEAD
[0.1-alpha]: https://github.com/ettmetal/Replicator/compare/a2010e58963b3f15a45031087ad54d5d1ac82bc0...0.1-alpha
