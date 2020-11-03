using System.Linq;
using XZMHui.Common.Entity;
using XZMHui.Common.Model;
using XZMHui.Repository;
using XZMHui.Repository.Sample;

namespace XZMHui.Services.SampleManager
{
    public class SampleService
    {
        private readonly EntityRepository<SampleEntity> _sampleEntityRepository;
        private readonly SampleModelRepository _sampleModelRepository;

        public SampleService(EntityRepository<SampleEntity> sampleEntityRepository, SampleModelRepository sampleModelRepository)
        {
            _sampleEntityRepository = sampleEntityRepository;
            _sampleModelRepository = sampleModelRepository;
        }

        public (IQueryable<SampleEntity> List, long Rows) GetSampleEntity(int pageIndex, int pageSize, string level)
        {
            return _sampleEntityRepository.GetPagedList(pageIndex, pageSize, "level.Contains(@0)", "", level);
        }

        public (IQueryable<SampleModel> List, long Rows) GetSampleModel(int pageIndex, int pageSize, string level)
        {
            return _sampleModelRepository.GetSampleModel(pageIndex, pageSize, level);
        }
    }
}