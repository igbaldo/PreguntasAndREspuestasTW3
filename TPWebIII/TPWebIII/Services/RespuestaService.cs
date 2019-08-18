using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using TPWebIII.Helpers;
using TPWebIII.Models;
using TPWebIII.Models.Enums;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Repositories;

namespace TPWebIII.Services
{
    public class RespuestaService
    {
        #region Constructor

        public RespuestaService()
        {
            this.PreguntaService = new PreguntaService();
            this.RespuestaRepository = new RespuestaRepository();
        }

        #endregion

        #region Properties

        private PreguntaService PreguntaService { get; set; }

        private RespuestaRepository RespuestaRepository { get; set; }

        #endregion

        public int GuardarRespuesta(RespuestaWrapper respuestaAlumno)
        {
            return this.RespuestaRepository.Insert(new RespuestaAlumno
                                                        {
                                                            IdPregunta = respuestaAlumno.IdPregunta,
                                                            IdAlumno = UserCache.IdUsuario,
                                                            FechaHoraRespuesta = DateTime.Now,
                                                            Respuesta = respuestaAlumno.Respuesta
                                                        });
        }

        public void EvaluarRespuesta(int idPregunta, int idRespuesta, int idResultadoEval)
        {
            int puntajeMaximoPorRespuestaCorrecta = Convert.ToInt32(WebConfigurationManager.AppSettings["PuntajeMaximoPorRespuestaCorrecta"]);
            int cupoMaximoRespuestasCorrectas = Convert.ToInt32(WebConfigurationManager.AppSettings["CupoMaximoRespuestasCorrectas"]);

            int cantidadRespuestasCorrectas = this.RespuestaRepository.GetByIdPregunta(idPregunta);

            long puntajeMinimo = puntajeMaximoPorRespuestaCorrecta / cupoMaximoRespuestasCorrectas;

            long puntajeRespuesta = 0;

            if (idResultadoEval == (int) EnumEstadoPreguntaFiltro.Correcta)
            {
                puntajeRespuesta = puntajeMaximoPorRespuestaCorrecta - (puntajeMaximoPorRespuestaCorrecta / 10 * (cantidadRespuestasCorrectas));

                if (puntajeRespuesta <= 0)
                    puntajeRespuesta = puntajeMinimo;

            }
            else if (idResultadoEval == (int) EnumEstadoPreguntaFiltro.Regular)
            {
                puntajeRespuesta = (puntajeMaximoPorRespuestaCorrecta - (puntajeMaximoPorRespuestaCorrecta / cupoMaximoRespuestasCorrectas * (cantidadRespuestasCorrectas))) / 2;

                if (puntajeRespuesta <= 0)
                    puntajeRespuesta = puntajeMinimo / 2;
            }

            var respuestaAlumno = new RespuestaAlumno
            {
                IdRespuestaAlumno = idRespuesta,
                IdProfesorEvaluador = UserCache.IdUsuario,
                FechaHoraEvaluacion = DateTime.Now,
                IdResultadoEvaluacion = idResultadoEval,
                RespuestasCorrectasHastaElMomento = cantidadRespuestasCorrectas,
                Puntos = puntajeRespuesta,
                MejorRespuesta = false
            };

            this.RespuestaRepository.UpdateResultadoEval(respuestaAlumno);
        }

        public void SetMejorRespuesta(int idRespuesta)
        {
            int puntajeMaximoPorRespuestaCorrecta = Convert.ToInt32(WebConfigurationManager.AppSettings["PuntajeMaximoPorRespuestaCorrecta"]);

            long puntajeMejorRespuesta = puntajeMaximoPorRespuestaCorrecta / 2;

            this.RespuestaRepository.SetMejorRespuesta(idRespuesta, puntajeMejorRespuesta);
        }

        public RespuestaAlumno Get(int idRespuesta)
        {
            return this.RespuestaRepository.GetById(idRespuesta);
        }
    }
}