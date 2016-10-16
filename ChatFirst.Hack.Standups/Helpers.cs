using System.Collections.Generic;
using ChatFirst.Hack.Standups.Models;

namespace ChatFirst.Hack.Standups
{
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
    }
}