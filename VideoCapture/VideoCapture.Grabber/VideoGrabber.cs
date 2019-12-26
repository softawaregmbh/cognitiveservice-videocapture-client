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

                    int analysisWidth = 320;
                    int analysisHeight = analysisWidth * image.Height / image.Width;
                    var analysisImage = image.Clone().Resize(new Size(analysisWidth, analysisHeight));

                    using (var analysisStream = analysisImage.ToMemoryStream(".jpg", new ImageEncodingParam(ImwriteFlags.JpegQuality, 50)))
                    using (var displayStream = image.ToMemoryStream(".jpg", new ImageEncodingParam(ImwriteFlags.JpegQuality, 100)))
                    {
                        OnFrameGrabbed?.Invoke(
                            displayStream,
                            analysisStream, "image/jpeg", analysisImage.Width, analysisImage.Height);
                    }

                    await Task.Delay(delay);
                }
            }
        }
    }
}
