using System;
using System.ComponentModel.DataAnnotations;

namespace TPWebIII.Models.WrapperEntities
{
    public class RespuestaWrapper
    {
        public string Clase { get; set; }
        public string Tema { get; set; }
        public int IdPregunta { get; set; }
        public int Nro { get; set; }
        public string Pregunta { get; set; }
        public DateTime? DisponibleHasta { get; set; }
        [Required(ErrorMessage = "Debe ingresar la respuesta")]
        public string Respuesta { get; set; }
    }
}