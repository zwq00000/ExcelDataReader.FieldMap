using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;
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
        public ModelStateDictionary ModelState { get; }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <value></value>
        public ReadSettings Settings { get => _settings; }

        public virtual IEnumerable<T> Read (Stream stream) {
            this.ModelState.Clear ();
            return ParseExcelFile (stream, this.ModelState).ToArray ();
        }

        public virtual IEnumerable<T> Read (Stream stream, ModelStateDictionary modelState) {
            return ParseExcelFile (stream, modelState).ToArray ();
        }

        private IEnumerable<T> ParseExcelFile (Stream stream, ModelStateDictionary modelState) {
            modelState.Clear ();
            using (var reader = ExcelReaderFactory.CreateReader (stream)) {
                if (MatchWorkSheet (reader)) {
                    if (ValidateColumns (_fieldMaps, modelState)) {
                        var rowNumField = _settings.GetRowNumberMap (_fieldMaps);
                        int row = 1;
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

        private bool MatchWorkSheet (IExcelDataReader reader) {
            reader.Reset ();
            do {
                _fieldMaps.Reset ();
                if (_settings.MatchSheetName (reader.Name)) {
                    return true;
                }
                for (var i = 0; i < _settings.StartRow; i++) {
                    //skip start rows
                    reader.Read ();
                }
                if (!_settings.IgnoreHeader) {
                    ReadHeader (reader, _fieldMaps);
                }
                if (_fieldMaps.Where (f => f.ColumnIndex > FieldMapBuilder<T>.DefaultColumnIndex).Count () > _fieldMaps.Count () / 2) {
                    //超过 50% 匹配
                    return true;
                }
            } while (reader.NextResult ());
            return false;
        }

        private static bool ReadHeader (IExcelDataReader reader, FieldMapBuilder<T> fields) {
            if (reader.Read ()) {
                for (var col = 0; col < reader.FieldCount; col++) {
                    if (reader.IsDBNull (col)) {
                        continue;
                    }
                    var cap = reader.GetString (col).Trim ();
                    if (!string.IsNullOrEmpty (cap)) {
                        var field = fields.FirstOrDefault (f => cap.StartsWith (f.Caption));
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
                            this.ModelState.AddModelError (field.Caption, $"第{rowNum}行 '{field.Caption}' 不能为空");
                            return false;
                        } else {
                            continue;
                        }
                    }

                    value = reader.GetValue (field.ColumnIndex);
                    field.SetValue (entity, value);
                } catch (FormatException) {
                    ModelState.AddModelError (field.Caption, $"值'{value}' 不能解析为{field.ValueType}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证 数据列是否 存在映射
        /// </summary>
        /// <param name="fieldMaps"></param>
        /// <param name="modelState"></param>
        private bool ValidateColumns (FieldMapBuilder<T> fieldMaps, ModelStateDictionary modelState) {
            foreach (var field in fieldMaps) {
                if (field.IsRequired && field.ColumnIndex < 0) {
                    modelState.AddModelError (field.Caption, $"缺少 '{field.Caption}' 列映射");
                }
            }
            return modelState.IsValid;
        }
    }
}