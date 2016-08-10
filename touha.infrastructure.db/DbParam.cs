using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touha.infrastructure.db
{
    /// <summary>
    /// 参数实体类
    /// </summary>
    public class DbParam
    {
        /// <summary>
        /// 参数方向
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }
    }
}
