using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        Model m = new Model();//Veritabanındaki tablolarda bulunan columnları modellemek ve manipüle etmek için her zaman kullanılacak olan geçici
                              //Model sınıfını global olarak tanımlandı.

        public ActionResult Index()
        {
            //Alert kodu incelemesi yapılıyor. Mevcut bir alert var ise ana sayfada kullanıcıya çıkartılıyor.
            if (TempData["0"] != null)
                ViewBag.Alert = TempData["0"];

            List<book> Kitaplar = m.books.ToList();

            return View(Kitaplar);
        }

        [HttpPost]
        public ActionResult FindBook(String bookName)
        {
            //Aranma parametresine göre kitap adının içinde geçtiği SQL sorgusu uygulanıyor.
            //List<book> getBooks = m.books.SqlQuery("SELECT * FROM books WHERE bName LIKE '" + bookName + "'").ToList();
            List<book> getBooks = m.books.Where(x => x.bName.Contains(bookName)).ToList();

            if (getBooks.Count == 0)//Listenin sayısı 0 ise ilgili alert mesajıyla index sayfasına yönlendirilir.
            {
                TempData["0"] = "Aradığınız kitap bulunamadı.";
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", getBooks);
            }


        }
    }
}