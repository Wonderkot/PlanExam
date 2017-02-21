namespace PlanExam.Abstract
{
    public interface IImageProcessor
    {
        void Init(string file, int clientWidth);
        string GetScaledImage(int step);
        string GetStartImage();
    }
}