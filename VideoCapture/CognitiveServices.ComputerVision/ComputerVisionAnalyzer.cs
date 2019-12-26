using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoCapture.Common;

namespace CognitiveServices.ComputerVision
{
    public class ComputerVisionAnalyzer : IImageAnalyzer
    {
        private ComputerVisionClient computerVisionApi;

        public ComputerVisionAnalyzer(IOptions<ComputerVisionSettings> settings)
        {
            this.computerVisionApi = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(settings.Value.SubscriptionKey));

            this.computerVisionApi.Endpoint = settings.Value.Endpoint;
        }

        public double CostsPerRequest => 0.84/1000;

        public async Task<ImageInformation> AnalyzeImageAsync(byte[] image, string mimeType)
        {
            using (var memoryStream = new MemoryStream(image))
            {
                var result = await this.computerVisionApi.DetectObjectsInStreamAsync(memoryStream);

                if (result.Objects.Any())
                {
                    var tags = result.Objects.Select(o => new RegionTag(o.Rectangle.X, o.Rectangle.Y, o.Rectangle.W, o.Rectangle.H, o.ObjectProperty, o.Confidence)).ToList();
                    return new ImageInformation()
                    {
                        RegionTags = tags
                    };
                }
            }

            return null;
        }
    }
}
