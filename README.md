# Protobuf.CodeFixes
A set of [Roslyn](https://github.com/dotnet/roslyn) diagnostics and fixes for [Protobuf-net](https://github.com/mgravell/protobuf-net)

[![Build status](https://ci.appveyor.com/api/projects/status/hklvnls8d7iusn3t?svg=true)](https://ci.appveyor.com/project/julienadam/protobuf-codefixes)

This will check for common errors when writing classes that are decorated with [Protobuf-net](https://github.com/mgravell/protobuf-net) attributes for [protocol buffers](https://developers.google.com/protocol-buffers/) serialization.

At this point it checks for:
* Tags set to 0
* Tags in the reserved range 19000-19999
* Duplicate tags

Future releases should include more complex analysis such as:
* Suport for `DataContract` / `DataMember`
* Inconsistencies between `DataContract` / `DataMember` and `ProtoContract` / `ProtoMember`
* Missing `KnownType` / `ProtoInclude`

This is a work in progress, the VS extension and nuget packages are not published anywhere yet. They will be when a minimum set of diagnotics and fixes are available. Anyone can use them though, just build the solution and register the extension in Visual Studio, or add the nupkg to a local repo.
