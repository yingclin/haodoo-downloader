using HaodooDownloader.Models;
using HaodooDownloader.Pages;
using HaodooDownloader.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HaodooDownloader
{
    class Haodoo
    {
        public const string BaseUrl = "http://www.haodoo.net";
        //
        private readonly Categories _categories;

        public Haodoo()
        {
            _categories = new Categories();
        }

        /// <summary>
        /// 下載全站電子書
        /// </summary>
        /// <param name="outPath"></param>
        /// <returns></returns>
        public async Task DownloadAll(string outPath)
        {
            var books = await GetAllBooks();
            //
            var downloadService = new BookDownloadService(outPath);

            await downloadService.DownloadAsync(books);
        }


        /// <summary>
        /// 取得全部的書本資訊
        /// </summary>
        /// <returns></returns>
        private async Task<List<Book>> GetAllBooks()
        {
            var allBooks = new List<Book>();

            foreach (var categoryName in _categories.GetCategoryNames())
            {
                Console.WriteLine($"\n查找分類 {categoryName}");
                //
                var links = await GetBookLinksByCategoryName(categoryName);
                var books = await GetBooks(links, categoryName);
                allBooks.AddRange(books);
                //
                Console.WriteLine($"\n分類 {categoryName} 有 {books.Count} 本書");
            }

            Console.WriteLine("取得全部書籍資訊.\n");

            return allBooks;
        }

        /// <summary>
        /// 依分類名稱取得書本頁的連結
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private async Task<List<Link>> GetBookLinksByCategoryName(string categoryName)
        {
            var urls = await _categories.GetUrls(categoryName);
            var linkUrls = new List<Link>();

            foreach (var url in urls)
            {
                var listPage = new ListPage(url);
                linkUrls.AddRange(await listPage.GetBookPageUrls());
            }

            return linkUrls;
        }

        /// <summary>
        /// 依書本頁的連結,取得當中的書本資訊
        /// </summary>
        /// <param name="bookLinks"></param>
        /// <returns></returns>
        private async Task<List<Book>> GetBooks(List<Link> bookLinks, string categoryName)
        {
            var allBooks = new List<Book>();

            foreach (var bookLink in bookLinks)
            {
                var bookPage = new BookPage(bookLink, categoryName);
                allBooks.AddRange(await bookPage.GetBooks());
            }

            return allBooks;
        }
    }
}
