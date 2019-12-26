using softaware.ViewPort.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoCapture.Common;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly IVideoGrabber videoGrabber;
        private readonly TimeSpan minDelayBetweenAnalysis;
        private readonly IImageAnalyzer[] imageAnalyzers;
        private IDictionary<IImageAnalyzer, AnalyzerStatisticsViewModel> statistics;
        private byte[] currentFrame;
        private int frameWidth;
        private int frameHeight;
        private double imageWidth;
        private DateTime? lastAnalysis = null;
        private bool currentlyAnalysing;

        public MainViewModel(
            IVideoGrabber videoGrabber, 
            TimeSpan minDelayBetweenAnalysis, 
            params IImageAnalyzer[] imageAnalyzers)
        {
            this.videoGrabber = videoGrabber ?? throw new ArgumentNullException(nameof(videoGrabber));
            this.minDelayBetweenAnalysis = minDelayBetweenAnalysis;
            this.imageAnalyzers = imageAnalyzers;
            this.RegionTags = new ObservableCollection<RegionTagViewModel>();
            this.Statistics = imageAnalyzers.ToDictionary(a => a, a => new AnalyzerStatisticsViewModel()
            {
                CostsPerRequest = a.CostsPerRequest,
                Count = 0,
                Name = a.GetType().Name
            });
        }

        public async Task InitializeAsync()
        {
            this.videoGrabber.OnFrameGrabbed += VideoGrabber_OnFrameGrabbedAsync;

            await this.videoGrabber.StartAsync();
        }

        private async Task VideoGrabber_OnFrameGrabbedAsync(MemoryStream displayStream, MemoryStream analysisStream, string mimeType, int width, int height)
        {
            var byteArray = displayStream.ToArray();
            this.CurrentFrame = byteArray;
            this.FrameWidth = width;

            if (!currentlyAnalysing && (lastAnalysis == null || DateTime.Now.Subtract(lastAnalysis.Value) > minDelayBetweenAnalysis))
            {
                lastAnalysis = DateTime.Now;
                this.currentlyAnalysing = true;

                var analysisArray = analysisStream.ToArray();

                foreach (var imageAnalyzer in imageAnalyzers)
                {
                    DateTime startTime = DateTime.Now;
                    var info = await imageAnalyzer.AnalyzeImageAsync(analysisArray, mimeType);

                    this.Statistics[imageAnalyzer].LastDuration = DateTime.Now.Subtract(startTime);
                    this.Statistics[imageAnalyzer].Count++;

                    var existingRegions = this.RegionTags.Where(r => r.ImageAnalyzerType == imageAnalyzer.GetType()).ToList();
                    if (existingRegions.Any())
                    {
                        foreach (var regionTag in existingRegions)
                        {
                            this.RegionTags.Remove(regionTag);
                        }
                    }

                    if (info != null)
                    {
                        foreach (var regionTag in info.RegionTags)
                        {
                            this.RegionTags.Add(new RegionTagViewModel()
                            {
                                ImageAnalyzerType = imageAnalyzer.GetType(),
                                RegionTag = regionTag
                            });
                        }
                    }
                }

                this.currentlyAnalysing = false;
            }
        }

        public ObservableCollection<RegionTagViewModel> RegionTags { get; set; }

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

        public IDictionary<IImageAnalyzer, AnalyzerStatisticsViewModel> Statistics
        {
            get { return statistics; }
            set { this.SetProperty(ref this.statistics, value); }
        }
    }
}
