using PlanExam.Abstract;

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