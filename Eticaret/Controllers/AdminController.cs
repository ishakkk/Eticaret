using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Eticaret.App_Classes;
using Eticaret.Models.Entity;
namespace Eticaret.Controllers
{
    public class AdminController : Controller
    {

        // GET: Admin
        EticaretEntities db = new EticaretEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Urunler()
        {
            return View(Context.Baglanti.Urun.ToList());
        }
        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = Context.Baglanti.Marka.ToList();
            return View();

        }
        [HttpPost]
        public ActionResult UrunEkle(Urun urn)
        {
            //urn.SonKullanmaTarihi = DateTime.Now;
            Context.Baglanti.Urun.Add(urn);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }
        public ActionResult UrunSil(int id)
        {
            var urun = Context.Baglanti.Urun.Find(id);
            Context.Baglanti.Urun.Remove(urun);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }
        public ActionResult Markalar()
        {
            return View(Context.Baglanti.Marka.ToList());
        }
        public ActionResult MarkaEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MarkaEkle(Marka mrk, HttpPostedFileBase fileUpload)
        {
            int resimId = -1;
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                string name = "/Content/MarkaResim/" + Guid.NewGuid() +
                    Path.GetExtension(fileUpload.FileName);
                Bitmap bm = new Bitmap(img, width, height);
                bm.Save(Server.MapPath(name));

                Resim rsm = new Resim();
                rsm.OrtaYol = name;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                if (rsm.Id != null)
                {
                    resimId = rsm.Id;
                }

            }
            if (resimId != -1)
                mrk.ResimID = resimId;
            Context.Baglanti.Marka.Add(mrk);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }
        public ActionResult MarkaSil(int id)
        {
            var marka = Context.Baglanti.Marka.Find(id);
            Context.Baglanti.Marka.Remove(marka);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }
        public ActionResult Kategoriler()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }
        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(Kategori ktg)
        {
            Context.Baglanti.Kategori.Add(ktg);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public ActionResult KategoriSil(int id)
        {
            var kategori = Context.Baglanti.Kategori.Find(id);
            Context.Baglanti.Kategori.Remove(kategori);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public ActionResult OzellikTipleri()
        {
            return View(Context.Baglanti.OzellikTip.ToList());
        }
        public ActionResult OzellikTipEkle()
        {
            return View(Context.Baglanti.Kategori.ToList());

        }
        [HttpPost]
        public ActionResult OzellikTipEkle(OzellikTip ot)
        {
            Context.Baglanti.OzellikTip.Add(ot);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }
        public ActionResult OzellikTipSil(int id)
        {
            var oztip = Context.Baglanti.OzellikTip.Find(id);
            Context.Baglanti.OzellikTip.Remove(oztip);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }
        public ActionResult OzellikDegerleri()
        {
            return View(Context.Baglanti.OzellikDeger.ToList());
        }
        public ActionResult OzellikDegerEkle()
        {
            return View(Context.Baglanti.OzellikTip.ToList());
        }
        [HttpPost]
        public ActionResult OzellikDegerEkle(OzellikDeger od)
        {
            Context.Baglanti.OzellikDeger.Add(od);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikDegerleri");
        }
        public ActionResult OzellikDegerSil(int id)
        {
            var deger = Context.Baglanti.OzellikDeger.Find(id);
            Context.Baglanti.OzellikDeger.Remove(deger);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikDegerleri");
            /*
             Güncelleme Kodları
             var kategoriler = db.TBL_KATEGORILER.Find(p1.KATEGORIID);
            kategoriler.KATEGORIAD = p1.KATEGORIAD;
            db.SaveChanges();
            return RedirectToAction("Index");
             */
    }
        public ActionResult UrunOzellikleri()
        {
            return View(Context.Baglanti.UrunOzellik.ToList());
        }
        public ActionResult UrunOzellikSil(int UrunId,int tipId,int degereId)
        {
            UrunOzellik uo = Context.Baglanti.UrunOzellik.FirstOrDefault(x => x.UrunId == UrunId && x.OzellikTipID == tipId && x.OzellikDegerID == degereId);
            Context.Baglanti.UrunOzellik.Remove(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }
        public ActionResult UrunOzellikEkle()
        {
            return View(Context.Baglanti.Urun.ToList());
        }
        public PartialViewResult UrunOzellikTipWidget(int? katid)
        {
             if(katid!=null)
            {
                var data = Context.Baglanti.OzellikTip.Where(x => x.KategoriID == katid).ToList();
                return PartialView(data);
            }
             else
            {
                var data = Context.Baglanti.OzellikTip.ToList();
                return PartialView(data);
            }
        }
        public PartialViewResult UrunOzellikDegerWidget(int? tipId)
        {
            if(tipId!=null)
            {
                var data = Context.Baglanti.OzellikDeger.Where(x=>x.OzellikTipID==tipId).ToList();
                return PartialView(data);
            }
            else
            {
                var data = Context.Baglanti.OzellikDeger.ToList();
                return PartialView(data);
            }
        }
       [HttpPost]
       public ActionResult UrunOzellikEkle(UrunOzellik uo)
        {
            Context.Baglanti.UrunOzellik.Add(uo);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }
        public ActionResult UrunResimEkle(int id)
        { 
            return View(id);
        }
        [HttpPost]
        public ActionResult UrunResimEkle(int uId, HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                Image img = Image.FromStream(fileupload.InputStream);

                Bitmap ortaResim = new Bitmap(img, Settings.UrunOrtaBoyut);
                Bitmap buyukResim = new Bitmap(img, Settings.UrunBuyukBoyut);

                string ortaYol = "/Content/UrunResim/Orta/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);
                string buyukYol = "/Content/UrunResim/Buyuk/" + Guid.NewGuid() + Path.GetExtension(fileupload.FileName);

                ortaResim.Save(Server.MapPath(ortaYol));
                buyukResim.Save(Server.MapPath(buyukYol));

                Resim rsm = new Resim();
                rsm.BuyukVeri = buyukYol;
                rsm.OrtaYol = ortaYol;
                rsm.UrunID = uId;

                if (Context.Baglanti.Resim.FirstOrDefault(x => x.UrunID == uId && x.Varsayilan == false) != null)
                    rsm.Varsayilan = true;
                else
                    rsm.Varsayilan = false;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                return View(uId);
            }
            return View(uId);
        }
        public ActionResult SliderResimleri()
        {
            return View();

        }
        [HttpPost]
        public ActionResult SliderResimEkle(HttpPostedFileBase fileUpload)
        {
            if(fileUpload!=null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                Bitmap bmp = new Bitmap(img,Settings.SliderResimBoyut);
                string yol = "/Content/SliderResim/"+Guid.NewGuid()+Path.GetExtension(fileUpload.FileName);
                bmp.Save(Server.MapPath(yol));
                Resim rsm = new Resim();
                rsm.BuyukVeri = yol;
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
            }
            return RedirectToAction("SliderResimleri");
        }
    }
  
}
