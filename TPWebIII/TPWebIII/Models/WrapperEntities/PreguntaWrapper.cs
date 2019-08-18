using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Models.Enums;

namespace TPWebIII.Models.WrapperEntities
{
    public class PreguntaWrapper
    {
        public string Clase { get; set; }
        public string Tema { get; set; }
        public int IdPregunta { get; set; }
        public int Nro { get; set; }
        public string Pregunta { get; set; }
        public DateTime? DisponibleDesde { get; set; }
        public DateTime? DisponibleHasta { get; set; }
        public bool PlazoVencido { get; set; }
        public bool YaRespondida { get; set; }
        public string ResultadoCorreccion { get; set; }
        public EnumEstadoPreguntaFiltro EstadoCorreccion { get; set; }
        public string Respuesta { get; set; }
    }
}