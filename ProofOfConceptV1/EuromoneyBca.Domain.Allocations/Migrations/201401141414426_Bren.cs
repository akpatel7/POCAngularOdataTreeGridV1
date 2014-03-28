namespace EuromoneyBca.Domain.Allocations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bren : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.History", "CommentForBren", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.History", "CommentForBren");
        }
    }
}
