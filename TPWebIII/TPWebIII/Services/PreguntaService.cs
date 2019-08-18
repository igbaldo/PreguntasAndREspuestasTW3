using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Helpers;
using TPWebIII.Models;
using TPWebIII.Models.Enums;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Repositories;
using TPWebIII.Services.Interface;

namespace TPWebIII.Services
{
    public class PreguntaService : IBaseService<Pregunta>
    {
        #region Constructor

        public PreguntaService()
        {
            this.PreguntaRepository = new PreguntaRepository();
        }

        #endregion

        #region Properties

        private PreguntaRepository PreguntaRepository { get; set; }

        #endregion

        public IEnumerable<Pregunta> GetAll()
        {
            return this.PreguntaRepository.GetAll();
        }

        public Pregunta GetById(int id)
        {
            return this.PreguntaRepository.GetById(id);
        }

        public int Insert(Pregunta entity)
        {
            return this.PreguntaRepository.Insert(entity);
        }

        public void Update(Pregunta entity)
        {
            this.PreguntaRepository.Update(entity);
        }

        public void Delete(int id)
        {
            this.PreguntaRepository.Delete(id);
        }

        public int GetProxNroPregunta()
        {
            return this.PreguntaRepository.GetProxNroPregunta();
        }

        public List<UltimaPreguntaHomeWrapper> GetUltimasPreguntasCorregidas()
        {
            var ultimasPreguntasCorregidasWrappers = new List<UltimaPreguntaHomeWrapper>();

            List<Pregunta> ultimasPreguntasCorregidas = this.PreguntaRepository.GetUltimasPreguntasCorregidas();

            foreach (Pregunta pregunta in ultimasPreguntasCorregidas)
            {
                var ultimaPreguntaHomeWrapper = new UltimaPreguntaHomeWrapper
                {
                    IdPregunta = pregunta.IdPregunta,
                    Pregunta = string.Format("{0}. {1}", pregunta.IdPregunta, pregunta.Pregunta1),
                    RankingPregunta = new List<UltimaPreguntaHomeRankingWrapper>()
                };

                List<RespuestaAlumno> respuestaAlumnoRanking = pregunta.RespuestaAlumno
                                                    .Where(x => x.Puntos > 0)
                                                    .OrderByDescending(m => m.Puntos)
                                                    .Take(12)
                                                    .ToList();

                foreach (RespuestaAlumno respuestaAlumno in respuestaAlumnoRanking)
                {
                    ultimaPreguntaHomeWrapper.RankingPregunta.Add(new UltimaPreguntaHomeRankingWrapper
                    {
                        Alumno = string.Format("{0}. {1} {2}", respuestaAlumnoRanking.IndexOf(respuestaAlumno) + 1, respuestaAlumno.Alumno.Nombre, respuestaAlumno.Alumno.Apellido),
                        Puntos = Convert.ToInt32(respuestaAlumno.Puntos),
                        MejorRespuesta = respuestaAlumno.MejorRespuesta
                    });
                }

                ultimasPreguntasCorregidasWrappers.Add(ultimaPreguntaHomeWrapper);
            }

            return ultimasPreguntasCorregidasWrappers;
        }

        public List<Pregunta> GetPreguntasSinResponder()
        {
            return this.PreguntaRepository.GetPreguntasSinResponder();
        }

        public IEnumerable<PreguntaWrapper> GetPreguntasAlumnoByEstado(EnumEstadoPreguntaFiltro estadoPreguntas)
        {
            var preguntasAll = new List<PreguntaWrapper>();

            List<Pregunta> preguntas = this.GetPreguntasByEstado((EnumEstadoPreguntaFiltro)estadoPreguntas).ToList();

            foreach (Pregunta pregunta in preguntas)
            {
                var preguntaWrapper = GetPreguntaWrapperByPregunta(pregunta);

                preguntasAll.Add(preguntaWrapper);
            }

            return preguntasAll.OrderByDescending(x => x.Nro);
        }

