namespace FaceRegonizeBot.Services
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFaceVerifyService
    {
        Task<string> GetVerifyAsync(string faceguid1, string faceguid2);

        Task<string> GetFaceGuidAsync(Stream stream);
    }
}