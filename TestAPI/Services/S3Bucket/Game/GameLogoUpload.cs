namespace WebAPI.Services.S3Bucket.Game
{
    public class GameLogoUpload : ImageUpload
    {
        public override string BucketUrl { get; }

        public GameLogoUpload(IConfiguration configuration)
        {
            BucketUrl = configuration["ApiLinks:GameUrl"]!;
        }
    }
}
