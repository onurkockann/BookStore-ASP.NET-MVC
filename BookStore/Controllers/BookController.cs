using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Web.Security;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        Model m = new Model();//Veritabanındaki tablolarda bulunan columnları modellemek ve manipüle etmek için her zaman kullanılacak olan geçici
                              //Model nesnesi global olarak tanımlandı.

        // GET: Book
        [AllowAnonymous]//Login olmamış kullanıcılara da görünmesi izin verilir.
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

        public ActionResult AddCart(decimal? isbn)
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.
            cart checkCart = m.carts.FirstOrDefault(x => x.bookID == isbn && x.userID == u.userId);
            if(checkCart != null)
            {
                TempData["0"] = "Bu ürün zaten sepetinizde mevcut.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                cart nCart = new cart();
                nCart.userID = u.userId;
                nCart.bookID = isbn;
                m.carts.Add(nCart);
                m.SaveChanges();

                TempData["1"] = "Ürün başarıyla sepetinize eklendi.";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SilCart(decimal? isbn)
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.
            cart checkCart = m.carts.FirstOrDefault(x => x.bookID == isbn && x.userID == u.userId);
            if (checkCart == null)
            {
                TempData["0"] = "Bu ürün zaten sepetinizde bulunmuyor.";
                return RedirectToAction("MyCart", "User");
            }
            else
            {
                m.carts.Remove(checkCart);
                m.SaveChanges();

                TempData["1"] = "Ürün başarıyla sepetinizden kaldırıldı.";
                return RedirectToAction("MyCart", "User");
            }
        }

        [HttpPost]
        public ActionResult ProceedCart(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult DirectBuy(decimal? isbn)
        {
            return View();
        }
    }
}