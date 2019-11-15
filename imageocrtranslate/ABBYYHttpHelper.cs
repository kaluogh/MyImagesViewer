using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace imageocrtranslate
{
    public class ABBYYHttpHelper
    {
        public static string urlPath = "https://api.ocr.space/Parse/Image";
        public static async Task<string> readImage(string sourceLanguage, string imagePath) {
            string result = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("helloworld"), "apikey");
                form.Add(new StringContent(getTargetLanguage(sourceLanguage)), "language");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("1"), "OCREngine");

                Console.WriteLine(Path.GetFileName(imagePath));
                byte[] imageData = File.ReadAllBytes(imagePath);
                form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", Path.GetFileName(imagePath));
                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                string strContent = await response.Content.ReadAsStringAsync();

                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);


                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        result += ocrResult.ParsedResults[i].ParsedText;
                    }
                }
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
                case "english":
                    result = "eng";break;
                case "japanese":
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
