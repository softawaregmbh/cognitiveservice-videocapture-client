using OpenCvSharp;
using OpenCvSharp.Extensions;
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

namespace VideoCapture.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenCvSharp.VideoCapture capture = new OpenCvSharp.VideoCapture(0);
            using (Mat image = new Mat())
            {
                while (true)
                {
                    capture.Read(image);
                    
                    if (image.Empty())
                    {
                        break;
                    }

                    using (var stream = image.ToMemoryStream(".jpg", new ImageEncodingParam(ImwriteFlags.JpegQuality, 100)))
                    {
                        videoFrame.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }

                    await Task.Delay(10);
                }
            }
        }
    }
}
