using System;
using System.Collections.Generic;
using System.Text;

namespace CognitiveServices.CustomVision
{
    public class CustomVisionSettings
    {
        public string Endpoint { get; set; }

        public string PredictionKey { get; set; }

        public string ProjectId { get; set; }

        public string Iteration { get; set; }

        public double RecognitionThreshold { get; set; }
    }
}
