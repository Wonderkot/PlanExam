using System;
using System.IO;
using Aspose.Pdf;
using Aspose.Pdf.Devices;
using NLog;
using PlanExam.Abstract;

namespace PlanExam.Implementation
{
    public class PdfScaleService : IScaleService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IImageProcessor _imageProcessor;

        public PdfScaleService()
        {
            _imageProcessor = new ImageProcessor();
        }

        public void Init(string file, int clientWidth)
        {
            Logger.Info("**** Начинается обработка файла {0} ****", file);
            string directory = Path.GetDirectoryName(file);

            var newFile = Path.Combine(directory, string.Concat(Path.GetFileNameWithoutExtension(file), ".png"));
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
                _imageProcessor.Init(newFile, clientWidth);

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            

            Logger.Info("**** Обработка файла {0} завершена****", file);
        }

        public string GetScaledImage(int step)
        {
            return _imageProcessor.GetScaledImage(step);
        }

        public string GetStartImage()
        {
            return _imageProcessor.GetStartImage();
        }
    }
}