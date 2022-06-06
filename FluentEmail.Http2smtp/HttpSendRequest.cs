using System.Collections.Generic;
using System.Linq;

namespace FluentEmail.Http2smtp
{
    class HttpSendRequest
    {
        public EmailAddress From { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }
        public string PlaintextAlternativeBody { get; set; }
        public string Body { get; set; }
        public SmtpOptions Smtp { get; set; }
        public IEnumerable<EmailAddress> ToAddresses { get; set; } = Enumerable.Empty<EmailAddress>();
        public IEnumerable<EmailAddress> CcAddresses { get; set; } = Enumerable.Empty<EmailAddress>();
        public IEnumerable<EmailAddress> BccAddresses { get; set; } = Enumerable.Empty<EmailAddress>();
        public IEnumerable<EmailAddress> ReplyToAddresses { get; set; } = Enumerable.Empty<EmailAddress>();

        public HttpSendRequest() { }

        public HttpSendRequest(FluentEmail.Core.Models.EmailData data, SmtpOptions smtpOptions)
        {
            Smtp = smtpOptions;
            IsHtml = data.IsHtml;
            Subject = data.Subject;
            PlaintextAlternativeBody = data.PlaintextAlternativeBody;
            Body = data.Body;
            From = data.FromAddress;
            ToAddresses = Map(data.ToAddresses);
            CcAddresses = Map(data.CcAddresses);
            BccAddresses = Map(data.BccAddresses);
            ReplyToAddresses = Map(data.ReplyToAddresses);
        }

        static IEnumerable<EmailAddress> Map(IEnumerable<FluentEmail.Core.Models.Address> addresses)
        {
            return addresses
                .Select(x => new EmailAddress(x))
                .ToArray();
        }
    }

    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public EmailAddress() {}

        public EmailAddress(FluentEmail.Core.Models.Address address)
        {
            Name = address.Name;
            Address = address.EmailAddress;
        }

        public static implicit operator EmailAddress(FluentEmail.Core.Models.Address address) => new EmailAddress(address);
    }
}
