namespace XZMHui.Repository
{
    public class EntityRepository<T> : RepositoryBase<T> where T : class
    {
        public override MyDbContext DbContext { get; }

        public EntityRepository(MyDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}