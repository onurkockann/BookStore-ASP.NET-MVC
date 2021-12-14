using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        Model m = new Model();//Veritabanındaki tablolarda bulunan columnları modellemek ve manipüle etmek için her zaman kullanılacak olan geçici
                              //Model nesnesi global olarak tanımlandı.
                              // GET: Book
        
        
        public ActionResult Detail(int? id)
        {
            author getAuth = m.authors.FirstOrDefault(x => x.id == id);//Yazar aranıyor.

            if (getAuth != null)//Yazar mevcut ise ilgili detay sayfasına yönlendiriliyor.
                return View(getAuth);
            else
            {
                TempData["0"] = "Yazar bulunamadı.";//Index sayfasında alert vermek için geçici data açılıyor ve içine mesajı yazılıyor.
                return RedirectToAction("Index", "Home");
            }
        }
    }
}