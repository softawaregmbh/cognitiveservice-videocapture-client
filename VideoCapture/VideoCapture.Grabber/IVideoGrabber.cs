// <copyright file="IVideoGrabber.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VideoCapture.Grabber
{
    /// <summary>
    /// Delegate for the OnFrameGrabbed event.
    /// </summary>
    /// <param name="displayStream">The display stream.</param>
    /// <param name="analysisStream">The analysis stream (usually smaller and with less quality).</param>
    /// <param name="mimeType">The MIME type of the image.</param>
    /// <param name="width">The width of the analysis stream frame.</param>
    /// <param name="height">The height of the analysis stream frame.</param>
    /// <returns>A task.</returns>
    public delegate Task FrameGrabbedHandler(MemoryStream displayStream, MemoryStream analysisStream, string mimeType, int width, int height);

    /// <summary>
    /// Interface for video grabber implementations.
    /// </summary>
    public interface IVideoGrabber
    {
        /// <summary>
        /// Occurs when a new frame is present.
        /// </summary>
        event FrameGrabbedHandler OnFrameGrabbed;

        /// <summary>
        /// Starts the video grabbing.
        /// </summary>
        /// <param name="delay">The delay between to frames.</param>
        /// <returns>A task.</returns>
        Task StartAsync(int delay = 30);
    }
}
