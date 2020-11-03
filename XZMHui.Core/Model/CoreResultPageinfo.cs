using Newtonsoft.Json;
using System;
using XZMHui.Utils.Extensions;

namespace XZMHui.Core.Model
{
    public class CoreResultPageinfo : CoreBasicPagedParam
    {
        /// <summary>
        /// 总页数
        /// </summary>
        [JsonProperty("pages")]
        public int Pages { get; private set; }

        private long _rows = 0;

        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonProperty("rows")]
        public long Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                this.Pages = Math.Ceiling((decimal)_rows / this.PageSize).ParseToInt();
            }
        }
    }
}