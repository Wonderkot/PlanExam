using PlanExam.Abstract;
using PlanExam.Models;

namespace PlanExam.Implementation
{
    public class ImageScaleService : IScaleService
    {
        private readonly IImageProcessor _imageProcessor;

        public ImageScaleService()
        {
            //тут уже рукам привязку сделал
            _imageProcessor = new ImageProcessor();
        }

        public void Init(string file, int clientWidth)
        {
            _imageProcessor.Init(file, clientWidth);
        }

        public string GetScaledImage(bool direction)
        {
            return _imageProcessor.GetScaledImage(direction);
        }

        public Plan GetStartImage()
        {
            return _imageProcessor.GetStartImage();
        }
    }
}