using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
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
                var result = await this.computerVisionApi.AnalyzeImageInStreamAsync(
                    memoryStream,
                    new []
                    {
                        VisualFeatureTypes.Faces,
                        VisualFeatureTypes.Objects
                    });
                
                var tags = new List<RegionTag>();

                if (result.Faces?.Any() ?? false)
                {
                    var faces = result.Faces.Select(f =>
                                    new RegionTag(
                                        f.FaceRectangle.Left,
                                        f.FaceRectangle.Top,
                                        f.FaceRectangle.Width,
                                        f.FaceRectangle.Height,
                                        $"{f.Gender}, Age: {f.Age}",
                                        1.0)).ToList();

                    tags.AddRange(faces);
                }

                if (result.Objects?.Any() ?? false)
                {
                    var objects = result.Objects.Select(o => new RegionTag(o.Rectangle.X, o.Rectangle.Y, o.Rectangle.W, o.Rectangle.H, o.ObjectProperty, o.Confidence)).ToList();
                    tags.AddRange(objects);
                }

                if (tags.Any())
                {
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
