using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FluentEmail.Http2smtp
{
    public static class FluentEmailHttp2smtpBuilderExtensions
    {
        public static FluentEmailServicesBuilder AddHttp2smtpSender(this FluentEmailServicesBuilder builder, string apiKey, SmtpOptions smtpOptions)
        {
            builder.Services.TryAdd(ServiceDescriptor.Scoped<ISender>(_ => new Http2smtpSender(apiKey, smtpOptions)));
            return builder;
        }
    }
}
