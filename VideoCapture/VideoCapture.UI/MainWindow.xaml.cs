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
using VideoCapture.DemoAnalyzer;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private IVideoGrabber grabber;
        private MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.grabber = new VideoGrabber();
            this.mainViewModel = new MainViewModel(grabber, new DemoAnalyzer.DemoAnalyzer());
            this.DataContext = this.mainViewModel;

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await this.mainViewModel.InitializeAsync();
        }

        private void videoFrame_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // You cannot bind the (read-only) Width property directly, 
            // so set it in the size changed event handler manually

            this.mainViewModel.ImageWidth = this.videoFrame.ActualWidth;
        }
    }
}
