using System.Diagnostics;
using System;
using System.ComponentModel;
using DownloadModule;
using ZipperModule;

namespace BoosterLoader.ProgramStarter
{
    public class ProgramStarter
    {
        private static Process process = new Process();
        //开启MetaBIM，如果本地的版本号是最新的，但删除了MetaBIM，程序会重新下载一个
        public static void Start(string path)
        {
            process.StartInfo.FileName = path;
            try
            {
                process.Start();
            }
            catch (Exception e) {
                //if the program has build file, but the folder is empty, need to download the file again. TODO
                Console.WriteLine(TipSentence.MissingLocalFile);
                Download.DownloadFileAsync().Wait();
                Zipper.ExtractZip(Path.Combine(Config.path, Config.fileName), Config.extractPath);
                Start(path);
            }
        }
    }
}