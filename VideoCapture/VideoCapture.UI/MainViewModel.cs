using softaware.ViewPort.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VideoCapture.Common;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly IVideoGrabber videoGrabber;
        private readonly IImageAnalyzer[] imageAnalyzers;
        private byte[] currentFrame;
        private int frameWidth;
        private int frameHeight;
        private double imageWidth;

        public MainViewModel(IVideoGrabber videoGrabber, params IImageAnalyzer[] imageAnalyzers)
        {
            this.videoGrabber = videoGrabber ?? throw new ArgumentNullException(nameof(videoGrabber));
            this.imageAnalyzers = imageAnalyzers;
            this.RegionTags = new ObservableCollection<RegionTag>();
        }

        public async Task InitializeAsync()
        {
            this.videoGrabber.OnFrameGrabbed += VideoGrabber_OnFrameGrabbedAsync;
            await this.videoGrabber.StartAsync();
        }

        private async Task VideoGrabber_OnFrameGrabbedAsync(MemoryStream stream, string mimeType, int width, int height)
        {
            var byteArray = stream.ToArray();
            this.CurrentFrame = byteArray;
            this.FrameWidth = width;

            this.RegionTags.Clear();

            foreach (var imageAnalyzer in imageAnalyzers)
            {
                var info = await imageAnalyzer.AnalyzeImageAsync(byteArray, mimeType);
                foreach (var regionTag in info.RegionTags)
                {
                    this.RegionTags.Add(regionTag);
                }
            }
        }

        public ObservableCollection<RegionTag> RegionTags { get; set; }

        public byte[] CurrentFrame
        {
            get { return this.currentFrame; }
            set { this.SetProperty(ref currentFrame, value); }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
            set 
            { 
                if (this.frameWidth != value)
                {
                    this.SetProperty(ref this.frameWidth, value);
                    this.RaisePropertyChanged(nameof(this.FrameToImageScale));
                }

            }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
            set { this.SetProperty(ref this.frameHeight, value); }
        }

        public double ImageWidth
        {
            get { return imageWidth; }
            set 
            {
                if (this.imageWidth != value)
                {
                    this.SetProperty(ref this.imageWidth, value);
                    this.RaisePropertyChanged(nameof(this.FrameToImageScale));
                }
            }
        }

        public double FrameToImageScale
        {
            get
            {
                return (double)this.ImageWidth / this.FrameWidth;
            }
        }
    }
}
