using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace imageocrtranslate
{
    public class ABBYYHttpHelper
    {
        private const string urlPath = "https://api.ocr.space/Parse/Image";

        // private const string ocrKey = "5a64d478-9c89-43d8-88e3-c65de9999580";
        private const string ocrKey = "9dbce3a1fd88957";
        // private const string ocrKey = "helloworld";

        public static async Task<string> readImage(string sourceLanguage, string imagePath) {
            string result = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(ocrKey), "apikey");
                form.Add(new StringContent(getTargetLanguage(sourceLanguage)), "language");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("1"), "OCREngine");

                Console.WriteLine(Path.GetFileName(imagePath));

                FileInfo tempFI = new FileInfo(imagePath);
                Console.WriteLine("文件大小=" + System.Math.Ceiling(tempFI.Length / 1024.0) + " KB");

                byte[] imageData;
                if (tempFI.Length / 1024.0 > 1000)
                {
                    Image tempImage = Image.FromFile(imagePath);
                    MemoryStream tempMS = ImageHelper.Zip(tempImage, ImageFormat.Jpeg, 1000);
                    imageData = tempMS.ToArray();
                    tempMS.Close();
                }
                else {
                    imageData = File.ReadAllBytes(imagePath);
                }
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
