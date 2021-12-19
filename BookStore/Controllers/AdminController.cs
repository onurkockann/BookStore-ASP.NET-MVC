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

        public void setVals()
        {
            //Yazar,Kategori ve Yayınevlerini çekme
            List<author> aut = m.authors.ToList();
            List<genre> kat = m.genres.ToList();
            List<publisher> pub = m.publishers.ToList();

            //Boş listeler oluşturulur.
            List<SelectListItem> itemsAut = new List<SelectListItem>();
            List<SelectListItem> itemsKat = new List<SelectListItem>();
            List<SelectListItem> itemsPub = new List<SelectListItem>();

            //Boş listelere text olarak isimler, value olarak ise idler eklenir.
            foreach (author itemA in aut)
                itemsAut.Add(new SelectListItem { Text = itemA.aName, Value = itemA.id.ToString() });
            foreach (genre itemK in kat)
                itemsKat.Add(new SelectListItem { Text = itemK.gName, Value = itemK.id.ToString() });
            foreach (publisher itemP in pub)
                itemsPub.Add(new SelectListItem { Text = itemP.pName, Value = itemP.id.ToString() });

            //Viewbaglere atılır.
            ViewBag.Yazarlar = itemsAut;
            ViewBag.Kategoriler = itemsKat;
            ViewBag.Yayinevleri = itemsPub;
        }

        // GET: Admin
        public ActionResult Index()
        {
            if(AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");


            if (TempData["0"] != null)
                ViewBag.FailMsg = TempData["0"];
            else if (TempData["1"] != null)
                ViewBag.DoneMsg = TempData["1"];

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

        [HttpPost]
        public ActionResult YazarSil(int id)
        {
            Model m = new Model();
            author k = m.authors.FirstOrDefault(x => x.id == id);

            try
            {
                m.authors.Remove(k);
                m.SaveChanges();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Yazarlar");
        }

        [HttpGet]
        public ActionResult Kitaplar()
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");

            setVals();//Yazar,Kategori ve Yayınevlerini çekme
            book kitap = new book();
            List<book> bks = m.books.ToList();
            bks.Reverse();
            ViewBag.Kitaplar = bks;

            return View(kitap);
        }

        [HttpPost]
        public ActionResult KitapSil(decimal id)
        {
            Model m = new Model();
            book k = m.books.FirstOrDefault(x => x.isbn == id);

            try
            {
                m.books.Remove(k);
                m.SaveChanges();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Kitaplar");
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

        [HttpPost]
        public ActionResult KategoriSil(int id)
        {
            Model m = new Model();
            genre k = m.genres.FirstOrDefault(x => x.id == id);

            try
            {
                m.genres.Remove(k);
                m.SaveChanges();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Kategoriler");
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

        [HttpPost]
        public ActionResult YayineviSil(int id)
        {
            Model m = new Model();
            publisher k = m.publishers.FirstOrDefault(x => x.id == id);

            try
            {
                m.publishers.Remove(k);
                m.SaveChanges();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Yayinevleri");
        }

        [HttpGet]
        public ActionResult Update(int op, decimal id)
        {
            if (AuthControl() == 1) //Authorize kontrolü yapılır,admin harici user erişimi engellenir.
                return RedirectToAction("Index", "Home");


            switch (op)
            {
                case 1:
                    // Yazar işlemi
                    author yazar = m.authors.FirstOrDefault(x => x.id == id);
                    ViewBag.Op = 1;
                    ViewBag.Id = yazar.id;
                    return View(yazar);
                case 2:
                    // Kitap işlemi
                    setVals();//Yazar,Kategori ve Yayınevlerini çekme

                    book kitap = m.books.FirstOrDefault(x => x.isbn == id);
                    ViewBag.Op = 2;
                    ViewBag.Id = kitap.isbn;
                    return View(kitap);
                case 3:
                    // Kategori işlemi
                    genre kategori = m.genres.FirstOrDefault(x => x.id == id);
                    ViewBag.Op = 3;
                    ViewBag.Id = kategori.id;
                    return View(kategori);
                case 4:
                    // Yayınevi işlemi
                    publisher yayinevi = m.publishers.FirstOrDefault(x => x.id == id);
                    ViewBag.Op = 4;
                    ViewBag.Id = yayinevi.id;
                    return View(yayinevi);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EkleYazar(author yazar)
        {
            author aut = m.authors.FirstOrDefault(x => x.id == yazar.id);

            if(aut != null)//Yazar mevcut ise güncelleme işlemi yapılacak.
            {
                aut.aName = yazar.aName;
                aut.aDescription = yazar.aDescription;
                aut.aImage = yazar.aImage;
                aut.aLink = yazar.aLink;

                m.SaveChanges();

                TempData["1"] = "Yazar bilgileri başarıyla düzenlendi.";
                return RedirectToAction("Index");
            }
            else//Mevcut olmayan bir yazar ise, ekleme yapılacak.
            {
                m.authors.Add(yazar);
                m.SaveChanges();

                TempData["1"] = "Yazar başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public ActionResult EkleKitap(book kitap, String Yazarlar, String Kategoriler, String Yayinevleri)
        {
            int yazarId = Convert.ToInt32(Yazarlar);
            int kategoriId = Convert.ToInt32(Kategoriler);
            int yayineviId = Convert.ToInt32(Yayinevleri);

            book ktp = m.books.FirstOrDefault(x => x.isbn == kitap.isbn);

            if (ktp != null)//Kitap mevcut ise güncelleme işlemi yapılacak.
            {
                ktp.bName = kitap.bName;
                ktp.detail = kitap.detail;
                ktp.bImage = kitap.bImage;
                ktp.bDate = kitap.bDate;
                ktp.price = kitap.price;
                ktp.stock = kitap.stock;
                ktp.bodyCount = 0;

                ktp.author1 = m.authors.FirstOrDefault(x => x.id == yazarId);
                ktp.genre1 = m.genres.FirstOrDefault(x => x.id == kategoriId);
                ktp.publisher1 = m.publishers.FirstOrDefault(x => x.id == yayineviId);

                m.SaveChanges();

                TempData["1"] = "Kitap bilgileri başarıyla düzenlendi.";
                return RedirectToAction("Index");
            }
            else//Mevcut olmayan bir Kitap ise, ekleme yapılacak.
            {
                kitap.author1 = m.authors.FirstOrDefault(x=> x.id == yazarId);
                kitap.genre1 = m.genres.FirstOrDefault(x => x.id == kategoriId);
                kitap.publisher1 = m.publishers.FirstOrDefault(x => x.id == yayineviId);

                m.books.Add(kitap);
                m.SaveChanges();

                TempData["1"] = "Kitap başarıyla eklendi.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult EkleKategori(genre kategori)
        {
            genre kat = m.genres.FirstOrDefault(x => x.id == kategori.id);

            if (kat != null)//Yazar mevcut ise güncelleme işlemi yapılacak.
            {
                kat.gName = kategori.gName;
                kat.gDescription = kategori.gDescription;

                m.SaveChanges();

                TempData["1"] = "Kategori bilgileri başarıyla düzenlendi.";
                return RedirectToAction("Index");
            }
            else//Mevcut olmayan bir yazar ise, ekleme yapılacak.
            {
                m.genres.Add(kategori);
                m.SaveChanges();

                TempData["1"] = "Kategori başarıyla eklendi.";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult EkleYayinevi(publisher yayinevi)
        {
            publisher yye = m.publishers.FirstOrDefault(x => x.id == yayinevi.id);

            if (yye != null)//Yazar mevcut ise güncelleme işlemi yapılacak.
            {
                yye.pName = yayinevi.pName;
                yye.pLogo = yayinevi.pLogo;

                m.SaveChanges();

                TempData["1"] = "Yayınevi bilgileri başarıyla düzenlendi.";
                return RedirectToAction("Index");
            }
            else//Mevcut olmayan bir yazar ise, ekleme yapılacak.
            {
                m.publishers.Add(yayinevi);
                m.SaveChanges();

                TempData["1"] = "Yayınevi başarıyla eklendi.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult ConfirmOrder(int id)
        {
            AuthControl();//İzinsiz erişim kontrolü.

            order siparis = m.orders.FirstOrDefault(x => x.orderId == id);
            siparis.status = 1;

            List<orderedItem> siparisUruns = m.orderedItems.Where(x => x.orderId == id).ToList();//Bu siparişe ait işleme alınan ürün-siparişler çekilir.
            foreach(orderedItem item in siparisUruns)
            {
                book kitap = m.books.FirstOrDefault(x => x.isbn == item.bookId);
                kitap.stock -= item.bookQty;//Sipariş adeti kadar kitabın stoğu düşürülür.
                kitap.bodyCount += 1; //Kitabın satış sayısı değeri 1 arttırılır.

                m.SaveChanges(); //Her döngüde işlem kaydedilir.
            }

            creditCard kkart = m.creditCards.FirstOrDefault(x => x.userId == siparis.userId);//Ödeyen kişinin kredi kartı elde edilir.
            kkart.balance -= siparis.totalPrice;//Bakiyesi toplam fiyat kadar düşürülür.

            m.SaveChanges();//Değişiklikler kaydedilir

            TempData["1"] = "Sipariş başarıyla onaylandı.";
            return RedirectToAction("Index");
        }

        public ActionResult CancelOrder(int id)
        {
            AuthControl();//İzinsiz erişim kontrolü.

            order siparis = m.orders.FirstOrDefault(x => x.orderId == id);//Sipariş çekilir.
            m.orders.Remove(siparis);//Kaldırılır.
            m.SaveChanges();//Değişiklikler kayıt edilir.

            TempData["1"] = "Sipariş başarıyla kaldırıldı.";
            return RedirectToAction("Index");
        }
    }
}