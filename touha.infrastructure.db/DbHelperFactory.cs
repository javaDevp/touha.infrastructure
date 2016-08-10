using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touha.infrastructure.db
{
    /// <summary>
    /// 数据库工具工厂类
    /// </summary>
    public class DbHelperFactory
    {
        /// <summary>
        /// 根据配置文件，获取相应的DbHelper
        /// </summary>
        /// <returns></returns>
        public static DbHelper GetDbHelper()
        {
            DbHelper db = null;
            string dbType = ConfigurationManager.AppSettings["dbType"] ?? "Oracle";
            Type type = typeof(DbTypeEnum);
            //判断配置的数据库类型是否符合条件
            foreach(string ele in Enum.GetNames(type))
            {
                if(dbType.Equals(ele, StringComparison.OrdinalIgnoreCase))
                {
                    db = CreateDbHelper(ele);
                    break;
                }
            }
            return db;
        }

        /// <summary>
        /// 反射创建于dbTypeName对应的DbHelper
        /// </summary>
        /// <param name="dbTypeName"></param>
        /// <returns></returns>
        private static DbHelper CreateDbHelper(string dbTypeName)
        {
            return (DbHelper)Activator.CreateInstance(Type.GetType(string.Format("touha.infrastructure.db.{0}DbHelper, touha.infrastructure.db", dbTypeName)));
        }
    }
}
