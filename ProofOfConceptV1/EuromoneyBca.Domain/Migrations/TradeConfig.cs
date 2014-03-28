using Euromoney.Isis.Data.Migration.Migrations;
using EuromoneyBca.Domain.Migrations;
using EuromoneyBca.Domain.Migrations.Support;

// No namespace because then we can call
// add-migration migration-name -configurationtypename TradeConfig
// Also update-database -configurationtypename TradeConfig

public class TradeConfig : MigrationConfiguration<TradeContext>
{
    public TradeConfig()
        : base(typeof(Schema).Namespace)
    {
        
    }
}