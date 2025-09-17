using Microsoft.EntityFrameworkCore;
using Trains.Base.DB.Contexts;

namespace Trains.Base.DB.Configurations;

public static class DbConfigExtensions
{
    public static void ConfigureDbContext(this WebApplicationBuilder builder)
    {

        var writeConnectionString = builder.Configuration.GetValue<string>("DbConnections:TrainsDb") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(writeConnectionString))
        {
            throw new InvalidOperationException("Database connection string 'DbConnections:TrainsDb' is not configured.");
        }

        builder.Services.AddPooledDbContextFactory<TrainsContext>(options =>
        {
            options.UseNpgsql(writeConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            }); 

            // I want this as the default behaviour as all queries should use struct projections
            // The only time I need it is when creating new entities for saving
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            /// TODO: Consider removing these in production builds for security and performance reasons.
            options.EnableDetailedErrors(true);

            options.EnableSensitiveDataLogging(true);
        });
    }

}
