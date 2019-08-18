using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using TPWebIII.Models.WrapperEntities;

namespace TPWebIII.Helpers
{
    public class UserCache
    {
        public static string Username
        {
            get
            {
                if (HttpContext.Current.Session["Username"] == null)
                {
                    CacheWrapper cacheWrapper = GetCacheWrapperFromCookie();

                    HttpContext.Current.Session["Username"] = cacheWrapper.Username;

                    return cacheWrapper.Username;
                }

                return Convert.ToString(HttpContext.Current.Session["Username"]);
            }
            set { HttpContext.Current.Session["Username"] = value; }
        }

        public static string Nombre
        {
            get
            {
                if (HttpContext.Current.Session["Nombre"] == null)
                {
                    CacheWrapper cacheWrapper = GetCacheWrapperFromCookie();

                    HttpContext.Current.Session["Nombre"] = cacheWrapper.Username;

                    return cacheWrapper.Nombre;
                }

                return Convert.ToString(HttpContext.Current.Session["Nombre"]);
            }
            set { HttpContext.Current.Session["Nombre"] = value; }
        }

        public static int IdUsuario
        {
            get
            {
                if (HttpContext.Current.Session["IdUsuario"] == null)
                {
                    CacheWrapper cacheWrapper = GetCacheWrapperFromCookie();

                    HttpContext.Current.Session["IdUsuario"] = cacheWrapper.IdUsuario;

                    return cacheWrapper.IdUsuario;
                }

                return Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            }
            set { HttpContext.Current.Session["IdUsuario"] = value; }
        }

        public static int IdPerfil
        {
            get
            {
                if (HttpContext.Current.Session["IdPerfil"] == null)
                {
                    CacheWrapper cacheWrapper = GetCacheWrapperFromCookie();

                    HttpContext.Current.Session["IdPerfil"] = cacheWrapper.IdPerfil;

                    return cacheWrapper.IdPerfil;
                }

                return Convert.ToInt32(HttpContext.Current.Session["IdPerfil"]);
            }
            set { HttpContext.Current.Session["IdPerfil"] = value; }
        }

        private static CacheWrapper GetCacheWrapperFromCookie()
        {
            string cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value;

            FormsAuthenticationTicket authTicket = null;

            authTicket = FormsAuthentication.Decrypt(cookie);

            var serializer = new JavaScriptSerializer();

            return serializer.Deserialize<CacheWrapper>(authTicket.Name);
        }
    }
}