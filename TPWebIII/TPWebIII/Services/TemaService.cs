using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models;
using TPWebIII.Repositories;
using TPWebIII.Services.Interface;

namespace TPWebIII.Services
{
    public class TemaService : IGetterService<Tema>
    {
        #region Constructor

        public TemaService()
        {
            this.TemaRepository = new TemaRepository();
        }

        #endregion

        #region Properties

        private TemaRepository TemaRepository { get; set; }

        #endregion

        public IEnumerable<Tema> GetAll()
        {
            return this.TemaRepository.GetAll();
        }

        #region NotImplementedMemebers

        public Tema GetById(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}