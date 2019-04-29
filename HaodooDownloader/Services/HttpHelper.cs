using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HaodooDownloader.Services
{
    internal class HttpHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpHelper()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }

        /// <summary>
        /// 取得頁面 HTML
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetHTMLByURLAsync(string url)
        {
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var response = await httpClient.GetAsync(url);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ERROR] get html exception,url:{url},message:{ex.Message}");
            }

            return await Task.FromResult("");
        }

        /// <summary>
        /// 讀入連結的文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]> ReadDataOrNullAsync(string url)
        {
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {

                    using (var result = await httpClient.GetAsync(url))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            return await result.Content.ReadAsByteArrayAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] read data from url exception,url:{url},message:{ex.Message}");
            }

            return null;
        }
    }
}
