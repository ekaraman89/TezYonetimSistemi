using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.UI.Filters
{
    public class OgrenciRoleAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Kullanici kul = (Kullanici)Helpers.CacheProvider.CachedenOku("kullanici");

            if (kul.RolID != 3)
            {
                filterContext.Result = new RedirectResult("/Default/Yetki");
            }

        }
    }
}