using System.Collections.Generic;

namespace XZMHui.Core.Model
{
    public class CoreResultPagedBase<T> : CoreResultPageinfo
    {
        public IEnumerable<T> List { get; set; }

        public CoreResultPagedBase(IEnumerable<T> data, long rows, int pageNo, int pageSize)
        {
            List = data;

            PageNo = pageNo;
            PageSize = pageSize;

            Rows = rows;
        }
    }
}