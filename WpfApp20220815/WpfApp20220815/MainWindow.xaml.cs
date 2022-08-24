using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp20220815
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Load_Info();
        }



        public class Account
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime DOB { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create_Directory(sender,e);
        }

        private void Save_Button(object sender, RoutedEventArgs e)
        {
            Save_Info(sender, e);
        }

        static int Execute_Command(string command, string arguments = "")
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(command)
                {
                    Arguments = arguments,
                    UseShellExecute = false,
                }
            };
            process.Start();
            process.WaitForExit();
            return process.ExitCode;
        }



        static void Git_Command(string arguments)
        {
            if (Execute_Command("git", arguments) != 0)
            {
                throw new Exception("git command error");
            }
        }

        static void Git_Flow(string outDir,string gitUrl, string commitId　,String target)
        {
            //ディレクトリ作成
            Directory.CreateDirectory(outDir);
            //リポジトリ初期化
            Git_Command($@"-C {outDir} init");
            //sparsecheckout設定
            Git_Command($@"-C {outDir} config core.sparsecheckout true");
            //sparsecheckout init
            Git_Command($@"-C {outDir} sparse-checkout init");
            //取得対象をsparse-checkoutに設定する
            Git_Command($@"-C {outDir} sparse-checkout add {target}");
            Git_Command($@"-C {outDir} remote add origin {gitUrl}");
            Git_Command($@"-C {outDir} pull origin {commitId}");
        }

        private void Create_Directory(object sender, RoutedEventArgs e)
        {
            string gitUrl = TargetGit.Text;
            Properties.Settings.Default.gitUrl = gitUrl;

            string userName = UserName.Text;
            Properties.Settings.Default.userName = userName;

            string Password = GitPassword.Text;
            Properties.Settings.Default.GitPassword = Password;

            string commitIdAfter = CommitIdAfter.Text;
            string commitIdBefore = CommitIdBefore.Text;
            string outDir = OutDir.Text;
            Properties.Settings.Default.outDir = outDir;
            string target = target1.Text;
            Properties.Settings.Default.Save();

            if (!String.IsNullOrEmpty(userName)){
                bool https = gitUrl.Contains("https://");
                bool http = gitUrl.Contains("http://");
                String stringHttps = "https://";
                String stringHttp = "http://";
                String cuttedUrl = gitUrl;
                String parts = "";

                //http部分を消す
                if (https) {
                    cuttedUrl = cuttedUrl.Replace(stringHttps, "@");
                    parts = stringHttps;
                }
                else if(http) {
                    cuttedUrl = cuttedUrl.Replace(stringHttp, "@");
                    parts = stringHttp;
                }

                //パスワードが必要なら入れる
                if (!String.IsNullOrEmpty(gitUrl))
                {
                    cuttedUrl = ":" + Password + cuttedUrl;
                };

                //user名を入れる
                cuttedUrl = userName + cuttedUrl;

                //http部分を戻して適用する
                gitUrl = parts + cuttedUrl;
            };

            Git_Flow(outDir + "/before", gitUrl, commitIdBefore, target);
            Git_Flow(outDir + "/after", gitUrl, commitIdAfter, target);

        }

        private void Save_Info(object sender, RoutedEventArgs e)
        {
            string gitUrl = TargetGit.Text;
            Properties.Settings.Default.gitUrl = gitUrl;
            //string commitIdAfter = CommitIdAfter.Text;
            //string commitIdBefore = CommitIdBefore.Text;
            string outDir = OutDir.Text;
            Properties.Settings.Default.outDir = outDir;
            string target = target1.Text;
            Properties.Settings.Default.Save();
            //Git_Fllow(outDir + "/before", gitUrl, commitIdBefore, target);
            //Git_Fllow(outDir + "/after", gitUrl, commitIdAfter, target);
            string userName = UserName.Text;
            Properties.Settings.Default.userName = userName;

            string Password = GitPassword.Text;
            Properties.Settings.Default.GitPassword = Password;

        }

        
        private void Load_Info() {
            TargetGit.Text = Properties.Settings.Default.gitUrl;
            OutDir.Text = Properties.Settings.Default.outDir;
            UserName.Text = Properties.Settings.Default.userName;
            GitPassword.Text = Properties.Settings.Default.GitPassword;
        }

        private void TargetGit_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        /*        private string GetIndex0()
       {
           ListBoxItem lbi = (ListBoxItem)
               (targetUrlList.ItemContainerGenerator.ContainerFromIndex(0));
           string hoge = lbi.Content.ToString();
           return hoge;
       }*/


    }
}
