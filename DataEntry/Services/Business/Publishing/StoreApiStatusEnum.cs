namespace dataentry.Services.Business.Publishing
{
    public enum StoreApiStatusEnum : int
        {
            Success = 2000,
            Failure = 4000,
            ValidationFailed = 4100,
            SchemaValidationFailed = 4102,
            DataValidationFailed = 4103,
            FailedToAccessResource = 4200
        }
}