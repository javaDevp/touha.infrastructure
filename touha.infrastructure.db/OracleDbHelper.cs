using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touha.infrastructure.db
{
    /// <summary>
    /// Oracle数据库工具类
    /// </summary>
    public class OracleDbHelper : DbHelper
    {
        #region 构造方法
        public OracleDbHelper()
            : base()
        {
            pSymbol = ":";
        }
        #endregion

        #region 重写分页查询
        public override System.Data.DataTable Execute(string sql, int start, int limit, IDictionary<string, DbParam> parameters)
        {
            var pageSql = BuilderPageSql(sql);
            BuildPageParameters(start, limit, ref parameters);
            return base.Execute(pageSql.ToString(), parameters);
        }

        public override System.Data.Common.DbDataReader ExecuteReader(string sql, int start, int limit, IDictionary<string, DbParam> parameters)
        {
            var pageSql = BuilderPageSql(sql);
            BuildPageParameters(start, limit, ref parameters);
            return base.ExecuteReader(pageSql.ToString(), parameters);
        }
        #endregion

        #region 分页辅助方法
        /// <summary>
        /// 构建分页sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private StringBuilder BuilderPageSql(string sql)
        {
            var pageSql = new StringBuilder();
            pageSql.AppendFormat("select * from (select rownum rn, t.* from ({0}) t where rownum <= :endrow) where rn >= :startrow", sql);
            return pageSql;
        }

        /// <summary>
        /// 构建分页参数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="parameters"></param>
        private void BuildPageParameters(int start, int limit, ref IDictionary<string, DbParam> parameters)
        {
            if (parameters == null)
                parameters = new Dictionary<string, DbParam>();
            parameters.Add("startrow", new DbParam { Direction = ParameterDirection.Input, Value = start });
            int end;
            if (start > 0)
                end = start + limit - 1;
            else
                end = limit;
            parameters.Add("endrow", new DbParam { Direction = ParameterDirection.Input, Value = end });
        }
        #endregion
    }
}
