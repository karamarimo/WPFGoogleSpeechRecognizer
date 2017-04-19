using System;
using System.Collections.Generic;
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
using Google.Cloud.Speech.V1;

namespace GoogleSpeechRecognizer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool good = true;

        public bool Good { get => good; set => good = value; }

        public MainWindow()
        {
            InitializeComponent();
            txtbox_result.Text = "a\na\na\na\na\na\n";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filepath = txtbox_filepath.Text;
            RecognitionAudio audio;
            if (filepath.Length == 0)
            {
                MessageBox.Show("No file path supplied.");
                return;
            }
            try
            {
                audio = RecognitionAudio.FromFile(filepath);
            }
            catch (Exception)
            {
                MessageBox.Show("Error happened while opneing file.");
                return;
            }

            SpeechClient client = SpeechClient.Create();
            RecognitionConfig config = new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Flac,
                SampleRateHertz = 44100,
                LanguageCode = LanguageCodes.Japanese.Japan
            };

            RecognizeResponse response = client.Recognize(config, audio);
            Console.WriteLine(response);
            string trans = response.Results.First().Alternatives.First().Transcript;
            txtbox_result.Text = trans + "\n[End]";
        }

        private void Btn_openfile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".flac",
                Filter = "FLAC Audio Files (*.flac)|*.flac"
            };
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                txtbox_filepath.Text = filename;
            }

        }
    }
}
