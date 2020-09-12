using System;
using System.ComponentModel.DataAnnotations;

namespace ExcelDataReader.FieldMaps.Tests {
    /// <summary>
    /// Account Number	Trader	Order Number	Leg Number	Buy Or Sell	Quantity	Contract	Fill Price	Fill Price Decimal	Fill Time
    /// 74085	User	412735810	1	b	1	DA	72040	0.7204	03/01/2016 17:56:14
    /// </summary>
    public class ReportDto {

        public int Row { get; set; }

        [Required]
        public long AccountNumber { get; set; }

        [Required]
        public string Trader { get; set; }
        public int OrderNumber { get; set; }
        public int LegNumber { get; set; }
        public bool BuyOrSell { get; set; }
        public string Quantity { get; set; }
        public string Contract { get; set; }
        public decimal FillPrice { get; set; }
        public decimal FillPriceDecimal { get; set; }
        public DateTime FillTime { get; set; }
    }
}