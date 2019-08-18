using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models;
using TPWebIII.Repositories.Interface;

namespace TPWebIII.Repositories
{
    public class ClaseRepository : IGetterRepository<Clase>
    {
        public IEnumerable<Clase> GetAll()
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Clase
                    .ToList();
            }
        }

        #region NotImplementedMembers

        public Clase GetById(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Clase GetUltimaClase()
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Clase.OrderByDescending(x => x.IdClase).FirstOrDefault();
            }
        }
    }
}