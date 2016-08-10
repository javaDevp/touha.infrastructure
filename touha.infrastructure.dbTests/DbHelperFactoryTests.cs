using Microsoft.VisualStudio.TestTools.UnitTesting;
using touha.infrastructure.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touha.infrastructure.db.Tests
{
    [TestClass()]
    public class DbHelperFactoryTests
    {
        /// <summary>
        /// 测试DbHelperFactory
        /// </summary>
        [TestMethod()]
        public void GetDbHelperTest()
        {
            var db =  DbHelperFactory.GetDbHelper();
            Assert.IsInstanceOfType(db, typeof(OracleDbHelper));
        }
    }
}