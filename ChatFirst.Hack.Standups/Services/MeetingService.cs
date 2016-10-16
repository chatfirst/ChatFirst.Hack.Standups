using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ChatFirst.Hack.Standups.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;

namespace ChatFirst.Hack.Standups.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IConnectorClient _connectorClient = new ConnectorClient();
        private readonly IMetingAnswersRepository _metingAnswersRepository = new MetingAnswersRepository();
        private readonly IChatRoomUserService _userService = new ChatRoomUserService();
        private readonly IRoomRepository _roomRepository = new RoomRepository();

        public MeetingService()
        {
        }

        public MeetingService(IConnectorClient connectorClient, IChatRoomUserService chatRoomUserService,
            IMetingAnswersRepository metingAnswersRepository, IRoomRepository roomRepository)
        {
            _connectorClient = connectorClient;
            _userService = chatRoomUserService;
            _metingAnswersRepository = metingAnswersRepository;
            _roomRepository = roomRepository;
        }

        public async Task StartMeetingAsync(long roomId)
        {
            var meetId = await BuildMeetingAsync(roomId);
            var answer = await GetNextMeetingPushAsync(meetId);
            if (answer == null)
                return;

            var botName = await _roomRepository.GetBotName(roomId);
            var userId = $"{await _roomRepository.GetSparkRoomId(roomId)}-{answer.UserId}";

            await _connectorClient.PushRemoteChatService(botName, userId, answer.UserName);
        }

        public async Task<Answer> GetNextMeetingPushAsync(long meetId)
        {
            return await _metingAnswersRepository.GetNextMeetingPushAsync(meetId);
        }

        private async Task<long> BuildMeetingAsync(long roomId)
        {
            var room = await _metingAnswersRepository.FindRoom(roomId);
            var users = await _userService.GetUsersAsync(room.RoomId, room.BotName);
            var meet = await _metingAnswersRepository.SaveMeeting(room, users);
            return meet.Id;
        }

        public async Task DismissMeetingAsync(string sparkRoomId)
        {
            var room = await _roomRepository.GetRoomBySparkRoomID(sparkRoomId);
            if (room == null)
                throw new ApplicationException("room not found");
            var meets = await _metingAnswersRepository.GetOpenMeetingsByRoomId(room.Id);
            if (!meets.Any())
                return;
            //close all : set DateEnd
            foreach (var meet in meets)
            {
                meet.DateEnd = DateTime.Now;
                await _metingAnswersRepository.UpdateMeeting(meet.Id, meet);
            }
        }
    }
}