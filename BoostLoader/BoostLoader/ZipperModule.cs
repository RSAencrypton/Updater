
using Ionic.Zip;

namespace ZipperModule
{
    public class Zipper {
        private static bool justHadByteUpdate = false;
        private static long totalSize;
        private static long currentSize;

        public static void ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            //if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            //{
            //    if (!justHadByteUpdate)
            //        Console.SetCursorPosition(0, Console.CursorTop);
            //    Console.Write("   {0}/{1} ({2:N0}%)", e.BytesTransferred, e.TotalBytesToTransfer,
            //                  e.BytesTransferred / (0.01 * e.TotalBytesToTransfer));
            //    justHadByteUpdate = true;
            //}
            //else if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            //{
            //    if (justHadByteUpdate)
            //        Console.WriteLine();
            //    Console.WriteLine("Extracting: {0}", e.CurrentEntry.FileName);
            //    justHadByteUpdate = false;
            //}

            if (e.EventType == ZipProgressEventType.Extracting_AfterExtractEntry) {
                currentSize++;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("* Extract Porgress: {0} / {1}", currentSize, totalSize);
            }
        }

        public static void ExtractZip(string zipToExtract, string directory)
        {
            using (var zip = ZipFile.Read(zipToExtract))
            {

                zip.ExtractProgress += ExtractProgress;
                totalSize = zip.Count;
                foreach (var e in zip)
                {
                    e.Extract(directory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }
    }
}