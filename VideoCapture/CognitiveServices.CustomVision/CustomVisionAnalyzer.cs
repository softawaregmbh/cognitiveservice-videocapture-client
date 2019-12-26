﻿using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoCapture.Common;

namespace CognitiveServices.CustomVision
{
    public class CustomVisionAnalyzer : IImageAnalyzer
    {
        private readonly CustomVisionPredictionClient customVisionApi;
        private readonly CustomVisionSettings settings;

        public double CostsPerRequest => 1.69/1000;

        public CustomVisionAnalyzer(IOptions<CustomVisionSettings> settings)
        {
            this.settings = settings.Value;

            customVisionApi = new CustomVisionPredictionClient();
            customVisionApi.Endpoint = this.settings.Endpoint;
            customVisionApi.ApiKey = this.settings.PredictionKey;
    }

        public async Task<ImageInformation> AnalyzeImageAsync(byte[] image, string mimeType, int width, int height)
        {
            using (var memoryStream = new MemoryStream(image))
            {
                var result = await customVisionApi.DetectImageAsync(
                                Guid.Parse(this.settings.ProjectId),
                                this.settings.Iteration,
                                memoryStream);

                if (result.Predictions?.Any() ?? false)
                {
                    var tags = from p in result.Predictions
                               where p.Probability > this.settings.RecognitionThreshold
                               select new RegionTag(
                                   (int)(p.BoundingBox.Left * width),
                                   (int)(p.BoundingBox.Top * height),
                                   (int)(p.BoundingBox.Width * width),
                                   (int)(p.BoundingBox.Height * height),
                                   p.TagName,
                                   p.Probability);

                    if (tags.Any())
                    {
                        return new ImageInformation()
                        {
                            RegionTags = tags.ToList()
                        };
                    }
                }
            }

            return null;
        }
    }
}
