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

        // GET: Admin
        public ActionResult Index()
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.
            if(u.isAdmin != 1)
            {
                TempData["BG"] = "1";
                return RedirectToAction("Index", "Home");
            }


            return View();
        }
    }
}