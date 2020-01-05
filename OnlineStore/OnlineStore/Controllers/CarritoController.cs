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
            IEnumerable<CarritoItem> items;
            if (userEmail != null)
            {
                items = db.CarritoItems.Where(x => x.UserEmail == userEmail).ToList();
            }
            else
            {
                items = db.CarritoItems.Where(x => x.UserEmail == "").ToList();
            }
            return View(items);
        }

        public ActionResult AgregarItem(int? productoId)
        {
            string userEmail = User.Identity.Name;
            if (productoId != null)
            {
                CarritoItem item = itemExistente(productoId, userEmail);
                if (item == null)
                {
                    item = crearItem(productoId);
                    db.CarritoItems.Add(item);
                }
                else
                {
                    item.Cantidad++;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges(); 
            }
            return RedirectToAction("Index", new { userEmail = userEmail });
        }

        private CarritoItem itemExistente(int? productoId, string userEmail)
        {
            return db.CarritoItems.Where(x => x.ProductoId == productoId && x.UserEmail == userEmail).SingleOrDefault();
        }

        private IEnumerable<CarritoItem> itemsDelCarrito(string userEmail)
        {
            IEnumerable<CarritoItem> items;
            if (userEmail != null)
            {
                items = db.CarritoItems.Where(x => x.UserEmail == userEmail).ToList();
            }
            else
            {
                items = db.CarritoItems.Where(x => x.UserEmail == "").ToList();
            }
            return items;
        }

        private CarritoItem crearItem(int? productoId)
        {
                return new CarritoItem()
                {
                    Producto = db.Productos.Find(productoId),
                    Cantidad = 1,
                    UserEmail = User.Identity.Name,
                };     
        }

        public ActionResult SumarUno(int? carritoItemId)
        {
            if (carritoItemId != null)
            {
                CarritoItem item = db.CarritoItems.Find(carritoItemId);
                item.Cantidad++;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { userEmail = User.Identity.Name });
        }

        public ActionResult RestarUno(int? carritoItemId)
        {
            if (carritoItemId != null)
            {
                
                CarritoItem item = db.CarritoItems.Find(carritoItemId);
                if (item.Cantidad > 1)
                {
                    item.Cantidad--;
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.CarritoItems.Remove(item);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { userEmail = User.Identity.Name });
        }

        public ActionResult BorrarUno(int? carritoItemId)
        {
            if (carritoItemId != null)
            {
                CarritoItem item = db.CarritoItems.Find(carritoItemId);
                db.CarritoItems.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { userEmail = User.Identity.Name });
        }

        public ActionResult BorrarTodos(string userEmail)
        {
            IEnumerable<CarritoItem> items = itemsDelCarrito(userEmail);
            foreach(var item in items)
            {
                db.CarritoItems.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { userEmail = userEmail });
        }
    }
}
