using Microsoft.VisualStudio.TestTools.UnitTesting;
using touha.infrastructure.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace touha.infrastructure.db.Tests
{
    [TestClass()]
    public class OracleDbHelperTests
    {
        /// <summary>
        /// 返回分页DataTable测试
        /// </summary>
        [TestMethod()]
        public void ExecuteTest()
        {
            var db = new OracleDbHelper();
            DataTable dt = db.Execute("ktoes_bs_item", 1, 5, null);
            Assert.AreEqual(5, dt.Rows.Count);
            Assert.AreEqual(dt.Rows[0]["ID"].ToString(), "791");
        }

        /// <summary>
        /// 返回分页DbDataReader测试
        /// </summary>
        [TestMethod()]
        public void ExecuteReaderTest()
        {
            var db = new OracleDbHelper();
            DbDataReader reader = db.ExecuteReader("ktoes_bs_item", 1, 5, null);
            if (reader.Read())
            {
                Assert.AreEqual(reader.GetInt32(1), 791);
            }
        }

        
    }
}