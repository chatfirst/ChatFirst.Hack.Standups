namespace ChatFirst.Hack.Standups.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _011 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "BotName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "BotName");
        }
    }
}
