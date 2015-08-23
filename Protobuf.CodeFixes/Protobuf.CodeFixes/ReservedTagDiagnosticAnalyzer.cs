using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ReservedTagDiagnosticAnalyzer : ProtobufTagDiagnosticAnalyzerBase
    {
        public const string DiagnosticId = "Protobuf-net code fixes : tag is in the reserved range [19000-19999]";
        public const string Title = "Protobuf-net code fixes : tag is in the reserved range [19000-19999]";
        public const string MessageFormat = "ProtoBuf tag set to reserved value {0} on {1}";
        public const string Description = "The range from 19000 to 19999 is reserved for internal Protocol Buffers implementation and must not be used.";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override void AnalyzeSymbolCore(IEnumerable<ProtobufAttributeData> attributes, SymbolAnalysisContext context)
        {
            foreach(var attributeData in attributes.Where(a => a.Tag >= 19000 && a.Tag <= 19999))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, attributeData.GetLocation(), attributeData.Tag, context.Symbol.Name));
            }
        }
    }
}
