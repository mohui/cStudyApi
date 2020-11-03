using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using XZMHui.Core.Attributes;
using XZMHui.IRepository;

namespace XZMHui.Repository
{
    [SkipInject]
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class RepositoryBase<T> : IEntityRepository<T> where T : class
    {
        //定义数据访问上下文对象
        public virtual MyDbContext DbContext { get => throw null; }

        public virtual ILogger Logger { get => throw null; }

        #region 同步

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public virtual bool IsExist(Expression<Func<T, bool>> predicate)
        {
            DbContext.Set<T>().FromSqlInterpolated($"");
            return DbContext.Set<T>().Any(predicate);
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public virtual bool Add(T entity, bool autoSave = true)
        {
            int row = 0;
            DbContext.Set<T>().Add(entity);
            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public virtual bool AddRange(IEnumerable<T> entities, bool autoSave = true)
        {
            int row = 0;
            DbContext.Set<T>().AddRange(entities);
            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public virtual bool Update(T entity, bool autoSave = true)
        {
            int row = 0;
            DbContext.Update(entity);
            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 更新实体部分属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <param name="updatedProperties">要更新的字段</param>
        /// <returns></returns>
        public virtual bool Update(T entity, bool autoSave = true, params Expression<Func<T, object>>[] updatedProperties)
        {
            int row = 0;
            //告诉EF Core开始跟踪实体的更改，
            //因为调用DbContext.Attach方法后，EF Core会将实体的State值
            //更改回EntityState.Unchanged，
            DbContext.Attach(entity);
            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    //告诉EF Core实体的属性已经更改。将属性的IsModified设置为true后，
                    //也会将实体的State值更改为EntityState.Modified，
                    //这样就保证了下面SaveChanges的时候会将实体的属性值Update到数据库中。
                    DbContext.Entry(entity).Property(property).IsModified = true;
                }
            }

            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 更新实体部分属性,泛型方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <param name="updatedProperties">要更新的字段</param>
        /// <returns></returns>
        public virtual bool Update<Entity>(Entity entity, bool autoSave = true, params Expression<Func<Entity, object>>[] updatedProperties) where Entity : class
        {
            int row = 0;
            //告诉EF Core开始跟踪实体的更改，
            //因为调用DbContext.Attach方法后，EF Core会将实体的State值
            //更改回EntityState.Unchanged，
            DbContext.Attach(entity);
            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    //告诉EF Core实体的属性已经更改。将属性的IsModified设置为true后，
                    //也会将实体的State值更改为EntityState.Modified，
                    //这样就保证了下面SaveChanges的时候会将实体的属性值Update到数据库中。
                    DbContext.Entry(entity).Property(property).IsModified = true;
                }
            }

            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public virtual bool UpdateRange(IEnumerable<T> entities, bool autoSave = true)
        {
            int row = 0;
            DbContext.UpdateRange(entities);
            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public virtual T GetModel(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public virtual bool Delete(T entity, bool autoSave = true)
        {
            int row = 0;
            DbContext.Set<T>().Remove(entity);
            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="T">对象集合</param>
        /// <returns></returns>
        public virtual bool Delete(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
            int row = DbContext.SaveChanges();
            return (row > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="T">对象集合</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public virtual bool Delete(IEnumerable<T> entities, bool autoSave = true)
        {
            int row = 0;
            DbContext.Set<T>().RemoveRange(entities);
            if (autoSave)
                row = Save();
            return (row > 0);
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetList()
        {
            return DbContext.Set<T>().AsQueryable();
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="top">前几条</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="ordering">排序</param>
        /// <param name="args">条件参数</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetList(int top, string predicate, string ordering, params object[] args)
        {
            var result = DbContext.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(predicate))
                result = result.Where(predicate, args);

            if (!string.IsNullOrWhiteSpace(ordering))
                result = result.OrderBy(ordering);

            if (top > 0)
            {
                result = result.Take(top);
            }
            return result;
        }

        /// <summary>
        /// 分页查询,返回实体对象
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="predicate">条件</param>
        /// <param name="ordering">排序</param>
        /// <param name="args">条件参数</param>
        /// <returns></returns>
        public virtual (IQueryable<T> List, long Rows) GetPagedList(int pageIndex, int pageSize, string predicate, string ordering, params object[] args)
        {
            var result = (from p in DbContext.Set<T>()
                          select p).AsQueryable();

            if (!string.IsNullOrWhiteSpace(predicate))
                result = result.Where(predicate, args);

            if (!string.IsNullOrWhiteSpace(ordering))
                result = result.OrderBy(ordering);

            return (result.Skip((pageIndex - 1) * pageSize).Take(pageSize), result.Count());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="args">条件参数</param>
        /// <returns></returns>
        public virtual int GetRecordCount(string predicate, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(predicate))
            {
                return DbContext.Set<T>().Count();
            }
            else
            {
                return DbContext.Set<T>().Where(predicate, args).Count();
            }
        }

        /// <summary>
        /// 事务性保存
        /// </summary>
        public virtual int Save()
        {
            int result = DbContext.SaveChanges();
            return result;
        }

        #endregion 同步
    }
}