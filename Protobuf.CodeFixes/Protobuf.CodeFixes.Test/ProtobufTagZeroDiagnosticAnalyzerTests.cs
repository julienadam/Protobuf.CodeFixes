using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ProtobufTagZeroDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<ProtobufTagZeroDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = ProtobufTagZeroDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = ProtobufTagZeroDiagnosticAnalyzer.MessageFormat;

        [Fact]
        public void No_error_for_tag_set_to_non_zero_on_property()
        {
            VerifyCSharpDiagnostic(GetOneTagPropertyClassSource(1));
        }

        [Fact]
        public void No_error_for_tag_set_to_non_zero_on_field()
        {
            VerifyCSharpDiagnostic(GetOneTagFieldClassSource(1));
        }

        [Fact]
        public void Tag_set_to_zero_on_property_triggers_error()
        {
            VerifyCSharpDiagnostic(GetOneTagPropertyClassSource(0), GetExpectedError("SomeProperty"));
        }

        [Fact]
        public void Tag_set_to_zero_on_field_triggers_error()
        {
            VerifyCSharpDiagnostic(GetOneTagFieldClassSource(0), GetExpectedError("SomeField"));
        }
    }
}