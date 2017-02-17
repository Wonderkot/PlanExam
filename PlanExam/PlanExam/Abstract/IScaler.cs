
namespace PlanExam.Abstract
{
    public interface IScaler
    {
        /// <summary>
        /// Увеличение
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        string ZoomIn(int step);

        /// <summary>
        /// Уменьшение
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        string ZoomOut(int step);
    }
}