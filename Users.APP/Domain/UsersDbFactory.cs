using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Users.APP.Domain
{
    public class UsersDbFactory : IDesignTimeDbContextFactory<UsersDb>
    {
        private const string ConnectionString = "data source=UsersDB";

        public UsersDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDb>();
            optionsBuilder.UseSqlite(ConnectionString);
            return new UsersDb(optionsBuilder.Options);
        }
    }
}
