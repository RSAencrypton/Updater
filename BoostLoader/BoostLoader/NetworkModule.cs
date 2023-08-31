using Downloader;
using MetaBIM;
using System;
using System.ComponentModel;
using System.Net;

namespace DownloadModule {
    public class Download
    {
        private static string url;
        private static string path = Config.path;
        public static string version { get; private set; }
        public static bool res { get; private set; }
        private static HttpClient httpClient = new HttpClient();

        private static DownloadConfiguration downloadConfig = new DownloadConfiguration() {
            ChunkCount = 10,
            ParallelDownload = true
        };

        private static ConsoleProgressBar progressBar;

        #region abuse

        //public static async Task DownloadFile()
        //{
        //    try
        //    {
        //        //发送GET请求获取资源
        //        using (Stream respoece = await httpClient.GetStreamAsync(url))
        //        {
        //            Console.WriteLine("* The web url is correct. The Program start to download.");

        //            //检查是否存在正确的保存路径
        //            if (!Directory.Exists(path)) {
        //                Directory.CreateDirectory(path);
        //                Console.WriteLine("* The path is not exist. The system create one.");
        //            }

        //            string targetPath = Path.Combine(path, Config.fileName);

        //            //开始下载数据包
        //            using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
        //            {
        //                Console.WriteLine("* Start to download the file. Please wait until finished.");
        //                Console.WriteLine("* The file is downloading, the waiting time depends on current network state......");
        //                await respoece.CopyToAsync(fileStream);
        //                Console.WriteLine($"* File downloaded to {targetPath}");
        //            }
        //            downloadRes = true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Write(e.Message);
        //        Console.WriteLine(url);
        //        downloadRes = false;
        //    }
        //}

        #endregion

        public static async Task DownloadFileAsync() {
            var downloader = new DownloadService(downloadConfig);
            string file = Path.Combine(path, Config.fileName);
            progressBar = new ConsoleProgressBar();

            IDownload download = DownloadBuilder.New()
                .WithUrl(url)
                .WithDirectory(path)
                .WithFileName(Config.fileName)
                .WithConfiguration(downloadConfig)
                .Build();

            download.DownloadStarted += OnDownloadStarted;
            download.DownloadProgressChanged += OnDownloadProgressChanged;
            download.DownloadFileCompleted += OnDownloadFileCompleted;

            await download.StartAsync();

        }

        private static void OnDownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            progressBar.Dispose();

            if (e.Error != null)
            {
                Console.WriteLine(TipSentence.downloadFileFailed + e.Error.Message);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine(TipSentence.downloadFileSuccessfully);

            }
        }

        private static void OnDownloadProgressChanged(object? sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            progressBar.SetProgress((float)e.ProgressPercentage);
        }

        private static void OnDownloadStarted(object? sender, DownloadStartedEventArgs e)
        {
            Console.WriteLine(TipSentence.downloadFile, Config.softwareName);
        }

        public static async Task PostPackage() {
            //发送的数据包内容
            Package package = new Package();
            package.profileGuid = Config.profileGuid;
            package.itemGuid = Config.Guid;
            package.target = Config.profileGuid;



            try {//组装成目标url并使用POST指令发送数据包
                StringContent content = new StringContent(package.ToJson());
                string TargetUrl = "https://" + Config.WebPath + "?package=" + package.ToJson();
                using HttpResponseMessage response = await httpClient.PostAsync(TargetUrl, null);

                //根据状态代码确定目标url是否有相应
                if (response.IsSuccessStatusCode)
                {
                    //解析从服务器获取的数据包
                    var result = await response.Content.ReadAsStringAsync();
                    DataProxyResponse<admin> TargetPackage = DataProxyResponse<admin>.FromJson(result);

                    if (TargetPackage.package.Count != 1) {
                        Console.WriteLine(TipSentence.sameID);
                        res = false;
                        return;
                    }

                    admin TargetContent = TargetPackage.package[0];
                    version = TargetContent.versionNumber;
                    url = @TargetContent.updatePackageUrl;
                    res = true;
                }
                else {
                    Console.WriteLine(TipSentence.noResponse);
                    res = false;
                    return;
                }
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
                res = false;
            }
        }

    }
}