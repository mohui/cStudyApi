using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace XZMHui.Utils
{
    public class ConvertHelper
    {
        public static KeyValuePair<string, object>[] ToKeyPair(object obj)
        {
            var type = obj.GetType();
            var list = new List<KeyValuePair<string, object>>();
            foreach (PropertyInfo item in type.GetProperties())
            {
                list.Add(new KeyValuePair<string, object>(item.Name, item.GetValue(obj, null)));
            }
            return list.ToArray();
        }

        public static object[] ToKeyPairArray(object obj)
        {
            return ToKeyPair(obj).Cast<object>().ToArray();
        }

        ///<summary>
        /// 序列化
        /// </summary>
        /// <param name="data">要序列化的对象</param>
        /// <returns>返回存放序列化后的数据缓冲区</returns>
        public static byte[] Serialize(object data)
        {
            var formatter = new BinaryFormatter();
            using (var rems = new MemoryStream())
            {
                formatter.Serialize(rems, data);
                return rems.GetBuffer();
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">数据缓冲区</param>
        /// <returns>对象</returns>
        public static object Deserialize(byte[] data)
        {
            var formatter = new BinaryFormatter();
            using (var rems = new MemoryStream(data))
            {
                return formatter.Deserialize(rems);
            }
        }

        /// <summary>
        /// 将指定类型转换为另外一个类型（通过匹配属性）
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="srcType">源对象类型</param>
        /// <returns></returns>
        public static TTarget ConvertClass<TTarget>(object source, Type srcType) where TTarget : class, new()
        {
            var dstType = typeof(TTarget);
            var dst = new TTarget();

            var properties = srcType.GetProperties().Intersect(dstType.GetProperties(), new ClassEqual());

            foreach (var item in properties)
            {
                var propSrc = srcType.GetProperty(item.Name);
                var propDst = dstType.GetProperty(item.Name);

                propDst.SetValue(dst, propSrc.GetValue(source));
            }

            return dst;
        }

        private class ClassEqual : IEqualityComparer<PropertyInfo>
        {
            public bool Equals([DisallowNull] PropertyInfo x, [DisallowNull] PropertyInfo y)
            {
                return string.Equals(x.Name, y.Name) && x.PropertyType == y.PropertyType;
            }

            public int GetHashCode([DisallowNull] PropertyInfo obj)
            {
                return obj.Name.GetHashCode() + obj.PropertyType.FullName.GetHashCode();
            }
        }

        /// <summary>
        /// 将指定类型转换为另外一个类型（通过匹配属性）
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns></returns>
        public static IEnumerable<TTarget> ConvertClass<TTarget>(IEnumerable source) where TTarget : class, new()
        {
            foreach (var item in source)
            {
                yield return ConvertClass<TTarget>(item, item.GetType());
            }
        }
    }
}