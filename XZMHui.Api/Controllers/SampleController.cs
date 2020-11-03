using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using XZMHui.Common.Dto;
using XZMHui.Common.Param;
using XZMHui.Core.Model;

namespace XZMHui.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly XZMHui.Services.SampleManager.SampleService _sampleService;
        private readonly XZMHui.Services.ISample _sample;

        public SampleController(ILogger<SampleController> logger,
            XZMHui.Services.SampleManager.SampleService infoCUID,
            XZMHui.Services.ISample sample)
        {
            _logger = logger;
            _sampleService = infoCUID;
            _sample = sample;
        }

        /// <summary>
        /// EF普通Get接口，DbContext中需要定义数据库实体
        /// </summary>
        /// <returns>接口的Service查询的是单个数据库实体</returns>
        [HttpGet("Get")]
        [Produces("application/json")]
        public CoreResultPagedData<SampleDto> Get()
        {
            System.Console.WriteLine(_sample.Hello());
            _logger.LogInformation("public List<Common.Dto.SampleDto> Get()");
            var ret = _sampleService.GetSampleEntity(1, 20, "Info");
            return new CoreResultPagedData<SampleDto>(ret.List.AsEnumerable(), ret.Rows, 1, 20);
        }

        /// <summary>
        /// 业务Get接口，DbContext中不需要定义业务实体
        /// </summary>
        /// <returns>接口的Service是按照业务执行指定sql后生成的实体</returns>
        [HttpGet("GetModel")]
        [Produces("application/json")]
        public CoreResultPagedData<SampleDto> GetModel()
        {
            _logger.LogInformation("public List<Common.Dto.SampleDto> GetModel()");
            var ret = _sampleService.GetSampleModel(1, 20, "Info");
            return new CoreResultPagedData<SampleDto>(ret.List.AsEnumerable(), ret.Rows, 1, 20);
        }

        /// <summary>
        /// EF普通post接口，DbContext中需要定义数据库实体
        /// </summary>
        /// <param name="sampleParam">包含页码信息的参数</param>
        /// <returns>接口的Service查询的是单个数据库实体</returns>
        [HttpPost("Post")]
        [Produces("application/json")]
        public CoreResultPagedData<SampleDto> Post([FromBody] SampleParam sampleParam)
        {
            _logger.LogInformation("public List<Common.Dto.SampleDto> Post()");
            var ret = _sampleService.GetSampleEntity(sampleParam.PageNo, sampleParam.PageSize, sampleParam.Level);
            return new CoreResultPagedData<SampleDto>(ret.List.AsEnumerable(), ret.Rows, sampleParam.PageNo, sampleParam.PageSize);
        }

        /// <summary>
        /// 业务post接口，DbContext中不需要定义业务实体
        /// </summary>
        /// <param name="sampleParam">包含页码信息的参数</param>
        /// <returns>接口的Service是按照业务执行指定sql后生成的实体</returns>
        [HttpPost("PostModel")]
        [Produces("application/json")]
        public CoreResultPagedData<SampleDto> PostModel([FromBody] SampleParam sampleParam)
        {
            _logger.LogInformation("public List<Common.Dto.SampleDto> PostModel()");
            var ret = _sampleService.GetSampleModel(sampleParam.PageNo, sampleParam.PageSize, sampleParam.Level);
            return new CoreResultPagedData<SampleDto>(ret.List.AsEnumerable(), ret.Rows, sampleParam.PageNo, sampleParam.PageSize);
        }
    }
}