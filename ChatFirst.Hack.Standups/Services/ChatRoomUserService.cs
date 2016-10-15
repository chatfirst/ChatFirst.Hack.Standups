using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Services
{
    using System.Diagnostics;
    using RestSharp;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Models;

    public class ChatRoomUserService : IChatRoomUserService
    {
        public async Task<List<ChatRoomUser>> GetUsersAsync(string roomId, string botName)
        {
            if (string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(botName))
                throw new ArgumentNullException();

            var userToken = ConfigService.Get(Constants.UserToken);

            var urlApi = string.Format(ConfigService.Get(Constants.UrlApiGetChatRoomUsers), userToken, botName, roomId);

            Trace.TraceInformation("[ChatRoomUserService.GetUsers] get url: " + urlApi);

            var users = await this.GetUsersAsync(urlApi);
            
            return users;
        }

        private async Task<List<ChatRoomUser>> GetUsersAsync(string url)
        {
            var restClient = new RestClient(url);
            var req = new RestRequest("", Method.GET);
            var response = await restClient.ExecuteTaskAsync<List<ChatRoomUser>>(req);

            Trace.TraceInformation("[ChatRoomUserService.GetUsers] response: " + response.Content);

            return response.Data;
        }
    }
}