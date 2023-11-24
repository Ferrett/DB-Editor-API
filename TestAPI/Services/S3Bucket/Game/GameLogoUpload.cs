namespace WebAPI.Services.S3Bucket.Game
{
    public class GameLogoUpload : ImageUpload
    {
        public override string BucketUrl { get => @"https://webapilogos.s3.eu-north-1.amazonaws.com/game/"; }
    }
}
