using System;
using System.Collections.Generic;
using System.IO; 

using Baidu.Aip.Ocr;

namespace DsLib.Baidu
{
    /// <summary>
    /// 百度文字识别
    /// API文档：http://ai.baidu.com/docs#/OCR-API/e1bd77f3
    /// </summary>
    public class OcrApi
    {
        /// <summary>APPID：百度 App ID</summary>
        public static string APP_ID = "你的 App ID";
        /// <summary>AK：百度 Api Key</summary>
        public static string API_KEY = "你的 Api Key";
        /// <summary>SK：百度 Secret Key </summary>
        public static string SECRET_KEY = "你的 Secret Key";

        /// <summary> 文字识别交互类 </summary>
        public static Ocr client = new Ocr(API_KEY, SECRET_KEY);


        /// <summary>
        /// 
        /// </summary>
        public static void GeneralBasic()
        {
            var image = File.ReadAllBytes("图片文件路径");
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasic(image);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                                    {"language_type", "CHN_ENG"},
                                    {"detect_direction", "true"},
                                    {"detect_language", "true"},
                                    {"probability", "true"}
                                };
            // 带参数调用通用文字识别, 图片参数为本地图片
            result = client.GeneralBasic(image, options);
            Console.WriteLine(result);
        }

        public void GeneralBasicUrlDemo()
        {
            var url = "https//www.x.com/sample.jpg";

            // 调用通用文字识别, 图片参数为远程url图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasicUrl(url);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
        {"language_type", "CHN_ENG"},
        {"detect_direction", "true"},
        {"detect_language", "true"},
        {"probability", "true"}
    };
            // 带参数调用通用文字识别, 图片参数为远程url图片
            result = client.GeneralBasicUrl(url, options);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 通用文字识别
        /// </summary>
        public static void GeneralBasic()
        { 
            var image = File.ReadAllBytes("图片文件路径");

            // 通用文字识别
            var result = client.GeneralBasic(image);

            // 图片url
            result = client.GeneralBasicUrl("https://www.baidu.com/img/bd_logo1.png");
        }

        /// <summary>
        /// 通用文字识别（含生僻字版）
        /// </summary>
        public static void GeneralEnhanced()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");

            // 带生僻字版
            var result = client.GeneralEnhanced(image);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GeneralWithLocatin()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");

            // 带位置版本
            var result = client.GeneralWithLocatin(image, null);
        }

        public static void WebImage()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");

            // 网图识别
            var result = client.WebImage(image, null);
        }

        public static void Accurate()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");

            // 高精度识别
            var result = client.Accurate(image);
        }

        public static void AccurateWithLocation()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");

            // 高精度识别(带位置信息)
            var result = client.AccurateWithLocation(image);
        }


        public static void BankCard()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");

            // 银行卡识别
            var result = client.BankCard(image);
        }

        public static void Idcard()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");

            var options = new Dictionary<string, object>
            {
                {"detect_direction", "true"} // 检测方向
            };
            // 身份证正面识别
            var result = client.IdCardFront(image, options);
            // 身份证背面识别
            result = client.IdCardBack(image);
        }

        public static void DrivingLicense()
        {
            var client = new Ocr.Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            var result = client.DrivingLicense(image);
        }

        public static void VehicleLicense()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            var result = client.VehicleLicense(image);
        }

        public static void PlateLicense()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            var result = client.PlateLicense(image);
        }

        public static void Receipt()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            var options = new Dictionary<string, object>
            {
                {"recognize_granularity", "small"} // 定位单字符位置
            };
            var result = client.Receipt(image, options);
        }


        public static void BusinessLicense()
        {
            var client = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            var result = client.BusinessLicense(image);
        }

        public static void FormBegin()
        {
            var form = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            form.DebugLog = false; // 是否开启调试日志

            var result = form.BeginRecognition(image);
            Console.Write(result);
        }

        public static void FormGetResult()
        {
            var form = new Ocr("Api Key", "Secret Key");
            var options = new Dictionary<string, object>
            {
                {"result_type", "json"} // 或者为excel
            };
            var result = form.GetRecognitionResult("123344", options);
            Console.Write(result);
        }

        public static void FormToJson()
        {
            var form = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            form.DebugLog = false; // 是否开启调试日志

            // 识别为Json
            var result = form.RecognizeToJson(image);
            Console.Write(result);
        }

        public static void FormToExcel()
        {
            var form = new Ocr("Api Key", "Secret Key");
            var image = File.ReadAllBytes("图片文件路径");
            form.DebugLog = false; // 是否开启调试日志

            // 识别为Excel
            var result = form.RecognizeToExcel(image);
            Console.Write(result);
        }
    }
}
