
using System.Text;
using TPWebIII.Helpers;
using TPWebIII.Models;
using TPWebIII.Models.WrapperEntities;

namespace TPWebIII.Utils.EmailsManager
{
    public class EmailsBodyGenerator
    {
        public static string GetRespuestaPreguntaBody(Pregunta pregunta, RespuestaWrapper respuestaAlumno, int orden)
        {
            var body = new StringBuilder();

            body.Append("<html><head><title>Preguntas UNLaM - Respuesta a pregunta</title>");
            body.Append("<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'></head>");
            body.Append("<body topmargin='0' leftmargin='0' bgcolor='#196a70'>");
            body.Append("<table align='center' cellpadding='4' cellspacing='4' style='background-color: #196a70; background-position: center top; width: 800px; border: thin groove #d4f0ff;'>");
            body.Append("<tr><td><table align='center' width='800' cellpadding='4' cellspacing='4'></td></tr>");
            body.Append("<tr><td><label style='font-family: Arial; font-size: small; font-weight: bold; color: #D4F0FF; '>Estimado/a, un alumno ha respondido la siguiente pregunta: </label></td></tr>");
            body.Append(string.Format("<tr><td><label style='color: #D4F0FF'><b>Pregunta:</b>&nbsp;{0}</td></tr>", pregunta.Pregunta1));
            body.Append(string.Format("<tr><td><label style='color: #D4F0FF'><b>Alumno:</b>&nbsp;{0}</td></tr>", UserCache.Nombre));
            body.Append(string.Format("<tr><td><label style='color: #D4F0FF'><b>Orden:</b>&nbsp;{0}</td></tr>", orden));
            body.Append(string.Format("<tr><td><label style='color: #D4F0FF'><b>Respuesta:</b>&nbsp;{0}</td></tr>", respuestaAlumno.Respuesta));
            body.Append("<tr><td><a style='color: #D4F0FF' title='http://localhost:6068/Profesores/EvalRespuestas?idPregunta=" + pregunta.IdPregunta + "&estadoPreguntas=-1' href='http://localhost:6068/Profesores/EvalRespuestas?idPregunta=" + pregunta.IdPregunta + "&estadoPreguntas=-1' target='_blank'>http://localhost:6068/Profesores/EvalRespuestas?idPregunta=" + pregunta.IdPregunta + "&estadoPreguntas=-1</a></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF'>Atentamente,</label></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF; font-size: 20px;'><b><i>El Equipo de Preguntas UNLaM</i></b></label></td></tr>");
            body.Append("</table></td></tr></table></body></html>");

            return body.ToString();
        }

        public static string GetCorreccionBody(int idResultadoEval, RespuestaAlumno respuesta)
        {
            var body = new StringBuilder();

            body.Append("<html><head><title>Preguntas UNLaM - Correccion de pregunta</title>");
            body.Append("<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'></head>");
            body.Append("<body topmargin='0' leftmargin='0' bgcolor='#196a70'>");
            body.Append("<table align='center' cellpadding='4' cellspacing='4' style='background-color: #196a70; background-position: center top; width: 800px; border: thin groove #d4f0ff;'>");
            body.Append("<tr><td><table align='center' width='800' cellpadding='4' cellspacing='4'></td></tr>");
            body.Append("<tr><td><label style='font-family: Arial; font-size: small; font-weight: bold; color: #D4F0FF; '>Estimado/a, un alumno ha respondido la siguiente pregunta: </label></td></tr>");
            body.Append(string.Format("<tr><td><label style='color: #D4F0FF'><b>Pregunta:</b>&nbsp;{0}</td></tr>", respuesta.Pregunta.Pregunta1));
            body.Append("<tr><td><label style='color: #D4F0FF'>Su respuesta:</label>&nbsp;<a style='color: #D4F0FF' title='http://localhost:6068/Alumnos/VerRespuesta/" + respuesta.IdPregunta + "' href='http://localhost:6068/Alumnos/VerRespuesta/" + respuesta.IdPregunta + "' target='_blank'>http://localhost:6068/Alumnos/VerRespuesta/" + respuesta.IdPregunta + "</a></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF'>Posiciones:</label>&nbsp;<a style='color: #D4F0FF' title='http://localhost:6068/Alumnos/HomeAlumnos' href='http://localhost:6068/Alumnos/HomeAlumnos' target='_blank'>http://localhost:6068/Alumnos/HomeAlumnos</a></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF'>Atentamente,</label></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF; font-size: 20px;'><b><i>El Equipo de Preguntas UNLaM</i></b></label></td></tr>");
            body.Append("</table></td></tr></table></body></html>");

            return body.ToString();
        }

        public static string GetMejorRespuestaBody(RespuestaAlumno respuesta)
        {
            var body = new StringBuilder();

            body.Append("<html><head><title>Preguntas UNLaM - ¡Mejor Respuesta!</title>");
            body.Append("<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'></head>");
            body.Append("<body topmargin='0' leftmargin='0' bgcolor='#196a70'>");
            body.Append("<table align='center' cellpadding='4' cellspacing='4' style='background-color: #196a70; background-position: center top; width: 800px; border: thin groove #d4f0ff;'>");
            body.Append("<tr><td><table align='center' width='800' cellpadding='4' cellspacing='4'></td></tr>");
            body.Append("<tr><td><label style='font-family: Arial; font-size: small; font-weight: bold; color: #D4F0FF; '>Estimado/a un alumno, </label></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF'>Su respuesta ha sido marcada como la mejor.</label></td></tr>");
            body.Append(string.Format("<tr><td><label style='color: #D4F0FF'><b>Pregunta:</b>&nbsp;{0}</td></tr>", respuesta.Pregunta.Pregunta1));
            body.Append("<tr><td><label style='color: #D4F0FF'>Su respuesta:</label>&nbsp;<a style='color: #D4F0FF' title='http://localhost:6068/Alumnos/VerRespuesta/" + respuesta.IdPregunta + "' href='http://localhost:6068/Alumnos/VerRespuesta/" + respuesta.IdPregunta + "' target='_blank'>http://localhost:6068/Alumnos/VerRespuesta/" + respuesta.IdPregunta + "</a></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF'>Posiciones:</label>&nbsp;<a style='color: #D4F0FF' title='http://localhost:6068/Alumnos/HomeAlumnos' href='http://localhost:6068/Alumnos/HomeAlumnos' target='_blank'>http://localhost:6068/Alumnos/HomeAlumnos</a></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF'>¡Felicitaciones!</label></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF'>Atentamente,</label></td></tr>");
            body.Append("<tr><td><label style='color: #D4F0FF; font-size: 20px;'><b><i>El Equipo de Preguntas UNLaM</i></b></label></td></tr>");
            body.Append("</table></td></tr></table></body></html>");

            return body.ToString();
        }
    }
}