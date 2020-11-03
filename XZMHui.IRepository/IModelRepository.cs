using System.Linq;

namespace XZMHui.IRepository
{
    /// <summary>
    /// 定义泛型仓储接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IModelRepository
    {
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">sql语句参数</param>
        /// <returns></returns>
        IQueryable<T> GetList<T>(string sql, params object[] parameters) where T : class, new();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="ordering">排序</param>
        /// <param name="parameters">sql语句参数</param>
        /// <returns></returns>
        (IQueryable<T> List, long Rows) GetPagedList<T>(string sql, int pageIndex, int pageSize, string ordering, params object[] parameters) where T : class, new();

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="sql">查询sql</param>
        /// <param name="parameters">sql参数</param>
        /// <returns></returns>
        long GetRecordCount(string sql, params object[] parameters);
    }
}