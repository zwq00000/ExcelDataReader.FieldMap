using System.Linq;

namespace ExcelDataReader.FieldMaps {
    public class ReadSettings {
        public ReadSettings () { }

        public string SheetName { get; set; }

        /// <summary>
        /// 起始行
        /// </summary>
        /// <value></value>
        public int StartRow { get; set; }

        /// <summary>
        /// 忽略 表头映射,手动指定数据列
        /// </summary>
        /// <value></value>
        public bool IgnoreHeader{get;set;   }

        /// <summary>
        /// 行号字段
        /// <see cref="IFieldMap{T}.Caption"/>
        /// 包含行号字段,会写入 行号
        /// </summary>
        /// <value></value>
        public string RowNumberField { get; set; }

        internal IFieldMap<T> GetRowNumberMap<T> (FieldMapBuilder<T> builder) {
            if (string.IsNullOrEmpty (RowNumberField)) {
                return default (IFieldMap<T>);

            }
            return builder.FirstOrDefault (f => string.Equals (f.Caption, RowNumberField));
        }

        public bool IsThisSheet (string sheetName) {
            if (string.IsNullOrEmpty (SheetName)) {
                return true;
            }
            return string.Equals (sheetName, SheetName, System.StringComparison.CurrentCultureIgnoreCase);
        }
    }
}