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
        Task<ImageInformation> AnalyzeImageAsync(byte[] image, string mimeType, int width, int height);

        double CostsPerRequest { get; }
    }
}
