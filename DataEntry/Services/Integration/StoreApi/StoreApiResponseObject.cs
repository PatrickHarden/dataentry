
using dataentry.Services.Business.Publishing;

namespace dataentry.Services.Integration.StoreApi
{
    public class StoreApiResponseObject
    {
        public StoreApiStatusEnum StoreApiStatusCode { get; set; }
        public string Data { get; set; }
        public string Message { get; set; }
    }
}