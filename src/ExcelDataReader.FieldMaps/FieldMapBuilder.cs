using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace ExcelDataReader.FieldMaps {
    /// <summary>
    /// <see cref="IFieldMap{T}"/> 构造器
    /// 流式方法构造 <see cref="IFieldMap{T}"/>集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldMapBuilder<T> : IEnumerable<IFieldMap<T>> {

        private IList<IFieldMap<T>> _innerList = new List<IFieldMap<T>> ();
        public FieldMapBuilder () {

        }

        /// <summary>
        /// 创建一个新的 <see cref="IFieldMap{T}"/>集合
        /// </summary>
        /// <param name="caption">列标题</param>
        /// <param name="propertyAccesser">属性访问表达式</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static FieldMapBuilder<T> Create<TValue> (string caption, Expression<Func<T, TValue>> propertyAccesser) {
            var builder = new FieldMapBuilder<T> ();
            return builder.Add (caption, propertyAccesser);
        }

        /// <summary>
        /// 增加映射的字段
        /// </summary>
        /// <param name="caption">列标题</param>
        /// <param name="propertyAccesser">属性访问表达式</param>
        /// <param name="IsRequired">手动指定是否必填,默认根据<see cref="RequiredAttribute"/>判断是否为必填项</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public FieldMapBuilder<T> Add<TValue> (string caption, Expression<Func<T, TValue>> propertyAccesser, bool IsRequired) {
            var setter = propertyAccesser.MakeSetter ();
            var map = new FieldMap<T, TValue> (caption, setter, IsRequired);
            _innerList.Add (map);
            return this;
        }

        /// <summary>
        /// 增加映射的字段
        /// </summary>
        /// <param name="caption">列标题</param>
        /// <param name="propertyAccesser">属性访问表达式</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public FieldMapBuilder<T> Add<TValue> (string caption, Expression<Func<T, TValue>> propertyAccesser) {
            var setter = propertyAccesser.MakeSetter ();
            var map = new FieldMap<T, TValue> (caption, setter, propertyAccesser.IsRequired ());
            _innerList.Add (map);
            return this;
        }

        /// <summary>
        /// 增加映射的字段
        /// </summary>
        /// <param name="caption">列标题</param>
        /// <param name="accesser">属性访问表达式</param>
        /// <param name="convert">类型转换表达式</param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public FieldMapBuilder<T> Add<TValue> (string caption, Expression<Func<T, TValue>> accesser, Func<object, TValue> convert) {
            var setter = accesser.MakeSetter ();
            var map = new FieldMap<T, TValue> (caption, setter, convert, accesser.IsRequired ());
            _innerList.Add (map);
            return this;
        }

        public IEnumerator<IFieldMap<T>> GetEnumerator () {
            return _innerList.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator () {
            return this.GetEnumerator ();
        }
    }
}