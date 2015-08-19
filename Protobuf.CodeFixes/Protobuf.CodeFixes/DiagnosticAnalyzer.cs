using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtobufCodeFixesAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Protobuf-net code fixes : tag is zero";

        public const string Title = "Protobuf-net code fixes";
        public const string MessageFormat = "ProtoBuf tag set to 0 on {0}";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var attributes = context.Symbol.GetAttributes();
            foreach (var attributeData in attributes)
            {
                if (attributeData.AttributeClass.Name != "ProtoMemberAttribute")
                {
                    continue;
                }
                
                var arg = attributeData.ConstructorArguments[0];
                if (arg.Value is int && (int) arg.Value == 0)
                {
                    var attributeSyntax = (AttributeSyntax) attributeData.ApplicationSyntaxReference.GetSyntax();
                    var diagnostic = Diagnostic.Create(Rule, attributeSyntax.ArgumentList.Arguments[0].GetLocation(), context.Symbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
