using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtoAttributesOnDerivedClassWithoutProtoInclude : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : protobuf attributes on derived class without protoinclude on the parent";
        public override string Title => "Protobuf-net code fixes : protobuf attributes on derived class without protoinclude on the parent";
        public override string MessageFormat => "ProtoBuf attributes found on {0} but base class {1} does not declare a ProtoInclude or KnownType for this class";
        public override string Description => "A class with ProtoMember/Contract or DataMember/Contract that derives from another class with proto attributes should have a ProtoInclude or KnownType attribute";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Warning;

        public override void Analyze(SymbolAnalysisContext context, List<IncludeAttributeData> includeTags, List<ProtobufAttributeData> memberTags, List<ContractAttributeData> contractAttributes)
        {
            var type = (INamedTypeSymbol) context.Symbol;
            if (type.BaseType == null)
            {
                return;
            }

            var includes = type.BaseType.GetIncludeAttributeData();
            if (includes.Any(i => i != null && i.IncludedType.Equals(type)))
            {
                return;
            }

            if (type.BaseType.GetMembersAttributeDate().Any() || type.BaseType.GetContractAttributeData().Any())
            {
                context.ReportDiagnostic(Diagnostic.Create(GetDescriptor(), type.Locations[0], type.Name, type.BaseType.Name));
            }
        }
    }
}