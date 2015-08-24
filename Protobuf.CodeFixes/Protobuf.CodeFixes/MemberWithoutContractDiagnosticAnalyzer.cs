using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MemberWithoutContractDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Protobuf-net code fixes : member found on class without contract";
        public const string Title = "Protobuf-net code fixes : member found on class without contract";
        public const string MessageFormat = "DataMember or ProtoMember tag found in class {0} that has no ProtoContract or DataContract attribute";
        public const string Description = "A class with members that have ProtoMember or ProtoContract attributes must have either a ProtoContract or a DataContract attribute";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var type = (INamedTypeSymbol)context.Symbol;
            var membersWithAttributes = type
                .GetMembers()
                .Select(m => m.GetMemberAttributeData())
                .Where(a => a != null);

            if (membersWithAttributes.Any())
            {
                var classAttributes = context.Symbol.GetAttributes();
                var protoContractAttribute = classAttributes.FirstOrDefault(a => a.AttributeClass.Name == "ProtoContractAttribute");
                var dataContractAttribute = classAttributes.FirstOrDefault(a => a.AttributeClass.Name == "DataContractAttribute");

                if (protoContractAttribute == null && dataContractAttribute == null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, context.Symbol.Locations.First(), context.Symbol.Name));
                }
            }
        }
    }
}
