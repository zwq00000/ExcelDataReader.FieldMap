using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace ExcelDataReader.FieldMaps {

    /// <summary>
    /// FieldMap Builder 扩展方法
    /// </summary>
    internal static class FieldMapExtensions {

        #region  MakeSetter
        /// <summary>
        /// 根据属性/字段访问表达式,构造 赋值语句
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static Action<T, TValue> MakeSetter<T, TValue> (this Expression<Func<T, TValue>> expression) {
            var memberExpression = GetMemberExpression (expression);
            return MakeSetter<T, TValue> (memberExpression, Expression.Parameter (typeof (TValue), "__value"));
        }

        /// <summary>
        /// 属性是否是 必选项 <see cref="RequiredAttribute"/>
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        internal static bool IsRequired<T, TValue> (this Expression<Func<T, TValue>> expression) {
            var memberExpression = GetMemberExpression (expression);
            return memberExpression.Member.CustomAttributes.Any (a => a.AttributeType == typeof (RequiredAttribute));
        }

        /// <summary>
        /// 向下搜索 成员表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static MemberExpression GetMemberExpression (Expression expression) {
            switch (expression) {
                case MemberExpression member:
                    return member;
                case LambdaExpression lambda:
                    return GetMemberExpression (lambda.Body);
                case UnaryExpression unary:
                    return GetMemberExpression (unary.Operand);
                default:
                    throw new InvalidExpressionException ($"不支持的表达式类型:{expression.NodeType}\n表达式:{expression}");
            }
        }

        /// <summary>
        /// 构造 Setter Action
        /// </summary>
        /// <param name="memberAccess"></param>
        /// <param name="rightParameter"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        private static Action<T, TValue> MakeSetter<T, TValue> (MemberExpression memberAccess, ParameterExpression rightParameter) {
            var leftParameter = GetParameterExpression (memberAccess);
            Expression<Action<T, TValue>> setterExp;
            switch (memberAccess.Expression) {
                case MemberExpression ownerMember:
                    // 检查 ownerMember 是否为空,并初始化 ownerMember
                    setterExp = Expression.Lambda<Action<T, TValue>> (
                        Expression.Block (
                            Expression.IfThen (Expression.Equal (ownerMember, Expression.Constant (null)),
                                Expression.Assign (ownerMember, Expression.New (ownerMember.Type))),
                            Expression.Assign (memberAccess, rightParameter)),
                        leftParameter, rightParameter
                    );
                    break;
                case ParameterExpression parameter:
                default:
                    setterExp = Expression.Lambda<Action<T, TValue>> (
                        Expression.Assign (memberAccess, rightParameter),
                        leftParameter, rightParameter
                    );
                    break;
            }

            Debug.WriteLine (setterExp);
            return setterExp.Compile ();
        }

        /// <summary>
        /// 搜索表达式树,查找并返回 参数表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static ParameterExpression GetParameterExpression (Expression expression) {
            switch (expression) {
                case LambdaExpression lambda:
                    return GetParameterExpression (lambda.Body);
                case MemberExpression member:
                    return GetParameterExpression (member.Expression);
                case ParameterExpression parameter:
                    return parameter;
                default:
                    throw new NotSupportedException ($"不支持的表达式类型:{expression.NodeType}\n表达式:{expression}");
            }
        }

        #endregion

    }

}