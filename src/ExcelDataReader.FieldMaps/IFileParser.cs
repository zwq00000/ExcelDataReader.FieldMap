using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExcelDataReader.FieldMaps {
    /// <summary>
    /// 文件/数据流 读取/解析器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFileParser<T> {

        /// <summary>
        /// 读取文件并解析为数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        IEnumerable<T> Read (Stream stream);

        /// <summary>
        /// 验证状态
        /// </summary>
        /// <value></value>
        ModelStateDictionary ModelState { get; }
    }

}