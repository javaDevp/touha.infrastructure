using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touha.infrastructure.db
{
    /// <summary>
    /// 数据库工具基类
    /// </summary>
    public abstract class DbHelper : IDisposable
    {
        #region 字段
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected DbConnection connection;

        /// <summary>
        /// 事务
        /// </summary>
        protected DbTransaction transaction;

        /// <summary>
        /// 适配器
        /// </summary>
        protected DbDataAdapter adapter;

        /// <summary>
        /// 工厂
        /// </summary>
        protected DbProviderFactory provider;

        protected bool isDisposed;

        /// <summary>
        /// 参数符号
        /// </summary>
        protected string pSymbol;

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string connectString;
        #endregion

        #region 属性
        /// <summary>
        /// 数据库连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (this.connection == null)
                {
                    this.connection = this.provider.CreateConnection();
                    this.connection.ConnectionString = this.connectString;
                }
                if (this.connection.State != System.Data.ConnectionState.Open)
                {
                    this.connection.Open();
                }
                return this.connection;
            }
        }

        /// <summary>
        /// 适配器
        /// </summary>
        public DbDataAdapter Adapter
        {
            get
            {
                if (this.adapter == null)
                {
                    this.adapter = this.provider.CreateDataAdapter();
                }
                return this.adapter;
            }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public DbProviderFactory Provider
        {
            get
            {
                return this.provider;
            }
        }
        #endregion

        #region 构造函数
        public DbHelper()
        {
            provider = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["touhadb"].ProviderName);
            connectString = ConfigurationManager.ConnectionStrings["touhadb"].ConnectionString;
        }
        #endregion

        #region 事务
        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            this.transaction = this.Connection.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            this.transaction.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            this.transaction.Rollback();
        }
        #endregion

        #region 执行sql或存储过程，返回影响行数
        /// <summary>
        /// 执行sql或存储过程，返回影响函数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, IDictionary<string, DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            using (DbCommand cmd = CreateCommand(commandText, commandType, parameters))
            {
                return ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 执行cmd，返回影响行数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(DbCommand cmd)
        {
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 执行sql或存储过程，返回DataReader
        /// <summary>
        /// 执行sql或存储过程，返回DataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, IDictionary<string, DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            using (DbCommand cmd = CreateCommand(commandText, commandType, parameters))
            {
                return ExecuteReader(cmd);
            }
        }

        /// <summary>
        /// 执行command，返回DataReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private DbDataReader ExecuteReader(DbCommand cmd)
        {
            try
            {
                return cmd.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 执行sql或存储过程，返回object
        /// <summary>
        /// 执行sql或存储过程，返回object
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, IDictionary<string, DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            using (DbCommand cmd = CreateCommand(commandText, commandType, parameters))
            {
                return ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// 执行command，返回object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private object ExecuteScalar(DbCommand cmd)
        {
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 执行sql或存储过程，返回DataTable
        /// <summary>
        /// 执行sql或存储过程，返回DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable Execute(string commandText, IDictionary<string, DbParam> parameters, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (DbCommand cmd = CreateCommand(commandText, commandType, parameters))
                {
                    return Execute(cmd);                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行command，返回DataTable
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private DataTable Execute(DbCommand cmd)
        {
            try
            {
                this.Adapter.SelectCommand = cmd;
                var dt = new DataTable();
                this.Adapter.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 分页
        public abstract DataTable Execute(string sql, int start, int limit, IDictionary<string, DbParam> parameters);

        public abstract DbDataReader ExecuteReader(string sql, int start, int limit, IDictionary<string, DbParam> parameters);
        #endregion

        #region 辅助函数
        /// <summary>
        /// 创建Command对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        private DbCommand CreateCommand(string commandText, CommandType commandType, IDictionary<string, DbParam> parameters)
        {
            DbCommand cmd = this.Connection.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            if(parameters != null)
            {
                foreach (string parameterName in parameters.Keys)
                {
                    DbParameter parameter = this.provider.CreateParameter();
                    parameter.ParameterName = parameterName;
                    parameter.Value = parameters[parameterName].Value;
                    parameter.Direction = parameters[parameterName].Direction;
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd;
        }

        /// <summary>
        /// 创建参数对象
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private DbParameter CreateParameter(string parameterName, object value,  DbType dbType, ParameterDirection direction)
        {
            DbParameter parameter = this.provider.CreateParameter();
            parameter.DbType = dbType;
            parameter.Direction = direction;
            if (parameterName.StartsWith(pSymbol))
            {
                parameter.ParameterName = parameterName;
            }
            else
            {
                parameter.ParameterName = pSymbol + parameterName;
            }
            parameter.Value = value;
            return parameter;
        }
        #endregion

        #region IDispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    if (this.connection != null)
                    {
                        if (this.connection.State == System.Data.ConnectionState.Open)
                        {
                            this.connection.Close();
                        }
                    }
                    if (this.transaction != null)
                    {
                        this.transaction.Dispose();
                    }
                }
            }
            this.isDisposed = true;
        }
        #endregion
    }
}
