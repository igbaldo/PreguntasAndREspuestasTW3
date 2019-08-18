using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Repositories.Interface;

namespace TPWebIII.Repositories
{
    public class AlumnoRepository : IGetterRepository<Alumno>
    {
        public IEnumerable<Alumno> GetAll()
        {
            using (var context = new TP_20191CEntities())
            {
                //string encodePassword = Sha1.Encode(loginWrapper.Password);

                return context.Alumno.Where(x => x.Habilitado);
            }
        }

        public Alumno GetById(int id)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Alumno.FirstOrDefault(x => x.Habilitado && x.IdAlumno == id);
            }
        }

        public Alumno GetByLogin(LoginWrapper loginWrapper)
        {
            using (var context = new TP_20191CEntities())
            {
                //string encodePassword = Sha1.Encode(loginWrapper.Password);

                return context.Alumno.FirstOrDefault(x => x.Habilitado && x.Email == loginWrapper.Email 
                                                        && x.Password == loginWrapper.Password);
            }
        }

        public List<Alumno> GetRanking()
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Alumno
                        .OrderByDescending(x => x.PuntosTotales)
                        .ThenByDescending(x => x.CantidadRespuestasCorrectas)
                        .ThenByDescending(x => x.CantidadMejorRespuesta)
                        .Take(12)
                        .ToList();
            }
        }
    }
}