namespace dataentry.Services.Integration.Edp
{
    public class AwsS3BucketOptions {
        public string AwsKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string BucketName { get; set; }
        public string BucketRegion { get; set; }
    }
}