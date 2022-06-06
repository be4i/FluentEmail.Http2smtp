using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FluentEmail.Http2smtp
{
    public class Http2smtpSender : ISender
    {
        static Uri BaseAddress { get; } = new Uri("https://http2smtp.p.rapidapi.com");
        private readonly SmtpOptions _smtpOptions;
        private readonly HttpClient _httpClient;

        public Http2smtpSender(string apiKey, SmtpOptions smtpOptions)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = BaseAddress
            };

            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", BaseAddress.Host);

            _smtpOptions = smtpOptions;
        }

        public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
        {
            return SendAsync(email, token).GetAwaiter().GetResult();
        }

        public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
        {
            var request = new HttpSendRequest(email.Data, _smtpOptions);
            var json = JsonContent.Create(request);
            var httpResponse = await _httpClient.PostAsync("send", json, token.GetValueOrDefault());
            var result = new SendResponse();

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<HttpSendResponse>(cancellationToken: token.GetValueOrDefault());
                result.MessageId = response.MessageId;

                if (!response.Successful)
                {
                    result.ErrorMessages.AddRange(response.ErrorMessages);
                }
            }
            else 
                result.ErrorMessages.Add(httpResponse.ReasonPhrase);

            return result;
        }
    }
}
