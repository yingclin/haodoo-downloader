using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HaodooDownloader.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaodooDownloader.Services
{
    /// <summary>
    /// 分類資訊
    /// </summary>
    class Categories
    {
        private readonly Dictionary<string, string> _categoryBaseUrl;

        public Categories()
        {
            // 分類名稱及基底網址
            _categoryBaseUrl = new Dictionary<string, string>
            {
                ["世紀百強"] = "M=hd&P=100",
                //["隨身智囊"] = "M=hd&P=wisdom",
                //["歷史煙雲"] = "M=hd&P=history",
                //["武俠小說"] = "M=hd&P=martial",
                //["懸疑小說"] = "M=hd&P=mystery",
                //["言情小說"] = "M=hd&P=romance",
                //["奇幻小說"] = "M=hd&P=scifi",
                //["小說園地"] = "M=hd&P=fiction"
            };
        }

        /// <summary>
        /// 取得分類名稱
        /// </summary>
        /// <returns></returns>
        public List<string> GetCategoryNames() => _categoryBaseUrl.Select(o => o.Key).ToList();

        /// <summary>
        /// 依分類名稱,取得各分頁網址
        /// </summary>
        /// <param name="categoryName">分類名稱</param>
        /// <returns></returns>
        public async Task<string[]> GetUrls(string categoryName)
        {
            if (_categoryBaseUrl.ContainsKey(categoryName))
            {
                var urls = new List<string>();
                var docHelper = new DocumentHelper();

                var baseParam = _categoryBaseUrl[categoryName];

                // 最多試到 50 頁, 足矣
                for (int i = 1; i <= 50; i++)
                {
                    var url = $"{Haodoo.BaseUrl}/?{baseParam}-{i}";

                    var is404 = await docHelper.Is404(url);

                    if (!is404)
                    {
                        urls.Add(url);
                    }
                    else
                    {
                        Console.WriteLine($"{categoryName} 有 {i - 1} 頁");
                        break;
                    }
                }

                return urls.ToArray();
            }

            Console.WriteLine($"[ERROR] {categoryName} 不是正確的分類!");

            return new string[0];
        }
    }
}
