using System;
using System.Collections.Generic;
using System.Text;

namespace VideoCapture.Grabber
{
    public class VideoGrabberSettings
    {
        public VideoType VideoType { get; set; }

        public string Url { get; set; }

        public int WebcamIndex { get; set; }
    }
}
