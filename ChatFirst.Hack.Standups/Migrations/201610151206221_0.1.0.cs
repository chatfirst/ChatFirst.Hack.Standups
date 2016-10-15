namespace ChatFirst.Hack.Standups.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _010 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        UserName = c.String(),
                        Ans1 = c.String(),
                        Ans2 = c.String(),
                        Ans3 = c.String(),
                        MeetingId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meetings", t => t.MeetingId)
                .Index(t => t.MeetingId);
            
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateStart = c.DateTime(),
                        DateEnd = c.DateTime(),
                        RoomId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoomId = c.String(),
                        TeamId = c.String(),
                        Cron = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meetings", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Answers", "MeetingId", "dbo.Meetings");
            DropIndex("dbo.Meetings", new[] { "RoomId" });
            DropIndex("dbo.Answers", new[] { "MeetingId" });
            DropTable("dbo.Rooms");
            DropTable("dbo.Meetings");
            DropTable("dbo.Answers");
        }
    }
}
