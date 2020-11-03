using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XZMHui.Utils.Extensions
{
    public static class Data
    {
        #region 序列化

        /// <summary>
        /// 将datatable 序列化成为一个动态对象，不会有智能提示，所有属性均和datatable一致
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IList<dynamic> SerializeToDynamicObject(this System.Data.DataTable dataTable)
        {
            if (dataTable == null)
                return default;

            // 如果数据表字段有下滑线会去掉
            var columns = dataTable.Columns.Cast<System.Data.DataColumn>().Select(c =>
                c.ColumnName
            );

            if (dataTable == null || dataTable.Rows.Count == 0)
                return default;

            var ret = new List<dynamic>();
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                dynamic obj = new System.Dynamic.ExpandoObject();
                var dic = obj as IDictionary<string, Object>;
                foreach (var colName in columns)
                {
                    dic.Add(colName, row[colName]);
                }
                ret.Add(obj);
            }

            return ret;
        }

        /// <summary>
        /// 将DataTable序列化成对象，如果DataTable中和对象中存在相同的字段名，就会赋值，否则不会赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="ignoreTableUnderline">是否忽略数据表中的下划线</param>
        /// <returns></returns>
        public static IList<T> SerializeToObject<T>(this System.Data.DataTable dataTable, bool ignoreTableUnderline = true) where T : new()
        {
            var ret = new List<T>();

            if (dataTable == null)
                return ret;

            var type = typeof(T);
            var props = type.GetProperties();
            // 如果数据表字段有下滑线会去掉
            var columns = dataTable.Columns.Cast<System.Data.DataColumn>().Select(c =>
                new
                {
                    ProptyName = ignoreTableUnderline ?
                        c.ColumnName.Replace("_", "").ToLower() :
                        c.ColumnName.ToLower(),
                    c.ColumnName
                }
            );
            if (columns.Count() == 0)
                return ret;

            if (dataTable == null || dataTable.Rows.Count == 0)
                return ret;

            var getDefaultValue = new Func<Type, object>(propType =>
            {
                // 如果是可空类型，直接返回null
                if (Nullable.GetUnderlyingType(propType) != null)
                    return null;
                // 如果时非可空类型 直接获取默认值
                else
                {
                    // 如果不是值类型，直接返回null
                    return propType.IsValueType ? Activator.CreateInstance(propType) : null;
                }
            });

            var convertTypeToProperty = new Func<Type, object, object>((prop, value) =>
            {
                try
                {
                    // 当datatable中字段的值为空时，需要根据实体类的属性类型来初始化默认值
                    if (value == null || value == DBNull.Value)
                    {
                        return getDefaultValue(prop);
                    }
                    // 当datatable中字段的值不为空时，将其转换为属性类型，如果失败，则抛出异常。
                    else
                    {
                        var t = Nullable.GetUnderlyingType(prop) ?? prop;
                        return Convert.ChangeType(value, t);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                var obj = new T();
                foreach (var prop in props)
                {
                    var c = columns.Where(x => x.ProptyName.ToLower() == prop.Name.ToLower()).FirstOrDefault();
                    // 如果对象属性在DataTable中存在相应的列，就赋值
                    if (c != null)
                    {
                        try
                        {
                            // 类型相同直接赋值
                            if (row[c.ColumnName].GetType().FullName == prop.PropertyType.FullName)
                                prop.SetValue(obj, row[c.ColumnName], null);
                            // 类型不同，将table的类型转换为属性的类型，转换失败时赋值为当前类型的默认值
                            else
                                prop.SetValue(obj, convertTypeToProperty(prop.PropertyType, row[c.ColumnName]), null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                ret.Add(obj);
            }

            return ret;
        }

        #endregion 序列化

        #region GerateToSQL

        //private class MyQueryTranslator : ExpressionVisitor
        //{
        //    private StringBuilder sb;
        //    private string _orderBy = string.Empty;
        //    private int? _skip = null;
        //    private int? _take = null;
        //    private string _whereClause = string.Empty;

        // public int? Skip { get { return _skip; } }

        // public int? Take { get { return _take; } }

        // public string OrderBy { get { return _orderBy; } }

        // public string WhereClause { get { return _whereClause; } }

        // public MyQueryTranslator() { }

        // public string Translate(Expression expression) { this.sb = new StringBuilder();
        // this.Visit(expression); _whereClause = this.sb.ToString(); return _whereClause; }

        // private static Expression StripQuotes(Expression e) { while (e.NodeType ==
        // ExpressionType.Quote) { e = ((UnaryExpression)e).Operand; } return e; }

        // protected override Expression VisitMethodCall(MethodCallExpression m) { if
        // (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where") {
        // this.Visit(m.Arguments[0]); LambdaExpression lambda =
        // (LambdaExpression)StripQuotes(m.Arguments[1]); this.Visit(lambda.Body); return m; } else
        // if (m.Method.Name == "Take") { if (this.ParseTakeExpression(m)) { Expression
        // nextExpression = m.Arguments[0]; return this.Visit(nextExpression); } } else if
        // (m.Method.Name == "Skip") { if (this.ParseSkipExpression(m)) { Expression nextExpression
        // = m.Arguments[0]; return this.Visit(nextExpression); } } else if (m.Method.Name ==
        // "OrderBy") { if (this.ParseOrderByExpression(m, "ASC")) { Expression nextExpression =
        // m.Arguments[0]; return this.Visit(nextExpression); } } else if (m.Method.Name ==
        // "OrderByDescending") { if (this.ParseOrderByExpression(m, "DESC")) { Expression
        // nextExpression = m.Arguments[0]; return this.Visit(nextExpression); } }

        // throw new NotSupportedException(string.Format("The method '{0}' is not supported",
        // m.Method.Name)); }

        // protected override Expression VisitUnary(UnaryExpression u) { switch (u.NodeType) { case
        // ExpressionType.Not: sb.Append(" NOT "); this.Visit(u.Operand); break;

        // case ExpressionType.Convert: this.Visit(u.Operand); break;

        // default: throw new NotSupportedException(string.Format("The unary operator '{0}' is not
        // supported", u.NodeType)); } return u; }

        // ///
        // <summary>
        // ///
        // </summary>
        // ///
        // <param name="b"></param>
        // ///
        // <returns></returns>
        // protected override Expression VisitBinary(BinaryExpression b) { sb.Append("("); this.Visit(b.Left);

        // switch (b.NodeType) { case ExpressionType.And: sb.Append(" AND "); break;

        // case ExpressionType.AndAlso: sb.Append(" AND "); break;

        // case ExpressionType.Or: sb.Append(" OR "); break;

        // case ExpressionType.OrElse: sb.Append(" OR "); break;

        // case ExpressionType.Equal: if (IsNullConstant(b.Right)) { sb.Append(" IS "); } else {
        // sb.Append(" = "); } break;

        // case ExpressionType.NotEqual: if (IsNullConstant(b.Right)) { sb.Append(" IS NOT "); }
        // else { sb.Append(" <> "); } break;

        // case ExpressionType.LessThan: sb.Append(" < "); break;

        // case ExpressionType.LessThanOrEqual: sb.Append(" <= "); break;

        // case ExpressionType.GreaterThan: sb.Append(" > "); break;

        // case ExpressionType.GreaterThanOrEqual: sb.Append(" >= "); break;

        // default: throw new NotSupportedException(string.Format("The binary operator '{0}' is not
        // supported", b.NodeType)); }

        // this.Visit(b.Right); sb.Append(")"); return b; }

        // protected override Expression VisitConstant(ConstantExpression c) { IQueryable q =
        // c.Value as IQueryable;

        // if (q == null && c.Value == null) { sb.Append("NULL"); } else if (q == null) { switch
        // (Type.GetTypeCode(c.Value.GetType())) { case TypeCode.Boolean: sb.Append(((bool)c.Value)
        // ? 1 : 0); break;

        // case TypeCode.String: sb.Append("'"); sb.Append(c.Value); sb.Append("'"); break;

        // case TypeCode.DateTime: sb.Append("'"); sb.Append(c.Value); sb.Append("'"); break;

        // case TypeCode.Object: throw new NotSupportedException(string.Format("The constant for
        // '{0}' is not supported", c.Value));

        // default: sb.Append(c.Value); break; } }

        // return c; }

        // protected override Expression VisitMember(MemberExpression m) { if (m.Expression != null
        // && m.Expression.NodeType == ExpressionType.Parameter) { sb.Append(m.Member.Name); return
        // m; } else { var obj = Expression.Convert(m, typeof(object)); var lambda =
        // Expression.Lambda<Func<object>>(obj); var ret = lambda.Compile(); sb.Append(ret());
        // return m; } //throw new NotSupportedException(string.Format("The member '{0}' is not
        // supported", m.Member.Name)); }

        // protected bool IsNullConstant(Expression exp) { return (exp.NodeType ==
        // ExpressionType.Constant && ((ConstantExpression)exp).Value == null); }

        // private bool ParseOrderByExpression(MethodCallExpression expression, string order) {
        // UnaryExpression unary = (UnaryExpression)expression.Arguments[1]; LambdaExpression
        // lambdaExpression = (LambdaExpression)unary.Operand;

        // //lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

        // MemberExpression body = lambdaExpression.Body as MemberExpression; if (body != null) { if
        // (string.IsNullOrEmpty(_orderBy)) { _orderBy = string.Format("{0} {1}", body.Member.Name,
        // order); } else { _orderBy = string.Format("{0}, {1} {2}", _orderBy, body.Member.Name,
        // order); }

        // return true; }

        // return false; }

        // private bool ParseTakeExpression(MethodCallExpression expression) { ConstantExpression
        // sizeExpression = (ConstantExpression)expression.Arguments[1];

        // int size; if (int.TryParse(sizeExpression.Value.ToString(), out size)) { _take = size;
        // return true; }

        // return false; }

        // private bool ParseSkipExpression(MethodCallExpression expression) { ConstantExpression
        // sizeExpression = (ConstantExpression)expression.Arguments[1];

        // int size; if (int.TryParse(sizeExpression.Value.ToString(), out size)) { _skip = size;
        // return true; }

        //        return false;
        //    }
        //}

        private static Dictionary<string, object> GenerateColumns<T>(T src) where T : class, new()
        {
            var type = typeof(T);
            var colDic = new Dictionary<string, object>();
            var properties = type.GetProperties();
            foreach (var item in properties)
            {
                var attributes = item.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                var value = item.GetValue(src, null);
                try
                {
                    if (!string.IsNullOrEmpty(attributes.TypeName))
                        colDic.Add(attributes.Name, Convert.ChangeType(value ?? default, Type.GetType(attributes.TypeName)));
                    else
                        colDic.Add(attributes.Name, Convert.ChangeType(value ?? default, item.PropertyType.GetType()));
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException($"{item.Name}在指定了类型后,转换类型失败,请确保所有数据均具备转换条件." + ex.Message);
                }
            }

            return colDic;
        }

        public static (string SQL, object[] Params) GerateToInsertSQL<T>(this T src) where T : class, new()
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            string tableName = attr?.Name ?? type.Name;

            var colDic = GenerateColumns(src);

            var sql = new StringBuilder();
            sql.Append($" insert into {tableName}({string.Join(",", colDic.Select(x => x.Key))}) values ({string.Join(",", colDic.Select(x => $"@{x.Key}"))})");

            return (sql.ToString(), colDic.Select(x => x.Value).ToArray());
        }

        //public static (string SQL, object[] Params) GerateToUpdateSQL<T>(this T src, Expression<Func<T, bool>> expression) where T : class, new()
        //{
        //    return default;
        //}

        //public static (string SQL, object[] Params) GerateToSelectSQL<T>(this T src, Expression<Func<T, bool>> expression) where T : class, new()
        //{
        //    return default;
        //}

        //public static (string SQL, object[] Params) GerateToDeleteSQL<T>(this T src, Expression<Func<T, bool>> expression) where T : class, new()
        //{
        //    return default;
        //}

        #endregion GerateToSQL
    }
}