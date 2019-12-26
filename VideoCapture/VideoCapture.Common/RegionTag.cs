// <copyright file="RegionTag.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace VideoCapture.Common
{
    /// <summary>
    /// Additional information concerning a region inside the image.
    /// </summary>
    public class RegionTag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionTag"/> class.
        /// </summary>
        /// <param name="x">The x-position of the bounding rectangle.</param>
        /// <param name="y">The y-position of the bounding rectangle.</param>
        /// <param name="width">The width of the bounding rectangle.</param>
        /// <param name="height">The height of the bounding rectangle.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="confidence">The confidence between 0 and 1 (= certain).</param>
        public RegionTag(int x, int y, int width, int height, string displayText, double confidence)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.DisplayText = displayText;
            this.Confidence = confidence;
        }

        /// <summary>
        /// Gets or sets the x-position of the bounding rectangle.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-position of the bounding rectangle.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the bounding rectangle.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the bounding rectangle.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the confidence (between 0 and 1).
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        public string DisplayText { get; set; }
    }
}
