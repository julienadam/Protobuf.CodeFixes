using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ProtoAttributesOnDerivedClassWithoutProtoIncludeTests : ProtobufBootstrappedDiagnosticAnalyzerTestsBase<ProtoAttributesOnDerivedClassWithoutProtoInclude>
    {
        [Fact]
        public void No_warning_when_include_is_properly_defined_on_base_class()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SampleDerivedType)]
        public class SampleType
        {   
            [ProtoMember(1)]
            public string SomeProperty { get; set}
        }

        public class SampleDerivedType : SampleType
        {   
            [ProtoMember(1)]
            public string SomeOtherProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source);
        }
      
        
        [Fact]
        public void Warning_when_include_is_not_defined_on_base_class_for_derived_class_with_proto_member()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        public class SampleType
        {   
            [ProtoMember(1)]
            public string SomeProperty { get; set}
        }

        public class SampleDerivedType : SampleType
        {   
            [ProtoMember(1)]
            public string SomeOtherProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedWarning(12, 22, "SampleDerivedType", "SampleType"));
        }
    }
}