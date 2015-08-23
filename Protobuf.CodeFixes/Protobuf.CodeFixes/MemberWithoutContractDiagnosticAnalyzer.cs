using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MemberWithoutContractDiagnosticAnalyzer : ProtobufTagDiagnosticAnalyzerBase
    {
        public const string DiagnosticId = "Protobuf-net code fixes : member found on class without contract";
        public const string Title = "Protobuf-net code fixes : member found on class without contract";
        public const string MessageFormat = "{0}Member tag found on property {1} but class {2} has no {0}Contract attribute";
        public const string Description = "A class with members that have ProtoMember or ProtoContract attributes must have a ProtoContract and/or DataContract attribute";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override void AnalyzeSymbolCore(IEnumerable<ProtobufAttributeData> attributes, SymbolAnalysisContext context)
        {
            var attributesList = attributes as IList<ProtobufAttributeData> ?? attributes.ToList();
            if (attributesList.Any(a => a is ProtoMemberAttributeData))
            {
                var classAttributes = context.Symbol.ContainingType.GetAttributes();
                var contract = classAttributes.FirstOrDefault(a => a.AttributeClass.Name == "ProtoContractAttribute");
                if (contract == null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, context.Symbol.ContainingType.Locations.First(), "Proto", context.Symbol.Name, context.Symbol.ContainingType.Name));
                }
            }

            if (attributesList.Any(a => a is DataMemberAttributeData))
            {
                var classAttributes = context.Symbol.ContainingType.GetAttributes();
                var contract = classAttributes.FirstOrDefault(a => a.AttributeClass.Name == "DataContractAttribute");
                if (contract == null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, context.Symbol.ContainingType.Locations.First(), "Data", context.Symbol.Name, context.Symbol.ContainingType.Name));
                }
            }
        }
    }
}
