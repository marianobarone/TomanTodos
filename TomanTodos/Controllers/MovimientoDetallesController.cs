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
    [Authorize]
    public class MovimientoDetallesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MovimientoDetalles
        public ActionResult Index()
        {
            var movimientosDetalle = db.MovimientosDetalle
                                    .Include(m => m.Sucursal)
                                    .Include(p => p.Producto)
                                    .OrderByDescending(m => m.FechaMovimiento);
            return View(movimientosDetalle.ToList());
        }

        // GET: MovimientoDetalles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoDetalle movimientoDetalle = db.MovimientosDetalle.Find(id);
            if (movimientoDetalle == null)
            {
                return HttpNotFound();
            }
            return View(movimientoDetalle);
        }

        // GET: MovimientoDetalles/Create
        public ActionResult Create()
        {
            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre");
            return View();
        }

        // POST: MovimientoDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FechaMovimiento,Cantidad,SucursalId,TipoMovimiento")] MovimientoDetalle movimientoDetalle)
        {
            if (ModelState.IsValid)
            {
                movimientoDetalle.Id = Guid.NewGuid();
                db.MovimientosDetalle.Add(movimientoDetalle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre", movimientoDetalle.SucursalId);
            return View(movimientoDetalle);
        }

        // GET: MovimientoDetalles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoDetalle movimientoDetalle = db.MovimientosDetalle.Find(id);
            if (movimientoDetalle == null)
            {
                return HttpNotFound();
            }
            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre", movimientoDetalle.SucursalId);
            return View(movimientoDetalle);
        }

        // POST: MovimientoDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaMovimiento,Cantidad,SucursalId,TipoMovimiento")] MovimientoDetalle movimientoDetalle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimientoDetalle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SucursalId = new SelectList(db.Sucursales, "Id", "Nombre", movimientoDetalle.SucursalId);
            return View(movimientoDetalle);
        }

        // GET: MovimientoDetalles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoDetalle movimientoDetalle = db.MovimientosDetalle.Find(id);
            if (movimientoDetalle == null)
            {
                return HttpNotFound();
            }
            return View(movimientoDetalle);
        }

        // POST: MovimientoDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MovimientoDetalle movimientoDetalle = db.MovimientosDetalle.Find(id);
            db.MovimientosDetalle.Remove(movimientoDetalle);
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
    }
}
