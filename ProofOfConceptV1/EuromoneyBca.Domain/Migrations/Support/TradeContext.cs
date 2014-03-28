namespace EuromoneyBca.Domain.Migrations.Support
{
    public class TradeContext : MigrationContext
    {
        public TradeContext()
            : base("Data Source=XPS12;Initial Catalog=BCATrade;Integrated Security=True")
        {
        }
    }
}