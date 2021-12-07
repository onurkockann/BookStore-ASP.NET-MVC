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
            else if (TempData["1"] != null)
                ViewBag.Done = TempData["1"];

            List<book> Kitaplar = m.books.ToList();
            ViewBag.Testo = "sa";

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

        public ActionResult FindKat(int id)
        {
            List<book> getKat = m.books.Where(x => x.genre1.id == id).ToList();//Parametre olarak gelen iddeki kategoriye sahip olan kitaplar.

            if (getKat == null)//Boş ise, ana sayfaya yönlendir ve uyarı göster.
            {
                TempData["0"] = "Kategoriye ait kitap bulunamadı";
                return RedirectToAction("Index");
            }
            else//Kitapları success alert mesajı ile ana sayfaya gönder.
            {
                ViewBag.Done = m.genres.FirstOrDefault(x => x.id == id).gName.ToString() + " kategorisine ait kitaplar listelendi...";
                return View("Index", getKat);
            }
                
        }

        public ActionResult FindPubls(int id)
        {
            List<book> getKat = m.books.Where(x => x.publisher1.id == id).ToList();//Parametre olarak gelen iddeki yayınevine sahip olan kitaplar.

            if (getKat == null)//Boş ise, ana sayfaya yönlendir ve uyarı göster.
            {
                TempData["0"] = "Bu yayınevine ait kitap bulunamadı";
                return RedirectToAction("Index");
            }
            else//Kitapları success alert mesajı ile ana sayfaya gönder.
            {
                ViewBag.Done = m.publishers.FirstOrDefault(x => x.id == id).pName.ToString() + " adlı yayınevine ait kitaplar listelendi...";
                return View("Index", getKat);
            }

        }
    }
}