
namespace PlanExam.Abstract
{
    public interface IScaler
    {
        /// <summary>
        /// Увеличение
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        string GetScaledImage(int step);

        string GetStartImage();
    }
}