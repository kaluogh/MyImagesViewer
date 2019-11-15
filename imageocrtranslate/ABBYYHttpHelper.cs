using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageocrtranslate
{
    public class ABBYYHttpHelper
    {
        public static string urlPath = "https://api.ocr.space/Parse/Image";
        public static string readImage(string imagePath) {
            string result = "";
            try
            {
                Console.WriteLine(imagePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }   
    }
}
