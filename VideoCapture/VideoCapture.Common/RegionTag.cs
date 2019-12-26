using System;
using System.Collections.Generic;
using System.Text;

namespace VideoCapture.Common
{
    public class RegionTag
    {
        public RegionTag(int x, int y, int width, int height, string displayText, double confidence)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.DisplayText = displayText;
            this.Confidence = confidence;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public double Confidence { get; set; }

        public string DisplayText { get; set; }
    }
}
