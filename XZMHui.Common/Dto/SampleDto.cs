using System;

namespace XZMHui.Common.Dto
{
    public class SampleDto
    {
        public string ID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        /// <example>巴拉巴</example>
        public string ProjectName { get; set; }

        /// <summary>
        /// 环境
        /// </summary>
        /// <example>Development</example>
        public string Env { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        /// <example>Info</example>
        public string Level { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        /// <example>这个时返回的消息</example>
        public string Message { get; set; }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        public DateTime LogDate { get; set; }
    }
}