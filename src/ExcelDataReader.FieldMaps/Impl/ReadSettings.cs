using System.Linq;

namespace ExcelDataReader.FieldMaps {
    public class ReadSettings {
        public ReadSettings () { }

        /// <summary>
        /// 手动指定 Worksheet 名
        /// 如果不设置 SheetName 则使用默认值
        /// </summary>
        /// <value></value>
        public string SheetName { get; set; }

        // /// <summary>
        // /// 自动匹配 Worksheet
        // /// </summary>
        // /// <value></value>
        // public bool AutoMatchSheet{get;set;}

        /// <summary>
        /// 起始行行号
        /// </summary>
        /// <value></value>
        public int StartRow { get; set; }

        /// <summary>
        /// 忽略 表头映射,手动指定数据列
        /// 用于 没有表头的固定表格
        /// </summary>
        /// <value></value>
        public bool IgnoreHeader { get; set; }

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
        internal bool MatchSheetName (string sheetName) {
            if (string.IsNullOrEmpty (SheetName)) {
                return false;
            }
            return string.Equals (sheetName, SheetName, System.StringComparison.CurrentCultureIgnoreCase);
        }
    }
}