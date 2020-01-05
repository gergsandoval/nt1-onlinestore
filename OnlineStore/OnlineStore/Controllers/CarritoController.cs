using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carrito
        public ActionResult Index(string userEmail)
        {
            return View(db.CarritoItems.Where(x => x.UserEmail == userEmail).ToList());
        }

        public ActionResult Agregar(int? productoId)
        {
            if (productoId != null)
            {
                CarritoItem item = new CarritoItem()
                {
                    Producto = db.Productos.Find(productoId),
                    Cantidad = 1,
                    UserEmail = User.Identity.Name,
                };
                db.CarritoItems.Add(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { userEmail = User.Identity.Name });
        }

    }
}
