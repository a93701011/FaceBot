namespace FaceRegonizeBot.Services
{
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Face.Contract;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    [Serializable]
    public class FaceApiService : IFaceCaptionService, IFaceVerifyService
    {
        /// <summary>
        /// Microsoft Computer Vision API key.
        /// </summary>
        //private static readonly string ApiKey = WebConfigurationManager.AppSettings["MicrosoftVisionApiKey"];
        const string ApiKey = "fab0cb2e16b3455ba760e08ee112407d";

        const string UriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";

        private static readonly FaceAttributeType[] faceAttributes = { FaceAttributeType.Emotion, FaceAttributeType.Age, FaceAttributeType .Gender};

        public async Task<string> GetCaptionAsync(string url)
        {
            var client = new FaceServiceClient(ApiKey, UriBase);
            Face[] result = await client.DetectAsync(url, returnFaceAttributes:faceAttributes);
            return ProcessAnalysisResult(result);
        }

        public async Task<string> GetCaptionAsync(Stream stream)
        {
            var client = new FaceServiceClient(ApiKey, UriBase);
            Face[] result = await client.DetectAsync(stream, returnFaceAttributes: faceAttributes);
            return ProcessAnalysisResult(result);
        }
        //Age
        private static string ProcessAnalysisResult(Face[] result)
        {
            if (result.Length > 0)
            {
                return result[0].FaceAttributes.Age.ToString();
            }
            else
            {
                return "Couldn't find a caption for this one";
            }
        }
        public async Task<string> GetFaceGuidAsync(Stream stream)
        {
            var client = new FaceServiceClient(ApiKey, UriBase);
            Face[] result = await client.DetectAsync(stream, returnFaceAttributes: faceAttributes);
            return ProcessGUIDResult(result);
        }

        public async Task<String> GetVerifyAsync(string faceguid1, string faceguid2)
        {
            var client = new FaceServiceClient(ApiKey, UriBase);
            VerifyResult result = await client.VerifyAsync(System.Guid.Parse(faceguid1), System.Guid.Parse(faceguid2));
            return result.IsIdentical.ToString(); 
        }

        private static string ProcessGUIDResult(Face[] result)
        {
            if (result.Length > 0)
            {
                return result[0].FaceId.ToString();
            }
            else
            {
                return "Couldn't find a caption for this one";
            }
        }

    }
}