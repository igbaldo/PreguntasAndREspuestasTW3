using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPWebIII.Models.WrapperEntities
{
    public class RankingAlumnoWrapper
    {
        public int IdAlumno { get; set; }
        public int Posicion { get; set; }
        public string Alumno { get; set; }
        public long Puntos { get; set; }
        public int RespuestasBien { get; set; }
        public int MejorRespuesta { get; set; }
    }
}