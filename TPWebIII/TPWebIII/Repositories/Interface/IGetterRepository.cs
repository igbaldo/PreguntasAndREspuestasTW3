using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPWebIII.Repositories.Interface
{
    public interface IGetterRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }
}