using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eticaret.App_Classes;
using Eticaret.Models.Entity;
namespace Eticaret.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Sepet()
        {
            return PartialView();
        }
        public PartialViewResult Slider()
        {
            var data = Context.Baglanti.Resim.Where(x=>x.BuyukVeri.Contains("Slider")).ToList();

            return PartialView(data);
        }
        public PartialViewResult YeniUrunler()
        {
            var data = Context.Baglanti.Urun.ToList();
            return PartialView(data);
        }
        public PartialViewResult Servisler()
        {
            return PartialView();
        }
       
        public ActionResult Markalar()
        {
            var data = Context.Baglanti.Marka.ToList();
            return PartialView(data);
        }
        public void SepeteEkle(int id)
        {
            SepetItem si = new SepetItem();
            Urun u = Context.Baglanti.Urun.FirstOrDefault(x=>x.id==id);
            
            si.Urun = u;
            si.Adet = 1;
            si.Indirim = 0;
            Sepet s = new Sepet();
            s.SepeteEkle(si);
        
        }
        public void SepetUrunAdetDusur(int id)
        { 
          if(HttpContext.Session["AktifSepet"]!=null)
            {
                Sepet s = (Sepet)HttpContext.Session["AktifSepet"];
                if(s.Urunler.FirstOrDefault(x=>x.Urun.id==id).Adet>1)
                {
                    s.Urunler.FirstOrDefault(x=>x.Urun.id==id).Adet--;
                }
                else
                {
                    SepetItem si = s.Urunler.FirstOrDefault(x=>x.Urun.id==id);
                    s.Urunler.Remove(si);
                }

            }
        }
        public PartialViewResult MiniSepetWidget()
        {
            if(HttpContext.Session["AktifSepet"]!=null)
            {
                return PartialView((Sepet)HttpContext.Session["AktifSepet"]);
            }
            else
            {
                return PartialView();
            }
        }
        public ActionResult UrunDetay(string id)
        {
            Urun u = Context.Baglanti.Urun.FirstOrDefault(x => x.Adi == id);

            List<UrunOzellik> uos = Context.Baglanti.UrunOzellik.Where(x => x.UrunId == u.id).ToList();
            Dictionary<string, List<OzellikDeger>> ozellik = new Dictionary<string, List<OzellikDeger>>();

            List<OzellikDeger> degers = new List<OzellikDeger>();

            foreach (UrunOzellik uo in uos)
            {
                OzellikTip ot = Context.Baglanti.OzellikTip.FirstOrDefault(x => x.Id == uo.OzellikTipID);

                bool keyKontrol = false;
                foreach (var item in ozellik)
                {
                    if (item.Key != ot.Adi)
                    {
                        keyKontrol = true;
                    }
                    else
                    {
                        keyKontrol = false;
                    }
                }
                if (keyKontrol)
                {
                    degers = new List<OzellikDeger>();
                }

                foreach (OzellikDeger deger in ot.OzellikDeger)
                {
                    OzellikDeger od = Context.Baglanti.OzellikDeger.FirstOrDefault(x => x.OzellikTipID == ot.Id && x.Id == uo.OzellikDegerID);
                    if (!degers.Any(x => x.Id == od.Id))
                        degers.Add(od);
                }
                ozellik.Add(ot.Adi, degers);
                degers = new List<OzellikDeger>();
            }

            ViewBag.Ozellikler = ozellik;
            return View(u);
        }
       
    }
}