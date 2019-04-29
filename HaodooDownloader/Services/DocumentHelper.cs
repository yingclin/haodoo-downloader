using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HaodooDownloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaodooDownloader.Services
{
    /// <summary>
    /// 頁面資料的提取和判斷
    /// </summary>
    class DocumentHelper
    {
        /// <summary>
        /// 從網址建立 IHtmlDocument
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IHtmlDocument> LoadDocument(string url)
        {
            var httpHelper = new HttpHelper();
            var htmlParser = new HtmlParser();

            var htmlDoc = await httpHelper.GetHTMLByURLAsync(url);

            return htmlParser.ParseDocument(htmlDoc);
        }

        /// <summary>
        /// 判斷有沒有指定頁面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> Is404(string url)
        {
            var dom = await LoadDocument(url);
            //
            return Is404(dom);
        }

        /// <summary>
        /// 判斷有沒有指定頁面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Is404(IHtmlDocument dom)
        {
            // 好讀網站就算網址不存在也會回頁面,只是內容中會有"404 無此網頁"

            var elements = dom.QuerySelectorAll("strong");
            return elements.Any(o => o.InnerHtml.Contains("404 無此網頁"));
        }

        /// <summary>
        /// 取得列表中的作者名稱
        /// </summary>
        /// <param name="ele"></param>
        /// <returns></returns>
        public string GetAuthorRecursion(IElement ele)
        {
            if (ele.TagName == "FONT" && ele.GetAttribute("color") == "CC0000")
            {
                return ele.InnerHtml;
            }
            else if (ele.Children != null && ele.Children.Any())
            {
                foreach (var child in ele.Children)
                {
                    var result = GetAuthorRecursion(child);

                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 取得連結內容,主要是用於取得書籍
        /// </summary>
        /// <param name="ele"></param>
        /// <returns></returns>
        public List<Link> GetLinkRecursion(IElement ele)
        {
            var ret = new List<Link>();

            if (ele.TagName == "A" && !string.IsNullOrEmpty(ele.GetAttribute("href")) && !string.IsNullOrEmpty(ele.InnerHtml))
            {
                ret.Add(new Link { Text = ele.TextContent.Trim(), Url = ele.GetAttribute("href").Trim() }); 
            }

            foreach (var child in ele.Children)
            {
                ret.AddRange(GetLinkRecursion(child, ret));
            }

            return ret;
        }

        private List<Link> GetLinkRecursion(IElement ele, List<Link> links)
        {
            if (ele.TagName == "A" && !string.IsNullOrEmpty(ele.GetAttribute("href")) && !string.IsNullOrEmpty(ele.InnerHtml))
            {
                links.Add(new Link { Text = ele.TextContent.Trim(), Url = ele.GetAttribute("href").Trim() });
            }

            foreach (var child in ele.Children)
            {
                links.AddRange(GetLinkRecursion(child));
            }

            return links;
        }
    }
}
