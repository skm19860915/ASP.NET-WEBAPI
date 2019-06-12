using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace rmxwebapi.Controllers.CacheManagement
{
    public class ReloadCacheController : Controller
    {
        [Route("ReloadCache")]
        public ActionResult Index()
        {
            return View();
        }
    }
}