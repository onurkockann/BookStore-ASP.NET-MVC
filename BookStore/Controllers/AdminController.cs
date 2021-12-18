using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Web.Security;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        Model m = new Model();//Veritabanındaki tablolarda bulunan columnları modellemek ve manipüle etmek için her zaman kullanılacak olan geçici
                              //Model nesnesi global olarak tanımlandı.

        public int AuthControl()
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.
            if (u.isAdmin != 1)
            {
                TempData["BG"] = "1";
                return 1;
                
            }
            return 0;
        }

        // GET: Admin
        public ActionResult Index()
        {
            if(AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        public ActionResult Users()
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");


            user us = new user();
            List<user> usrs = m.users.ToList();//Adminin görüntülemesi için bütün kullanıcılar listelenir ve frontende gönderilir.
            usrs.Reverse();
            ViewBag.Users = usrs;

            return View(us);
        }

        [HttpGet]
        public ActionResult AllOrders()
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");


            List<orderedItem> odt = m.orderedItems.ToList();//Adminin görüntülemesi için bütün siparişler listelenir ve frontende gönderilir.
            odt.Reverse();
            ViewBag.Orders = odt;

            return View();
        }

        [HttpGet]
        public ActionResult Yazarlar()
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");

            author yazar = new author();
            List<author> yzs = m.authors.ToList();
            yzs.Reverse();
            ViewBag.Yazarlar = yzs;

            return View(yazar);
        }

        [HttpGet]
        public ActionResult Kitaplar()
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");

            book kitap = new book();
            List<book> bks = m.books.ToList();
            bks.Reverse();
            ViewBag.Kitaplar = bks;

            return View(kitap);
        }

        [HttpGet]
        public ActionResult Kategoriler()
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");

            genre kategoriler = new genre();
            List<genre> kats = m.genres.ToList();
            kats.Reverse();
            ViewBag.Kategoriler = kats;

            return View(kategoriler);
        }

        [HttpGet]
        public ActionResult Yayinevleri()
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");

            publisher yye = new publisher();
            List<publisher> yes = m.publishers.ToList();
            yes.Reverse();
            ViewBag.Yayinevleri = yes;

            return View(yye);
        }
    }
}