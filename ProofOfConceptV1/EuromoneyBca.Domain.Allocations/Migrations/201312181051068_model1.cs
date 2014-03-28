namespace EuromoneyBca.Domain.Allocations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.History",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentAllocation = c.Single(nullable: false),
                        PreviousAllocation = c.Single(nullable: false),
                        Date = c.DateTime(),
                        Comment = c.String(),
                        Allocation_Id = c.Int(),
                        Portfolio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Allocation", t => t.Allocation_Id)
                .ForeignKey("dbo.Portfolio", t => t.Portfolio_Id)
                .Index(t => t.Allocation_Id)
                .Index(t => t.Portfolio_Id);
            
            DropColumn("dbo.Tradable_Thing", "tradable_thing_class_id23");
            DropColumn("dbo.Tradable_Thing", "location_id2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tradable_Thing", "location_id2", c => c.Int());
            AddColumn("dbo.Tradable_Thing", "tradable_thing_class_id23", c => c.Int());
            DropForeignKey("dbo.History", "Portfolio_Id", "dbo.Portfolio");
            DropForeignKey("dbo.History", "Allocation_Id", "dbo.Allocation");
            DropIndex("dbo.History", new[] { "Portfolio_Id" });
            DropIndex("dbo.History", new[] { "Allocation_Id" });
            DropTable("dbo.History");
        }
    }
}
