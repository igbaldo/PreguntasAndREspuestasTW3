using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models;
using TPWebIII.Repositories;
using TPWebIII.Services.Interface;

namespace TPWebIII.Services
{
    public class ClaseService : IGetterService<Clase>
    {
        #region Constructor

        public ClaseService()
        {
            this.ClaseRepository = new ClaseRepository();
        }

        #endregion

        #region Properties

        private ClaseRepository ClaseRepository { get; set; }

        #endregion

        public IEnumerable<Clase> GetAll()
        {
            return this.ClaseRepository.GetAll();
        }

        #region NotImplementedMemebers

        public Clase GetById(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        public string GetNombreUltimaClase()
        {
            Clase ultimaClase = this.ClaseRepository.GetUltimaClase();

            return ultimaClase != null ? string.Format("({0})", ultimaClase.Nombre) : string.Empty;
        }
    }
}