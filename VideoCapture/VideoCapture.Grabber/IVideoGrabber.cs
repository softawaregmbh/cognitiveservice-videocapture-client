using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VideoCapture.Grabber
{
    public delegate Task FrameGrabbedHandler(MemoryStream displayStream, MemoryStream analysisStream, string mimeType, int width, int height);
    
    public interface IVideoGrabber
    {
        event FrameGrabbedHandler OnFrameGrabbed;

        Task StartAsync(int delay = 30);
    }
}
