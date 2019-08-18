using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models;
using TPWebIII.Repositories.Interface;

namespace TPWebIII.Repositories
{
    public class TemaRepository : IGetterRepository<Tema>
    {
        public IEnumerable<Tema> GetAll()
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Tema
                    .ToList();
            }
        }

        #region NotImplementedMembers

        public Tema GetById(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}