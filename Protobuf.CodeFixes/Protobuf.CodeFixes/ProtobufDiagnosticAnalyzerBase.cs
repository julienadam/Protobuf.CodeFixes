using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    public abstract class ProtobufDiagnosticAnalyzerBase
    {
        public abstract string DiagnosticId { get; }
        public abstract string Title { get; }
        public abstract string MessageFormat { get; }
        public abstract string Description { get; }
        public abstract DiagnosticSeverity Severity { get; }
        
        public DiagnosticDescriptor GetDescriptor()
        {
            return new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, "Protobuf", Severity, true, Description);
        }

        public abstract void Analyze(SymbolAnalysisContext symbolAnalysisContext);
    }
}