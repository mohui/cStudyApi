using XZMHui.Core.Model;

namespace XZMHui.Common.Param
{
    public class SampleParam : CoreBasicPagedParam
    {
        public string ID { get; set; }
        public string ProjectName { get; set; }
        public string Env { get; set; }
        public string Level { get; set; }
    }
}