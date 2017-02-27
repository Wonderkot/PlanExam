
using PlanExam.Models;

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
        /// Масштабирование
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        string GetScaledImage(bool direction);

        Plan GetStartImage();
    }
}