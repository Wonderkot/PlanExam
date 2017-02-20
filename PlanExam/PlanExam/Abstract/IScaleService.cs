
namespace PlanExam.Abstract
{
    public interface IScaleService
    {
        /// <summary>
        /// Инициализация сервиса-масшатибазтора
        /// </summary>
        /// <param name="file"></param>
        /// <param name="clientWidth"></param>
        void Init(string file, int clientWidth);
        
        /// <summary>
        /// Увеличение
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        string GetScaledImage(int step);

        string GetStartImage();
    }
}