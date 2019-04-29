# 好讀書櫃下載工具 Haodoo Downloader

## 目的
[好讀網站](http://www.haodoo.net/?M=hd&P=about) 是推廣中文電子書的公益網站。由於創辦人周劍輝先生已辭世，網站前景不明，為了收藏讀友們多年來不斷努力不斷貢獻的成果，寫了這個小工具，可將網站中的書籍檔案備份下來。

## 說明
* 透過即時解析網站的方式取得書籍資訊。網站版本以 2019/04/29 為準。
* 預設只試著下載 updb, pdb, epub 格式。若要增減，請調整程式中的設定值。
* 由於網頁格式有不一致的情況，沒有特別核對是否有缺失的書籍。
* 快速開發，只有簡單的錯誤處理及訊息顯示。
* 沒有提供編譯好的執行檔，需自行由原始碼編譯。

## 開發環境
* Windows 10
* .NET Core 2.1.505

.NET Core 為跨作業系統開發環境，可在 Windows, MacOS 及 Linux 下執行。

## 編譯

以 Windows 下為例。

安裝:  
 [.NET Core SDK](https://dotnet.microsoft.com/download)

確認安裝成功:  
開啟 命令提示字元，執行 `dotnet --version`
```
>dotnet --version
2.1.505
```

建置:  
進到方案目錄(~/haodoo-downloader)
```
>dotnet build
```

發佈:  
```
>dotnet publish
```
會發佈在 haodoo-downloader\HaodooDownloader\bin\Debug\netcoreapp2.1\publish\ 下。

## 執行
在發佈目錄下，執行
```
dotnet HaodooDownloader.dll [輸出路徑]
```
* [輸出路徑] 非必要，預設為執行目錄下的 Haodoo 目錄。
