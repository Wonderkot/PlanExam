using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageResizer;
using NLog;
using PlanExam.Abstract;
using PlanExam.Models;

namespace PlanExam.Implementation
{
    public class ImageScaleService : IScaleService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Plan> _images = new Dictionary<int, Plan>();
        private string _sourceFile;

        public  void Init(string file, int clientWidth)
        {
            Image image = Image.FromFile(file);
            var width = image.Width;
            var height = image.Height;
            image.Dispose();
            //возьмем фиксированный шаг в 5%
            DeltaX = (int)(width * 0.05);
            DeltaY = (int)(height * 0.05);
            Logger.Info("**** Изображение будет меняться на {0} по оси X и на {1} по оси Y. ****", DeltaX, DeltaY);

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

                ResizeSettings resizeSettings = new ResizeSettings(width, height, FitMode.Stretch,
                    Path.GetExtension(file))
                { Scale = ScaleMode.Both };

                string directory = Path.GetDirectoryName(file);

                var newFile = Path.Combine(directory, "Temp", Path.GetFileName(file));
                Logger.Info("Исходный файл был преобразован для отображения по ширине экрана клиента.");
                _sourceFile = newFile;
                ImageBuilder.Current.Build(file, newFile, resizeSettings);
            }
            else
            {
                string directory = Path.GetDirectoryName(file);
                _sourceFile = Path.Combine(directory, "Temp", Path.GetFileName(file));
                File.Copy(file, _sourceFile);
            }
            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            Plan plan = new Plan(_sourceFile)
            {
                Width = width,
                Height = height
            };

            _images.Add(0, plan);
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

            string directory = Path.GetDirectoryName(_sourceFile);
            var newFile = string.Concat(directory, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(_sourceFile), step.ToString(), Path.GetExtension(_sourceFile));

            var width = step > 0 ? container.Width + DeltaX : container.Width - DeltaX;
            var height = step > 0 ? container.Height + DeltaY : container.Height - DeltaY;

            try
            {
                ResizeSettings resizeSettings = new ResizeSettings(width, height, FitMode.Stretch, Path.GetExtension(_sourceFile));
                resizeSettings.Scale = ScaleMode.Both;
                ImageBuilder.Current.Build(_sourceFile, newFile, resizeSettings);
                if (File.Exists(newFile))
                {
                    newFile = string.Concat(Path.DirectorySeparatorChar, "Files", Path.DirectorySeparatorChar, "Temp", Path.DirectorySeparatorChar, Path.GetFileName(newFile));
                    Plan newPlan = new Plan(newFile)
                    {
                        Width = width,
                        Height = height
                    };
                    _images.Add(step, newPlan);
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
            return string.Concat(Path.DirectorySeparatorChar, "Files", Path.DirectorySeparatorChar, "Temp",
                Path.DirectorySeparatorChar, Path.GetFileName(_sourceFile));

        }

        private int DeltaX { get; set; }

        private int DeltaY { get; set; }
    }
}