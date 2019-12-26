// <copyright file="VideoGrabber.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading.Tasks;
using OpenCvSharp;

namespace VideoCapture.Grabber
{
    /// <summary>
    /// Video grabber using the OpenCvSharp library.
    /// </summary>
    /// <seealso cref="VideoCapture.Grabber.IVideoGrabber" />
    public class VideoGrabber : IVideoGrabber
    {
        /// <inheritdoc/>
        public event FrameGrabbedHandler OnFrameGrabbed;

        /// <inheritdoc/>
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
                        this.OnFrameGrabbed?.Invoke(
                            displayStream,
                            analysisStream,
                            "image/jpeg",
                            analysisImage.Width,
                            analysisImage.Height);
                    }

                    await Task.Delay(delay);
                }
            }
        }
    }
}
