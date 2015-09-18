using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class TagZeroDiagnosticAnalyzerTests : ProtobufBootstrappedDiagnosticAnalyzerTestsBase<TagZeroDiagnosticAnalyzer>
    {
        [Fact]
        public void No_error_for_tag_set_to_non_zero_on_property()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void No_error_for_tag_set_to_non_zero_on_field()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void Datamember_tag_set_to_zero_on_property_triggers_error()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 0)]
            public string SomeProperty { get; set; }
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 33, "SomeProperty"));
        }

        [Fact]
        public void Datamember_tag_set_to_zero_on_field_triggers_error()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 0)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 33, "SomeField"));
        }

        [Fact]
        public void Tag_set_to_zero_on_property_triggers_error()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(0)]
            public string SomeProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 26, "SomeProperty"));
        }

        [Fact]
        public void Tag_set_to_zero_on_field_triggers_error()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(0)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 26, "SomeField"));
        }
    }
}