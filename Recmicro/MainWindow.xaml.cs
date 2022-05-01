using EasyEditorLib;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.Windows.Threading;

namespace Recmicro
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Recorder recorder = new Recorder();
        FileSys fileSys = new FileSys();
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (o, e) =>
            {
                name_file.Text = recorder.NameAudioFile;
                UpdateFiles();
            };
            name_file.TextChanged += (o, e) =>
            {
                recorder.NameAudioFile = name_file.Text;
            };

            fileSys.LocalPath = recorder.DirectoryRecorder;
            fileSys.fileSystemEventHandler += (o, e) =>
            {
                Thread.Sleep(100);
                this.Dispatcher.Invoke(new Action(() => 
                {

                    UpdateFiles();
                }));
            };
            fileSys.InFileSystemWatcher();
        }
        void UpdateFiles()
        {
            if (!Directory.Exists(recorder.DirectoryRecorder))
                return;

            ListBox.Items.Clear();
            foreach (string item in Directory.GetFiles(recorder.DirectoryRecorder))
            {
                if (Regex.IsMatch(item, @"[\s\S]+?.(wav|mp3)"))
                {

                    Label label = new Label();
                    label.Content = item;
                    label.Foreground = Brushes.White;
                    label.FontSize = 20;

                    ListBox.Items.Add(label);
                }
            }
        }
        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            recorder.Start();
        }
        private void Border_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            recorder.Stop();
        }
    }
}
