namespace ExcelDataReader.FieldMaps.Tests {
    public class SampleFieldParser : ExcelFileParser<Sample> {
        private static readonly FieldMapBuilder<Sample> builder = FieldMapBuilder<Sample>
        .Create ("行号", s => s.Row)
        .Add ("姓名", s => s.Name)
        .Add ("日期", s => s.Date)
        .Add ("地址", s => s.Address);

        public SampleFieldParser () : base (builder) { }

    }
}