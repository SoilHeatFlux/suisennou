using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Account account = new Account
            {
                Name = "John Doe",
                Email = "john@microsoft.com",
                DOB = new DateTime(1980, 2, 20, 0, 0, 0, DateTimeKind.Utc),
            };
            string json = JsonConvert.SerializeObject(account, Formatting.Indented);
            TextBlock.Text = json;
        }

        private void Create_Directory(object sender, RoutedEventArgs e)
        {

            string sampleText = SampleTextBox.Text;
            Directory.CreateDirectory(sampleText);
            SampleTextBox.Text = "Sample text retrieved and "  + sampleText;
            

        }



    }
}
