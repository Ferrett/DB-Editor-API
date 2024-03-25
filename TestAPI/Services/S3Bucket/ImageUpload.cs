using Amazon;
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

        public async Task AddObject(IFormFile file, Guid fileName)
        {
            using (IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUNorth1))
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);

                var putObject = new PutObjectRequest
                {
                    BucketName =  BucketName,
                    ContentType = ContentType,
                    Key = new Uri(BucketUrl).AbsolutePath.TrimStart('/') + fileName.ToString(),
                    InputStream = ms,
                };

                await client.PutObjectAsync(putObject);
            }
        }

        public async Task DeleteObject(string fileName)
        {
            using (IAmazonS3 client = new AmazonS3Client(RegionEndpoint.EUNorth1))
            {
                fileName = fileName.Split('/').Last();

                if (fileName == Placeholder)
                    return;

                var deleteObject = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = new Uri(BucketUrl).AbsolutePath.TrimStart('/') + fileName
                };

                await client.DeleteObjectAsync(deleteObject);
            }
        } 
    }
}