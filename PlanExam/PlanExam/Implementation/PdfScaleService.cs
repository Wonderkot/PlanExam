using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Pdf;
using Aspose.Pdf.Devices;
using NLog;
using PlanExam.Abstract;
using PlanExam.Models;
using PlanExam.Utils;
using Image = System.Drawing.Image;

namespace PlanExam.Implementation
{
    public class PdfScaleService : IScaleService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Plan> _images;
        private string _sourceFile;

        public PdfScaleService()
        {
            _images = new Dictionary<int, Plan>();
        }

        public void Init(string file, int clientWidth)
        {
            Logger.Info("**** Начинается обработка файла {0} ****", file);
            string directory = Path.GetDirectoryName(file);

            var newFile = Path.Combine(directory, "Temp", string.Concat(Path.GetFileNameWithoutExtension(file), ".png"));
            Logger.Info("Будет создан файл {0}", newFile);
            try
            {
                //исходную пдфку перегоняем в картинку и потом уже картинку масштабируем
                Document pdfDocument = new Document(file);
                using (FileStream imageStream = new FileStream(newFile, FileMode.OpenOrCreate))
                {
                    // Create Resolution object
                    Resolution resolution = new Resolution(300);
                    // Create PNG device with specified attributes (Width, Height, Resolution)
                    PngDevice pngDevice = new PngDevice(resolution);

                    // Convert a particular page and save the image to stream
                    pngDevice.Process(pdfDocument.Pages[1], imageStream);

                    // Close stream
                    imageStream.Close();
                }
                _sourceFile = newFile;

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            //полученный файл конвертируем в картинку
            Logger.Info("**** Обработка файла {0} завершена****", file);
        }

        public string GetScaledImage(int step)
        {
            return _sourceFile;
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
    }
}