# Protobuf.CodeFixes
A set of [Roslyn](https://github.com/dotnet/roslyn) diagnostics and fixes for [Protobuf-net](https://github.com/mgravell/protobuf-net)

[![Build status](https://ci.appveyor.com/api/projects/status/hklvnls8d7iusn3t?svg=true)](https://ci.appveyor.com/project/julienadam/protobuf-codefixes)

This will check for common errors when writing classes that are decorated with [Protobuf-net](https://github.com/mgravell/protobuf-net) attributes for [protocol buffers](https://developers.google.com/protocol-buffers/) serialization.

At this point it checks for:
* Tags set to 0
* Tags in the reserved range 19000-19999
* Duplicate tags
* Inconsistencies between `DataMember` and `ProtoMember` tags
* Tags set to negative values

Planned features:
* Duplicate tags for `KnownType` / `ProtoInclude`
* Missing `KnownType` / `ProtoInclude` for derived classes with `DataContract` / `ProtoContract` / `ProtoMember` / `DataMember` attributes
* Inconsistencies between `DataContract` / `ProtoContract`
* Re-number tags on existing `ProtoMember` / `DataMember` attributes
* Add tags to properties or fields on existing class marked with *Contract attributes
* Switching from Data* to Proto* tags

The vsix extension is available as an artifact on the [Appveyor build](https://ci.appveyor.com/project/julienadam/protobuf-codefixes/build/artifacts).
