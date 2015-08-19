using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.Test.Verifiers;
using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class DiagnosticAnalyzerTests : DiagnosticVerifier
    {
        protected override IEnumerable<MetadataReference> GetAdditionalReferences()
        {
            yield return MetadataReference.CreateFromFile("protobuf-net.dll");
        }

        //No diagnostics expected to show up
        [Fact]
        public void No_diagnostic_on_empty()
        {
            VerifyCSharpDiagnostic(@"");
        }

        //Diagnostic and CodeFix both triggered and checked for
        [Fact]
        public void TestMethod2()
        {
            const string test = @"    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using ProtoBuf;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            [ProtoMember(0)]
            public string SomeProperty { get; set; }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = ProtobufCodeFixesAnalyzer.DiagnosticId,
                Message =  string.Format(ProtobufCodeFixesAnalyzer.MessageFormat, "SomeProperty"),
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 13, 26) }
            };

            VerifyCSharpDiagnostic(test, expected);
        }
        
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ProtobufCodeFixesAnalyzer();
        }
    }
}