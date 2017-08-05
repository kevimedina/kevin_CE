using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMarket.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        private Models.DatabaseEntities bd = new Models.DatabaseEntities();

        // GET: Home
        public ActionResult Index(string id = "")
        {
            //Logica de acceso a base de datos

            var productosi = bd.Producto.Where(x => x.NombreProducto.Contains(id))
            .Take(10)
            .ToList();
            //Devolver una lista de Productos .OrderByDescending(x => x.ProductoId)

            ViewBag.clave = id;
            return View(productosi);
        }

        public ActionResult Buscar(string id = "")
        {
            //Logica de acceso a base de datos
            var productos = bd.Producto.Where(x => x.NombreProducto.Contains(id))
                .Take(12)
                .ToList();
            //Devolver una lista de Productos
            ViewBag.clave = id;
            return View(productos);
        }
    }
}