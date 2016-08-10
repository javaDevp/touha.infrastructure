using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using touha.infrastructure.db;
using touha.infrastructure.model;

namespace touha.infrasturcture.repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : EntityObject
    {
        protected DbHelper db;

        public IQueryable<T> GetList(T entity)
        {
            db = new OracleDbHelper();
            return null;
        }

        public int Update(T entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
