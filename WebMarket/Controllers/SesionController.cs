using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMarket.Controllers
{
    public class SesionController : Controller
    {
        private Models.DatabaseEntities bd = new Models.DatabaseEntities();
        // GET: Sesion
        public ActionResult ILogin()
        {
            return View();
        }

        public ActionResult Crear()
        {
            return View();
        }

        public ActionResult Login(string usuario, string clave)
        {
            var u = bd.Cliente.FirstOrDefault(x => x.Usuario == usuario && x.Clave == clave);
            if (u != null)
            {
                Helper.SessionHelper.AddUserToSession(u.ClienteId.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Helper.SessionHelper.DestroyUserSession();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RegistrarCliente(Models.Cliente c)
        {
            bd.Cliente.Add(c);
            bd.SaveChanges();
            Helper.SessionHelper.AddUserToSession(c.ClienteId.ToString());
            return RedirectToAction("Index", "Home");
        }

        public static string ObtenerNombreUsuario()
        {
            using (var b = new Models.DatabaseEntities())
            {
                return b.Cliente.Find(Helper.SessionHelper.GetUser()).Nombres;
            }
        }




    }
}