using softaware.ViewPort.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VideoCapture.Common;

namespace VideoCapture.UI
{
    public class RegionTagViewModel : NotifyPropertyChanged
    {
        public Type ImageAnalyzerType { get; set; }

        public RegionTag RegionTag { get; set; }
    }
}
