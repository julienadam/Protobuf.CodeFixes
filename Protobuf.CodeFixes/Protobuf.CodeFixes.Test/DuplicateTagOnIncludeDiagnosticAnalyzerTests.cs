using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class DuplicateTagOnIncludeDiagnosticAnalyzerTests : ProtobufBootstrappedDiagnosticAnalyzerTestsBase<DuplicateTagOnIncludeDiagnosticAnalyzer>
    {
        [Fact]
        public void Different_proto_include_tags_are_ok()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SubType1))]
        [ProtoInclude(2, typeof(SubType2))]
        class SampleType {}
        class SubType1 : SampleType { }
        class SubType2 : SampleType { }
    }";

            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void Duplicate_proto_include_tags_show_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SubType1))]
        [ProtoInclude(1, typeof(SubType2))]
        class SampleType {}
        class SubType1 : SampleType { }
        class SubType2 : SampleType { }
    }";

            var error1 = GetExpectedError(6, 23, 1, "SampleType");
            var error2 = GetExpectedError(7, 23, 1, "SampleType");
            VerifyCSharpDiagnostic(source, error1, error2);
        }
    }
}
