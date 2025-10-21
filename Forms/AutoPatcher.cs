using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using Aspose.Zip.Rar;
using System.Net;

namespace BruteGamingMacros.UI.Forms
{
    public partial class AutoPatcher : Form
    {
        private HttpClient client = new HttpClient();
        public AutoPatcher()
        {
            InitializeComponent();
            /**
             * Autopatch Process
             * 1. Fetch the latest tag in url
             * 2. Compare with current version code in AppConfig.cs
             * 3. If different, should download the .rar in github
             * 3.1  Rename current BruteGamingMacros.exe to BruteGamingMacros_old.exe
             * 3.2  Rename BruteGamingMacros to BruteGamingMacros_old
             * 3.3  Extract .rar file in folder
             * 3.3  Delete .rar in file folder.
             * 3.4  Delete BruteGamingMacros_old in file folder.
             * 4. If equals, version are updated.
             */
            StartAutopatcher();
        }

        private async void StartAutopatcher()
        {

            //Get Latest Version
            //List[0] = Tag
            //List[1] = Url
            try
            {
                String oldFileName = "BruteGamingMacros_old.exe";
                String oldBackupFileName = "BruteGamingMacros_backup.exe";
                String sourceFileName = "BruteGamingMacros.exe";
                File.Delete(oldBackupFileName); //Delete old backup
                File.Delete(oldFileName); //Delete old version
                //Fetch Github latest Tag
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Add("User-Agent", "request");

                string latestVersion = await client.GetStringAsync(AppConfig.LatestVersionURL);
                JObject obj = JsonConvert.DeserializeObject<JObject>(latestVersion);

                string tag = obj["name"].ToString(); //Tag Name

                #region comment this for no att versions
                if (tag != AppConfig.Version)
                {
                    string downloadUrl = obj["assets"][0]["browser_download_url"].ToString(); //Latest download url
                    string fileName = obj["assets"][0]["name"].ToString(); //Latest file name
                    //If different, 4R is outdated.
                    //Need to download and update
                    await Download(downloadUrl, fileName); //Download the .rar file
                    RarArchive arch = new RarArchive(fileName);
                    File.Move(sourceFileName, oldFileName);
                    arch.ExtractToDirectory(".");
                    arch.Dispose();
                    File.Delete(fileName); //Delete .rar file downloaded
                    Environment.Exit(0);
                }
                #endregion
            }
            finally
            {
                Hide();
            }
        }

        private async Task<bool> Download(string url, string filename)
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(BruteGamingMacros_DownloadProgressChanged);
            await client.DownloadFileTaskAsync(url, @filename);
            return true;
        }

        void BruteGamingMacros_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                pbPatcher.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
