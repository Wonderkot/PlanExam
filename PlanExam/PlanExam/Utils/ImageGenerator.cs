using System.IO;
using ImageResizer;
using NLog;
using PlanExam.Models;

namespace PlanExam.Utils
{
    public class ImageGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 
        public static Plan GeneratePicPlan(string sourceFile, int width, int height, string postFix)
        {
            string directory = Path.GetDirectoryName(sourceFile);
            string newFile = string.Concat(directory, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(sourceFile), "_", postFix, Path.GetExtension(sourceFile));
            Logger.Debug("Будет сгенерирован файл {0}", newFile);
            Plan newPlan = null;
            ResizeSettings resizeSettings = new ResizeSettings(width, height, FitMode.Stretch, Path.GetExtension(sourceFile));
            resizeSettings.Scale = ScaleMode.Both;
            ImageBuilder.Current.Build(sourceFile, newFile, resizeSettings);
            if (File.Exists(newFile))
            {
                newFile = string.Concat(Path.DirectorySeparatorChar, "Files", Path.DirectorySeparatorChar, "Temp", Path.DirectorySeparatorChar, Path.GetFileName(newFile));
                newPlan = new Plan(newFile)
                {
                    Width = width,
                    Height = height
                };
            }
            return newPlan;
        }
    }
}