using System.IO;

namespace PlanExam.Utils
{
    public class CleanUtil
    {
        public static void CleanDirecory(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles();
            foreach (FileInfo f in fi)
            {
                f.Delete();
            }
            foreach (DirectoryInfo df in diA)
            {
                CleanDirecory(df.FullName);
            }
        }
    }
}