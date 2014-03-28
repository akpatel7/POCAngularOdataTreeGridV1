namespace EuromoneyBca.Domain.Migrations.Support
{
    public class AlertContext : MigrationContext
    {
        public AlertContext() 
            : base("Data Source=(local)\\SQLEXPRESS;Initial Catalog=Euromoney.JobService.Alerting-dev;Integrated Security=True")
        {            
        }
    }
}