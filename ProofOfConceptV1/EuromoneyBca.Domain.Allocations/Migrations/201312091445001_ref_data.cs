using EuromoneyBca.Domain.Allocations.Data;

namespace EuromoneyBca.Domain.Allocations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ref_data : DbMigration
    {
        public override void Up()
        {
            Sql(Seed.GetReferenceDataSql());
        }
        
        public override void Down()
        {
        }
    }
}
