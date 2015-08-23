using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class DuplicateTagDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<DuplicateTagDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = DuplicateTagDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = DuplicateTagDiagnosticAnalyzer.MessageFormat;

        [Fact]
        public void No_error_on_different_tags_for_property()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeProperty { get; set; }

            [ProtoMember(2)]
            public string SomeField;
        }
    }";

            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void Duplicate_protomember_tags_show_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeProperty { get; set; }

            [ProtoMember(1)]
            public string SomeField;
        }
    }";
            var error1 = GetExpectedError(8, 26, 1, "SomeProperty, SomeField");
            var error2 = GetExpectedError(11, 26, 1, "SomeProperty, SomeField");
            VerifyCSharpDiagnostic(source, error1, error2);
        }

        [Fact]
        public void Duplicate_datamember_tags_show_as_error()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeProperty { get; set; }

            [DataMember(Order = 1)]
            public string SomeField;
        }
    }";

            var error1 = GetExpectedError(8, 33, 1, "SomeProperty, SomeField");
            var error2 = GetExpectedError(11, 33, 1, "SomeProperty, SomeField");
            VerifyCSharpDiagnostic(source, error1, error2);
        }

        [Fact]
        public void Protomember_with_same_tag_as_datamember_but_on_different_properties_show_as_error()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeProperty { get; set; }

            [ProtoMember(1)]
            public string SomeField;
        }
    }";

            var error1 = GetExpectedError(9, 33, 1, "SomeProperty, SomeField");
            var error2 = GetExpectedError(12, 26, 1, "SomeProperty, SomeField");
            VerifyCSharpDiagnostic(source, error1, error2);
        }
    }
}
