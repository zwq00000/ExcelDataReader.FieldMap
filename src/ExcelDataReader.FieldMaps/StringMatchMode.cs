using System;

namespace ExcelDataReader.FieldMaps {
    /// <summary>
    /// 字符串匹配模式
    /// </summary>
    public enum StringMatchMode {
        ///<summary>
        /// 完全相同
        /// 不包括空白字符
        ///</summary>
        Default = 0,
        Same = 0,

        ///<summary>
        /// 开始部分相同
        ///</summary>
        StartWith = 1 << 1,

        ///<summary>
        /// 结束部分相同
        ///</summary>
        EndsWith = 1 << 2,

        ///<summary>
        /// 包含
        ///</summary>
        Contains = 1 << 3,

    }
}