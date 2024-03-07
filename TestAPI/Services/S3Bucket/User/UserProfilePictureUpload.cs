namespace WebAPI.Services.S3Bucket.User
{
    public class UserProfilePictureUpload : ImageUpload
    {
        public override string BucketUrl { get; }

        public UserProfilePictureUpload(IConfiguration configuration)
        {
            BucketUrl= configuration["ApiLinks:UserUrl"]!;
        }
       
    }
}
