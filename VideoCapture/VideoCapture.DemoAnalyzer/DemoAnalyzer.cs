using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoCapture.Common;

namespace VideoCapture.DemoAnalyzer
{
    public class DemoAnalyzer : IImageAnalyzer
    {
        public Task<ImageInformation> AnalyzeImageAsync(byte[] image, string mimeType)
        {
            return Task.FromResult(new ImageInformation()
            {
                RegionTags = new List<RegionTag>()
                {
                    new RegionTag(50, 50, 100, 50, "Demo")
                }
            });
        }
    }
}
