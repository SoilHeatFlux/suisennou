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

        static void Git_Fllow(string outDir,string gitUrl, string commitId　,String target)
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
            string commitIdAfter = CommitIdAfter.Text;
            string commitIdBefore = CommitIdBefore.Text;
            string outDir = OutDir.Text;
            string target = target1.Text;
            Git_Fllow(outDir + "/before", gitUrl, commitIdBefore, target);
            Git_Fllow(outDir + "/after", gitUrl, commitIdAfter, target);

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
