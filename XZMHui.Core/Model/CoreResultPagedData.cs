using System.Collections;
using XZMHui.Utils;

namespace XZMHui.Core.Model
{
    public class CoreResultPagedData<T> : CoreResult<CoreResultPagedBase<T>> where T : class, new()
    {
        public CoreResultPagedData(IEnumerable data, long rows, int pageNo, int pageSize)
        {
            this.Data = new CoreResultPagedBase<T>(
                ConvertHelper.ConvertClass<T>(data),
                rows,
                pageNo,
                pageSize
            );
        }
    }
}