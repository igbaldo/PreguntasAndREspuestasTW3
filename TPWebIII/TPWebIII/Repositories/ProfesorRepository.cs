using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Repositories.Interface;

namespace TPWebIII.Repositories
{
    public class ProfesorRepository : IGetterRepository<Profesor>
    {
        public IEnumerable<Profesor> GetAll()
        {
            using (var context = new TP_20191CEntities())
            {
                //string encodePassword = Sha1.Encode(loginWrapper.Password);

                return context.Profesor.Where(x => x.Habilitado);
            }
        }

        public Profesor GetById(int id)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Profesor.FirstOrDefault(x => x.Habilitado && x.IdProfesor == id);
            }
        }

        public Profesor GetByLogin(LoginWrapper loginWrapper)
        {
            using (var context = new TP_20191CEntities())
            {
                //string encodePassword = Sha1.Encode(loginWrapper.Password);

                return context.Profesor.FirstOrDefault(x => x.Habilitado && x.Email == loginWrapper.Email 
                                                        && x.Password == loginWrapper.Password);
            }
        }
    }
}