        public PreguntaWrapper GetPreguntaWrapperByPregunta(Pregunta pregunta)
        {
            var preguntaWrapper = new PreguntaWrapper
            {
                Clase = pregunta.Clase != null ? pregunta.Clase.Nombre : string.Empty,
                Tema = pregunta.Tema != null ? pregunta.Tema.Nombre : string.Empty,
                IdPregunta = pregunta.IdPregunta,
                Pregunta = pregunta.Pregunta1,
                Nro = pregunta.Nro,
                DisponibleDesde = pregunta.FechaDisponibleDesde,
                DisponibleHasta = pregunta.FechaDisponibleHasta,
                YaRespondida = pregunta.RespuestaAlumno.Any(x => x.IdAlumno == UserCache.IdUsuario),
                PlazoVencido = pregunta.FechaDisponibleHasta < DateTime.Now
            };

            if (preguntaWrapper.YaRespondida)
            {
                RespuestaAlumno respuestaAlumno = pregunta.RespuestaAlumno.FirstOrDefault(x => x.IdAlumno == UserCache.IdUsuario);

                if (respuestaAlumno != null)
                {
                    switch (respuestaAlumno.IdResultadoEvaluacion)
                    {
                        case (int) EnumEstadoPreguntaFiltro.Correcta:
                            preguntaWrapper.ResultadoCorreccion = string.Format("Correcta | {0}° | {1}pts ", respuestaAlumno.Orden, respuestaAlumno.Puntos);
                            if (respuestaAlumno.MejorRespuesta)
                                preguntaWrapper.ResultadoCorreccion = string.Format("{0} | ¡Mejor Respuesta!", preguntaWrapper.ResultadoCorreccion);
                            break;
                        case (int) EnumEstadoPreguntaFiltro.Regular:
                            preguntaWrapper.ResultadoCorreccion = string.Format("Regular | {0}° | {1}pts", respuestaAlumno.Orden, respuestaAlumno.Puntos);
                            break;
                        case (int) EnumEstadoPreguntaFiltro.Mal:
                            preguntaWrapper.ResultadoCorreccion = "Mal";
                            break;
                        default:
                            preguntaWrapper.ResultadoCorreccion = "Corrección Pendiente";
                            break;
                    }

                    if (respuestaAlumno.IdResultadoEvaluacion != null)
                        preguntaWrapper.EstadoCorreccion = (EnumEstadoPreguntaFiltro) respuestaAlumno.IdResultadoEvaluacion;

                    preguntaWrapper.Respuesta = respuestaAlumno.Respuesta;
                }
            }

            return preguntaWrapper;
        }

        public RespuestaEvaluarWrapper GetRespuestaWrapperByRespuesta(RespuestaAlumno respuesta)
        {
            var respuestaWrapper = new RespuestaEvaluarWrapper
            {
                IdRespuestaAlumno = respuesta.IdRespuestaAlumno,
                Alumno = string.Format("{0} {1}", respuesta.Alumno.Nombre, respuesta.Alumno.Apellido),
                FechaHoraRespuesta = respuesta.FechaHoraRespuesta,
                Respuesta = respuesta.Respuesta,
                MejorRespuesta = respuesta.MejorRespuesta,
                EstadoCorreccion = respuesta.IdResultadoEvaluacion != null ? (EnumEstadoPreguntaFiltro)respuesta.IdResultadoEvaluacion : (EnumEstadoPreguntaFiltro?) null
            };

            return respuestaWrapper;
        }

        #region HelperMethods

        private List<Pregunta> GetPreguntasByEstado(EnumEstadoPreguntaFiltro estadoPreguntas, int? idPregunta = 0)
        {
            switch (estadoPreguntas)
            {
                case EnumEstadoPreguntaFiltro.Todas:
                    return this.PreguntaRepository.GetAllRespuestasAlumno(idPregunta).ToList();
                case EnumEstadoPreguntaFiltro.SinCorregir:
                    return this.PreguntaRepository.GetSinCorregir(idPregunta).ToList();
                case EnumEstadoPreguntaFiltro.Correcta:
                case EnumEstadoPreguntaFiltro.Regular:
                case EnumEstadoPreguntaFiltro.Mal:
                    return this.PreguntaRepository.GetByEstadoCorreccion(estadoPreguntas, idPregunta).ToList();
                default:
                    return this.PreguntaRepository.GetAllRespuestasAlumno(idPregunta).ToList();
            }
        }

        #endregion

        public IEnumerable<RespuestaEvaluarWrapper> GetRespuestasByPregunta(int idPregunta, EnumEstadoPreguntaFiltro estadoPreguntas)
        {
            var respuestasAll = new List<RespuestaEvaluarWrapper>();

            List<Pregunta> preguntas = this.GetPreguntasByEstado(EnumEstadoPreguntaFiltro.Todas, idPregunta).ToList();

            if (preguntas.Any())
            {
                Pregunta pregunta = preguntas.FirstOrDefault();

                if (pregunta != null)
                {
                    if (estadoPreguntas != EnumEstadoPreguntaFiltro.Todas)
                    {
                        pregunta.RespuestaAlumno = estadoPreguntas == EnumEstadoPreguntaFiltro.SinCorregir 
                                ? pregunta.RespuestaAlumno.Where(x => x.IdResultadoEvaluacion == null).ToList() 
                                : pregunta.RespuestaAlumno.Where(x => x.IdResultadoEvaluacion == (int)estadoPreguntas).ToList();
                    }

                    foreach (RespuestaAlumno respuestaAlumno in pregunta.RespuestaAlumno)
                    {
                        RespuestaEvaluarWrapper respuestaEvaluarWrapper = this.GetRespuestaWrapperByRespuesta(respuestaAlumno);

                        respuestasAll.Add(respuestaEvaluarWrapper);
                    }
                }
            }

            return respuestasAll.OrderBy(x => x.FechaHoraRespuesta);
        }

        public Pregunta GetByNroPregunta(int preguntaNro)
        {
            return this.PreguntaRepository.GetByNroPregunta(preguntaNro);
        }
    }
}