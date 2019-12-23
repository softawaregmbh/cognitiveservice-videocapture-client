using System;
using System.Collections.Generic;
using System.Text;

namespace VideoCapture.Common
{
    public class RegionTag
    {
        public RegionTag(int x, int y, int width, int height, string displayText)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            DisplayText = displayText;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string DisplayText { get; set; }
    }
}
