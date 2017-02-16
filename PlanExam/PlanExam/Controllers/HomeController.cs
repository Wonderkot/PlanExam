using System;
using System.Web;
using System.Web.Mvc;
using PlanExam.Utils;

namespace PlanExam.Controllers
{
    public class HomeController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload == null) return RedirectToAction("Index");
            string fileName = System.IO.Path.GetFileName(upload.FileName);
            if (!HttpPostedFileBaseExtensions.IsImage(upload))
            {

                return View("Index");
            }
            Logger.Info("Выполняется сохранение файла {0} на сервере ...", upload.FileName);
            try
            {
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
            }
            catch (Exception e)
            {
                Logger.Error(e, "При сохранении файла на сервере произошла ошибка.");
                return View("Index");
            }
            Logger.Info("Файл успешно сохранен.");
            return RedirectToAction("Index", "ViewPlan");
        }
    }
}