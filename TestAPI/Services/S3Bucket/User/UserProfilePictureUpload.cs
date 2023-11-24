namespace WebAPI.Services.S3Bucket.User
{
    public class UserProfilePictureUpload : ImageUpload
    {
        public override string BucketUrl { get => @"https://webapilogos.s3.eu-north-1.amazonaws.com/user/"; }
    }
}
