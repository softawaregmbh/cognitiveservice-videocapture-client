// <copyright file="MainViewModel.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using softaware.ViewPort.Core;
using VideoCapture.Common;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    /// <seealso cref="softaware.ViewPort.Core.NotifyPropertyChanged" />
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="videoGrabber">The video grabber.</param>
        /// <param name="minDelayBetweenAnalysis">The minimum delay between two analysis steps.</param>
        /// <param name="imageAnalyzers">The image analyzers.</param>
        /// <exception cref="System.ArgumentNullException">The video grabber instance must be set.</exception>
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
                Name = a.GetType().Name,
            });
        }

        /// <summary>
        /// Gets or sets the region tags.
        /// </summary>
        public ObservableCollection<RegionTagViewModel> RegionTags { get; set; }

        /// <summary>
        /// Gets or sets the current frame of the video grabber.
        /// </summary>
        public byte[] CurrentFrame
        {
            get { return this.currentFrame; }
            set { this.SetProperty(ref this.currentFrame, value); }
        }

        /// <summary>
        /// Gets or sets the width of the analysis stream frame.
        /// </summary>
        public int FrameWidth
        {
            get
            {
                return this.frameWidth;
            }

            set
            {
                if (this.frameWidth != value)
                {
                    this.SetProperty(ref this.frameWidth, value);
                    this.RaisePropertyChanged(nameof(this.FrameToImageScale));
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the analysis stream frame.
        /// </summary>
        public int FrameHeight
        {
            get { return this.frameHeight; }
            set { this.SetProperty(ref this.frameHeight, value); }
        }

        /// <summary>
        /// Gets or sets the width of the display stream frame.
        /// </summary>
        public double ImageWidth
        {
            get
            {
                return this.imageWidth;
            }

            set
            {
                if (this.imageWidth != value)
                {
                    this.SetProperty(ref this.imageWidth, value);
                    this.RaisePropertyChanged(nameof(this.FrameToImageScale));
                }
            }
        }

        /// <summary>
        /// Gets the scale factor between the analysis stream and the visualized image.
        /// </summary>
        public double FrameToImageScale
        {
            get
            {
                return (double)this.ImageWidth / this.FrameWidth;
            }
        }

        /// <summary>
        /// Gets or sets the statistics dictionary with information per image analyzer.
        /// </summary>
        public IDictionary<IImageAnalyzer, AnalyzerStatisticsViewModel> Statistics
        {
            get { return this.statistics; }
            set { this.SetProperty(ref this.statistics, value); }
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InitializeAsync()
        {
            this.videoGrabber.OnFrameGrabbed += this.VideoGrabber_OnFrameGrabbedAsync;

            await this.videoGrabber.StartAsync();
        }

        private async Task VideoGrabber_OnFrameGrabbedAsync(MemoryStream displayStream, MemoryStream analysisStream, string mimeType, int width, int height)
        {
            var byteArray = displayStream.ToArray();
            this.CurrentFrame = byteArray;
            this.FrameWidth = width;

            if (!this.currentlyAnalysing && (this.lastAnalysis == null || DateTime.Now.Subtract(this.lastAnalysis.Value) > this.minDelayBetweenAnalysis))
            {
                this.lastAnalysis = DateTime.Now;
                this.currentlyAnalysing = true;

                var analysisArray = analysisStream.ToArray();

                foreach (var imageAnalyzer in this.imageAnalyzers)
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
                                RegionTag = regionTag,
                            });
                        }
                    }
                }

                this.currentlyAnalysing = false;
            }
        }
    }
}
