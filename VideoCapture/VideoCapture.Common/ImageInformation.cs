// <copyright file="ImageInformation.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace VideoCapture.Common
{
    /// <summary>
    /// Contains additional information about an image, usually provided by an IImageAnalyzer implementation.
    /// </summary>
    public class ImageInformation
    {
        /// <summary>
        /// Gets or sets the region tags.
        /// </summary>
        public IList<RegionTag> RegionTags { get; set; }
    }
}
