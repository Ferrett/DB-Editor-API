namespace WebAPI.Services.S3Bucket
{
    public class GameLogoUpload : ImageUpload
    {
        public override string BucketUrl { get => @"https://webapilogos.s3.eu-north-1.amazonaws.com/game/"; }
        public override string BucketPath { get => @"webapilogos/game"; }

    }
}
