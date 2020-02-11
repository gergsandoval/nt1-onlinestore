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

        public ActionResult Index(string usuarioEmail, bool? error, string descripcion)
        {
            if (usuarioEmail == null)
            {
                usuarioEmail = User.Identity.Name;
            }
            IEnumerable<CarritoItem> items = obteneritemsDelCarrito(usuarioEmail);
            if (error.GetValueOrDefault(false))
            {
                ViewBag.Error = descripcion;
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
                    item = crearItem(productoId, usuarioEmail);
                    bool itemInexistente = item.Producto == null;
                    if (itemInexistente)
                    {
                        string itemExistenteError = "El producto agregado no existe.";
                        return RedirectToAction("Index", new { usuarioEmail = usuarioEmail, error = itemInexistente, descripcion = itemExistenteError });
                    }
                    db.CarritoItems.Add(item);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
                }
                bool stockSuperado = item.Producto.Stock - item.Cantidad <= 0;
                if (stockSuperado)
                {
                    string stockSuperadoDescripcion = "El producto agregado se encuentra sin stock.";
                    return RedirectToAction("Index", new { usuarioEmail = usuarioEmail, error = stockSuperado, descripcion = stockSuperadoDescripcion });
                }
                else
                {
                    item.Cantidad++;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
        }

        private CarritoItem itemExistente(int? productoId, string usuarioEmail)
        {
            return db.CarritoItems.Where(x => x.ProductoId == productoId && x.UsuarioEmail == usuarioEmail)
                                  .SingleOrDefault();
        }

        private IEnumerable<CarritoItem> obteneritemsDelCarrito(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items;
            if (usuarioEmail != "" && usuarioEmail != null)
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
            IEnumerable<CarritoItem> itemsCarrito = obteneritemsDelCarrito(usuarioEmail);
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
                NombreProducto = item.Producto.Nombre,
                PrecioProducto = item.Producto.Precio,
                Cantidad = item.Cantidad,
            };
        }

        private CarritoItem crearItem(int? productoId, string usuarioEmail)
        {
                return new CarritoItem()
                {
                    Producto = db.Productos.Find(productoId),
                    Cantidad = 1,
                    UsuarioEmail = usuarioEmail,
                };     
        }

        public ActionResult SumarUno(int? carritoItemId, string usuarioEmail)
        {
            if (carritoItemId != null)
            {
                CarritoItem item = db.CarritoItems.Find(carritoItemId);
                bool itemInexistente = item == null || !elProductoPerteneceAlCarrito(usuarioEmail, carritoItemId);
                if (itemInexistente)
                {
                    string itemExistenteError = "El producto modificado no existe.";
                    return RedirectToAction("Index", new { usuarioEmail = usuarioEmail, error = itemInexistente, descripcion = itemExistenteError });
                }
                bool stockSuperado = item.Producto.Stock - item.Cantidad <= 0;
                if (stockSuperado)
                {
                    string stockSuperadoError = "El producto agregado se encuentra sin stock.";
                    return RedirectToAction("Index", new { usuarioEmail = usuarioEmail, error = stockSuperado, descripcion = stockSuperadoError });
                } 
                else
                {
                    item.Cantidad++;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
        }

        public ActionResult RestarUno(int? carritoItemId, string usuarioEmail)
        {
            if (carritoItemId != null)
            {
                CarritoItem item = db.CarritoItems.Find(carritoItemId);
                bool itemInexistente = item == null || !elProductoPerteneceAlCarrito(usuarioEmail, carritoItemId);
                if (itemInexistente)
                {
                    string itemExistenteError = "El producto modificado no existe.";
                    return RedirectToAction("Index", new { usuarioEmail = usuarioEmail, error = itemInexistente, descripcion = itemExistenteError });
                }
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
            return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
        }

        public ActionResult BorrarUno(int? carritoItemId, string usuarioEmail)
        {
            if (carritoItemId != null)
            {
                CarritoItem item = db.CarritoItems.Find(carritoItemId);
                bool itemInexistente = item == null || !elProductoPerteneceAlCarrito(usuarioEmail, carritoItemId);
                if (itemInexistente)
                {
                    string itemExistenteError = "El producto borrado no existe.";
                    return RedirectToAction("Index", new { usuarioEmail = usuarioEmail, error = itemInexistente, descripcion = itemExistenteError });
                }
                db.CarritoItems.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
        }

        public ActionResult BorrarTodos()
        {
            string usuarioEmail = User.Identity.Name;
            IEnumerable<CarritoItem> items = obteneritemsDelCarrito(usuarioEmail);
            foreach(var item in items)
            {
                db.CarritoItems.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
        }
        [Authorize]
        public ActionResult FinalizarCompra()
        {
            string usuarioEmail = User.Identity.Name; //Si la propiedad esta vacia significa que la compra la hizo un guest
            if (usuarioEmail == "" || usuarioEmail == null)
            {
                BorrarTodos(); 
                actualizarItemsHuerfanos(usuarioEmail);
                return RedirectToAction("Index", new { usuarioEmail = usuarioEmail });
            }
            bool errorCompra = validarStock(usuarioEmail);
            if (errorCompra)
            {
                string errorCompraDescripcion = "La compra no pudo ser procesada debido que alguno de los articulos se encuentra sin stock.";
                return RedirectToAction("Index", new { usuarioEmail = usuarioEmail, error = errorCompra, descripcion = errorCompraDescripcion });
            }
            else
            {
                descontarStock(usuarioEmail);
                Orden orden = crearOrden(usuarioEmail);
                db.Ordenes.Add(orden);
                db.SaveChanges();
                return RedirectToAction("Gracias", new { id = orden.OrdenId });
            }  
        }

        private Orden crearOrden(string usuarioEmail)
        {
            return new Orden()
            {
                UsuarioEmail = usuarioEmail,
                FechaCompra = DateTime.Now,
                Detalles = generarOrdenDetalles(usuarioEmail),
            };
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

        private void descontarStock(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items = obteneritemsDelCarrito(usuarioEmail);
            foreach(var item in items)
            {
                Producto producto = db.Productos.Find(item.ProductoId);
                producto.Stock -= item.Cantidad;
                db.Entry(producto).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        private bool validarStock(string usuarioEmail)
        {
            IEnumerable<CarritoItem> items = obteneritemsDelCarrito(usuarioEmail);
            bool noHayStock = false;
            int i = 0;
            while (i < items.Count() && !noHayStock)
            {
                Producto producto = db.Productos.Find(items.ElementAt(i).ProductoId);
                if (producto.Stock - items.ElementAt(i).Cantidad < 0)
                {
                    noHayStock = true;
                }
                else
                {
                    i++;
                }
            }
            return noHayStock;
        }

        private bool elProductoPerteneceAlCarrito(string usuarioEmail, int? carritoItemId)
        {
            IEnumerable<CarritoItem> items = obteneritemsDelCarrito(usuarioEmail);
            bool encontreElProducto = false;
            int i = 0;
            while (i < items.Count() && !encontreElProducto)
            {
                var item = items.ElementAt(i);
                if (item.CarritoItemId == carritoItemId)
                {
                    encontreElProducto = true;
                }
                else
                {
                    i++;
                }
            }
            return encontreElProducto;

        }
       
    }
}
