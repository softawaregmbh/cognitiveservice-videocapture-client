using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VideoCapture.Grabber
{
    public delegate void FrameGrabbedHandler(MemoryStream stream);
    
    public interface IVideoGrabber
    {
        event FrameGrabbedHandler OnFrameGrabbed;

        Task StartAsync(int delay = 30);
    }
}
