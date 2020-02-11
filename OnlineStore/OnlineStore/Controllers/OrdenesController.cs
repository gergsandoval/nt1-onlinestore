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
    [Authorize]
    public class OrdenesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ordenes
        public ActionResult Index()
        {
            string usuarioEmail = User.Identity.Name;
            var ordenes = obtenerOrdenes(usuarioEmail);
            return View(ordenes);
        }

        private IEnumerable<Orden> obtenerOrdenes(string usuarioEmail)
        {
            var ordenes = db.Ordenes.Where(x => x.UsuarioEmail == usuarioEmail).ToList();
            if (User.IsInRole("Admin"))
            {
                ordenes = db.Ordenes.ToList();
            }
            return ordenes;
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = obtenerOrden(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        private Orden obtenerOrden(int? id)
        {
            Orden orden;
            if (User.IsInRole("Admin"))
            {
                orden = db.Ordenes.Find(id);
            }
            else
            {
                orden = db.Ordenes.Where(x => x.OrdenId == id && x.UsuarioEmail == User.Identity.Name).SingleOrDefault();
            }
            return orden;
        }







    }
}
