using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        Model m = new Model();//Veritabanındaki tablolarda bulunan columnları modellemek ve manipüle etmek için her zaman kullanılacak olan geçici
                              //Model sınıfını global olarak tanımlandı.
        [HttpGet]
        [AllowAnonymous]//Login olmamış kullanıcılara da görünmesi izin verilir.
        public ActionResult Login()
        {
            FormsAuthentication.SignOut();//Login sayfasına gelindiği zaman kullanıcılar her durum için logout edilir.

            if ((string)TempData["Y"] == "1")
                ViewBag.FailMsg = "Kullanıcı adı veya parola hatalı";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]//Login olmamış kullanıcılara da görünmesi izin verilir.
        public ActionResult Login(user user)
        {
            user u = m.users.FirstOrDefault(x => x.email == user.email);

            if (u == null || !BCrypt.Net.BCrypt.Verify(user.password, u.password))//Böyle bir mail yok OR şifre uyumsuz durumunda login başarısız.
            {
                TempData["Y"] = "1";
                return RedirectToAction("Login", "User");
            }
            else//Login işlemi gerçekleşir.
            {
                FormsAuthentication.SetAuthCookie(u.email, true);
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]//Login olmamış kullanıcılara da görünmesi izin verilir.
        public ActionResult Register()
        {
            FormsAuthentication.SignOut();

            if ((string)TempData["Y"] == "4")
                ViewBag.FailMsg = "Girdiğiniz şifreler birbirine eşit değil.";
            else if ((string)TempData["Y"] == "6")
                ViewBag.FailMsg = "Bu maile sahip bir kullanıcı var.Lütfen başka bir mail deneyin.";
            else if ((string)TempData["Y"] == "7")
                ViewBag.FailMsg = "Book Store'e kayıt olup kullanmak için kullanım koşullarımızı kabul etmeniz gerekiyor. Detaylı bilgi için gizlilik sözleşmesi, kullanım koşulları ve çerez politikamızı inceleyin.";

            user Usr = new user();
            return View(Usr);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(user kat, String pwt, String KVKK)
        {
            if (kat.password != pwt) // Onay için tekrar istenen şifreler birbirine eşitmi kontrolü
            {
                TempData["Y"] = "4";
                return RedirectToAction("Register");
            }
            if (KVKK == null) // Kullanım koşulları kabul edildimi kontrolü
            {
                TempData["Y"] = "7";
                return RedirectToAction("Register");
            }

            user k = m.users.FirstOrDefault(x => x.email == kat.email);
            if (k == null)//Bu maile sahip bir kullanıcı varmı kontrolü
            {
                kat.isAdmin = 0;//Normal yollar ile kayıt olan kullanıcılar admin olamaz bundan dolayı adminlik yetkisi 0 verilir.
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(kat.password);//Password hashlenir.
                kat.password = passwordHash;

                m.users.Add(kat);
                FormsAuthentication.SetAuthCookie(kat.email, true);


                m.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else//Var ise alert mesajı ile birlikte register sayfasına tekrar yönlendir.
            {
                TempData["Y"] = "6";
                return RedirectToAction("Register");
            }
        }
    }
}