using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HaodooDownloader.Services;
using HaodooDownloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaodooDownloader.Pages
{
    /// <summary>
    /// 列表頁資訊
    /// </summary>
    class ListPage
    {
        private readonly string _listPageUrl;

        public ListPage(string listPageUrl)
        {
            _listPageUrl = listPageUrl;
        }

        /// <summary>
        /// 依列表頁網址,取得當中的書籍頁網址
        /// </summary>
        /// <param name="url">列表頁網址</param>
        /// <returns></returns>
        public async Task<List<Link>> GetBookPageUrls()
        {
            var docHelper = new DocumentHelper();

            var dom = await docHelper.LoadDocument(_listPageUrl);

            if(docHelper.Is404(dom))
            {
                Console.WriteLine($"[ERROR] {_listPageUrl} 無此頁面!");
                return new List<Link>();
            }

            var parent = dom.QuerySelector("div[class='a03']");

            if(parent == null)
            {
                Console.WriteLine($"[ERROR] dom 中沒有必要標示!");
                return new List<Link>();
            }

            var urls = new List<Link>();

            foreach (var child in parent.Children)
            {
                // 列表頁中的作者名稱不重要
                // docHelper.GetAuthorRecursion

                var links = docHelper.GetLinkRecursion(child);

                foreach (var link in links)
                {
                    link.Url = link.Url.ToLower().StartsWith("http") ? link.Url : $"{Haodoo.BaseUrl}/{link.Url}";
                    urls.Add(link);
                }
            }

            return urls;
        }
    }
}
