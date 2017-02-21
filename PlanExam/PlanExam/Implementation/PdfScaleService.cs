using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using PlanExam.Abstract;
using PlanExam.Models;
using Image = System.Drawing.Image;

namespace PlanExam.Implementation
{
    public class PdfScaleService : IScaleService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Plan> _images = new Dictionary<int, Plan>();
        private string _sourceFile;

        public void Init(string file, int clientWidth)
        {
            Logger.Info("**** Начинается обработка файла {0} ****", file);
            string directory = Path.GetDirectoryName(file);

            var newFile = Path.Combine(directory, "Temp", Path.GetFileName(file));

            try
            {
                //исходную пдфку перегоняем в картинку и потом уже картинку масштабируем


            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            _sourceFile = PicGen(newFile);
            //полученный файл конвертируем в картинку
            Logger.Info("**** Обработка файла {0} завершена****", file);
        }

        public string GetScaledImage(int step)
        {
            return _sourceFile;
        }

        public string GetStartImage()
        {
            return _sourceFile;
        }

        /// <summary>
        /// Генерируем картинку из файла
        /// </summary>
        /// <param name="inFile"></param>
        /// <returns></returns>
        private static string PicGen(string inFile)
        {
            Logger.Info("*** Начинается генерация изображения из файла {0} ***", inFile);
            return null;
        }
    }
}