# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.16] - 2022-04-09
### Added
- Add json appSettings file to replace constants for list of VS exes.
- Add check for \*.csproj file type as possible file to open.
- Add Directory.Build.props file to easily manage version.

### Changed
- Moved code to check file ext. and to find VS executable path to helper.

### Deleted
- Removed launchSettings json file.


## [0.0.1] - 2022-03-31
### Added 
- Initial implementation and installer.
- Update README file contents.
- Retarget solution to net5.0 in order to comply with build image.
- Add dotnet action definition yml.
- Set build image to windows-2019 to get wix toolset.

