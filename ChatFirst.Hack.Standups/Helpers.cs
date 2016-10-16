using System.Collections.Generic;
using ChatFirst.Hack.Standups.Models;

namespace ChatFirst.Hack.Standups
{
    using Models;
    using System.Linq;
    using Services;

    public class Helpers
    {
        public static ExternalMessage CreateExternalMessage()
        {
            return new ExternalMessage { Count = 0, Messages = new List<string>() };
        }

        public static ExternalMessage CreateExternalMessage(string text)
        {
            return new ExternalMessage { Count = 1, Messages = new List<string>() { text } };
        }

        public static ExternalMessage CreateMarkdownResultAnswers(IEnumerable<Answer> ans)
        {
            if (!ans.Any())
                return CreateExternalMessage();
            var count = ans.Count();
            var msgs = ans.Select(i => {
                return $"**{i.UserName}:**" + System.Environment.NewLine +
                        $"1. {ConfigService.Get(Constants.BotQuestion1)}" + System.Environment.NewLine +
                        $"- {i.Ans1}" + System.Environment.NewLine +
                        $"2. {ConfigService.Get(Constants.BotQuestion2)}" + System.Environment.NewLine +
                        $"- {i.Ans2}" + System.Environment.NewLine +
                        $"3. {ConfigService.Get(Constants.BotQuestion3)}" + System.Environment.NewLine +
                        $"- {i.Ans3}";
            }).ToList();
            return new ExternalMessage { Count = count, Messages = msgs };
        }
    }
}