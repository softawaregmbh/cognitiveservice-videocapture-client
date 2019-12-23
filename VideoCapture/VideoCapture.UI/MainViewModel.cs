using softaware.ViewPort.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly IVideoGrabber videoGrabber;
        private byte[] currentFrame;

        public MainViewModel(IVideoGrabber videoGrabber)
        {
            this.videoGrabber = videoGrabber ?? throw new ArgumentNullException(nameof(videoGrabber));
        }

        public async Task InitializeAsync()
        {
            this.videoGrabber.OnFrameGrabbed += VideoGrabber_OnFrameGrabbed;
            await this.videoGrabber.StartAsync();
        }

        private void VideoGrabber_OnFrameGrabbed(MemoryStream stream)
        {
            this.CurrentFrame = stream.ToArray();
        }
        
        public byte[] CurrentFrame
        {
            get { return this.currentFrame; }
            set { this.SetProperty(ref currentFrame, value); }
        }
    }
}
