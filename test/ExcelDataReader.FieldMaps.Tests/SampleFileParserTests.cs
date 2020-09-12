using System.IO;
using Xunit;

namespace ExcelDataReader.FieldMaps.Tests {
    public class SampleFileParserTests {
        public SampleFileParserTests () {
             System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);
         }

        private string GetSampleFile (string fileName) {
            var path = Path.GetFullPath ($"../../../../Resources/{fileName}.xlsx");
            Assert.True (File.Exists (path), path);
            return path;
        }

        [Fact]
        public void Test1 () {
            var parser = new SampleFieldParser ();
            var file = GetSampleFile ("Sample");
            using (var stream = File.OpenRead (file)) {
                var result = parser.Read (stream);
                Assert.NotNull (result);
                Assert.NotEmpty (result);
                Assert.True (parser.ModelState.IsValid);
            }
        }
    }
}