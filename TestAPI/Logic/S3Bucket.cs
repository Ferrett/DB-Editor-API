using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebAPI.Logic
{
    public static class S3Bucket
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUNorth1;

        private static IAmazonS3 client = new AmazonS3Client(bucketRegion);

        public static string DeveloperBucketUrl = @"https://webapilogos.s3.eu-north-1.amazonaws.com/developer/";
        public static string GameBucketUrl = @"https://webapilogos.s3.eu-north-1.amazonaws.com/game/";
        public static string UserBucketUrl = @"https://webapilogos.s3.eu-north-1.amazonaws.com/user/";

        public static string DeveloperBucketPath = @"webapilogos/developer";
        public static string GameBucketPath = @"webapilogos/game";
        public static string UserBucketPath = @"webapilogos/user";

        public static string DefaultLogoName = "dummy.png";

        public static async Task AddObject(IFormFile file, string bucket, Guid guid)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);

            var putRequest2 = new PutObjectRequest
            {
                BucketName = bucket,
                ContentType = "image/png",
                Key = guid.ToString(),
                InputStream = ms,
            };

            await client.PutObjectAsync(putRequest2);
        }

        public static async Task DeleteObject(string file, string bucket)
        {
            file = file.Split('/').Last();

            if (file == DefaultLogoName)
                return;

            var cock = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = file
            };

            await client.DeleteObjectAsync(cock);
        }
    }
}
