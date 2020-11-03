using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace XZMHui.Utils.Extensions
{
    public static class Generic
    {
        /// <summary>
        /// 深克隆一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T src)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                T ret = default(T);
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, src);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深表副本)
                ret = (T)bf.Deserialize(ms);
                return ret;
            }
        }

        #region map reduce

        /// <summary>
        /// Map实现 通过指定函数会返回一个新的集合
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<B> Map<A, B>(this IEnumerable<A> src, Func<A, B> func)
        {
            var bs = new List<B>();
            foreach (A item in src)
            {
                bs.Add(func(item));
            }
            return bs.AsEnumerable();
        }

        /// <summary>
        /// Map实现 通过指定函数会返回一个新的集合
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<B> Map<A, B>(this IEnumerable<A> src, Func<A, int, B> func)
        {
            var bs = new List<B>();
            var index = 0;
            foreach (A item in src)
            {
                bs.Add(func(item, index++));
            }
            return bs.AsEnumerable();
        }

        /// <summary>
        /// Reduce实现
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <param name="accumulator">结果初始值</param>
        /// <returns></returns>
        public static B Reduce<A, B>(this IEnumerable<A> src, Func<B, A, B> func, B accumulator)
        {
            var b = accumulator;
            foreach (A item in src)
            {
                b = func(b, item);
            }
            return b;
        }

        /// <summary>
        /// Reduce实现
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <param name="accumulator"></param>
        /// <returns></returns>
        public static B Reduce<A, B>(this IEnumerable<A> src, Func<B, A, int, B> func, B accumulator)
        {
            var b = accumulator;
            var index = 0;
            foreach (A item in src)
            {
                b = func(b, item, index++);
            }
            return b;
        }

        /// <summary>
        /// Reduce实现
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <param name="accumulator">结果初始值</param>
        /// <returns></returns>
        public static B Reduce<A, B>(this IEnumerable<A> src, Func<B, A, int, IEnumerable<A>, B> func, B accumulator)
        {
            var b = accumulator;
            var index = 0;
            foreach (A item in src)
            {
                b = func(b, item, index++, src);
            }
            return b;
        }

        #endregion map reduce
    }
}