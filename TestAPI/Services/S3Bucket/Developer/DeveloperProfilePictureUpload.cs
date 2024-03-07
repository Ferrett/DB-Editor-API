namespace WebAPI.Services.S3Bucket.Developer
{
    public class DeveloperProfilePictureUpload : ImageUpload
    {
        public override string BucketUrl { get; }

        public DeveloperProfilePictureUpload(IConfiguration configuration)
        {
            BucketUrl = configuration["ApiLinks:DeveloperUrl"]!;
        }
    }
}
