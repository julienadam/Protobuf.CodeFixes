using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class NegativeTagDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<NegativeTagDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = NegativeTagDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = NegativeTagDiagnosticAnalyzer.MessageFormat;
        
        [Fact]
        public void No_error_on_positive_tag_on_field()
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
        public void No_error_on_positive_tag_on_property()
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
        public void Protomember_with_negative_tag_on_property_show_as_error()
        {
           var source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(-1)]
            public string SomeProperty { get; set; }
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 26, "SomeProperty"));
        }

        [Fact]
        public void Protomember_with_negative_tag_on_field_show_as_error()
        {
           var source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(-1)]
            public string SomeField
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 26, "SomeField"));
        }      
          
        [Fact]
        public void No_error_on_positive_datamember_tag_on_field()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void No_error_on_positive_datamember_tag_on_property()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeProperty { get; set; }
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void Datamember_with_negative_tag_on_property_show_as_error()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = -1)]
            public string SomeProperty { get; set; }
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 33, "SomeProperty"));
        }

        [Fact]
        public void Datamember_with_negative_tag_on_field_show_as_error()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = -1)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 33, "SomeField"));
        }

    }
}