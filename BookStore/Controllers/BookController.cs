using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        Model m = new Model();//Veritabanındaki tablolarda bulunan columnları modellemek ve manipüle etmek için her zaman kullanılacak olan geçici
                              //Model sınıfını global olarak tanımlandı.
        // GET: Book
        public ActionResult Detail(decimal? isbn)
        {
            book getKitap = m.books.FirstOrDefault(x => x.isbn == isbn);//Kitap aranıyor.

            if (getKitap != null)//Kitap mevcut ise ilgili detay sayfasına yönlendiriliyor.
                return View(getKitap);
            else
            {
                TempData["0"] = "Kitap bulunamadı.";//Index sayfasında alert vermek için geçici data açılıyor ve içine mesajı yazılıyor.
                return RedirectToAction("Index", "Home");
            }
        }
    }
}