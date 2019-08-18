using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPWebIII.Helpers;
using TPWebIII.Models;
using TPWebIII.Models.Enums;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Utils.EmailsManager;

namespace TPWebIII.Services
{
    public static class EmailService
    {
        public static void SendMailRespuesta(Pregunta pregunta, RespuestaWrapper respuestaAlumno, int orden)
        {
            var emailRespuesta = new EmailsManager
            {
                recipients = "diego.gonzalez301@gmail.com",
                subject = string.Format("Respuesta a Pregunta {0} - {1} - {2}", pregunta.Nro, orden, UserCache.Nombre) ,
                body = EmailsBodyGenerator.GetRespuestaPreguntaBody(pregunta, respuestaAlumno, orden)
            };

            emailRespuesta.SendEmail();
        }

        public static void SendMailCorreccion(int idResultadoEval, RespuestaAlumno respuesta)
        {
            var emailRespuesta = new EmailsManager
            {
                recipients = "diego.gonzalez301@gmail.com",
                subject = string.Format("Su respuesta fue calificada como {0}", ((EnumEstadoPreguntaFiltro)idResultadoEval).ToString()),
                body = EmailsBodyGenerator.GetCorreccionBody(idResultadoEval, respuesta)
            };

            emailRespuesta.SendEmail();
        }

        public static void SendMailMejorRespuesta(RespuestaAlumno respuesta)
        {
            var emailRespuesta = new EmailsManager
            {
                recipients = "diego.gonzalez301@gmail.com",
                subject = "Su respuesta ha sido marcada como la mejor. ¡Felicitaciones!",
                body = EmailsBodyGenerator.GetMejorRespuestaBody(respuesta)
            };

            emailRespuesta.SendEmail();
        }
    }
}