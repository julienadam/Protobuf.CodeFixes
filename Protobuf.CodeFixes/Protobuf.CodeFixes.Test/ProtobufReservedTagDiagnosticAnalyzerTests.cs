using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ProtobufReservedTagDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<ProtobufReservedTagDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = ProtobufReservedTagDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = ProtobufReservedTagDiagnosticAnalyzer.MessageFormat;

        [Fact]
        public void No_error_on_non_reserved_tag_for_property()
        {
            VerifyCSharpDiagnostic(GetOneTagPropertyClassSource(1));
        }

        [Fact]
        public void No_error_on_non_reserved_tag_for_field()
        {
            VerifyCSharpDiagnostic(GetOneTagFieldClassSource(1));
        }

        [Theory]
        [InlineData(19000)]
        [InlineData(19500)]
        [InlineData(19999)]
        public void Protomember_with_tag_on_property_in_reserved_range_show_as_error(int tag)
        {
           VerifyCSharpDiagnostic(GetOneTagPropertyClassSource(tag), GetExpectedErrorOnSingleTag(tag, "SomeProperty"));
        }

        [Theory]
        [InlineData(19000)]
        [InlineData(19500)]
        [InlineData(19999)]
        public void Protomember_with_tag_on_field_in_reserved_range_show_as_error(int tag)
        {
           VerifyCSharpDiagnostic(GetOneTagFieldClassSource(tag), GetExpectedErrorOnSingleTag(tag, "SomeField"));
        }
    }
}