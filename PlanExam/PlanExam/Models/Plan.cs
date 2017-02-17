using System.Drawing;

namespace PlanExam.Models
{
    public class Plan
    {
        public Plan()
        {
            
        }
        public Plan(string file)
        {
            Picture = file;
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Picture { get; set; }
    }
}