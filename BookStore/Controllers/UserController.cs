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
                if (u.isAdmin != 1)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Admin");
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
        public ActionResult Register(user kat, String pwt, String KVKK, String Admin)
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
                if (Admin != null && Admin == "ok")
                    kat.isAdmin = 1;

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

        // GET: Profil
        public ActionResult Profil()
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Sisteme login edilmiş kullanıcı varlığı.

            if ((string)TempData["Y"] == "1")
                ViewBag.DoneMsg = "Profil bilgileri başarıyla güncellendi.";
            else if ((string)TempData["PwErr"] != null)
                ViewBag.FailMsg = TempData["PwErr"];
            else if ((string)TempData["PwDone"] != null)
                ViewBag.DoneMsg = TempData["PwDone"];
            else if ((string)TempData["PwOErr"] != null)
                ViewBag.FailMsg = TempData["PwOErr"];
            else if ((string)TempData["DG"] != null)
                ViewBag.DoneMsg = TempData["DG"];
            else if ((string)TempData["DGErr"] != null)
                ViewBag.FailMsg = TempData["DGErr"];


            return View(u);
        }

        [HttpGet]
        public ActionResult GuncelleUser(int id)
        {
            user k = m.users.FirstOrDefault(x => x.userId == id);

            if (k != null)//Kullanıcı var ise
            {
                if (k != m.users.FirstOrDefault(x=> x.email == HttpContext.User.Identity.Name))//Mevcut işlemi yapan kullanıcı değil ise;
                {
                    TempData["BG"] = "1";
                    return RedirectToAction("Index", "Home");
                }
            }
            else//Kullanıcı yok ise, izinsiz erişim denemesi yapılmıştır.
            {
                TempData["BG"] = "1";
                return RedirectToAction("Index", "Home");
            }

            if ((string)TempData["Y"] == "2")
                ViewBag.FailMsg = "Bu maile sahip bir kullanıcı var.Lütfen başka bir mail adresi deneyin.";
            else if ((string)TempData["MErr"] != null)
                ViewBag.FailMsg = TempData["MErr"];

            //Güvenlik caselerinden geçildikten sonra kullanıcı modeli güncelleme formuna gönderilir.
            return View(k);
        }

        [HttpPost]
        public ActionResult GuncelleUser(user kat)
        {
            user k = m.users.FirstOrDefault(x => x.userId == kat.userId);


            user k2 = m.users.FirstOrDefault(x => x.email == kat.email && x.userId != kat.userId);
            if (k2 == null) // email uygun.
            {
                k.email = kat.email;
                k.firstname = kat.firstname;
                k.lastname = kat.lastname;
                k.phone = kat.phone;
                k.birthday = kat.birthday;

                m.SaveChanges();
                FormsAuthentication.SetAuthCookie(kat.email, true);
                TempData["Y"] = "1";
                return RedirectToAction("Profil", "User");
            }
            else
            {
                TempData["Y"] = "2";
                return RedirectToAction("GuncelleUser", new { id = kat.userId });
            }
        }

        [HttpPost]
        public ActionResult ChgPw(String pwold, String pwnew1, String pwnew2)
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.

            if (!BCrypt.Net.BCrypt.Verify(pwold, u.password))//Onay için kullanıcı şifresini doğru girdimi?
            {
                TempData["PwOErr"] = "Eski şifrenizi yalnış girdiniz.";
                return RedirectToAction("Profil");
            }

            if (pwnew1 != pwnew2)//İki defa girilen yeni şifreler birbirine eşitmi?
            {
                TempData["PwErr"] = "Yeni şifreler birbirine eşit değil.";
                return RedirectToAction("Profil");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(pwnew1);//Password hashlenir.
            u.password = passwordHash;
            m.SaveChanges();
            TempData["PwDone"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction("Profil");
        }

        public ActionResult Orders()
        {
            if (TempData["1"] != null)
                ViewBag.DoneMsg = TempData["1"];

            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.

            //Kullanıcıya ait bütün siparişler elde ediliyor;
            List<order> siparisler = m.orders.Where(x => x.userId == u.userId).ToList();
            siparisler.Reverse();
            return View(siparisler);
        }

        public ActionResult OrderDetail(int id)
        {
            //Order idsi gelen parametre ile bu orderı içeren ürünler elde ediliyor;
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.
            List<orderedItem> siparisUruns = m.orderedItems.Where(x => x.orderId == id).ToList();
            return View(siparisUruns);
        }

        public ActionResult MyCart()
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.
            List<cart> sepet = m.carts.Where(x => x.userID == u.userId).ToList();
            return View(sepet);
        }

        public ActionResult ClearCart()
        {
            user u = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Login olmuş mevcut kullanıcı alınıyor.
            List<cart> sepet = m.carts.Where(x => x.userID == u.userId).ToList();
            m.carts.RemoveRange(sepet);
            m.SaveChanges();

            ViewBag.Done = "Sepetiniz başarıyla temizlendi.";
            return View("MyCart",new List<cart>());
        }
    }
}