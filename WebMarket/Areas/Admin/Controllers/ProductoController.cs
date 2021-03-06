﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMarket.Models;

namespace WebMarket.Areas.Admin.Controllers
{
    public class ProductoController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: Admin/Producto
        public ActionResult Index()
        {
            var producto = db.Producto.Include(p => p.Categoria).Include(p => p.Marca);
            return View(producto.ToList());
        }

        // GET: Admin/Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Admin/Producto/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria");
            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "NombreMarca");
            return View();
        }

        // POST: Admin/Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,NombreProducto,MarcaId,CategoriaId,Descripcion,Precio,Existencias,Activo")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "NombreMarca", producto.MarcaId);
            return View(producto);
        }

        // GET: Admin/Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "NombreMarca", producto.MarcaId);
            return View(producto);
        }

        // POST: Admin/Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,NombreProducto,MarcaId,CategoriaId,Descripcion,Precio,Existencias,Activo")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaId = new SelectList(db.Categoria, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            ViewBag.MarcaId = new SelectList(db.Marca, "MarcaId", "NombreMarca", producto.MarcaId);
            return View(producto);
        }

        // GET: Admin/Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Admin/Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }





        public JsonResult Adjuntar(int ProductoId, HttpPostedFileBase documento)
        {
            var respuesta = new Models.ResponseModel
            {
                respuesta = true,
                error = ""
            };

            if (documento != null)
            {
                string adjunto = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(documento.FileName);
                documento.SaveAs(Server.MapPath("~/ImgProductos/" + adjunto));

                db.ProductoImagen.Add(new ProductoImagen { ProductoId = ProductoId, Imagen = adjunto, Titulo = "Ejemplo", Descripcion = "Ejemplo" });
                db.SaveChanges();

            }
            else
            {
                respuesta.respuesta = false;
                respuesta.error = "Debe adjuntar un documento";
            }

            return Json(respuesta);
        }

        public PartialViewResult Adjuntos(int ProductoId)
        {
            return PartialView(db.ProductoImagen.Where(x => x.ProductoId == ProductoId).ToList());
        }

        public JsonResult EliminarImagen(int ProductoImagenId)
        {
            var rpt = new Models.ResponseModel()
            {
                respuesta = true,
                error = ""
            };
            var img = db.ProductoImagen.Find(ProductoImagenId);

            if (System.IO.File.Exists(Server.MapPath("~/ImgProductos/" + img.Imagen)))
                System.IO.File.Delete(Server.MapPath("~/ImgProductos/" + img.Imagen));

            db.ProductoImagen.Remove(img);
            db.SaveChanges();

            return Json(rpt);
        }


    }
}
