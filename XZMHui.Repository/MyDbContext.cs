using Microsoft.EntityFrameworkCore;
using XZMHui.Common.Entity;
using XZMHui.Core.Attributes;

namespace XZMHui.Repository
{
    [SkipInject]
    public class MyDbContext : DbContext
    {
        #region Entity 数据库实体

        public DbSet<SampleEntity> SampleEntity { get; set; }

        #endregion Entity 数据库实体

        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="options"></param>
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
    }
}