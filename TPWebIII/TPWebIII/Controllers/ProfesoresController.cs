using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TPWebIII.Models;
using TPWebIII.Models.Enums;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Services;

namespace TPWebIII.Controllers
{
    public class ProfesoresController : Controller
    {
        #region Constructor

        public ProfesoresController()
        {
            this.PreguntaService = new PreguntaService();
            this.ClaseService = new ClaseService();
            this.TemaService = new TemaService();
            this.RespuestaService = new RespuestaService();
            this.AlumnoService = new AlumnoService();
        }

        #endregion

        #region Properties

        private PreguntaService PreguntaService { get; set; }
        private ClaseService ClaseService { get; set; }
        private TemaService TemaService { get; set; }
        private RespuestaService RespuestaService { get; set; }
        private AlumnoService AlumnoService { get; set; }

        #endregion

        #region PreguntasList

        public ActionResult Preguntas()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            List<Pregunta> preguntas = this.PreguntaService.GetAll().ToList();

            return View(preguntas);
        }

        #region JQGrid

        public JsonResult GetPreguntas(string sidx, string sord, int page, int rows, bool _search, string filters)
        {
            try
            {
                TempData["messageERROR"] = string.Empty;

                IEnumerable<Pregunta> preguntas = this.PreguntaService.GetAll();

                var totalRecords = preguntas.Count();

                var json = new JsonResult();

                json = Json(new
                {
                    total = (totalRecords + rows - 1) / rows,
                    page,
                    records = totalRecords,
                    rows = (from item in preguntas
                            select new
                            {
                                IdPregunta = item.IdPregunta,
                                Nro = item.Nro,
                                Clase = item.Clase != null ? item.Clase.Nombre : string.Empty,
                                Tema = item.Tema != null ? item.Tema.Nombre : string.Empty,
                                Pregunta = item.Pregunta1,
                                FechaDesde = item.FechaDisponibleDesde.ToString(),
                                FechaHasta = item.FechaDisponibleHasta.ToString()
                            }).OrderByDescending(x => x.IdPregunta).ToArray()
                });

                json.MaxJsonLength = int.MaxValue;

                return json;
            }
            catch (Exception ex)
            {
                TempData["messageERROR"] = "Se produjo un error en la aplicacion. " + ex.Message;
            }

            return new JsonResult();
        }

        #endregion

        #endregion

        #region Preguntas Manager

        public ActionResult PreguntasManager(int? id)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IEnumerable<Clase> clases = this.ClaseService.GetAll();
            IEnumerable<Tema> temas = this.TemaService.GetAll();

            if (id == 0)
            {
                ViewBag.IdClase = new SelectList(clases, "IdClase", "Nombre");
                ViewBag.IdTema = new SelectList(temas, "IdTema", "Nombre");

                int maxNroPregunta = this.PreguntaService.GetProxNroPregunta();

                return View(new Pregunta { IdPregunta = 0, Nro = maxNroPregunta });
            }

            Pregunta pregunta = this.PreguntaService.GetById(Convert.ToInt32(id));

            if (pregunta == null)
                return HttpNotFound();

            ViewBag.IdClase = new SelectList(clases, "IdClase", "Nombre", pregunta.IdClase);
            ViewBag.IdTema = new SelectList(temas, "IdTema", "Nombre", pregunta.IdTema);

            return View(pregunta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreguntasManager([Bind(Include = "IdPregunta,Nro,IdClase,IdTema,FechaDisponibleDesde,FechaDisponibleHasta,Pregunta1,IdProfesorCreacion,FechaHoraCreacion,IdProfesorModificacion,FechaHoraModificacion")] Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Clase> clases = this.ClaseService.GetAll();
                IEnumerable<Tema> temas = this.TemaService.GetAll();

                ViewBag.IdClase = new SelectList(clases, "IdClase", "Nombre");
                ViewBag.IdTema = new SelectList(temas, "IdTema", "Nombre");

                using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {
                        if (pregunta.FechaDisponibleDesde != null && pregunta.FechaDisponibleHasta != null && Convert.ToDateTime(pregunta.FechaDisponibleHasta) < Convert.ToDateTime(pregunta.FechaDisponibleDesde))
                        {
                            transactionScope.Dispose();

                            TempData["messageERROR"] = "El campo Hasta debe ser mayor al campo Desde.";

                            return View("PreguntasManager", pregunta);
                        }

                        if (string.IsNullOrEmpty(pregunta.Pregunta1))
                        {
                            transactionScope.Dispose();

                            TempData["messageERROR"] = "El campo Pregunta es obligatorio.";

                            return View("PreguntasManager", pregunta);
                        }

                        if (pregunta.IdPregunta == 0)
                        {
                            if (this.NumeroExistente(pregunta.Nro))
                            {
                                transactionScope.Dispose();

                                TempData["messageERROR"] = "Ya existe una pregunta con este número de pregunta.";

                                return View("PreguntasManager", pregunta);
                            }

                            this.PreguntaService.Insert(pregunta);
                        }
                        else
                        {
                            this.PreguntaService.Update(pregunta);
                        }

                        transactionScope.Complete();

                        return RedirectToAction("Preguntas");
                    }
                    catch (Exception ex)
                    {
                        TempData["messageERROR"] = "Se produjo un error en la aplicación. " + ex.Message;

                        transactionScope.Dispose();

                        ViewBag.IdClase = new SelectList(clases, "IdClase", "Nombre", pregunta.IdClase);
                        ViewBag.IdTema = new SelectList(temas, "IdTema", "Nombre", pregunta.IdTema);

                        return RedirectToAction("PreguntasManager", pregunta);
                    }
                }
            }

