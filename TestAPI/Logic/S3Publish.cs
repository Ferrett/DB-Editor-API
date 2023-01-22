using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;


namespace WebAPI.Logic
{
    public static class S3Publish
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUNorth1;

        private static IAmazonS3 client = new AmazonS3Client(bucketRegion);

        public static async Task WritingAnObjectAsync(IFormFile file,string bucket, Guid guid)
        {

            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);

            var putRequest2 = new PutObjectRequest
            {
                BucketName = bucket,
                Key = guid.ToString(),
                ContentType = file.ContentType,
                InputStream = ms,
            };

            PutObjectResponse response2 = await client.PutObjectAsync(putRequest2);
        }
    }
}
