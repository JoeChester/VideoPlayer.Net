using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;
using System.Threading;

namespace VideoPlayer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnOpen(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if(result == true)
            {
                String outputFileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".avi";
                File.Create(outputFileName).Dispose();
                AviConverter avi = new AviConverter(dlg.FileName, outputFileName);
                string output = avi.Convert2Avi();
                mediaElement.Source = new Uri(output);
                mediaElement.Play();
            }
        }

        
        private void OnPlay(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void OnPause(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void OnStop(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = new TimeSpan(0, 0, 0, 0);
            mediaElement.Stop();
        }
    }

    public class AviConverter
    {
        public String inputFilePath { get; set; }
        public String outputFilePath { get; set; }

        public AviConverter(String inputFilePath , String outputFilePath)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
        }

        public string Convert2Avi()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "ffmpeg.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-i " + inputFilePath + " -y " + outputFilePath;
            Process exeProcess = Process.Start(startInfo);
            exeProcess.WaitForExit();
            return outputFilePath;
        }

    }
}
