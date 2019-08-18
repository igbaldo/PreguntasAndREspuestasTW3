using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using TPWebIII.Helpers;
using TPWebIII.Models.WrapperEntities;
using TPWebIII.Services;
using TPWebIII.Services.Interface;

namespace TPWebIII.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Ingresar(string returnUrl = null)
        {
            FormsAuthentication.SignOut();

            return View(new LoginWrapper { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Ingresar(LoginWrapper model)
        {
            if (ModelState.IsValid)
            {
                UserAuthenticationService usuarioService = model.Profesor ? (UserAuthenticationService) new ProfesorService() : (UserAuthenticationService) new AlumnoService();

                UsuarioWrapper usuarioWrapper = usuarioService.GetUsuarioByLogin(model);

                if (usuarioWrapper != null)
                {
                    var cacheWrapper = new CacheWrapper
                    {
                        IdUsuario = usuarioWrapper.IdUsuario,
                        Username = usuarioWrapper.Username,
                        Nombre = usuarioWrapper.Nombre,
                        IdPerfil = Convert.ToInt32(usuarioWrapper.PerfilUsuario),
                    };

                    string loginJson = new JavaScriptSerializer().Serialize(cacheWrapper);

                    var ticket = new FormsAuthenticationTicket(1,
                        usuarioWrapper.IdUsuario.ToString(),
                        DateTime.Now,
                        DateTime.Now.AddDays(2),
                        true,
                        loginJson,
                        FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    FormsAuthentication.SetAuthCookie(loginJson, false);
                    System.Web.HttpContext.Current.Session.Timeout = 2500;

                    UserCache.IdUsuario = usuarioWrapper.IdUsuario;
                    UserCache.Username = usuarioWrapper.Username;
                    UserCache.IdPerfil = Convert.ToInt32(usuarioWrapper.PerfilUsuario);

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);

                    return model.Profesor
                        ? RedirectToAction("HomeProfesores", "Profesores")
                        : RedirectToAction("HomeAlumnos", "Alumnos");
                }

                ModelState.AddModelError("", "");
            }

            return View(model);
        }
    }
}