            return View("Preguntas");
        }

        #endregion

        #region EliminacionPreguntas

        public ActionResult PreguntasDelete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Pregunta pregunta = this.PreguntaService.GetById(Convert.ToInt32(id));

            if (pregunta == null)
                return HttpNotFound();

            return View(pregunta);
        }

        [HttpPost, ActionName("PreguntasDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.PreguntaService.Delete(id);

            return RedirectToAction("Preguntas");
        }

        #endregion

        #region Evaluacion Respuestas

        public ActionResult EvalRespuestas(int idPregunta, int estadoPreguntas)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            Pregunta pregunta = this.PreguntaService.GetById(Convert.ToInt32(idPregunta));

            ViewBag.Respuestas = this.PreguntaService.GetRespuestasByPregunta(idPregunta, (EnumEstadoPreguntaFiltro)estadoPreguntas).ToList();

            return View(pregunta);
        }

        #region Resultados Evals

        public ActionResult Evaluar(int idPregunta, int idRespuesta, int idResultadoEval)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    this.RespuestaService.EvaluarRespuesta(idPregunta, idRespuesta, idResultadoEval);

                    RespuestaAlumno respuesta = this.RespuestaService.Get(idRespuesta);

                    EmailService.SendMailCorreccion(idResultadoEval, respuesta);

                    transactionScope.Complete();

                    return View("Preguntas");
                }
                catch (Exception ex)
                {
                    TempData["messageERROR"] = "Se produjo un error en la aplicación. " + ex.Message;

                    transactionScope.Dispose();

                    return View("Preguntas");
                }
            }
        }

        public ActionResult MejorRespuesta(int idRespuesta)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    this.RespuestaService.SetMejorRespuesta(idRespuesta);

                    RespuestaAlumno respuesta = this.RespuestaService.Get(idRespuesta);

                    EmailService.SendMailMejorRespuesta(respuesta);

                    transactionScope.Complete();

                    return View("Preguntas");
                }
                catch (Exception ex)
                {
                    TempData["messageERROR"] = "Se produjo un error en la aplicación. " + ex.Message;

                    transactionScope.Dispose();

                    return View("Preguntas");
                }
            }
        }

        #endregion

        #endregion

        #region Inicio

        public ActionResult HomeProfesores()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            ViewBag.Ranking = this.AlumnoService.GetRankingAlumnos();
            ViewBag.UltimaClase = this.ClaseService.GetNombreUltimaClase();

            List<UltimaPreguntaHomeWrapper> ultimasPreguntasCorregidas = this.PreguntaService.GetUltimasPreguntasCorregidas();

            ViewBag.UltimaPregunta = ultimasPreguntasCorregidas.OrderByDescending(x => x.IdPregunta).FirstOrDefault();
            ViewBag.PenultimaPregunta = ultimasPreguntasCorregidas.OrderBy(x => x.IdPregunta).FirstOrDefault();

            return View();
        }

        public ActionResult Salir()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Ingresar", "Home");
        }

        #endregion

        #region Acerca de

        [ActionName("acerca-de")]
        public ActionResult About()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Ingresar", "Home", new { returnUrl = Request.Url.ToString() });

            ViewBag.Layout = "~/Views/Shared/_ProfesoresLayout.cshtml";

            return View();
        }

        #endregion

        #region Helper Methods

        private bool NumeroExistente(int preguntaNro)
        {
            Pregunta preguntaMismoNum = this.PreguntaService.GetByNroPregunta(preguntaNro);

            return preguntaMismoNum != null;
        }

        #endregion
    }
}