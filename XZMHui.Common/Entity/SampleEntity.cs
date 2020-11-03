using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace XZMHui.Common.Entity
{
    [Table("log_info")]
    public class SampleEntity
    {
        [Column("id")] public string ID { get; set; }
        [Column("project_name")] public string ProjectName { get; set; }
        [Column("env")] public string Env { get; set; }
        [Column("level")] public string Level { get; set; }
        [Column("logger")] public string Logger { get; set; }
        [Column("machine_name")] public string MachineName { get; set; }
        [Column("message")] public string Message { get; set; }
        [Column("app_domain")] public string AppDomain { get; set; }
        [Column("assembly_version")] public string AssemblyVersion { get; set; }
        [Column("base_dir")] public string BaseDir { get; set; }
        [Column("call_site")] public string CallSite { get; set; }
        [Column("call_site_line_number")] public string CallSiteLineNumber { get; set; }
        [Column("stacktrace")] public string Stacktrace { get; set; }
        [Column("exception_string")] public string ExceptionString { get; set; }
        [Column("log_date")] public DateTime LogDate { get; set; }
    }
}