using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TomanTodos.Models;
using TomanTodos.Models.TomanTodosModels;

namespace TomanTodos.Controllers
{
    public class StockItemsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            var stockItems = db.StockItems.Include(s => s.Producto).Include(s => s.Sucursal);
            return View(stockItems.ToList());
        }
        #endregion

        #region Details
        [HttpGet]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockItem stockItem = db.StockItems.Find(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }
        #endregion

        #region Create
        [HttpGet]
        // GET: StockItems/Create
        public ActionResult Create()
        {
            ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Nombre");
            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre");
            return View();
        }

        //// POST: StockItems/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cantidad,ProductoId,SucursalId")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                stockItem.Id = Guid.NewGuid();
                db.StockItems.Add(stockItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }
        #endregion



        // GET: StockItems/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockItem stockItem = db.StockItems.Find(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }

        // POST: StockItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cantidad,ProductoId,SucursalId")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductoId = new SelectList(db.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StockItems/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockItem stockItem = db.StockItems.Find(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }

        // POST: StockItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            StockItem stockItem = db.StockItems.Find(id);
            db.StockItems.Remove(stockItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region IntercambiarProductos
        [HttpGet]
        public ActionResult IntercambiarProductos(Guid? id)
        {
            var productoSeleccionado = id == null ? db.Productos.Include(s => s.Stock.Select(suc => suc.Sucursal)).OrderBy(p => p.Nombre).FirstOrDefault() : db.Productos.FirstOrDefault(p => p.Id == id);

            ViewBag.productoId = new SelectList(db.Productos.OrderBy(p => p.Nombre), "Id", "Nombre", productoSeleccionado.Id);
            ViewBag.sucursalOrigenId = new SelectList(db.Sucursales.OrderBy(s => s.Nombre), "Id", "Nombre");
            ViewBag.sucursalDestinoId = new SelectList(db.Sucursales.OrderBy(s => s.Nombre), "Id", "Nombre");

            if (id != null)
            {
                var producto = db.Productos
                                 .Include(s => s.Stock.Select(suc => suc.Sucursal))
                                 .FirstOrDefault(p => p.Id == id);

                return View(producto);
            }

            return View(productoSeleccionado);
        }

        [HttpPost]
        public JsonResult IntercambiarProductos(Guid productoId, Guid sucursalOrigenId, Guid sucursalDestinoId, int cantidad)
        {
            var producto = db.Productos.FirstOrDefault(p => p.Id == productoId);
            var sucursalOrigen = db.Sucursales.Include(s => s.StockItems).FirstOrDefault(s => s.Id == sucursalOrigenId);
            var sucursalDestino = db.Sucursales.Include(s => s.StockItems).FirstOrDefault(s => s.Id == sucursalDestinoId);

            var stockOrigen = sucursalOrigen.StockItems.FirstOrDefault(s => s.Producto == producto);
            var stockDestino = sucursalDestino.StockItems.FirstOrDefault(s => s.Producto == producto);

            try
            {
                if (stockDestino == null)
                {
                    StockItem nuevoStock = new StockItem
                    {
                        Id = Guid.NewGuid(),
                        ProductoId = productoId,
                        SucursalId = sucursalDestinoId
                    };

                    db.StockItems.Add(nuevoStock);

                    stockDestino = nuevoStock;
                }

                if (stockOrigen.Cantidad >= cantidad)
                {
                    stockOrigen.Cantidad -= cantidad;
                    stockDestino.Cantidad += cantidad;

                    MovimientoDetalle detalle = new MovimientoDetalle
                    {
                        Id = Guid.NewGuid(),
                        FechaMovimiento = DateTime.Now,
                        TipoMovimiento = TipoMovimiento.Sustraccion,
                        Producto = producto,
                        SucursalId = sucursalOrigenId,
                        Sucursal = sucursalOrigen,
                        Cantidad = cantidad
                    };

                    MovimientoDetalle detalle2 = new MovimientoDetalle
                    {
                        Id = Guid.NewGuid(),
                        FechaMovimiento = DateTime.Now,
                        TipoMovimiento = TipoMovimiento.Adicion,
                        Producto = producto,
                        SucursalId = sucursalDestinoId,
                        Sucursal = sucursalDestino,
                        Cantidad = cantidad
                    };

                    var movimientoExistente = db.Movimientos.Include(d => d.MovimientosDetalle).FirstOrDefault(p => p.ProductoId == productoId);

                    movimientoExistente.MovimientosDetalle.Add(detalle);
                    movimientoExistente.MovimientosDetalle.Add(detalle2);

                    db.SaveChanges();
                }

                return Json("ok", JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index", "Home");
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        #endregion

        #region AgregarStock
        [HttpGet]
        public ActionResult AgregarStock(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var producto = db.Productos.Include(s => s.Stock)
                            .FirstOrDefault(p => p.Id == id);

                ViewBag.SucursalId = new SelectList(db.Sucursales.OrderBy(s => s.Nombre), "Id", "Nombre");

                return View(producto);
            }
            catch (Exception error)
            {
                throw error;
            }  
        }

        [HttpPost]
        public ActionResult AgregarStock([Bind(Include = "Id,Cantidad,SucursalId,ProductoId")] StockItem stockItem)
        {
            try
            {
                var sucursal = db.Sucursales
                            .Include(s => s.StockItems)
                            .FirstOrDefault(s => s.Id == stockItem.SucursalId);

                var stockItemExistente = sucursal.StockItems.FirstOrDefault(s => s.ProductoId == stockItem.ProductoId);

                var movimientoExistente = db.Movimientos.Include(d => d.MovimientosDetalle).FirstOrDefault(p => p.ProductoId == stockItem.ProductoId);

                if (stockItemExistente != null)
                {
                    stockItemExistente.Cantidad += stockItem.Cantidad;
                }

                if (ModelState.IsValid && stockItemExistente == null)
                {
                    stockItem.Id = Guid.NewGuid();
                    db.StockItems.Add(stockItem);
                }

                MovimientoDetalle detalle = new MovimientoDetalle
                {
                    Id = Guid.NewGuid(),
                    FechaMovimiento = DateTime.Now,
                    TipoMovimiento = TipoMovimiento.Adicion,
                    Producto = db.Productos.FirstOrDefault(p => p.Id == stockItem.ProductoId),
                    SucursalId = stockItem.SucursalId,
                    Sucursal = db.Sucursales.FirstOrDefault(s => s.Id == stockItem.SucursalId),
                    Cantidad = stockItem.Cantidad
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
                        ProductoId = stockItem.ProductoId,
                        Producto = stockItem.Producto,
                        MovimientosDetalle = new List<MovimientoDetalle>()
                    };

                    nuevoMovimiento.MovimientosDetalle.Add(detalle);
                    db.Movimientos.Add(nuevoMovimiento);
                }

                TempData["OK"] = true;
                TempData["Movimiento"] = Models.TomanTodosModels.TipoMovimiento.Adicion;
                TempData["Cantidad"] = stockItem.Cantidad;
                TempData["Producto"] = db.Productos.FirstOrDefault(p => p.Id == stockItem.ProductoId).Nombre;
                TempData["Sucursal"] = db.Sucursales.FirstOrDefault(s => s.Id == stockItem.SucursalId).Nombre;
                //db.MovimientosDetalle.Add(detalle);
                db.SaveChanges();

                //return RedirectToAction("VerStockProductos", "Productos");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception error)
            {
                TempData["OK"] = false;
                throw error;
            }

        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
