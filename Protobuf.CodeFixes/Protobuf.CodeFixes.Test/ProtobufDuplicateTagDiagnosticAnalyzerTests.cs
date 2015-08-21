using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ProtobufDuplicateTagDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<ProtobufDuplicateTagDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = ProtobufDuplicateTagDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = ProtobufDuplicateTagDiagnosticAnalyzer.MessageFormat;

        [Fact]
        public void No_error_on_different_tags_for_property()
        {
            VerifyCSharpDiagnostic(GetOneTagPropertyClassSource(1));
        }
        
        [Fact]
        public void Protomember_with_tag_on_property_in_reserved_range_show_as_error()
        {
            var error1 = GetExpectedError(8, 26, 1, "SomeProperty, SomeField");
            var error2 = GetExpectedError(11, 26, 1, "SomeProperty, SomeField");
            VerifyCSharpDiagnostic(GetTwoTagClassSource(1, 1),error1, error2);
        }
    }
}
