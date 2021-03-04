using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TomanTodos.Models;

namespace TomanTodos.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ApplicationDbContext db;

        //public HomeController(ApplicationDbContext context)
        //{
        //    db = context;
        //}

        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //[Authorize]
        public ActionResult Index()
        {
            var productos = db.Productos.Include(s => s.Stock.Select(suc => suc.Sucursal));

            ViewBag.Movimientos = db.MovimientosDetalle
                                    .Include(s => s.Sucursal)
                                    .OrderByDescending(f => f.FechaMovimiento)
                                    .Take(10);

            return View(productos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}