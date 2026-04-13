using System.Data.Common;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DrivingSchoolApi.E2ETest;

internal class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            var overrides = new Dictionary<string, string?>
            {
                { "Authentication:Schemes:Access:SigningKeys:0:Value", "gS2VZzcj8yY7cT1qz0xPq1N2a9iXHt7xqE2ZqJ5q3n4=" },
                { "Authentication:Schemes:Refresh:SigningKeys:0:Value", "gS2VZzcj8yY7cT1qz0xPq1N2a9iXHt7xqE2ZqJ5q3n4=" }
            };

            configBuilder.AddInMemoryCollection(overrides);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDrivingSchoolDbContext>();
            services.RemoveAll<DbConnection>();

            services.AddSingleton<DbConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });

            services.AddDbContext<IDrivingSchoolDbContext, TestDbContext>((sp, options) =>
            {
                var conn = sp.GetRequiredService<DbConnection>();
                options.UseSqlite(conn);
            });
            
            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            db.Database.EnsureCreated();
        });
        builder.UseEnvironment("Testing");
    }
}