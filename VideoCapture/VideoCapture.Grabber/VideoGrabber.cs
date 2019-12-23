using OpenCvSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VideoCapture.Grabber
{
    public class VideoGrabber : IVideoGrabber
    {
        public event FrameGrabbedHandler OnFrameGrabbed;

        public async Task StartAsync(int delay = 30)
        {
            OpenCvSharp.VideoCapture capture = new OpenCvSharp.VideoCapture(0);
            using (Mat image = new Mat())
            {
                while (true)
                {
                    capture.Read(image);

                    if (image.Empty())
                    {
                        break;
                    }

                    using (var stream = image.ToMemoryStream(".jpg", new ImageEncodingParam(ImwriteFlags.JpegQuality, 100)))
                    {
                        OnFrameGrabbed?.Invoke(stream);
                    }

                    await Task.Delay(delay);
                }
            }
        }
    }
}
