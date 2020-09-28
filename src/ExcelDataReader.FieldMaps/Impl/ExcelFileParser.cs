using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExcelDataReader.FieldMaps {
    /// <summary>
    /// Excel 文件数据解析
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelFileParser<T> : IFileParser<T> where T : new () {
        private readonly FieldMapBuilder<T> _fieldMaps;
        private readonly ReadSettings _settings;

        public ExcelFileParser (FieldMapBuilder<T> fieldMaps) : this (fieldMaps, new ReadSettings ()) { }

        public ExcelFileParser (FieldMapBuilder<T> fieldMaps, ReadSettings settings) {
            this._fieldMaps = fieldMaps;
            this._settings = settings;
            this.ModelState = new ModelStateDictionary ();
        }

        /// <summary>
        /// 解析结果错误信息
        /// </summary>
        /// <value></value>
        public ModelStateDictionary ModelState { get; private set;}

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <value></value>
        public ReadSettings Settings { get => _settings; }

        public virtual IEnumerable<T> Read (Stream stream) {
            this.ModelState.Clear ();
            return ParseExcelFile (stream).ToArray ();
        }

        public virtual IEnumerable<T> Read (Stream stream, ModelStateDictionary modelState) {
            this.ModelState = modelState;
            return ParseExcelFile (stream).ToArray ();
        }

        private IEnumerable<T> ParseExcelFile (Stream stream) {
            ModelState.Clear ();
            using (var reader = ExcelReaderFactory.CreateReader (stream)) {
                if (MatchWorkSheet (reader, out var headerRow)) {
                    if (ValidateColumns (_fieldMaps)) {
                        var rowNumField = _settings.GetRowNumberMap (_fieldMaps);
                        int row = headerRow;
                        while (reader.Read ()) {
                            row++;
                            if (!IsEmptyRow (reader, _fieldMaps)) {
                                if (TryReadDataRow (reader, row, out var entity)) {
                                    if (rowNumField != null) {
                                        rowNumField.SetValue (entity, (object) row);
                                    }
                                    yield return entity;
                                }
                            }
                        }
                    }
                } else {
                    ModelState.AddModelError ("WorkSheet", "没有找到合适的工作表");
                }
            }
        }

        private bool MatchWorkSheet (IExcelDataReader reader, out int headerRow) {
            reader.Reset ();
            do {
                _fieldMaps.Reset ();
                if (_settings.MatchSheetName (reader.Name)) {
                    TryReadHeader (reader, out headerRow);
                    return true;
                }
                if (TryReadHeader (reader, out headerRow)) {
                    return true;
                }
            } while (reader.NextResult ());
            headerRow = 1;
            return false;
        }

        /// <summary>
        /// 尝试读取标题行
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="headerRow">表头行号</param>
        /// <returns></returns>
        private bool TryReadHeader (IExcelDataReader reader, out int headerRow) {
            if (_settings.IgnoreHeader) {
                headerRow = 0;
                return true;
            }
            for (var i = 1; i < _settings.MaxHeaderRow; i++) {
                if (ReadHeader (reader)) {
                    if (_fieldMaps.Where (f => f.ColumnIndex > FieldMapBuilder<T>.DefaultColumnIndex).Count () > _fieldMaps.Count () / 2) {
                        headerRow = i;
                        return true;
                    }
                } else {
                    //读取失败,跳出循环
                    break;
                }
            }
            headerRow = 0;
            return false;
        }

        /// <summary>
        /// 读取标题行,并设置 <see cref="IFieldMap{T}.ColumnIndex"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool ReadHeader (IExcelDataReader reader) {
            var matchMethod = Settings.GetHeaderMatchMethod ();
            _fieldMaps.Reset ();
            if (reader.Read ()) {
                for (var col = 0; col < reader.FieldCount; col++) {
                    if (reader.IsDBNull (col)) {
                        continue;
                    }
                    var cap = reader.GetValue (col).ToString ().Trim ();
                    if (!string.IsNullOrEmpty (cap)) {
                        var field = _fieldMaps.FirstOrDefault (f => matchMethod (cap, f.Caption));
                        if (field != null) {
                            field.ColumnIndex = col;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查当前行是否为空行
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private static bool IsEmptyRow (IExcelDataReader reader, FieldMapBuilder<T> fields) {
            return fields.Where (f => f.IsRequired)
                .Select (f => f.ColumnIndex)
                .Any (i => reader.IsDBNull (i) && string.IsNullOrWhiteSpace (reader.GetString (i)));
        }

        private bool TryReadDataRow (IExcelDataReader reader, int rowNum, out T entity) {
            entity = default (T);
            entity = Activator.CreateInstance<T> ();
            object value = null;
            foreach (var field in this._fieldMaps) {
                try {
                    if (field.ColumnIndex < 0) {
                        continue;
                    }
                    if (reader.IsDBNull (field.ColumnIndex)) {
                        if (field.IsRequired) {
                            this.ModelState.AddModelError ($"第{rowNum}行", $"列 '{field.Caption}'的值不能为空");
                            return false;
                        } else {
                            continue;
                        }
                    }

                    value = reader.GetValue (field.ColumnIndex);
                    field.SetValue (entity, value);
                } catch (FormatException) {
                    ModelState.AddModelError ($"第{rowNum}行", $"列 '{field.Caption}'的值'{value}' 不能解析为{field.ValueType.Name}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证 数据列是否 存在映射
        /// </summary>
        /// <param name="fieldMaps"></param>
        private bool ValidateColumns (FieldMapBuilder<T> fieldMaps) {
            foreach (var field in fieldMaps) {
                if (field.IsRequired && field.ColumnIndex < 0) {
                    ModelState.AddModelError ("表头", $"缺少 '{field.Caption}' 列的映射");
                }
            }
            return ModelState.IsValid;
        }
    }
}