using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XZMHui.Core.Attributes;
using XZMHui.Utils.Extensions;

namespace XZMHui.Repository
{
    [SkipInject]
    public class ModelRepositoryBase : XZMHui.IRepository.IModelRepository
    {
        public virtual MyDbContext DbContext { get => throw null; }

        public virtual ILogger Logger { get => throw null; }

        public virtual IQueryable<T> GetList<T>(string sql, params object[] parameters) where T : class, new()
        {
            var sqlStr = $"select * from ({sql}) as __ExecuteNonQuery__";
            var conn = DbContext.Database.GetDbConnection() as MySqlConnection;
            try
            {
                using var command = new MySqlCommand();
                if (conn.State != ConnectionState.Open) conn.Open();
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlStr;
                InitParameters(command, parameters);

                Logger?.LogInformation(sqlStr, parameters);
                using var DataAdapter = new MySqlDataAdapter
                {
                    SelectCommand = command
                };
                var ds = new DataSet();
                DataAdapter.Fill(ds);
                return ds.Tables[0].SerializeToObject<T>(false).AsQueryable();
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.Message);
                return default;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public virtual (IQueryable<T> List, long Rows) GetPagedList<T>(string sql, int pageIndex, int pageSize, string ordering, params object[] parameters) where T : class, new()
        {
            if (string.IsNullOrEmpty(ordering)) ordering = "1";

            string sqlStr = $"select SQL_CALC_FOUND_ROWS * from ({sql}) as __ExecuteNonQuery__ order by {ordering} limit {pageSize} offset {(pageIndex - 1) * pageSize}; select FOUND_ROWS() as totalRecords;";

            var conn = DbContext.Database.GetDbConnection() as MySqlConnection;
            try
            {
                using var command = new MySqlCommand();
                if (conn.State != ConnectionState.Open) conn.Open();
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlStr;
                InitParameters(command, parameters);

                Logger?.LogInformation(sql, parameters);
                using var DataAdapter = new MySqlDataAdapter
                {
                    SelectCommand = command
                };
                var ds = new DataSet();
                DataAdapter.Fill(ds);
                return (ds.Tables[0].SerializeToObject<T>(false).AsQueryable(), Convert.ToInt64(ds.Tables[1].Rows[0]["totalRecords"]));
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.Message);
                return default;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public virtual long GetRecordCount(string sql, params object[] parameters)
        {
            var sqlStr = $"select count(1) from ({sql}) as __ExecuteNonQuery__";
            var conn = DbContext.Database.GetDbConnection() as MySqlConnection;
            try
            {
                using var command = new MySqlCommand();
                if (conn.State != ConnectionState.Open) conn.Open();
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.CommandText = sqlStr;
                InitParameters(command, parameters);

                Logger?.LogInformation(sql, parameters);
                return (long)(command.ExecuteScalar() ?? 0);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.Message);
                return default;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void InitParameters(MySqlCommand cmd, object[] param)
        {
            if (param == null || param.Length == 0)
                return;
            cmd.Parameters.Clear();
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] is KeyValuePair<string, object>)
                {
                    var p = (KeyValuePair<string, object>)param[i];
                    cmd.Parameters.AddWithValue($"@{p.Key.TrimStart('@')}", p.Value);
                }
                else
                    cmd.Parameters.AddWithValue($"@{i}", param[i]);
            }
        }
    }
}