using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebAPI.Logic
{
    public class ImageUploadS3Bucket : IS3Bucket
    {
        public IAmazonS3 client { get; } = new AmazonS3Client(RegionEndpoint.EUNorth1);
        public string DeveloperBucketUrl { get; } = @"https://webapilogos.s3.eu-north-1.amazonaws.com/developer/";
        public string GameBucketUrl { get; } = @"https://webapilogos.s3.eu-north-1.amazonaws.com/game/";
        public string UserBucketUrl { get; } = @"https://webapilogos.s3.eu-north-1.amazonaws.com/user/";
        public string DeveloperBucketPath { get; } = @"webapilogos/developer";
        public string GameBucketPath { get; } = @"webapilogos/game";
        public string UserBucketPath { get; } = @"webapilogos/user";
        public string DefaultLogoName { get; } = @"dummy.png";


        public async Task AddObject(IFormFile file, string bucket, Guid userGuid)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);

            var putRequest2 = new PutObjectRequest
            {
                BucketName = bucket,
                ContentType = "image/png",
                Key = userGuid.ToString(),
                InputStream = ms,
            };

            await client.PutObjectAsync(putRequest2);
        }

        public async Task DeleteObject(string file, string bucket)
        {
            file = file.Split('/').Last();

            if (file == DefaultLogoName)
                return;

            var deleteObject = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = file
            };

            await client.DeleteObjectAsync(deleteObject);
        }
    }
}
