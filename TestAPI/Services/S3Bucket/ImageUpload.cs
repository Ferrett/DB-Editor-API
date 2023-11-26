using Amazon;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebAPI.Services.S3Bucket
{
    public abstract class ImageUpload : IS3Bucket
    {
        public string Placeholder { get; } = @"dummy.png";
        public string ContentType { get; } = @"image/png";
        public string BucketName { get; } = @"webapilogos";
        public abstract string BucketUrl { get; }


        public async Task AddObject(IFormFile file, Guid fileGuid)
        {
            using (IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUNorth1))
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);

                var putObject = new PutObjectRequest
                {
                    BucketName = BucketName + new Uri(BucketUrl).AbsolutePath.TrimEnd('/'),
                    ContentType = ContentType,
                    Key = fileGuid.ToString(),
                    InputStream = ms,
                };

                await client.PutObjectAsync(putObject);
            }
        }

        public async Task DeleteObject(string file)
        {
            using (IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUNorth1))
            {
                file = file.Split('/').Last();

                if (file == Placeholder)
                    return;

                var deleteObject = new DeleteObjectRequest
                {
                    BucketName = BucketName + new Uri(BucketUrl).AbsolutePath.TrimEnd('/'),
                    Key = file
                };

                await client.DeleteObjectAsync(deleteObject);
            }
        }
    }
}
