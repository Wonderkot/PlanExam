using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NLog;
using PlanExam.Abstract;
using PlanExam.Implementation;
using PlanExam.Models;
using PlanExam.Utils;

namespace PlanExam.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IScaler _scaler;

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload == null) return View("Index");
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
                _scaler = new ImageScaler(saveFile);
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

        public string ZoomIn(int step)
        {
            if (_scaler != null) return _scaler.ZoomIn(step);
            return null;
            //var content = Url.Content(string.Concat("~/Files/", "1.png"));
            //return content;
        }
    }
}