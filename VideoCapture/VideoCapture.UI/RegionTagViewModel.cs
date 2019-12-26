// <copyright file="RegionTagViewModel.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using softaware.ViewPort.Core;
using VideoCapture.Common;

namespace VideoCapture.UI
{
    public class RegionTagViewModel : NotifyPropertyChanged
    {
        public Type ImageAnalyzerType { get; set; }

        public RegionTag RegionTag { get; set; }
    }
}
