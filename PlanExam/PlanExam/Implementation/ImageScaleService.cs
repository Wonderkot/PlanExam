using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using PlanExam.Abstract;
using PlanExam.Models;
using PlanExam.Utils;
using Image = System.Drawing.Image;

namespace PlanExam.Implementation
{
    public class ImageScaleService : IScaleService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Plan> _images;
        private string _sourceFile;

        public ImageScaleService()
        {
            _images = new Dictionary<int, Plan>();
        }

        public void Init(string file, int clientWidth)
        {
            Logger.Info("**** Начинается обработка файла {0} ****", file);
            Image image = Image.FromFile(file);
            var width = image.Width;
            var height = image.Height;
            image.Dispose();
            //возьмем фиксированный шаг в 5%
            DeltaX = (int)(width * 0.05);
            DeltaY = (int)(height * 0.05);
            Logger.Info("Изображение будет меняться на {0} по оси X и на {1} по оси Y. ", DeltaX, DeltaY);

            //сконвертируем изображение под ширину экрана если нужно
            if (width < clientWidth)
            {
                while (width <= clientWidth)
                {
                    width += DeltaX;
                    height += DeltaY;
                }
                //обычно забирает немного больше, поэтому отнимем обратно
                width -= DeltaX;
                height -= DeltaY;
                Logger.Info("Исходный файл был преобразован для отображения по ширине экрана клиента.");
                Plan temp = ImageGenerator.GeneratePicPlan(file, width, height, "original", true);
                if (temp != null)
                {
                    if (!_images.ContainsKey(0))
                    {
                        _images.Add(0, temp);
                    }
                }
            }
            else
            {
                string directory = Path.GetDirectoryName(file);
                _sourceFile = Path.Combine(directory, "Temp", Path.GetFileName(file));
                File.Copy(file, _sourceFile);
                Plan plan = new Plan(_sourceFile)
                {
                    Width = width,
                    Height = height
                };

                if (!_images.ContainsKey(0)) _images.Add(0, plan);
            }
            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            Logger.Info("**** Обработка файла {0} завершена****", file);
        }

        public string GetScaledImage(int step)
        {
            if (_images.ContainsKey(step))
            {
                return _images[step].Picture;
            }
            IOrderedEnumerable<KeyValuePair<int, Plan>> temp = _images.OrderByDescending(x => x.Key);

            //в зависимости от направления отдаём нужную картинку
            var container = step > 0 ? temp.First().Value : temp.Last().Value;
            string newFile = null;
            var width = step > 0 ? container.Width + DeltaX : container.Width - DeltaX;
            var height = step > 0 ? container.Height + DeltaY : container.Height - DeltaY;

            try
            {
                Plan plan = ImageGenerator.GeneratePicPlan(_sourceFile, width, height, step.ToString(), false);
                if (plan != null)
                {
                    _images.Add(step, plan);
                    newFile = plan.Picture;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return newFile;
        }

        public string GetStartImage()
        {
            string startImage = _sourceFile;
            if (_images == null) return startImage;
            if (_images.ContainsKey(0))
            {
                startImage = Path.GetFileName(_images[0].Picture);
                _sourceFile = _images[0].FullPath;
            }
            return startImage;
        }

        private int DeltaX { get; set; }

        private int DeltaY { get; set; }
    }
}