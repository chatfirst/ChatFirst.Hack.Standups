using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using ChatFirst.Hack.Standups.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace ChatFirst.Hack.Standups.Services
{
    internal class ConnectorClient : IConnectorClient
    {
        private readonly RestClient _restClient;

        public ConnectorClient()
        {
            _restClient = new RestClient("https://ch-core-mp.azurewebsites.net/v1/")
            {
                Authenticator = new HttpBasicAuthenticator(ConfigService.Get(Constants.UserToken), string.Empty)
            };
        }

        public Task PushEndOfMeetingAsync(string botName, string userId)
        {
            var message = ConfigService.Get(Constants.TemplateMessage3);
            var pushData = new ExternalMessage
            {
                Count = 1,
                ForcedState = string.Empty,
                Messages = new List<string> {message}
            };

            return PerformPushRequest(botName, userId, pushData);
        }

        public Task PushRemoteChatService(string botName, string userId, string userName)
        {
            //0 - userId, 1 - userName
            const string templatePerson = "<@personId:{0}|{1}>";
            var templateMsg1 = string.Format(ConfigService.Get(Constants.TemplateMessage1), templatePerson);
            var templateMsg2 = ConfigService.Get(Constants.TemplateMessage2);
            var pushData = new ExternalMessage
            {
                Count = 1,
                ForcedState = ConfigService.Get(Constants.ForcedState),
                Messages =
                    new List<string> {string.Format(templateMsg1, userId, userName), templateMsg2}
            };

            return PerformPushRequest(botName, userId, pushData);
        }

        private async Task PerformPushRequest(string botName, string userId, ExternalMessage pushData)
        {
            var req = new RestRequest("push/{botName}", Method.POST);
            req.AddUrlSegment("botName", botName);
            req.AddQueryParameter("id", userId);
            req.AddQueryParameter("channel", "spark");

            req.AddJsonBody(pushData);
            var response = await _restClient.ExecuteTaskAsync(req);
            Trace.TraceInformation("[MeetingService.PushRemoteChatService] response: " + response.Content);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpException((int) HttpStatusCode.OK, "Failed to push answer", response.ErrorException);
        }
    }
}