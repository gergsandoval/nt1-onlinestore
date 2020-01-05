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
        public ActionResult Index(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items;
            if (usuarioEmail != null)
            {
                items = db.CarritoItems.Where(x => x.UsuarioEmail == usuarioEmail).ToList();
            }
            else
            {
                items = db.CarritoItems.Where(x => x.UsuarioEmail == "").ToList();
            }
            return View(items);
        }

        public ActionResult AgregarItem(int? productoId)
        {
            string usuarioEmail = User.Identity.Name;
            if (productoId != null)
            {
                CarritoItem item = itemExistente(productoId, usuarioEmail);
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
            return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
        }

        private CarritoItem itemExistente(int? productoId, string usuarioEmail)
        {
            return db.CarritoItems.Where(x => x.ProductoId == productoId && x.UsuarioEmail == usuarioEmail).SingleOrDefault();
        }

        private IEnumerable<CarritoItem> itemsDelCarrito(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items;
            if (usuarioEmail != null)
            {
                items = db.CarritoItems.Where(x => x.UsuarioEmail == usuarioEmail).ToList();
            }
            else
            {
                items = db.CarritoItems.Where(x => x.UsuarioEmail == "").ToList();
            }
            return items;
        }

        private void agregarItemsDelCarritoALaOrden(string usuarioEmail, int orderId)
        {
            IEnumerable<CarritoItem> items = itemsDelCarrito(usuarioEmail);
            foreach (var item in items)
            {
                OrdenDetalle ordenDetalle = crearOrdenDetalle(item, orderId);
                db.OrdenDetalle.Add(ordenDetalle);
                db.CarritoItems.Remove(item);
                db.SaveChanges();
            }
        }

        private OrdenDetalle crearOrdenDetalle(CarritoItem item, int orderId)
        {
            return new OrdenDetalle()
            {
                OrdenId = orderId,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
            };
        }

        private CarritoItem crearItem(int? productoId)
        {
                return new CarritoItem()
                {
                    Producto = db.Productos.Find(productoId),
                    Cantidad = 1,
                    UsuarioEmail = User.Identity.Name,
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
            return RedirectToAction("Index", new { usuarioEmail = User.Identity.Name });
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
            return RedirectToAction("Index", new { usuarioEmail = User.Identity.Name });
        }

        public ActionResult BorrarUno(int? carritoItemId)
        {
            if (carritoItemId != null)
            {
                CarritoItem item = db.CarritoItems.Find(carritoItemId);
                db.CarritoItems.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { usuarioEmail = User.Identity.Name });
        }

        public ActionResult BorrarTodos(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items = itemsDelCarrito(usuarioEmail);
            foreach(var item in items)
            {
                db.CarritoItems.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
        }

        public ActionResult FinalizarCompra(string usuarioEmail)
        {
            Orden orden = new Orden()
            {
                UsuarioEmail = usuarioEmail,
                FechaCompra = DateTime.Now,
            };
            db.Ordenes.Add(orden);
            db.SaveChanges();
            agregarItemsDelCarritoALaOrden(usuarioEmail, orden.OrdenId);
            return View("Gracias");
        }
    }
}
