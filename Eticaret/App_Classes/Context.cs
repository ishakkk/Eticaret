using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eticaret.Models.Entity;
using Eticaret.Models;
namespace Eticaret.App_Classes
{
    public class Context
    {
        private static EticaretEntities baglanti;

        public static EticaretEntities Baglanti
        {
            get
            {
                if (baglanti == null)
                    baglanti = new EticaretEntities();
                return baglanti;
            }
            set
            {
                baglanti = value;
            }
        }
    }
}