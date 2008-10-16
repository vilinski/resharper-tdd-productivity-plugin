using System;
using System.Net;
using System.Windows.Forms;
using EnvDTE;
using JetBrains.Application;
using JetBrains.ComponentModel;

namespace TddProductivity
{
    [ShellComponentImplementation(ProgramConfigurations.ALL)]
    public class AutoUpdate : IShellComponent
    {
        public void Dispose()
        {

        }

        public void Init()
        {
            WebClient client = new WebClient();
            client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(client_DownloadDataCompleted);
            client.DownloadDataAsync(new Uri("http://resharper-tdd-productivity-plugin.googlecode.com/svn/trunk/LatestVersion/version.txt"));
        }

        private void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if(e.Error!=null)
            {
                return;    
            }

            bool newVersionIsAvailable = GetNewVersionIsAvailable(e.Result);

            if (newVersionIsAvailable)
            {
                var downloadTheNewVersion = GetDownloadNewVersionDecisionFromTheUI();

                if (downloadTheNewVersion)
                {                    
                    DownloadFile("http://resharper-tdd-productivity-plugin.googlecode.com/svn/trunk/LatestVersion/TddProductivity.Setup.msi");
                }
            }
            
        }

        private bool GetNewVersionIsAvailable(byte[] result)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string version = enc.GetString(result);
            Version newVersion = new Version(version);
            Version currentVersion = this.GetType().Assembly.GetName().Version;

            return newVersion.CompareTo(currentVersion)>0;
        }

        private bool GetDownloadNewVersionDecisionFromTheUI()
        {
            return System.Windows.Forms.MessageBox.Show( "There is a new version of this plugin available. Download the latest version?","TDD Productivty Plugin for Resharper",MessageBoxButtons.YesNo)==DialogResult.Yes;
        }

        private void DownloadFile(string url)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents=false;
            proc.StartInfo.FileName="iexplore";
            proc.StartInfo.Arguments = url;
            proc.Start();
        }
    }
}