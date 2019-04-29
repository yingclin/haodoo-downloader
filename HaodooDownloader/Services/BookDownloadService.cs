using HaodooDownloader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HaodooDownloader.Services
{
    class BookDownloadService
    {
        private const string DownloadBaseUrl = "http://www.haodoo.net/?M=d&P=";
        //
        private readonly string _outRootPath;
        // TODO 下載格式: updb epub pdb
        private readonly string[] _bookExts = { "updb", "epub", "pdb" };
        //
        public BookDownloadService(string outRootPath)
        {
            _outRootPath = outRootPath;
        }

        public async Task DownloadAsync(List<Book> books)
        {
            var httpHelper = new HttpHelper();
            var fileWriter = new FileWriter();

            foreach (var book in books)
            {
                foreach (var ext in _bookExts)
                {
                    var url = GetUrl(book.BookCode, ext);
                    var filePath = GetOutPath(book, ext);
                    //
                    var fileInfo = new FileInfo(filePath);

                    if (!fileInfo.Exists)
                    {
                        var bookBytes = await httpHelper.ReadDataOrNullAsync(url);

                        if (bookBytes != null && bookBytes.Length > 0)
                        {
                            await fileWriter.WriteAsync(fileInfo, bookBytes);
                        }
                        else
                        {
                            Console.WriteLine($"檔案不存在: {fileInfo.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"檔案已存在: {fileInfo.Name}");
                    }
                }
            }
        }

        private string GetUrl(string bookCode, string ext)
        {
            return $"{DownloadBaseUrl}{bookCode}.{ext}";
        }

        private string GetOutPath(Book book, string ext)
        {
            return $"{_outRootPath}" +
                $"{Path.DirectorySeparatorChar}{book.Category}" +
                $"{Path.DirectorySeparatorChar}{book.Author}.{book.Name}.{ext}";
        }
    }
}
