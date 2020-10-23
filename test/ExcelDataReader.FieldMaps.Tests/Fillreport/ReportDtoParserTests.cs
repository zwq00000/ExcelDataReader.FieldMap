using System.IO;
using System.Linq;
using Xunit;

namespace ExcelDataReader.FieldMaps.Tests {
    public class ReportDtoParserTests {
        public ReportDtoParserTests () {
            System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);
        }

        private string GetSampleFile (string fileName) {
            var path = Path.GetFullPath ($"../../../../Resources/{fileName}");
            Assert.True (File.Exists (path), path);
            return path;
        }

        [Fact]
        public void TestRead () {
            var parser = new ReportDtoParser ();
            var file = GetSampleFile ("Fillreport.xlsx");
            using (var stream = File.OpenRead (file)) {
                var result = parser.Read (stream);
                Assert.NotNull (result);
                Assert.NotEmpty (result);
                Assert.Equal (16, result.Count ());
                Assert.True (parser.ParseResult.IsValid);
            }
        }

        [Fact]
        public void TestTryReadSheet () {
            var parser = new ReportDtoParser ();
            var file = GetSampleFile ("Fillreport.xlsx");
            bool hasData = false;
            using (var reader = ExcelReaderFactory.CreateReader (File.OpenRead (file))) {
                do {
                    if (parser.TryReadSheet (reader, out var values)) {
                        Assert.NotEmpty (values);
                        hasData = true;
                    }
                } while (reader.NextResult ());
            }
            Assert.True (hasData);
        }
    }
}