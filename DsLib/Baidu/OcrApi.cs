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
        public static string APP_ID = "10903897";
        /// <summary>AK：百度 Api Key</summary>
        public static string API_KEY = "IQpURMVXdBqkDGnGlsaMlnZz";
        /// <summary>SK：百度 Secret Key </summary>
        public static string SECRET_KEY = "aTgR0SKaKxSzNF2rDmcOBn1af8V1SRf3 ";

        /// <summary> 文字识别交互类 </summary>
        public static Ocr client = new Ocr(API_KEY, SECRET_KEY);

        //通用文字识别（本地图片）
        #region - GeneralBasic(string _img) -
        /// <summary>
        /// 通用文字识别（本地图片）
        /// 用户向服务请求识别某张图中的所有文字
        /// </summary>
        /// <param name="_img">本地图片路径</param>
        public static void GeneralBasic(string _img)
        {
            var image = File.ReadAllBytes(_img);
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasic(image);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                                    {"language_type", "CHN_ENG"},  //识别语言类型，默认为CHN_ENG。可选值包括：- CHN_ENG：中英文混合；- ENG：英文；
                                    { "detect_direction", "true"}, //是否检测图像朝向，默认不检测，即：false。
                                    {"detect_language", "true"},   //是否检测语言，默认不检测。当前支持（中文、英语、日语、韩语）
                                    {"probability", "true"}        //是否返回识别结果中每一行的置信度
                                };
            // 带参数调用通用文字识别, 图片参数为本地图片
            result = client.GeneralBasic(image, options);
            Console.WriteLine(result);
        }
        #endregion

        //通用文字识别（远程图片）
        #region - GeneralBasicUrl(string _url) -
        /// <summary>
        /// 通用文字识别（远程图片）
        /// 用户向服务请求识别某张图中的所有文字
        /// </summary>
        /// <param name="_url">远程图片路径</param>
        public static void GeneralBasicUrl(string _url)
        {
            // 调用通用文字识别, 图片参数为远程url图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasicUrl(_url);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                                {"language_type", "CHN_ENG"},
                                {"detect_direction", "true"},
                                {"detect_language", "true"},
                                {"probability", "true"}
                            };
            // 带参数调用通用文字识别, 图片参数为远程url图片
            result = client.GeneralBasicUrl(_url, options);
            Console.WriteLine(result);
        }
        #endregion

    }
}
