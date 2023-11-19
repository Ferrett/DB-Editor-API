using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebAPI.Services.S3Bucket
{
    public abstract class ImageUpload : IS3Bucket
    {
        public static IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUNorth1);
        public string Placeholder { get; } = @"dummy.png";
        public string ContentType { get; } = @"image/png";

        public abstract string BucketUrl { get; }
        public abstract string BucketPath { get; }


        public async Task AddObject(IFormFile file, Guid fileGuid)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);

            var putObject = new PutObjectRequest
            {
                BucketName = BucketPath,
                ContentType = ContentType,
                Key = fileGuid.ToString(),
                InputStream = ms,
            };

            await client.PutObjectAsync(putObject);
        }

        public async Task DeleteObject(string file)
        {
            file = file.Split('/').Last();

            if (file == Placeholder)
                return;

            var deleteObject = new DeleteObjectRequest
            {
                BucketName = BucketPath,
                Key = file
            };

            await client.DeleteObjectAsync(deleteObject);
        }
    }
}
