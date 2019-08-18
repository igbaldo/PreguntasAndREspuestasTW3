using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TPWebIII.Models;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Services;
using System.Transactions;
using TPWebIII.Helpers;
using TPWebIII.Models.Enums;
using TPWebIII.Utils.EmailsManager;

namespace TPWebIII.Controllers
{
    public class AlumnosController : Controller
    {
        #region Constructor

        public AlumnosController()
        {
            this.AlumnoService = new AlumnoService();
            this.ClaseService = new ClaseService();
            this.PreguntaService = new PreguntaService();
            this.RespuestaService = new RespuestaService();
        }

        #endregion

        #region Properties

        private AlumnoService AlumnoService { get; set; }
        private ClaseService ClaseService { get; set; }
        private PreguntaService PreguntaService { get; set; }
        private RespuestaService RespuestaService { get; set; }

        #endregion

        #region Home Alumnos

        public ActionResult HomeAlumnos()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() } );

            ViewBag.Ranking = this.AlumnoService.GetRankingAlumnos();
            ViewBag.UltimaClase = this.ClaseService.GetNombreUltimaClase();

            List<UltimaPreguntaHomeWrapper> ultimasPreguntasCorregidas = this.PreguntaService.GetUltimasPreguntasCorregidas();

            ViewBag.UltimaPregunta = ultimasPreguntasCorregidas.OrderByDescending(x => x.IdPregunta).FirstOrDefault();
            ViewBag.PenultimaPregunta = ultimasPreguntasCorregidas.OrderBy(x => x.IdPregunta).FirstOrDefault();

            ViewBag.PreguntasSinResponder = this.PreguntaService.GetPreguntasSinResponder();

            return View();
        }

        #endregion

        #region Responder Pregunta

        [HttpGet]
        public ActionResult ResponderPregunta(int id)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            Pregunta pregunta = this.PreguntaService.GetById(Convert.ToInt32(id));

            if (pregunta == null)
                return HttpNotFound();

            var respuestaWrapper = new RespuestaWrapper
            {
                Clase = pregunta.Clase != null ? pregunta.Clase.Nombre : string.Empty,
                Tema = pregunta.Tema != null ? pregunta.Tema.Nombre : string.Empty,
                IdPregunta = pregunta.IdPregunta,
                Pregunta = pregunta.Pregunta1,
                Nro = pregunta.Nro,
                DisponibleHasta = pregunta.FechaDisponibleHasta
            };

            return View(respuestaWrapper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResponderPregunta([Bind(Include = "IdPregunta,Respuesta")] RespuestaWrapper respuestaAlumno)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    Pregunta pregunta = this.PreguntaService.GetById(respuestaAlumno.IdPregunta);

                    var respuestaWrapper = new RespuestaWrapper
                    {
                        Clase = pregunta.Clase != null ? pregunta.Clase.Nombre : string.Empty,
                        Tema = pregunta.Tema != null ? pregunta.Tema.Nombre : string.Empty,
                        IdPregunta = pregunta.IdPregunta,
                        Pregunta = pregunta.Pregunta1,
                        DisponibleHasta = pregunta.FechaDisponibleHasta,
                        Respuesta = respuestaAlumno.Respuesta
                    };

                    if (respuestaAlumno.Respuesta == null)
                        return View("ResponderPregunta", respuestaWrapper);

                    if (pregunta.FechaDisponibleHasta < DateTime.Now)
                    {
                        TempData["messageERROR"] = "Se ha vencido el plazo para responder a esta pregunta.";

                        return View("ResponderPregunta", respuestaWrapper);
                    }

                    int orden = this.RespuestaService.GuardarRespuesta(respuestaAlumno);

                    EmailService.SendMailRespuesta(pregunta, respuestaAlumno, orden);

                    transactionScope.Complete();
                    
                    return RedirectToAction("Preguntas", new { estadoPreguntas = -1 });
                }
                catch (Exception ex)
                {
                    TempData["messageERROR"] = "Se produjo un error en la aplicación. " + ex.Message;

                    transactionScope.Dispose();

                    return View("ResponderPregunta", respuestaAlumno);
                }
            }
        }

        #endregion

        #region VerRespuesta

        [HttpGet]
        public ActionResult VerRespuesta(int id)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            Pregunta pregunta = this.PreguntaService.GetById(Convert.ToInt32(id));

            if (pregunta == null)
                return HttpNotFound();

            PreguntaWrapper preguntaWrapper = this.PreguntaService.GetPreguntaWrapperByPregunta(pregunta);

            return View(preguntaWrapper);
        }

        #endregion

        #region Preguntas

        public ActionResult Preguntas(int estadoPreguntas)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            ViewBag.Preguntas = this.PreguntaService.GetPreguntasAlumnoByEstado((EnumEstadoPreguntaFiltro)estadoPreguntas).ToList();

            return View();
        }

        #endregion

        #region Acerca de

        [ActionName("acerca-de")]
        public ActionResult About()
        {
            ViewBag.Layout = "~/Views/Shared/_AlumnosLayout.cshtml";

            return View();
        }

        #endregion

        #region Salir

        public ActionResult Salir()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Ingresar", "Home");
        }

        #endregion
    }
}