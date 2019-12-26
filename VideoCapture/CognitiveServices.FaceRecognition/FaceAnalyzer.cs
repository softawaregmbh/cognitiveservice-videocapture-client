using Microsoft.Azure.CognitiveServices.Vision.Face;
using System;
using System.Threading.Tasks;
using VideoCapture.Common;

namespace CognitiveServices.FaceRecognition
{
    public class FaceAnalyzer : IImageAnalyzer
    {
        private readonly string subscriptionKey;
        private readonly FaceClient faceApi;

        public FaceAnalyzer(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey ?? throw new ArgumentNullException(nameof(subscriptionKey));
            this.faceApi = new FaceClient(new ApiKeyServiceClientCredentials(this.subscriptionKey));
        }

        public Task<ImageInformation> AnalyzeImageAsync(byte[] image, string mimeType)
        {
            throw new NotImplementedException();
        }
    }
}
