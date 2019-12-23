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

        private async Task VideoGrabber_OnFrameGrabbedAsync(MemoryStream stream)
        {
            var byteArray = stream.ToArray();
            this.CurrentFrame = byteArray;

            this.RegionTags.Clear();

            foreach (var imageAnalyzer in imageAnalyzers)
            {
                var info = await imageAnalyzer.AnalyzeImageAsync(byteArray);
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
    }
}
