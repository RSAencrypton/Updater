using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

public class Container {
    public string WebPath;
    public string DevelopmentStage;
    public string VersionNum;

    public Container(string WebPath, string DevelopmentStage, string VersionNum)
    {
        this.WebPath = WebPath;
        this.DevelopmentStage = DevelopmentStage;
        this.VersionNum = VersionNum;
    }
}

public class VersionChecker {
    private string LocalVersion;
    private string RemoteVersion;
    private Container container;
    public bool IsNewVersion { get; private set; }

    public VersionChecker(Container container,string RemoteVersion)
    {
        this.container = container;
        LocalVersion = container.VersionNum;
        this.RemoteVersion = RemoteVersion;
    }

    public void CheckVersion() {

        //检查是否本地是否有Version文件，如果没有把最新的版本号作为本地的版本号记录下来
        if (string.IsNullOrEmpty(LocalVersion)) {
            container.VersionNum = RemoteVersion;
            FileChecker.FileWriter(container);
            Console.WriteLine(TipSentence.createNewFile);
            return;
        }

        //把版本号转换为日期进行比较
        try {
            DateTime LocalTime = DateTime.ParseExact(LocalVersion, "MMddyy", CultureInfo.InvariantCulture);
            DateTime RemoteTime = DateTime.ParseExact(RemoteVersion, "MMddyy", CultureInfo.InvariantCulture);

            if (DateTime.Compare(LocalTime, RemoteTime) < 0)
            {
                Console.WriteLine(TipSentence.findNewVersion);
                container.VersionNum = RemoteVersion;
                FileChecker.FileWriter(container);
                IsNewVersion = false;
            }
            else
            {
                Console.WriteLine(TipSentence.localIsLatest);
                IsNewVersion = true;
            }
        }catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}

public class FileChecker {
    public static Container CheckAndCreate()
    {
        //检查本地是否存在Version文件
        Container container = new Container(Config.WebPath, Config.DevelopmentStage, null);
        if (!File.Exists("version.txt"))
        {
            Console.WriteLine(TipSentence.createNewFile);
            using (StreamWriter writer = File.CreateText("version.txt"))
            {
                writer.Close();
            }

            FileWriter(container);
        }
        else {
            string json = File.ReadAllText("version.txt");
            container = JsonConvert.DeserializeObject<Container>(json);
        }

        return container;
    }

    public static void FileWriter(Container container) {
        string json = JsonConvert.SerializeObject(container);
        File.WriteAllText("version.txt", json);
    }
}