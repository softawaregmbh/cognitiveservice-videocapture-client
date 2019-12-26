// <copyright file="MainWindow.xaml.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

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
using OpenCvSharp;
using OpenCvSharp.Extensions;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    /// <summary>
    /// The interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private IVideoGrabber grabber;
        private MainViewModel mainViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="grabber">The grabber.</param>
        /// <param name="mainViewModel">The main view model.</param>
        public MainWindow(IVideoGrabber grabber, MainViewModel mainViewModel)
        {
            this.InitializeComponent();

            this.grabber = grabber;
            this.mainViewModel = mainViewModel;
            this.DataContext = this.mainViewModel;

            this.Loaded += this.MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await this.mainViewModel.InitializeAsync();
        }

        private void VideoFrame_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // You cannot bind the (read-only) Width property directly,
            // so set it in the size changed event handler manually
            this.mainViewModel.ImageWidth = this.videoFrame.ActualWidth;
        }
    }
}
