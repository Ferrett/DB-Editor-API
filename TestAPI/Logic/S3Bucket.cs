using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;


namespace WebAPI.Logic
{
    public static class S3Bucket
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUNorth1;

        private static IAmazonS3 client = new AmazonS3Client(bucketRegion);

        public static async Task AddObject(IFormFile file,string bucket, Guid guid)
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

            PutObjectResponse response2 = await client.PutObjectAsync(putRequest2);
        }

        public static async Task DeleteObject(string file, string bucket)
        {
            file = file.Split('/').Last();

            if (file == "dummy.png")
                return;

            var cock = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = file
            };

            DeleteObjectResponse response2 = await client.DeleteObjectAsync(cock);
        }
    }
}
