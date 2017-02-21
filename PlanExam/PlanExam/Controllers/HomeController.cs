using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using NLog;
using PlanExam.Abstract;
using PlanExam.App_Start;
using PlanExam.Models;
using PlanExam.Utils;

namespace PlanExam.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IScaleService _scaleService;

        private static int _clientWidth;
        
        public HomeController(IScaleService scaleService)
        {
            //По умолчанию происходит обработка изображений
            _scaleService = scaleService;
        }

        // GET: Home
        public ActionResult Index()
        {
            //очистка директории с картинками
            var folder = Server.MapPath("~/Files/");
            try
            {
                CleanUtil.CleanDirecory(folder);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return View();
        }

        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload == null) return View("Index");
            string fileName = Path.GetFileName(upload.FileName);

            bool isPdf = false;

            if (upload.ContentType.ToLower().Equals("application/pdf"))
            {
                Logger.Info("Вероятнее всего, загружаемый файл - PDF документ.");
                isPdf = true;

            }

            if (!HttpPostedFileBaseExtensions.IsImage(upload) || string.IsNullOrEmpty(fileName))
            {
                return View("Index");
            }

            if (isPdf)
            {
                //если есть подтверждение, что работаем с пдф, то перенаправляем действие на соответствующий сервис
                IWindsorContainer container = ContainerBootstrapper.Bootstrap().Container;
                _scaleService = container.Resolve<IScaleService>("PdfScaleService");
            }

            Logger.Info("Выполняется сохранение файла {0} на сервере ...", upload.FileName);
            var saveFile = Path.Combine(Server.MapPath("~/Files/"), fileName);
            try
            {
                upload.SaveAs(saveFile);
                if (_scaleService != null) _scaleService.Init(saveFile, _clientWidth);
            }
            catch (Exception e)
            {
                Logger.Error(e, "При сохранении файла на сервере произошла ошибка.");
                return View("Index");
            }
            Logger.Info("Файл успешно сохранен.");
            if (_scaleService != null) fileName = _scaleService.GetStartImage();
            Plan plan = new Plan(fileName);
            
            return View("Exam", plan);
        }

        /// <summary>
        /// Возврат отмасштабированного изображения
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public string GetScaledImage(int step)
        {
            if (_scaleService != null) return _scaleService.GetScaledImage(step);
            return null;
        }

        /// <summary>
        /// Получение размеров экрана пользователя
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [HttpPost]
        public void GetClientScreenSize(int width, int height)
        {
            _clientWidth = width;
            Logger.Info(" Client width : {0}, height  : {1}", _clientWidth, height);
        }
    }
}