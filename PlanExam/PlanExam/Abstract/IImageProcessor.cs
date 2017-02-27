using PlanExam.Models;

namespace PlanExam.Abstract
{
    public interface IImageProcessor
    {
        void Init(string file, int clientWidth);
        string GetScaledImage(bool direction);
        Plan GetStartImage();
    }
}