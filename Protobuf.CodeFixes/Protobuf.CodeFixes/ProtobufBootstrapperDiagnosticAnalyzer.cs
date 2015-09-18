using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtobufBootstrapperDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private readonly ProtobufDiagnosticAnalyzerBase[] analyzers;

        public ProtobufBootstrapperDiagnosticAnalyzer()
        {
            analyzers = new ProtobufDiagnosticAnalyzerBase[]
            {
                // TODO: load all analyzers
            };
        }

        public ProtobufBootstrapperDiagnosticAnalyzer(params ProtobufDiagnosticAnalyzerBase[] analyzers)
        {
            this.analyzers = analyzers;
            SupportedDiagnostics = analyzers.Select(a => a.GetDescriptor()).ToImmutableArray();
        }


        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(analysisContext =>
            {
                if (analysisContext.Compilation.ReferencedAssemblyNames.Any(a => a.Name == "protobuf-net"))
                {
                    analysisContext.RegisterSymbolAction(Action, SymbolKind.NamedType);
                }
            });
        }

        private void Action(SymbolAnalysisContext symbolAnalysisContext)
        {
            foreach (var analyzer in analyzers)
            {
                analyzer.Analyze(symbolAnalysisContext);
            }
        }
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
    }
}