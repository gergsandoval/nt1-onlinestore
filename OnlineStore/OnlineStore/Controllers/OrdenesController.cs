﻿using System;
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
    [Authorize(Roles = "Admin")]
    public class OrdenesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ordenes
        public ActionResult Index()
        {
            var ordenes = db.Ordenes.ToList();
            return View(ordenes);
        }

        public ActionResult Details(int? id)
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







    }
}
