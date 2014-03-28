using System;
using System.Data.Entity.Migrations;

namespace EuromoneyBca.Web
{
    public partial class SeedData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var configuration = new EuromoneyBca.Domain.Allocations.Migrations.Configuration();
            var migrator = new DbMigrator(configuration);

            migrator.Update();

        }
    }
}