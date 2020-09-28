using System;
using System.ComponentModel;
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

        /// <summary>
        /// 起始行行号
        /// </summary>
        /// <value></value>
        [Obsolete ("改为自动匹配标题行,搜索前5行检查标题")]
        public int StartRow { get; set; }

        /// <summary>
        /// 最大表头行出现的行号
        /// 默认为 5
        /// </summary>
        /// <value></value>
        [DefaultValue (5)]
        public int MaxHeaderRow { get; set; } = 5;

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

        /// <summary>
        /// 表头和 <see cref="IFieldMap{T}.Caption"/> 匹配的方式
        /// 默认为 <see cref="StringMatchMode.Same">完全相同</see>
        /// </summary>
        /// <remark>
        /// 设置 表头和 <see cref="IFieldMap{T}.Caption"/> 匹配的方式
        /// </remark>
        public StringMatchMode HeaderMatchMode { get; set; } = StringMatchMode.Same;

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

        internal Func<string, string, bool> GetHeaderMatchMethod () {
            var comparison = StringComparison.CurrentCultureIgnoreCase;
            switch (HeaderMatchMode) {
                case StringMatchMode.Same:
                    return (s, t) => string.Equals (s, t, comparison);
                case StringMatchMode.StartWith:
                    return (s, t) => s.StartsWith (t, comparison);
                case StringMatchMode.EndsWith:
                    return (s, t) => s.EndsWith (t, comparison);
                case StringMatchMode.Contains:
                    return (s, t) => s.Contains (t);
                default:
                    return string.Equals;
            }
        }
    }
}