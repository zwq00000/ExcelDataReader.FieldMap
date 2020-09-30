namespace ExcelDataReader.FieldMaps
{
    public class RowError {
        public RowError (int row, string message) {
            this.Row = row;
            this.Message = message;
        }

        public RowError (int row, string caption, string message) : this (row, message) {
            this.Caption = caption;
        }

        /// <summary>
        /// 行号
        /// </summary>
        /// <value></value>
        public int Row { get; set; }

        /// <summary>
        /// 列标题
        /// </summary>
        /// <value></value>
        public string Caption { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
    }

}