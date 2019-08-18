using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models;
using TPWebIII.Models.Enums;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Repositories;
using TPWebIII.Repositories.Interface;
using TPWebIII.Services.Interface;

namespace TPWebIII.Services
{
    public class AlumnoService : UserAuthenticationService
    {
        #region Properties

        private AlumnoRepository _alumnoRepository { get; set; }

        #endregion

        #region Constructor

        public AlumnoService()
        {
            this._alumnoRepository = new AlumnoRepository();
        }

        #endregion

        public override UsuarioWrapper GetUsuarioByLogin(LoginWrapper loginWrapper)
        {
            UsuarioWrapper usuarioWrapper = null;

            Alumno alumno = this._alumnoRepository.GetByLogin(loginWrapper);

            if (alumno != null)
            {
                usuarioWrapper = new UsuarioWrapper
                {
                    IdUsuario = alumno.IdAlumno,
                    Username = string.Format("{0} {1} (Alumno)", alumno.Nombre, alumno.Apellido),
                    Nombre = string.Format("{0} {1}", alumno.Nombre, alumno.Apellido),
                    PerfilUsuario = EnumPerfilUsuario.Alumno
                };
            }

            return usuarioWrapper;
        }

        public List<RankingAlumnoWrapper> GetRankingAlumnos()
        {
            var alumnosRankingWrappers = new List<RankingAlumnoWrapper>();

            List<Alumno> alumnosRanking = this._alumnoRepository.GetRanking();

            foreach (Alumno alumno in alumnosRanking)
            {
                alumnosRankingWrappers.Add(new RankingAlumnoWrapper
                {
                    IdAlumno = alumno.IdAlumno,
                    Posicion = alumnosRanking.IndexOf(alumno) + 1,
                    Alumno = string.Format("{0} {1}", alumno.Nombre, alumno.Apellido),
                    Puntos = alumno.PuntosTotales,
                    RespuestasBien = alumno.CantidadRespuestasCorrectas,
                    MejorRespuesta = alumno.CantidadMejorRespuesta
                });
            }

            return alumnosRankingWrappers;
        }
    }
}