using DownloadModule;
using ZipperModule;
using BoosterLoader.ProgramStarter;
using MongoDB.Driver.Core.Events;


/*
 * 自动化程序流程如下：
 * 1. 先使用CheckAndCreate函数查找本地是否存在版本JSON文件，并且读取或者初始化连接网站，开发进度和版本号
 * 2. 使用PostPcakage函数发送post请求获取服务器最新的版本号和下载地址，并通过res判断请求是否成功
 * 3. 创建一个BersionChecker类并使用CheckVersion函数对版本号进行比对，决定是否需要下载最新版本
 * 4. 使用DownloadFile函数下载最新的MetaBIM到本地上，并使用downloadRes判断是否下载成功
 * 5. 使用Unzip函数来解压最新版本的MetaBIM
 * 6. 使用Start函数启动MetaBIM并且Delete函数用来删除下载数据包
 */

public class Viewer{

    private static VersionChecker versionChecker;
    public static void Main(string[] args) {
        Openning();
        Container content = FileChecker.CheckAndCreate();
        SetBackValue(content.WebPath, content.DevelopmentStage);

        CheckProgress(content);

        if (!versionChecker.IsNewVersion)
        {
            DownloadFile();
            UnzipFile();
        }

        StartProgram();
    }

    public static void Openning() {
        Console.CursorVisible = false;
        Console.WriteLine("==========================================");
        Console.WriteLine(TipSentence.welcome, Config.softwareName);
        Console.WriteLine("=========================================");
        Console.WriteLine(TipSentence.checkLocalFile);
    }

    public static void CheckProgress( Container content) {
        Console.WriteLine(TipSentence.checkVersion, Config.softwareName);
        Download.PostPackage().Wait();
        if (!Download.res) Environment.Exit(0);
        versionChecker = new VersionChecker(content, Download.version);
        versionChecker.CheckVersion();
    }

    public static void DownloadFile()
    {
        Download.DownloadFileAsync().Wait();
        Console.WriteLine(TipSentence.unzipStart);
    }

    public static void UnzipFile()
    {
        Zipper.ExtractZip(Path.Combine(Config.path, Config.fileName), Config.extractPath);
        Console.WriteLine(TipSentence.unzipFileSuccessfully);
    }

    public static void StartProgram()
    {
        Console.WriteLine(TipSentence.startProgram, Config.softwareName);
        ProgramStarter.Start(Config.programPath);
        File.Delete(Path.Combine(Config.path, Config.fileName));
    }

    public static void SetBackValue(string WebPage, string DevStage) {
        Config.WebPath = WebPage;
        Config.DevelopmentStage = DevStage;
    }
}