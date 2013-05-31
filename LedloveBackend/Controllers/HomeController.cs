using LedloveBackend.Daemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LedloveBackend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            String msg = Twitter.GetLatest("awlled");
            if (msg == null)
            {
                ViewBag.Message = "Error: could not fetch status";
            }
            else
            {
                ViewBag.Message = "@awlled: " + msg;
                ViewBag.Response = new Transmitter().Send(ViewBag.Message);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This server pushes tweets by @awlled and tweets to @appswithlove to a funky LED panel. More to come soon.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
