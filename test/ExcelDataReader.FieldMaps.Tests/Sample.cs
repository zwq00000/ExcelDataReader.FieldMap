using System;
using System.ComponentModel.DataAnnotations;

namespace ExcelDataReader.FieldMaps.Tests {
    public class Sample {

        public int Row { get; set; }

        [Required]
        public int Name { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }
    }
}