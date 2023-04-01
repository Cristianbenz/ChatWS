using DB;
using Microsoft.EntityFrameworkCore;

namespace ChatWS.Helpers
{
    public static class MigrationHelper
    {
        public static async Task MigrateDataAsync(IServiceProvider svcProvider)
        {
            //Service: An instance of db context
            var dbContextSvc = svcProvider.GetRequiredService<AppDbContext>();

            //Migration: This is the programmatic equivalent to Update-Database
            await dbContextSvc.Database.MigrateAsync();
        }
    }
}