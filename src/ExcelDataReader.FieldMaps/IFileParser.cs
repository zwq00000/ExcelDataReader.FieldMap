using System.Collections.Generic;
using System.IO;

namespace ExcelDataReader.FieldMaps
{
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
        /// 尝试读取 从 WorkSheet 中读取数据
        /// 用于从多个表头匹配的工作表中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objs"></param>
        /// <example>
        /// <code>
        ///  using (var reader = ExcelReaderFactory.CreateReader (stream)) {
        ///    do {
        ///       if(TryReadSheet(reader,out var values)) {
        ///         ...
        ///        }
        ///    } while (reader.NextResult ());)
        /// }
        /// </code>
        /// </example>
        /// <returns></returns>
        bool TryReadSheet (IExcelDataReader reader,out IEnumerable<T> objs);

        /// <summary>
        /// 验证状态
        /// </summary>
        /// <value></value>
        ParseResult ParseResult { get; }

        /// <summary>
        /// 文件读取配置
        /// </summary>
        /// <value></value>
        ReadSettings Settings { get; }
    }

}