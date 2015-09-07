using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ProtobufBootstrapperDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<ProtobufBootstrapperDiagnosticAnalyzer>
    {
        protected override string DiagnosticId => "Foo";
        protected override string MessageFormat => "Bar";

        [Fact(Skip = "Need to fix the paket / fake scripts before")]
        public void Test()
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
            VerifyCSharpDiagnostic(source, GetExpectedError(0,0));
        }
    }
}