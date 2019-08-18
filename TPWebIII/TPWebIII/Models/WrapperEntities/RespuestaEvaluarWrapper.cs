using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models.Enums;

namespace TPWebIII.Models.WrapperEntities
{
    public class RespuestaEvaluarWrapper
    {
        public int IdRespuestaAlumno { get; set; }
        public string Alumno { get; set; }
        public DateTime? FechaHoraRespuesta { get; set; }
        public string Respuesta { get; set; }
        public bool MejorRespuesta { get; set; }
        public EnumEstadoPreguntaFiltro? EstadoCorreccion { get; set; }
    }
}