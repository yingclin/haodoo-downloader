using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HaodooDownloader.Services
{
    class FileWriter
    {
        public async Task WriteAsync(FileInfo fileInfo, byte[] bytes)
        {
            try
            {
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                    Console.WriteLine($"建立目錄: {fileInfo.Directory.Name}");
                }

                await File.WriteAllBytesAsync(fileInfo.FullName, bytes);
                Console.WriteLine($"建立檔案: {fileInfo.Name}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ERROR] 寫檔失敗,檔案:{fileInfo.FullName},錯誤:{ex.Message}");
            }
        }
    }
}
