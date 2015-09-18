using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MemberWithoutContractDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : member found on class without contract";
        public override string Title => "Protobuf-net code fixes : member found on class without contract";
        public override string MessageFormat => "DataMember or ProtoMember tag found in class {0} that has no ProtoContract or DataContract attribute";
        public override string Description => "A class with members that have ProtoMember or ProtoContract attributes must have either a ProtoContract or a DataContract attribute";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Warning;

        public override void Analyze(SymbolAnalysisContext context, IEnumerable<IncludeAttributeData> includeTags, IEnumerable<ProtobufAttributeData> memberTags)
        {
            if (!memberTags.Any()) return;

            var classAttributes = context.Symbol.GetAttributes();
            var protoContractAttribute = classAttributes.FirstOrDefault(a => a.AttributeClass.Name == "ProtoContractAttribute");
            var dataContractAttribute = classAttributes.FirstOrDefault(a => a.AttributeClass.Name == "DataContractAttribute");

            if (protoContractAttribute == null && dataContractAttribute == null)
            {
                context.ReportDiagnostic(Diagnostic.Create(GetDescriptor(), context.Symbol.Locations.First(), context.Symbol.Name));
            }
        }
    }
}
