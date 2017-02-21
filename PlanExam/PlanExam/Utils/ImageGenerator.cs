using System.IO;
using ImageResizer;
using NLog;
using PlanExam.Models;

namespace PlanExam.Utils
{
    public class ImageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static Plan GeneratePicPlan(string sourceFile, int width, int height, string postFix, bool genOriginal)
        {
            string directory = Path.GetDirectoryName(sourceFile);
            string newFile;
            if (genOriginal)
            {
                newFile = string.Concat(directory, Path.DirectorySeparatorChar, "Temp", Path.DirectorySeparatorChar,
                    Path.GetFileNameWithoutExtension(sourceFile), "_", postFix, Path.GetExtension(sourceFile));
            }
            else
            {
                newFile = string.Concat(directory, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(sourceFile), "_", postFix, Path.GetExtension(sourceFile));
            }
            Logger.Debug("Будет сгенерирован файл {0}", newFile);
            ResizeSettings resizeSettings = new ResizeSettings(width, height, FitMode.Stretch, Path.GetExtension(sourceFile));
            resizeSettings.Scale = ScaleMode.Both;
            ImageBuilder.Current.Build(sourceFile, newFile, resizeSettings);
            if (!File.Exists(newFile)) return null;
            var viewPath = string.Concat(Path.DirectorySeparatorChar, "Files", Path.DirectorySeparatorChar, "Temp", Path.DirectorySeparatorChar, Path.GetFileName(newFile));
            var newPlan = new Plan(viewPath)
            {
                Width = width,
                Height = height,
                FullPath = newFile
            };
            return newPlan;
        }
    }
}