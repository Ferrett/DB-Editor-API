namespace WebAPI.Services.S3Bucket
{
    public class DeveloperProfilePictureUpload : ImageUpload
    {
        public override string BucketUrl { get => @"https://webapilogos.s3.eu-north-1.amazonaws.com/developer/"; }
        public override string BucketPath { get => @"webapilogos/developer"; }
    }
}
