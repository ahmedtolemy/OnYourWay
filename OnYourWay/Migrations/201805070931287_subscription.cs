namespace OnYourWay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscription : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Subscriptions", new[] { "USERID" });
            //CreateIndex("dbo.Subscriptions", "userID");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.Subscriptions", new[] { "userID" });
            //CreateIndex("dbo.Subscriptions", "USERID");
        }
    }
}
