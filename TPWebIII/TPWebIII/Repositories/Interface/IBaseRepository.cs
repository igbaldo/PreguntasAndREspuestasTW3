using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPWebIII.Repositories.Interface
{
    public interface IBaseRepository<T> : IGetterRepository<T>
    {
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}