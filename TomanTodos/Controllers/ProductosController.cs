using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TomanTodos.Models;
using TomanTodos.Models.TomanTodosModels;

namespace TomanTodos.Controllers
{
    public class ProductosController : Controller
    {

        //private readonly ApplicationDbContext _context;

        //public ProductosController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Productos
        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categoria);
            return View(productos.ToList());
        }

        // GET: StockProductos
        public ActionResult VerStockProductos()
        {
            var productos = db.Productos
                .Include(c => c.Categoria)
                .Include(s => s.Stock)
                .ToList();

            return View(productos);
        }

        [HttpPost]
        public ActionResult DetallesProducto(Guid? id)
        {
            //var sucursales = db.Sucursales
            //                .Include(s => s.StockItems.Select(p => p.Producto))
            //                .Where(p => p.== id);

            //Select(p => p.Producto).Where(p => p.Id == id))
            // .ToList();


            var producto = db.Productos
                            .Include(s => s.Stock.Select(suc => suc.Sucursal))
                            .FirstOrDefault(p => p.Id == id);

            return View(producto);
        }

        // GET: Productos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Include(p => p.Categoria).FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Precio,Descuento,Activo,CategoriaId")] Producto producto, HttpPostedFileBase upload)
        {

            if (upload != null)
            {
                HttpPostedFileBase FileBase = Request.Files[0];
                WebImage image = new WebImage(FileBase.InputStream);

                producto.Foto = image.GetBytes();
            }

            if (ModelState.IsValid)
            {
                producto.Id = Guid.NewGuid();
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        public ActionResult RetrieveImage(Guid id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(Guid id)
        {
            var q = db.Productos.FirstOrDefault(p => p.Id == id);
            byte[] cover = q.Foto;
            //var q = from temp in db.Productos where temp.ID == Id select temp.Image;
            //byte[] cover = q.First();
            return cover;
        }



        #region ActualizarStock

        [HttpGet]
        public ActionResult ActualizarStock(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = db.Productos.Include(s => s.Stock)
                            .FirstOrDefault(p => p.Id == id);

            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre");

            return View(producto);
        }
        [HttpPost]
        public ActionResult ActualizarStock(Guid productoId, Guid sucursalId, int cantidad)
        {
            var sucursal = db.Sucursales.FirstOrDefault(s => s.Id == sucursalId);

            var stock = db.StockItems.FirstOrDefault(s => s.ProductoId == productoId && s.SucursalId == sucursalId);

            var movimientoExistente = db.Movimientos.Include(d => d.MovimientosDetalle).FirstOrDefault(p => p.ProductoId == productoId);

            try
            {
                if (stock != null)
                {
                    if (stock.Cantidad >= cantidad)
                    {
                        stock.Cantidad -= cantidad;

                        MovimientoDetalle detalle = new MovimientoDetalle
                        {
                            Id = Guid.NewGuid(),
                            FechaMovimiento = DateTime.Now,
                            TipoMovimiento = TipoMovimiento.Sustraccion,
                            Producto = db.Productos.FirstOrDefault(p => p.Id == productoId),
                            SucursalId = sucursalId,
                            Sucursal = db.Sucursales.FirstOrDefault(s => s.Id == sucursalId),
                            Cantidad = cantidad
                        };

                        if (movimientoExistente != null)
                        {
                            movimientoExistente.MovimientosDetalle.Add(detalle);
                        }
                        else
                        {
                            Movimiento nuevoMovimiento = new Movimiento
                            {
                                Id = Guid.NewGuid(),
                                ProductoId = productoId,
                                Producto = db.Productos.FirstOrDefault(p => p.Id == productoId),
                                MovimientosDetalle = new List<MovimientoDetalle>()
                            };

                            nuevoMovimiento.MovimientosDetalle.Add(detalle);
                            db.Movimientos.Add(nuevoMovimiento);
                        }

                        db.SaveChanges();

                        TempData["OK"] = true;
                        TempData["Movimiento"] = Models.TomanTodosModels.TipoMovimiento.Sustraccion;
                        TempData["Cantidad"] = cantidad;
                        TempData["Producto"] = db.Productos.FirstOrDefault(p => p.Id == productoId).Nombre;
                        TempData["Sucursal"] = db.Sucursales.FirstOrDefault(s => s.Id == sucursalId).Nombre;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["Error"] = true;
                        ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre");
                        return View(db.Productos.FirstOrDefault(p => p.Id == productoId));
                    }
                }
                else
                {
                    ViewData["Error"] = true;
                    ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre");
                    return View(db.Productos.FirstOrDefault(p => p.Id == productoId));
                }

            }
            catch (Exception e)
            {
                throw e;
            }


            //return RedirectToAction("VerStockProductos");
            // return RedirectToAction("Index","Home", new { ok = });
        }
        #endregion


        // GET: Productos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid Id, string Nombre, Guid CategoriaId, HttpPostedFileBase upload)
        {

            var productoActual = db.Productos.FirstOrDefault(p => p.Id == Id);

            productoActual.Nombre = Nombre;
            //productoActual.Precio = Precio;
            //productoActual.Descuento = Descuento;
            //productoActual.Activo =  Activo;
            productoActual.CategoriaId = CategoriaId;

            if (upload != null)
            {
                HttpPostedFileBase FileBase = Request.Files[0];
                WebImage image = new WebImage(FileBase.InputStream);
                var nuevaFoto = image.GetBytes();

                if (productoActual.Foto != nuevaFoto)
                {
                    productoActual.Foto = nuevaFoto;
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
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

        [HttpPost]
        //public ActionResult Prueba(Guid productoId, Guid sucursalID, int cantidad)
        public ActionResult Prueba(Guid p, Guid so, Guid sd)
        {
            //var sucursal = db.Sucursales.FirstOrDefault(s => s.Id == sucursalID);

            //var stock = db.StockItems.FirstOrDefault(s => s.ProductoId == productoId && s.SucursalId == sucursalID);

            //var movimientoExistente = db.Movimientos.Include(d => d.MovimientosDetalle).FirstOrDefault(p => p.ProductoId == productoId);

            //if (stock != null)
            //{
            //    if (stock.Cantidad >= cantidad)
            //    {
            //        stock.Cantidad -= cantidad;
            //    }
            //}
            return this.Json(new { code = (int)HttpStatusCode.OK, text = "funciona" });

            //return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}


//[HttpGet]
//public ActionResult AgregarStock(Guid? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }

//    var producto = db.Productos.Include(s => s.Stock)
//                    .FirstOrDefault(p => p.Id == id);

//    ViewBag.SucursalId = new SelectList(db.Sucursales.OrderByDescending(s => s.Nombre), "Id", "Nombre");

//    return View(producto);
//}

//[HttpPost]
//public ActionResult AgregarStock(Guid productoId, Guid sucursalId, int cantidad)
//{
//    var sucursal = db.Sucursales.FirstOrDefault(s => s.Id == sucursalId);

//    var stock = db.StockItems.FirstOrDefault(s => s.ProductoId == productoId && s.SucursalId == sucursalId);

//    if (stock == null)
//    {
//        return RedirectToAction("ActualizarStock", "Productos", new { sucursalId = sucursalId, productoId = productoId, cantidad = cantidad });
//        //return Redirect("/StockItems/Create");
//    }
//    else
//    {
//        stock.Cantidad += cantidad;

//        MovimientoDetalle detalle = new MovimientoDetalle
//        {
//            Id = Guid.NewGuid(),
//            ProductoId = productoId,
//            Producto = db.Productos.FirstOrDefault(p => p.Id == productoId),
//            SucursalId = sucursalId,
//            Sucursal = db.Sucursales.FirstOrDefault(s => s.Id == sucursalId),
//            Cantidad = cantidad
//        };

//        Movimiento nuevoMovimiento = new Movimiento
//        {
//            Id = Guid.NewGuid(),
//            FechaMovimiento = DateTime.Now,
//            MovimientosDetalle = new List<MovimientoDetalle>()
//        };

//        nuevoMovimiento.MovimientosDetalle.Add(detalle);

//        db.Movimientos.Add(nuevoMovimiento);
//        db.MovimientosDetalle.Add(detalle);
//        db.SaveChanges();
//    }

//    return RedirectToAction("VerStockProductos");
//}
