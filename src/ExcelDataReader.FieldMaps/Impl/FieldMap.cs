using System;
using System.Linq;

namespace ExcelDataReader.FieldMaps {

    /// <summary>
    /// 字段映射表,用于从分段读取并解析字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal class FieldMap<T, TValue> : IFieldMap<T> {

        private readonly Action<T, TValue> _setter;
        private readonly Func<object, TValue> _converter;

        internal const int DefaultColumnIndex = -1;

        /// <summary>
        /// 真值列表
        /// </summary>
        /// <value></value>
        public static string[] TrueValues = new string[]{
            "t",
            "1",
            "是",
            "真"
        };

        /// <summary>
        /// 是否为必填项
        /// </summary>
        /// <value></value>
        public bool IsRequired { get; }

        public FieldMap (string caption, Action<T, TValue> setter, bool required = false) : this (
            caption, setter, Convert, required
        ) { }

        public FieldMap (string caption, Action<T, TValue> setter, Func<object, TValue> convert, bool required = false) {
            this.Caption = caption;
            this._setter = setter;
            this._converter = convert;
            this.ColumnIndex = DefaultColumnIndex;
            this.IsRequired = required;
        }
        internal FieldMap (string caption, int columnIndex, bool required = false) {
            this.Caption = caption;
            this.ColumnIndex = columnIndex;
            this.IsRequired = required;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// 单元格列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 映射字段的数据类型
        /// </summary>
        /// <value></value>
        public Type ValueType => typeof (TValue);

        public void SetValue (T owner, object value) {
            if (value == null || value is DBNull) {
                return;
            }

            _setter (owner, this._converter (value));
        }

        private static TValue Convert (object value) {
            return (TValue) Convert (typeof (TValue), value);
        }

        private static object Convert (Type targetType, object cellValue) {
            if (targetType.IsEnum) {
                if (TryParseEnum (targetType, cellValue, out var result)) {
                    return result;
                }
            }

            switch (Type.GetTypeCode (targetType)) {
                case TypeCode.Boolean:
                    var val = cellValue.ToString ();
                    return val != null && TrueValues.Any (v => val.StartsWith (v,StringComparison.CurrentCultureIgnoreCase));
                case TypeCode.Byte:
                    return System.Convert.ToByte (cellValue);
                case TypeCode.Char:
                    return System.Convert.ToChar (cellValue);
                case TypeCode.DateTime:
                    return System.Convert.ToDateTime (cellValue);
                case TypeCode.Decimal:
                    return System.Convert.ToDecimal (cellValue);
                case TypeCode.Double:
                    return System.Convert.ToDouble (cellValue);
                case TypeCode.Int16:
                    return System.Convert.ToInt16 (cellValue);
                case TypeCode.Int32:
                    return System.Convert.ToInt32 (cellValue);
                case TypeCode.Int64:
                    return System.Convert.ToInt64 (cellValue);
                case TypeCode.SByte:
                    return System.Convert.ToSByte (cellValue);
                case TypeCode.Single:
                    return System.Convert.ToSingle (cellValue);
                case TypeCode.String:
                    return  cellValue.ToString ().Trim(); //System.Convert.ToString (cellValue.ToString ());
                case TypeCode.UInt16:
                    return System.Convert.ToUInt16 (cellValue);
                case TypeCode.UInt32:
                    return System.Convert.ToUInt32 (cellValue);
                case TypeCode.UInt64:
                    return System.Convert.ToUInt64 (cellValue);
                case TypeCode.Object:
                    return ConvertObject (targetType, cellValue);
                default:
                    return cellValue;
            }
        }

        private static object ConvertObject (Type targetType, object value) {
            if (targetType.IsEnum) {
                if (TryParseEnum (targetType, value, out var result)) {
                    return result;
                }
            }
            return null;
        }

        private static bool TryParseEnum (Type targetType, object value, out object result) {
            if (value == null) {
                result = null;
                return false;
            }
            result = Enum.Parse (targetType, value.ToString (), true);
            return result != null;
        }
    }
}