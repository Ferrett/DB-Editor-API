using Amazon;
using Amazon.S3;

namespace WebAPI.Logic
{
    public interface IS3Bucket
    {
        IAmazonS3 client { get; }
        string DeveloperBucketUrl { get; }
        string GameBucketUrl { get; }
        string UserBucketUrl { get; }
        string DeveloperBucketPath { get; }
        string GameBucketPath { get; }
        string UserBucketPath { get; }
        string DefaultLogoName { get; }

        Task AddObject(IFormFile file, string bucket, Guid userGuid);
        Task DeleteObject(string file, string bucket);
    }
}
