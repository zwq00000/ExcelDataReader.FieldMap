using System;
using System.ComponentModel;

namespace ExcelDataReader.FieldMaps
{
    /// <summary>
    /// 字段/属性映射
    /// </summary>
    /// <typeparam name="T">Owner Object type</typeparam>
    public interface IFieldMap<T> {
        /// <summary>
        /// 匹配的 列标题
        /// excel 表格标题
        /// </summary>
        /// <value></value>
        string Caption { get; }

        /// <summary>
        /// 单元格列索引
        /// </summary>
        /// <remark>
        /// -1 表示未指定对应列
        /// </remark>
        [DefaultValue (-1)]
        int ColumnIndex { get; set; }

        /// <summary>
        /// 是否为必填项
        /// </summary>
        /// <value></value>
        bool IsRequired { get; }

        /// <summary>
        /// 映射字段的数据类型
        /// </summary>
        /// <value></value>
        Type ValueType { get; }

        /// <summary>
        /// 设置 值
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="value"></param>
        void SetValue (T owner, object value);

    }
}