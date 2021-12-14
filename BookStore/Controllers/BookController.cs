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

        [HttpGet]
        public ActionResult ConfirmPayout(decimal[] isbn, int[] quantity)
        {
            //Sayfada bir uyarı mesajı mevcutmu kontrolü
            if (TempData["0"] != null)
            {
                ViewBag.FailMsg = TempData["0"];
                isbn = (decimal[])TempData["isbn"];
                quantity = (int[])TempData["quantity"];
            }
                

            List<book> getBooks = new List<book>();//Boş bir kitap modeli listesi oluşturuluyor.

            int qtCount = 0;//kitaplarla birlikte adetlerinde elde edilmesi için bir indis olusturuluyor.
            double totPrice = 0;//Toplam fiyat
            foreach(decimal item in isbn)//Gelen kitap isbnleri içinde gezerek;
            {
                getBooks.Add(m.books.FirstOrDefault(x => x.isbn == item));//isbne sahip kitabı ekle
                getBooks[qtCount].stock = quantity[qtCount];//sipariş adeti
                totPrice += Convert.ToDouble(getBooks[qtCount].price) * quantity[qtCount];//Toplam fiyata ekle
                qtCount++;
            }

            ViewBag.TotalPrice = totPrice;//Frontende gönderilmek üzere toplam fiyatı ata.
            ViewBag.BookList = getBooks;//Sipariş edilecek kitap(lar) gönderiliyor.
            creditCard card = new creditCard();//Kredi kartı bilgileri isteneceğinden dolayı boş bir kredi kartı nesnesi oluştur.
            return View(card);
        }

        [HttpPost]
        public ActionResult ConfirmPayout(decimal[] isbn, int[] quantity, creditCard card, String Adres)
        {
            List<book> getBooks = new List<book>();//Boş bir kitap modeli listesi oluşturuluyor.

            //STOK KONTROLU VE SATIN ALINACAK KITAPLARIN ADETLERIYLE BIRLIKTE ELDE EDILMESI;
            int stkCounter = 0; //adet,stoğa uyum sağlamayan bir kitap olduğunda tetikleme yapılması için.
            int qtCount = 0;//kitaplarla birlikte adetlerinde elde edilmesi için bir indis olusturuluyor.
            int tempStk = 0;//Stok değerini kaybetmemek için geçici değişken.
            double totPrice = 0;//Toplam fiyat
            foreach (decimal item in isbn)//Gelen kitap isbnleri içinde gezerek;
            {
                getBooks.Add(m.books.FirstOrDefault(x => x.isbn == item));//isbne sahip kitabı ekle
                if (getBooks[qtCount].stock >= quantity[qtCount])
                {
                    tempStk = (int)getBooks[qtCount].stock;//Mevcut stok adeti tutuluyor.
                    getBooks[qtCount].stock = quantity[qtCount];//sipariş adeti
                    totPrice += Convert.ToDouble(getBooks[qtCount].price) * quantity[qtCount];//Toplam fiyata ekle

                    quantity[qtCount] = tempStk;//quantity listesi, asıl stok adeti için kullanıma geçiriliyor.
                    qtCount++;
                }
                else
                    stkCounter++;//Adedi, stok sayısına uymayan bir veya birden fazla kitap var.
            }
            if(stkCounter > 0)//Stok sorunundan dolayı alım gerçekleşemiyor.
            {
                TempData["0"] = "Stok yetersiz, lütfen tekrar deneyin veya adet güncellemesi yapın.";
                TempData["isbn"] = isbn;
                TempData["quantity"] = quantity;
                return RedirectToAction("ConfirmPayout");
            }


            //Bilgileri verilen kart sistemde sorgulanıyor.
            creditCard KK = m.creditCards.FirstOrDefault(x => x.cardNumber == card.cardNumber
            && x.cvv == card.cvv && x.expireDate == card.expireDate.Date
            && x.user.email == HttpContext.User.Identity.Name);

            if(KK != null)
            {
                if (KK.balance < totPrice) // Kart mevcut fakat bakiye yetersiz.
                {
                    TempData["0"] = "Kredi kartınızın bakiyesi yetersiz.";
                    TempData["isbn"] = isbn;
                    TempData["quantity"] = quantity;
                    return RedirectToAction("ConfirmPayout");
                }

                order siparis = new order();
                siparis.user = m.users.FirstOrDefault(x => x.email == HttpContext.User.Identity.Name);//Mevcut kullanıcı=order sahibi.
                siparis.totalPrice = totPrice; //Siparişin toplam bedeli.
                siparis.dateAndTime = DateTime.Now; //Sipariş tarihi=Şuan.
                siparis.address = Adres;//Adres bilgisi.
                m.orders.Add(siparis);//Sipariş ekleniyor.

                stkCounter = 0;
                foreach(book item in getBooks)//Sipariş verilen kitaplar içerisinde gez;
                {
                    //Kitaplar ve sipariş arasında N to N ilişki dolayısı ile siparişe ait kitaplar ordereditems tablosuna ekleniyor.
                    orderedItem siparisUruns = new orderedItem();
                    siparisUruns.book = item;
                    siparisUruns.order = siparis;
                    siparisUruns.bookQty = item.stock;
                    m.orderedItems.Add(siparisUruns);

                    //Kitap üzerinde yapılacak değişiklikler için varlığın kendisi elde edilir.
                    book getBook = m.books.FirstOrDefault(x => x.isbn == item.isbn);
                    getBook.stock = quantity[stkCounter] - getBook.stock;//Sipariş adeti kadar stok düşürülür.
                    getBook.bodyCount += 1;
                    stkCounter++;

                    m.SaveChanges();
                }

                KK.balance -= Convert.ToDouble(totPrice);//Kredi kartının bakiyesi düşürülüyor.

                m.SaveChanges();
                TempData["1"] = "Siparişiniz başarıyla onaylanmıştır.";
                return RedirectToAction("Orders", "User");
            }
            else
            {
                TempData["0"] = "Kredi kartı bilgileriniz doğrulanamadı. Lütfen kart bilgilerinizi doğru girerek tekrar deneyin.";
                TempData["isbn"] = isbn;
                TempData["quantity"] = quantity;
                return RedirectToAction("ConfirmPayout");
            }
        }
    }
}