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
    public class ProfesorService : UserAuthenticationService
    {
        #region Properties

        private ProfesorRepository _profesorRepository { get; set; }

        #endregion

        #region Constructor

        public ProfesorService()
        {
            this._profesorRepository = new ProfesorRepository();
        }

        #endregion

        public override UsuarioWrapper GetUsuarioByLogin(LoginWrapper loginWrapper)
        {
            UsuarioWrapper usuarioWrapper = null;

            Profesor profesor = this._profesorRepository.GetByLogin(loginWrapper);

            if (profesor != null)
            {
                usuarioWrapper = new UsuarioWrapper
                {
                    IdUsuario = profesor.IdProfesor,
                    Username = string.Format("{0} {1} (Profesor)", profesor.Nombre, profesor.Apellido),
                    Nombre = string.Format("{0} {1}", profesor.Nombre, profesor.Apellido),
                    PerfilUsuario = EnumPerfilUsuario.Docente
                };
            }

            return usuarioWrapper;
        }
    }
}