namespace ChatFirst.Hack.Standups.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ChatFirst.Hack.Standups.Models.HackDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ChatFirst.Hack.Standups.Models.HackDbContext context)
        {
        }
    }
}
