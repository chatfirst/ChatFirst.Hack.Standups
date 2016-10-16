using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatFirst.Hack.Standups.Models;
using ChatFirst.Hack.Standups.Services;
using FizzWare.NBuilder;
using NSubstitute;
using NUnit.Framework;

namespace ChatFirst.Hack.Standups.Tests
{
    [TestFixture]
    public class AcceptanceTests
    {
        private class RepositoryMock : IMetingAnswersRepository
        {
            public Task<Answer> GetNextMeetingPushAsync(long meetId)
            {
                return  Task.FromResult(Builder<Answer>.CreateNew().With(i => i.MeetingId = meetId).Build());
            }

            public Task<Room> FindRoom(long roomId)
            {
                return Task.FromResult(Builder<Room>.CreateNew().With(i => i.Id = roomId).Build());
            }

            public Task<Meeting> SaveMeeting(Room room, IEnumerable<ChatRoomUser> users)
            {
                Console.WriteLine($"saved {room.Id}");
                return Task.FromResult(Builder<Meeting>.CreateNew().With(i => i.RoomId = room.Id).Build());
            }
        }

        [Test]
        public async Task MeetingService_OneUser_ShouldHaveFirstPush()
        {
            // ARRANGE
            string sparkRoomId = "roomId";
            string botName = "bot";

            var connetorService = Substitute.For<IConnectorClient>();

            var ms = CreateMeetingService(sparkRoomId, botName, connetorService);
            // ACT
            await ms.StartMeetingAsync(1);


            // ASSERT
            await connetorService.Received().PushRemoteChatService(Arg.Any<string>(), 
                Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task MeetingService_OneUser_HasProperUserId()
        {
            // ARRANGE
            string sparkRoomId = "roomId";
            string botName = "bot";

            var connetorService = Substitute.For<IConnectorClient>();

            var ms = CreateMeetingService(sparkRoomId, botName, connetorService);
            // ACT
            await ms.StartMeetingAsync(1);


            // ASSERT
            await connetorService.Received().PushRemoteChatService(Arg.Any<string>(),
                $"{sparkRoomId}-userId", Arg.Any<string>());
        }

        private static IMeetingService CreateMeetingService(string sparkRoomId, string botName, IConnectorClient connetorService)
        {
            var chatRoomUserService = Substitute.For<IChatRoomUserService>();
            chatRoomUserService.GetUsersAsync(sparkRoomId, botName)
                .Returns(new List<ChatRoomUser>() {new ChatRoomUser() { userId = "userId", userName = "userName"} });

            var roomRepo = Substitute.For<IRoomRepository>();
            roomRepo.GetBotName(1).Returns(botName);
            roomRepo.GetSparkRoomId(1).Returns(sparkRoomId);

            IMeetingService ms = new MeetingService(connetorService, chatRoomUserService, new RepositoryMock(), roomRepo);
            return ms;
        }
    }
}
