using Microsoft.Extensions.Logging;
using System.Linq;
using XZMHui.Common.Model;

namespace XZMHui.Repository.Sample
{
    public class SampleModelRepository : ModelRepositoryBase
    {
        public override MyDbContext DbContext { get; }

        public override ILogger Logger { get; }

        public SampleModelRepository(MyDbContext dbContext, ILogger<SampleModelRepository> log)
        {
            DbContext = dbContext;
            Logger = log;
        }

        public (IQueryable<SampleModel> List, long Rows) GetSampleModel(int pageIndex, int pageSize, string level)
        {
            System.Console.WriteLine("总记录数：" + this.GetRecordCount($"select 1 from log_info"));
            return this.GetPagedList<SampleModel>($"select id,project_name as projectName, env, level, message, log_date as logDate from log_info where level=@0", pageIndex, pageSize, "logDate asc", level);
        }
    }
}