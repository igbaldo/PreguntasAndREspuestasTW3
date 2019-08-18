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
    public class PreguntaRepository : IBaseRepository<Pregunta>
    {
        public IEnumerable<Pregunta> GetAll()
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Pregunta
                            .Include("Clase")
                            .Include("Tema")
                            .Include("RespuestaAlumno")
                        .ToList();
            }
        }

        public Pregunta GetById(int id)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Pregunta
                        .Include("Clase")
                        .Include("Tema")
                        .Include("RespuestaAlumno")
                    .FirstOrDefault(x => x.IdPregunta == id);
            }
        }

        public int Insert(Pregunta entity)
        {
            using (var context = new TP_20191CEntities())
            {
                entity.IdProfesorCreacion = UserCache.IdUsuario;
                entity.FechaHoraCreacion = DateTime.Now;
                entity.IdProfesorModificacion = UserCache.IdUsuario;
                entity.FechaHoraModificacion = DateTime.Now;

                context.Pregunta.Add(entity);

                context.SaveChanges();

                return entity.IdPregunta;
            }
        }

        public void Update(Pregunta entity)
        {
            using (var context = new TP_20191CEntities())
            {
                Pregunta preguntaToUpdate =
                    context.Pregunta.FirstOrDefault(x => x.IdPregunta == entity.IdPregunta);

                if (preguntaToUpdate != null)
                {
                    preguntaToUpdate.IdProfesorModificacion = UserCache.IdUsuario;
                    preguntaToUpdate.FechaHoraModificacion = DateTime.Now;

                    preguntaToUpdate.Nro = entity.Nro;
                    preguntaToUpdate.IdClase = entity.IdClase;
                    preguntaToUpdate.IdTema = entity.IdTema;

                    preguntaToUpdate.FechaDisponibleDesde = entity.FechaDisponibleDesde;
                    preguntaToUpdate.FechaDisponibleHasta = entity.FechaDisponibleHasta;

                    preguntaToUpdate.Pregunta1 = entity.Pregunta1;

                    entity.Profesor = null;
                    entity.Profesor = null;
                    entity.Clase = null;
                    entity.Tema = null;

                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new TP_20191CEntities())
            {
                Pregunta preguntaToDelete = context.Pregunta.FirstOrDefault(x => x.IdPregunta == id);

                if (preguntaToDelete != null)
                {
                    context.Pregunta.Remove(preguntaToDelete);

                    context.SaveChanges();
                }
            }
        }

        public int GetProxNroPregunta()
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Pregunta.Max(x => x.Nro) + 1;
            }
        }

        public List<Pregunta> GetUltimasPreguntasCorregidas()
        {
            using (var context = new TP_20191CEntities())
            {
                DateTime fechaHoraActual = DateTime.Now;

                return context.Pregunta
                            .Include("RespuestaAlumno")
                            .Include("RespuestaAlumno.Alumno")
                        .Where(x => x.RespuestaAlumno.Any(m => m.IdResultadoEvaluacion != null) && x.FechaDisponibleHasta < fechaHoraActual)
                        .OrderByDescending(y => y.Nro)
                        .Take(2)
                        .ToList();
            }
        }

        public List<Pregunta> GetPreguntasSinResponder()
        {
            using (var context = new TP_20191CEntities())
            {
                int idAlumnoLogueado = UserCache.IdUsuario;
                DateTime fechaActual = DateTime.Now;

                return context.Pregunta
                        .Include("RespuestaAlumno")
                    .Where(x => !x.RespuestaAlumno.Any(y => y.IdAlumno == idAlumnoLogueado) 
                                && ((x.FechaDisponibleHasta == null || x.FechaDisponibleHasta >= fechaActual)
                                && (x.FechaDisponibleDesde == null || x.FechaDisponibleDesde <= fechaActual)))
                    .OrderByDescending(y => y.Nro)
                    .ToList();
            }
        }

        public IEnumerable<Pregunta> GetAllRespuestasAlumno(int? idPregunta = 0)
        {
            using (var context = new TP_20191CEntities())
            {
                DateTime fechaActual = DateTime.Now;

                return context.Pregunta
                        .Include("Clase")
                        .Include("Tema")
                        .Include("RespuestaAlumno")
                        .Include("RespuestaAlumno.Alumno")
                        .Where(x => (idPregunta == 0 || x.IdPregunta == idPregunta)
                                    && 
                                    (
                                        (x.FechaDisponibleHasta == null && x.FechaDisponibleDesde == null)
                                        ||
                                        (x.FechaDisponibleDesde <= fechaActual)
                                    ))
                    .ToList();
            }
        }

        public List<Pregunta> GetSinCorregir(int? idPregunta = 0)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Pregunta
                        .Include("Clase")
                        .Include("Tema")
                        .Include("RespuestaAlumno")
                        .Include("RespuestaAlumno.Alumno")
                    .Where(x => x.RespuestaAlumno.Any(m => m.IdAlumno == UserCache.IdUsuario 
                                                        && m.IdResultadoEvaluacion == null)
                                && (idPregunta == 0 || x.IdPregunta == idPregunta))
                    .OrderByDescending(y => y.Nro)
                    .ToList();
            }
        }

        public List<Pregunta> GetByEstadoCorreccion(EnumEstadoPreguntaFiltro estadoCorreccion, int? idPregunta = 0)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Pregunta
                        .Include("Clase")
                        .Include("Tema")
                        .Include("RespuestaAlumno")
                        .Include("RespuestaAlumno.Alumno")
                        .Where(x => x.RespuestaAlumno.Any(m => m.IdAlumno == UserCache.IdUsuario 
                                                        && m.IdResultadoEvaluacion != null 
                                                        && m.IdResultadoEvaluacion == (int)estadoCorreccion)
                                    && (idPregunta == 0 || x.IdPregunta == idPregunta))
                    .OrderByDescending(y => y.Nro)
                    .ToList();
            }
        }

        public Pregunta GetByNroPregunta(int preguntaNro)
        {
            using (var context = new TP_20191CEntities())
            {
                return context.Pregunta.FirstOrDefault(x => x.Nro == preguntaNro);
            }
        }
    }
}