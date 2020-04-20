using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TezYonetimSistemi.UI.Filters
{
    public class CustomAuthAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["kullanici"] == null)
            {
                filterContext.Result = new RedirectResult("/Default/GirisYap");
            }
            else
            {
                Helpers.CacheProvider.CacheEkle("kullanici", filterContext.HttpContext.Session["kullanici"], 15);
            }
        }

    }
}