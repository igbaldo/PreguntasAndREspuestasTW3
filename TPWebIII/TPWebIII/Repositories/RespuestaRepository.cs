using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Helpers;
using TPWebIII.Models;
using TPWebIII.Models.Enums;
using TPWebIII.Repositories.Interface;

namespace TPWebIII.Repositories
{
    public class RespuestaRepository : IBaseRepository<RespuestaAlumno>
    {
        public int Insert(RespuestaAlumno entity)
        {
            using (var context = new TP_20191CEntities())
            {
                int cantidadRespuestasExistentes = context.RespuestaAlumno.Count(x => x.IdPregunta == entity.IdPregunta);

                entity.Orden = cantidadRespuestasExistentes + 1;

                context.RespuestaAlumno.Add(entity);

                context.SaveChanges();

                return entity.Orden;
            }
        }

        #region NotImplementedMembers

        public void Update(RespuestaAlumno entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RespuestaAlumno> GetAll()
        {
            throw new NotImplementedException();
        }

        public RespuestaAlumno GetById(int id)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.RespuestaAlumno
                                .Include("Pregunta")
                            .FirstOrDefault(x => x.IdRespuestaAlumno == id);
            }
        }

        #endregion

        public int GetByIdPregunta(int idPregunta)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.RespuestaAlumno.Count(x =>
                    x.IdPregunta == idPregunta 
                    && x.IdResultadoEvaluacion == (int) EnumEstadoPreguntaFiltro.Correcta);
            }
        }

        public void UpdateResultadoEval(RespuestaAlumno respuestaAlumno)
        {
            using (var context = new TP_20191CEntities())
            {
                RespuestaAlumno respuestaAlumnoToUpdate = context.RespuestaAlumno.FirstOrDefault(x => x.IdRespuestaAlumno == respuestaAlumno.IdRespuestaAlumno);

                if (respuestaAlumnoToUpdate != null)
                {
                    respuestaAlumnoToUpdate.IdProfesorEvaluador = respuestaAlumno.IdProfesorEvaluador;
                    respuestaAlumnoToUpdate.FechaHoraEvaluacion = respuestaAlumno.FechaHoraEvaluacion;
                    respuestaAlumnoToUpdate.IdResultadoEvaluacion = respuestaAlumno.IdResultadoEvaluacion;
                    respuestaAlumnoToUpdate.RespuestasCorrectasHastaElMomento = respuestaAlumno.RespuestasCorrectasHastaElMomento;
                    respuestaAlumnoToUpdate.Puntos = respuestaAlumno.Puntos;
                    respuestaAlumnoToUpdate.MejorRespuesta = respuestaAlumno.MejorRespuesta;

                    respuestaAlumnoToUpdate.Alumno = null;
                    respuestaAlumnoToUpdate.Pregunta = null;
                    respuestaAlumnoToUpdate.Profesor = null;
                    respuestaAlumnoToUpdate.ResultadoEvaluacion = null;

                    context.SaveChanges();
                }
            }
        }

        public void SetMejorRespuesta(int idRespuesta, long puntajeMejorRespuesta)
        {
            using (var context = new TP_20191CEntities())
            {
                RespuestaAlumno respuestaAlumnoToUpdate = context.RespuestaAlumno.FirstOrDefault(x => x.IdRespuestaAlumno == idRespuesta);

                if (respuestaAlumnoToUpdate != null)
                {
                    respuestaAlumnoToUpdate.IdProfesorEvaluador = UserCache.IdUsuario;
                    respuestaAlumnoToUpdate.FechaHoraEvaluacion = DateTime.Now;
                    respuestaAlumnoToUpdate.Puntos = respuestaAlumnoToUpdate.Puntos + puntajeMejorRespuesta;
                    respuestaAlumnoToUpdate.MejorRespuesta = true;

                    respuestaAlumnoToUpdate.Alumno = null;
                    respuestaAlumnoToUpdate.Pregunta = null;
                    respuestaAlumnoToUpdate.Profesor = null;
                    respuestaAlumnoToUpdate.ResultadoEvaluacion = null;

                    context.SaveChanges();
                }
            }
        }
    }
}