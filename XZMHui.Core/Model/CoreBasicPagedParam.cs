using Newtonsoft.Json;

namespace XZMHui.Core.Model
{
    public abstract class CoreBasicPagedParam
    {
        private int _pageNo = 1;

        /// <summary>
        /// 页码
        /// </summary>
        [JsonProperty("pageNo")]
        public int PageNo
        {
            get => _pageNo;
            set
            {
                if (value <= 0) _pageNo = 1;
                else _pageNo = value;
            }
        }

        private int _pageSize = 20;

        /// <summary>
        /// 页大小
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 0) _pageSize = 1;
                else _pageSize = value;
            }
        }
    }
}