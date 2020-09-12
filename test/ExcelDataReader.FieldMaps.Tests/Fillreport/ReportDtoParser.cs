namespace ExcelDataReader.FieldMaps.Tests {
    public class ReportDtoParser : ExcelFileParser<ReportDto> {
        private static readonly FieldMapBuilder<ReportDto> builder = FieldMapBuilder<ReportDto>
        .Create ("Row", s => s.Row)
        .Add ("Account Number", r => r.AccountNumber)
        .Add ("Trader", r => r.Trader)
        .Add ("Order Number", r => r.OrderNumber)
        .Add ("Leg Number", r => r.LegNumber)
        .Add ("Buy Or Sell", r => r.BuyOrSell)
        .Add ("Quantity", r => r.Quantity)
        .Add ("Contract", r => r.Contract)
        .Add ("Fill Price", r => r.FillPrice)
        .Add ("Fill Price Decimal", r => r.FillPriceDecimal)
        .Add ("Fill Time", r => r.FillTime);

        private static ReadSettings settings = new ReadSettings(){
            RowNumberField = "Row",
            StartRow = 1
        };

        public ReportDtoParser () : base (builder,settings) { }

    }
}