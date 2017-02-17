using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ImageResizer;
using PlanExam.Abstract;
using PlanExam.Models;

namespace PlanExam.Implementation
{
    public class ImageScaler : IScaler
    {
        private readonly Dictionary<int, Plan> _images = new Dictionary<int, Plan>();
        private readonly string _file;

        public ImageScaler(string file)
        {
            _file = file;
            Image image = Image.FromFile(_file);
            Width = image.Width;
            Height = image.Height;
            Plan plan = new Plan(file);
            plan.Width = Width;
            plan.Height = Height;
            image.Dispose();
            _images.Add(0, plan);
        }

        public string ZoomIn(int step)
        {
            if (_images.ContainsKey(step))
            {
                return _images[step].Picture;
            }
            IOrderedEnumerable<KeyValuePair<int, Plan>> temp = _images.OrderByDescending(x => x.Key);
            Plan container = null;
            if (step > 0)
            {
                container = temp.First().Value;
            }
            else
            {
                container = temp.Last().Value;
            }

            var directory = Path.GetDirectoryName(_file);
            var newFile = string.Concat(directory, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(_file), step.ToString(), Path.GetExtension(_file));
            var width = container.Width + step;
            var height = container.Height + step;
            try
            {
                ResizeSettings resizeSettings = new ResizeSettings(width, height, FitMode.Crop, Path.GetExtension(_file));
                ImageBuilder.Current.Build(_file, newFile, resizeSettings);
                if (File.Exists(newFile))
                {
                    string folderName = new DirectoryInfo(Path.GetDirectoryName(_file)).Name;
                    newFile = string.Concat(Path.DirectorySeparatorChar, folderName, Path.DirectorySeparatorChar, Path.GetFileName(newFile));
                    Plan newPlan = new Plan(newFile);
                    newPlan.Width = width;
                    newPlan.Height = height;
                    _images.Add(step, newPlan);
                }
            }
            catch (Exception EX_NAME)
            {
                Console.WriteLine(EX_NAME);
            }
            return newFile;
        }

        public int Height { get; set; }

        public int Width { get; set; }

        public string ZoomOut(int step)
        {
            return null;
        }
    }
}