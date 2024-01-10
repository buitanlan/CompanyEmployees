using System.Reflection;
using CompanyEmployees.IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CompanyEmployees.IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddRazorPages();

        var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

        builder.Services.AddNpgsql<UserContext>(configuration.GetConnectionString("identitySqlConnection"));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(opt =>
            {
                opt.ConfigureDbContext = c => c.UseNpgsql(configuration.GetConnectionString("sqlConnection"),
                    sql => sql.MigrationsAssembly(migrationAssembly));
            })
            .AddOperationalStore(opt =>
            {
                opt.ConfigureDbContext = c => c.UseNpgsql(configuration.GetConnectionString("sqlConnection"),
                    sql => sql.MigrationsAssembly(migrationAssembly));
            }).AddAspNetIdentity<User>();


        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
            
        app.UseIdentityServer();
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
