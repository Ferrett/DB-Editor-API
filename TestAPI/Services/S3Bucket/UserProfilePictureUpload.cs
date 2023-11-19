namespace WebAPI.Services.S3Bucket
{
    public class UserProfilePictureUpload : ImageUpload
    {

        public override string BucketUrl { get => @"https://webapilogos.s3.eu-north-1.amazonaws.com/user/"; }
        public override string BucketPath { get => @"webapilogos/user"; }
    }
}
