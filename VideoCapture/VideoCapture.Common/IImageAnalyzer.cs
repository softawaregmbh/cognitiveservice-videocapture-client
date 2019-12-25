using System;
using System.Threading.Tasks;

namespace VideoCapture.Common
{
    public interface IImageAnalyzer
    {
        Task<ImageInformation> AnalyzeImageAsync(byte[] image, string mimeType);
    }
}
