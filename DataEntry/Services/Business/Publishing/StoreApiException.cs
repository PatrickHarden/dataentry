using dataentry.Data.Enums;
using System;

namespace dataentry.Services.Business.Publishing
{
    public class StoreApiException : Exception
    {
        private string _status;
        private string _message;

        public StoreApiException(string status, string message)
        {
            _status = status;
            _message = message;
        }

        public override string Message {
            get
            {
                if (_status == null)
                    return $"{{\"Message\": \"{_message}\"}}";

                return $"{{\"StatusCode\": {_status}, \"Message\": \"{_message}\"}}";
            }
        }
    }
}
