using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using PlanExam.Models;
using PlanExam.Utils;

namespace PlanExam.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload == null) return RedirectToAction("Index");
            string fileName = Path.GetFileName(upload.FileName);
            if (!HttpPostedFileBaseExtensions.IsImage(upload))
            {

                return View("Index");
            }
            Logger.Info("Выполняется сохранение файла {0} на сервере ...", upload.FileName);
            var saveFile = Path.Combine(Server.MapPath("~/Files/"), fileName);
            try
            {
                upload.SaveAs(saveFile);
            }
            catch (Exception e)
            {
                Logger.Error(e, "При сохранении файла на сервере произошла ошибка.");
                return View("Index");
            }
            Logger.Info("Файл успешно сохранен.");
            Plan plan = new Plan(fileName);
            return View("Exam", plan);
        }
    }
}