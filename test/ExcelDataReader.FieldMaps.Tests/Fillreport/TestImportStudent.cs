using System;
using System.Globalization;
using System.IO;
using Xunit;

namespace ExcelDataReader.FieldMaps.Tests {
    public class StudentDraftTests {

        private static readonly FieldMapBuilder<StudentDraft> fieldMaps = FieldMapBuilder<StudentDraft>
            .Create (nameof (StudentDraft.RowNum), s => s.RowNum)
            .Add ("学号", s => s.XH)
            .Add ("姓名", s => s.XM)
            .Add ("性别", s => s.XBM)
            .Add ("出生日期", s => s.CSRQ, DateConvert)
            .Add ("身份证号", s => s.SFZJH)
            .Add ("学籍号", s => s.XJH)
            .Add ("入学年份", s => s.RXNY)
            .Add ("行政班代码", s => s.XZBDM)
            .Add ("专业代码", s => s.ZYDM)
            .Add ("专业名称", s => s.ZYMC);

        private static DateTime DateConvert (object source) {
            var dateStr = source.ToString ();
            if (DateTime.TryParse (dateStr, out var result)) {
                return result;
            }
            if (DateTime.TryParseExact (dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)) {
                return date;
            }
            return DateTime.MinValue;
        }

        public StudentDraftTests () {
            System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);
        }

        private string GetSampleFile (string fileName) {
            var path = Path.GetFullPath ($"../../../../Resources/{fileName}");
            Assert.True (File.Exists (path), path);
            return path;
        }

        [Fact]
        public void TestImport () {
            var parser = new ExcelFileParser<StudentDraft> (fieldMaps);
            var values = parser.Read (File.OpenRead (GetSampleFile("Test1014.xlsx")));
            Assert.True (parser.ParseResult.IsValid);
            Assert.NotEmpty (values);
        }
    }
}