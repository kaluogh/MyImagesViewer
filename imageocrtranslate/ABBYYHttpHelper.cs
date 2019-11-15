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
        public static string readImage(string sourceLanguage, string imagePath) {
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

        private static string getTargetLanguage(string language) {
            string result = "";
            switch (language) {
                case "English":
                    result = "eng";break;
                case "Japanese":
                    result = "jpn"; break;
                default:
                    result = "eng"; break;
            }
            return result;
        }
    }

    public class Parsedresult
    {
        public object FileParseExitCode { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class Rootobject
    {
        public Parsedresult[] ParsedResults { get; set; }
        public int OCRExitCode { get; set; }
        public bool IsErroredOnProcessing { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
}
