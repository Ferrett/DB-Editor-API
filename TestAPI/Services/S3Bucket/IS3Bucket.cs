using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebAPI.Services.S3Bucket
{
    public interface IS3Bucket
    {
        Task AddObject(IFormFile file, Guid fileGuid);

        Task DeleteObject(string file);
    }
}
