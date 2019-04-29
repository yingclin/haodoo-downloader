using AngleSharp.Dom;
using HaodooDownloader.Models;
using HaodooDownloader.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HaodooDownloader.Pages
{
    /// <summary>
    /// 書籍頁資訊
    /// </summary>
    class BookPage
    {
        private readonly Link _bookLink;
        private readonly string _categoryName;
        private readonly Regex _regex;

        public BookPage(Link bookLink, string categoryName)
        {
            _bookLink = bookLink;
            _categoryName = categoryName;
            //
            _regex = new Regex("<[^>]*>");
        }

        public async Task<List<Book>> GetBooks()
        {
            var docHelper = new DocumentHelper();

            var dom = await docHelper.LoadDocument(_bookLink.Url);

            if (docHelper.Is404(dom))
            {
                Console.WriteLine($"[ERROR] {_bookLink.Url} 無此頁面!");
                return new List<Book>();
            }

            var children = dom.QuerySelectorAll("tr>td>font[color='CC0000']");

            foreach (var item in children)
            {
                // 找到真正包含書籍資料的 td (td 的 first child 為 font)
                if (!string.IsNullOrEmpty(item.InnerHtml) && item.ParentElement.FirstElementChild == item)
                {
                    return GetBooks(item.ParentElement);
                }
            }

            return new List<Book>();
        }

        /// <summary>
        /// 取得頁面中的書籍資訊
        /// 一頁中可能有多筆書籍
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private List<Book> GetBooks(IElement element)
        {
            var bookHeads = element.QuerySelectorAll("font[color='CC0000']");
            var books = new List<Book>();

            foreach (var head in bookHeads)
            {
                var author = head.InnerHtml?.Trim();
                var bookName = head.NextSibling?.TextContent?.Trim();
                var click = head.NextElementSibling?.GetAttribute("onclick"); // 線上閱讀連結

                // 判斷有書籍資訊才進行資料提取
                if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(bookName) && !string.IsNullOrEmpty(click))
                {
                    var code = GetCode(click);
                    author = CleanAuthor(author);
                    bookName = CleanName(bookName);

                    if (!string.IsNullOrEmpty(code))
                    {
                        books.Add(new Book {
                            Author = author,
                            Name = bookName,
                            Category = _categoryName,
                            BookCode = code
                        });
                        //
                        Console.WriteLine($"書名:{bookName},作者:{author},書碼:{code},分類:{_categoryName}");
                    }
                    else
                    {
                        Console.WriteLine($"[ERROR] 找不到書碼! 作者:{author},書名:{bookName},click:{click}");
                    }
                }
            }

            return books;
        }

        private string GetCode(string click)
        {
            // format:ReadOnline('A435', 'book')
            if(click.StartsWith("Read"))
            {
                var startIndex = click.IndexOf("'");
                var t = click.Substring(startIndex + 1, click.Substring(startIndex + 1).IndexOf("'"));
                return t;
            }

            return null;
        }

        private string CleanName(string name)
        {
            if(name.StartsWith("《"))
            {
                name = name.Substring(1);
            }

            if (name.EndsWith("》"))
            {
                name = name.Substring(0, name.Length - 1);
            }

            name = _regex.Replace(name, "");

            return name;
        }

        public string CleanAuthor(string author)
        {
            return _regex.Replace(author.Replace("/", "_"), "");
        }
    }
}
