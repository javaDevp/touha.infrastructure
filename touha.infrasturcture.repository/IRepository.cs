using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace touha.infrasturcture.repository
{
    /// <summary>
    /// 数据仓库接口
    /// </summary>
    /// <typeparam name="T">操作实体类型</typeparam>
    interface IRepository<T>
    {
        /// <summary>
        /// 获取满足条件列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IQueryable<T> GetList(T entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(T entity);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(string id);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsExist(string id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetList();
    }
}
