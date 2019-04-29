using System;
using System.IO;
using System.Threading.Tasks;

namespace HaodooDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            // 決定輸出路徑
            string outPath = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}Haodoo";

            if(args.Length > 0)
            {
                outPath = args[0];

                if (args[0].EndsWith(Path.DirectorySeparatorChar) || args[0].EndsWith(Path.AltDirectorySeparatorChar))
                {
                    outPath = outPath.Substring(0, outPath.Length - 1);
                }
            }

            Console.WriteLine("Haodoo, Thank You!");
            Console.WriteLine($"輸出路徑為 {outPath}\n");
            //
            var p = new Program();
            p.Run(outPath).Wait();
            //
            Console.Write("結束, 按任意鍵離開!");
            Console.ReadKey();
        }

        async Task Run(string outPath)
        {
            var haodoo = new Haodoo();

            await haodoo.DownloadAll(outPath);
        }
    }
}
