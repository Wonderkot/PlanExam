using System.IO;
using System.Web;

namespace PlanExam.Utils
{
    public static class HttpPostedFileBaseExtensions
    {
        //честно нагуглено на stackoverflow

        public static bool IsImage(this HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png" &&
                        postedFile.ContentType.ToLower() != "application/pdf")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var extension = Path.GetExtension(postedFile.FileName);
            if (extension != null && (extension.ToLower() != ".jpg" && extension.ToLower() != ".png" && extension.ToLower() != ".gif" && extension.ToLower() != ".jpeg" && extension.ToLower() != ".pdf") )
            {
                return false;
            }
            return true;
        }
    }
}