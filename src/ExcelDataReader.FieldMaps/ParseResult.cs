using System.Collections.Generic;

namespace ExcelDataReader.FieldMaps
{
    /// <summary>
    /// Excel 文件 解析结果
    /// </summary>
    public class ParseResult {
        public const int Ok = 0;

        public const int ParseError = 1;
        public ParseResult () {
            this.Errors = new List<RowError> ();
        }

        internal void SetParseError (string message) {
            this.Message = message;
            this.Code = ParseError;
        }

        /// <summary>
        /// 重置结果
        /// </summary>
        public void Reset () {
            this.Code = Ok;
            this.Errors.Clear ();
        }

        public bool IsValid { get => Code == Ok && this.Errors.Count == 0; }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value></value>
        public int Code { get; set; }

        /// <summary>
        /// 全局错误消息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }

        public IList<RowError> Errors { get; }

        public void AddRowError (int row, string message) {
            Errors.Add (new RowError (row, message));
        }

        public void AddRowError (int row, string caption, string message) {
            Errors.Add (new RowError (row, message));
        }
    }

}