using Microsoft.VisualStudio.TestTools.UnitTesting;
using touha.infrastructure.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace touha.infrastructure.db.Tests
{
    [TestClass()]
    public class DbHelperTests
    {
        /// <summary>
        /// 返回object测试
        /// </summary>
        [TestMethod()]
        public void ExecuteScalarTest()
        {
            var db = new OracleDbHelper();
            object obj = db.ExecuteScalar("select count(1) from ktoes_bs_item", null);
            int count = Convert.ToInt32(obj);
            Assert.AreEqual(count, 5041);
            //Assert.Fail();
        }

        [TestMethod()]
        public void MyTest()
        {
            var db = new OracleDbHelper();
            var sql1 = @"SELECT B.*, A.MUTE_REMARKS_TYPE, A.REMARKS
  FROM KTOES_BS_MUtEXGROUP A, KTOES_BS_ITEM_MUTEX B
 WHERE A.ID = B.MUTEX_CODE AND A.MUTE_TYPE = 2";
            DataTable dt1 = db.Execute(sql1, null);
            
            var sql2 = @"SELECT a.item_id,
       b.item_code,
       b.item_name,
       a.itemgroup_id,
       c.code itemgroup_code,
       a.order_date,
       a.timespan1,
       a.timespan2,
       a.ORG_ITEM_MNO
  FROM ktoes_yy_record a, ktoes_bs_item b, ktoes_bs_itemgroup c
 WHERE a.item_id = b.id AND a.itemgroup_id = c.id
    and a.request_no = :requestno and a.record_state in ('SQ', 'CG')";
            var parameters = new Dictionary<string, DbParam>();
            parameters.Add("requestno", new DbParam{Value = 201604261000002});
            DataTable dt2 = db.Execute(sql2, parameters);

            var datas = from r2 in dt2.AsEnumerable()
                        join r1 in dt1.AsEnumerable().Where(r => r.Field<decimal>("LEVELNO") == 1)
                        on new { orgCode = "41961191", itemCode = r2.Field<string>("ITEM_CODE") } equals new { orgCode = r1.Field<string>("ORG_CODE"), itemCode = r1.Field<string>("ITEM_CODE") }
                        select r1;
            Assert.IsTrue(datas.Count() > 0);
            if (datas.Count() > 0)
            {
                //带关联项目
                //var datas2 = from r1 in dt.AsEnumerable()
                //             join d in datas
                //             on r1.Field<string>("MUTEX_CODE") equals d.Field<string>("MUTEX_CODE")
                //             where r1.Field<string>("ITEM_CODE") != d.Field<string>("ITEM_CODE")
                //             select r1;
                //if (datas2.Count() > 0)
                //{
                //存在依赖项?多个依赖，消息类型不一致怎么办。。。
                //IDictionary<string, object> itemsDic = new Dictionary<string, object>();
                foreach (var item in datas)
                {
                    //遍历申请单存在关联关系的项目，找到其所关联的项目列表
                    var datas2 = from r in dt1.AsEnumerable()
                                 where r.Field<string>("MUTEX_CODE") == item.Field<string>("MUTEX_CODE")
                              && r.Field<string>("ITEM_CODE") != item.Field<string>("ITEM_CODE")
                                 select r;
                    Assert.IsTrue(datas2.Count() > 0);
                }
                
            }
            ////所有与申请单关联的关联组信息
            //var datas = from r2 in dt2.AsEnumerable()
            //            join r1 in dt1.AsEnumerable().Where(r => r.Field<decimal>("LEVELNO") == 1 && r.Field<string>("ORG_CODE") == "41961191")
            //            on r2.Field<string>("ITEM_CODE") equals r1.Field<string>("ITEM_CODE")
            //            select r1;
            //foreach (var item1 in datas)
            //{
            //    Assert.AreEqual(item1["ITEM_CODE"].ToString(), "1405");
            //}
            ////带关联项目
            //var datas2 = from r1 in dt1.AsEnumerable()
            //             join d in datas
            //             on r1.Field<string>("MUTEX_CODE") equals d.Field<string>("MUTEX_CODE")
            //             where r1.Field<string>("ITEM_CODE") != d.Field<string>("ITEM_CODE")
            //             select r1;
            //foreach (var item2 in datas2)
            //{
            //    Assert.AreEqual(item2["ITEM_CODE"].ToString(), "1117");
            //}
           // Assert.IsTrue(datas2.Count() > 0);

            Assert.IsTrue(dt1.Rows.Count > 0 && dt2.Rows.Count > 0);
           // Console.WriteLine
        }
    }
}