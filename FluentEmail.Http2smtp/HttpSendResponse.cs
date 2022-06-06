using System.Collections.Generic;
using System.Linq;

namespace FluentEmail.Http2smtp
{
    class HttpSendResponse
    {
        public string MessageId { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public bool Successful => !ErrorMessages.Any();
    }
}
