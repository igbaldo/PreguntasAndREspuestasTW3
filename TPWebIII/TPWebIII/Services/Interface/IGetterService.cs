using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPWebIII.Services.Interface
{
    public interface IGetterService<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }
}