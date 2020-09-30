using System.Collections.Generic;

namespace ExcelDataReader.FieldMaps {
    /// <summary>
    /// Excel 文件 解析结果
    /// </summary>
    public class ParseResult {
        public const int Ok = 0;

        public const int ParseError = 1;

        public const int NotFoundSheet = 1001;

        public const int MissingRequiredColumn = 1002;

        public const int ParseRowError = 1003;

        private ISet<string> _messages = new HashSet<string> ();

        public ParseResult () {
            this.Errors = new List<RowError> ();
        }

        /// <summary>
        /// 重置结果
        /// </summary>
        public void Reset () {
            this.Code = Ok;
            this.Errors.Clear ();
        }

        /// <summary>
        /// 增加消息
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <param name="message"></param>
        public void SetError (int errorCode, string message) {
            if (!_messages.Contains (message)) {
                _messages.Add (message);
            }
            this.Code = errorCode;
        }

        public bool IsValid { get => Code == Ok && this.Errors.Count == 0; }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value></value>
        public int Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        /// <value></value>
        public string Message { get => string.Join (",", _messages); }

        /// <summary>
        /// 错误详细列表
        /// </summary>
        /// <value></value>
        public IList<RowError> Errors { get; }

        public void AddRowError (int row, string message) {
            Errors.Add (new RowError (row, message));
        }

        public void AddRowError (int row, string caption, string message) {
            Errors.Add (new RowError (row, message));
        }
    }

}