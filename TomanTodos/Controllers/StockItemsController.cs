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

        // GET: StockItems
        public ActionResult Index()
        {
            var stockItems = db.StockItems.Include(s => s.Producto).Include(s => s.Sucursal);
            return View(stockItems.ToList());
        }

        // GET: StockItems/Details/5
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

        // GET: StockItems/AgregarStock

        public ActionResult AgregarStock(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = db.Productos.Include(s => s.Stock)
                            .FirstOrDefault(p => p.Id == id);

            ViewBag.SucursalId = new SelectList(db.Sucursales.OrderBy(s => s.Nombre), "Id", "Nombre");

            return View(producto);
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
            catch (Exception e)
            {
                TempData["OK"] = false;
                throw e;
            }

        }

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
