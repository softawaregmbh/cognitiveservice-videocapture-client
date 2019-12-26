// <copyright file="IImageAnalyzer.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace VideoCapture.Common
{
    /// <summary>
    /// Interface for image analyzers.
    /// </summary>
    public interface IImageAnalyzer
    {
        /// <summary>
        /// Gets the approx. costs per request.
        /// </summary>
        /// <value>
        /// The costs per request.
        /// </value>
        double CostsPerRequest { get; }

        /// <summary>
        /// Exracts further information e.g. by using external services.
        /// </summary>
        /// <param name="image">The image as byte array.</param>
        /// <param name="mimeType">The MIME type of the image.</param>
        /// <returns>An instance of ImageInformation class or null.</returns>
        Task<ImageInformation> AnalyzeImageAsync(byte[] image, string mimeType);
    }
}
