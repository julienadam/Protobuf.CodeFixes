# Protobuf.CodeFixes
A set of Roslyn diagnostics and fixes for Protobuf-net

This will check for common errors when writing classes that are serialized with the excellent Protobu-net library.

At this point it checks for:
* Tags set to 0
* Tags in the reserved range 19000-19999

Future releases should include more complex analysis such as:
* Duplicate tags
* Inconsistencies between DataContract / DataMember and ProtoContract / ProtoMember
* Missing KnowType / ProtoInclude
