using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.IO;
using System.Linq;

namespace EuromoneyBca.Domain.Migrations
{
    public abstract class ResourceMigration : DbMigration
    {
        private readonly string _resourceName;
        
        protected ResourceMigration()
        {
            var type = GetType();

            _resourceName = string.Format("{0}.Scripts.{1}.sql", type.Namespace, ((IMigrationMetadata)this).Id);
        }


        public override void Up()
        {
            RunResource(_resourceName);
        }

        public override void Down()
        {
            throw new NotSupportedException();
        }

        protected void RunResource(string resourceName)
        {
            var assembly = GetType().Assembly;

            var resources = assembly.GetManifestResourceNames();
            var selectedResource = resources.Single(r => r.Contains(resourceName));
            using (var stream = assembly.GetManifestResourceStream(selectedResource))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        Sql(reader.ReadToEnd());
                    }
                }
            }

            
        }

    }
}
