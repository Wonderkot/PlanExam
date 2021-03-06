using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NLog;
using PlanExam.Abstract;
using PlanExam.Models;
using PlanExam.Utils;

namespace PlanExam.Implementation
{
    public class ImageProcessor : IImageProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Plan> _images;
        private string _sourceFile;

        private int _scaleCount;

        public ImageProcessor()
        {
            _images = new Dictionary<int, Plan>();
        }

        private int DeltaX { get; set; }
        private int DeltaY { get; set; }

        public void Init(string file, int clientWidth)
        {
            //TODO кодирвоать имена файлов, а то проблемы с именами при получении будут
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
            _sourceFile = _images[0].FullPath;
            Logger.Info("**** Обработка файла {0} завершена****", file);
        }

        public string GetScaledImage(bool direction)
        {
            //true - вверх, false - вниз
            if (direction)
            {
                _scaleCount++;
            }
            else
            {
                _scaleCount--;
            }

            if (_images.ContainsKey(_scaleCount))
            {
                return _images[_scaleCount].Picture;
            }

            IOrderedEnumerable<KeyValuePair<int, Plan>> temp = _images.OrderByDescending(x => x.Key);

            //в зависимости от направления отдаём нужную картинку
            var container = direction ? temp.First().Value : temp.Last().Value;
            string newFile = null;
            var width = direction ? container.Width + DeltaX : container.Width - DeltaX;
            var height = direction ? container.Height + DeltaY : container.Height - DeltaY;

            try
            {
                Plan plan = ImageGenerator.GeneratePicPlan(_sourceFile, width, height, _scaleCount.ToString(), false);
                if (plan != null)
                {
                    if (!_images.ContainsKey(_scaleCount)) _images.Add(_scaleCount, plan);
                    newFile = plan.Picture;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return newFile;
        }

        public Plan GetStartImage()
        {
            //нам нужно знать путь до оригинального файла, чтобы конвертировать всегда только его и минимизировать потери качества
            //также нам нужен относительный путь до файла, но разово
            //Plan startImage = new Plan(_sourceFile);
            //Image image = Image.FromFile(_sourceFile);
            //var width = image.Width;
            //var height = image.Height;
            //image.Dispose();
            //startImage.Width = width;
            //startImage.Height = height;
            //if (_images == null) return startImage;
            //if (!_images.ContainsKey(0)) return startImage;
            var sourceFile = _images[0].FullPath;
            if (sourceFile != null)
            {
                _sourceFile = sourceFile;
            }
            return _images[0];
        }
    }
}