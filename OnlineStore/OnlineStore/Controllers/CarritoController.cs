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

        public ActionResult Index(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items = itemsDelCarrito(usuarioEmail);
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
            return db.CarritoItems.Where(x => x.ProductoId == productoId && x.UsuarioEmail == usuarioEmail)
                                  .SingleOrDefault();
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
                items = obtenerItemsHuerfanos();
            }
            return items;
        }

        private List<OrdenDetalle> generarOrdenDetalles(string usuarioEmail)
        {
            IEnumerable<CarritoItem> itemsCarrito = itemsDelCarrito(usuarioEmail);
            List<OrdenDetalle> itemsOrden = new List<OrdenDetalle>();
            foreach (var item in itemsCarrito)
            {
                OrdenDetalle ordenDetalle = crearOrdenDetalle(item);
                itemsOrden.Add(ordenDetalle);
                db.CarritoItems.Remove(item);
            }
            return itemsOrden;
        }

        private OrdenDetalle crearOrdenDetalle(CarritoItem item)
        {
            return new OrdenDetalle()
            {
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
        [Authorize]
        public ActionResult FinalizarCompra(string usuarioEmail)
        {
            //Si el usuarioEmail llega null significa que la compra la hizo un guest
            if (usuarioEmail == null)
            {
                usuarioEmail = User.Identity.Name;
                actualizarItemsHuerfanos(usuarioEmail);
            }
            Orden orden = new Orden()
            {
                UsuarioEmail = usuarioEmail,
                FechaCompra = DateTime.Now,
                Detalles = generarOrdenDetalles(usuarioEmail),
            };
            db.Ordenes.Add(orden);
            db.SaveChanges();
            return RedirectToAction("Gracias", new { id = orden.OrdenId });
        }
        public ActionResult Gracias(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        private IEnumerable<CarritoItem> obtenerItemsHuerfanos()
        {
            return db.CarritoItems.Where(x => x.UsuarioEmail == "").ToList();
        }
        private void actualizarItemsHuerfanos(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items = obtenerItemsHuerfanos();
            foreach (var item in items)
            {
                item.UsuarioEmail = usuarioEmail;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();
        }
    }
}
