using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Web.Security;

namespace BookStore.Controllers
{
    public class OrderController : Controller
    {
        Model m = new Model();//Veritabanındaki tablolarda bulunan columnları modellemek ve manipüle etmek için her zaman kullanılacak olan geçici
                              //Model nesnesi global olarak tanımlandı.

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CancelOrder(int id)
        {
            order getSprs = m.orders.FirstOrDefault(x => x.orderId == id);
            List<orderedItem> getUSprs = m.orderedItems.Where(x => x.orderId == getSprs.orderId).ToList();

            m.orderedItems.RemoveRange(getUSprs);
            m.orders.Remove(getSprs);
            m.SaveChanges();

            TempData["1"] = "Siparişiniz başarıyla iptal edilmiştir.";
            return RedirectToAction("Orders","User");
        }
    }
}