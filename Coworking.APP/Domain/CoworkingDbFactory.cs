using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Coworking.APP.Domain
{
    public class CoworkingDbFactory : IDesignTimeDbContextFactory<CoworkingDb>
    {
        private const string ConnectionString = "data source=CoworkingDB";

        public CoworkingDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoworkingDb>();
            optionsBuilder.UseSqlite(ConnectionString);
            return new CoworkingDb(optionsBuilder.Options);
        }
    }
}
