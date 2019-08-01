//#define IN_MEMORY
//#define MIGRATE_DATABASE

using System.Reflection;
using Grw.Gin.Auth.Configurations;
using Grw.Gin.Auth.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Grw.Gin.Auth.Extensions
{
    public static class IdentityServerExtension
    {
        public static void AddIdentityServerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var connectionStringAuth = configuration.GetConnectionString("Grw.Gin.Auth");
            var connectionStringData = configuration.GetConnectionString("Grw.Gin.Data");


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Grw.Gin.Data")));

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/Identity/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Identity/Account/Logout";
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(AuthConfig.GetIdentityResources(configuration))
                .AddInMemoryApiResources(AuthConfig.GetApis(configuration))
                .AddInMemoryClients(AuthConfig.GetClients(configuration))
                .AddAspNetIdentity<IdentityUser>()
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = builder =>
                //        builder.UseSqlServer(connectionStringAuth,
                //        sql => sql.MigrationsAssembly(migrationsAssembly));
                //})
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionStringAuth,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                });

            //services.AddAuthentication();
        }

        public static void UseIdentityServerConfig(this IApplicationBuilder app, IConfiguration configuration)
        {
#if DEBUG && !IN_MEMORY && MIGRATE_DATABASE
            MigrateInMemoryDataToSqlServer(app, configuration);
#endif
            app.UseIdentityServer();

            //app.UseAuthentication();

            //app.UseCookiePolicy();
        }

#if DEBUG && !IN_MEMORY && MIGRATE_DATABASE
        private static void MigrateInMemoryDataToSqlServer(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                //scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                MigrateAuthDb(scope, configuration);
                MigrateDataDb(scope);
            }
        }

        private static void MigrateAuthDb(IServiceScope scope, IConfiguration configuration)
        {
            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            context.Database.Migrate();

            if (!context.Clients.Any())
            {
                foreach (var client in AuthConfig.GetClients(configuration))
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in AuthConfig.GetIdentityResources(configuration))
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in AuthConfig.GetApis(configuration))
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }

        private static void MigrateDataDb(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            if (!context.Users.Any())
            {
                context.Users.Add(new IdentityUser
                {
                    Id = "s4ksHuV4vX",
                    UserName = "DESCONHECIDO@DESCONHECIDO.pt",
                    PasswordHash = HashHelper.Sha512("DESCONHECIDO" + "DESCONHECIDO@DESCONHECIDO.pt")
                });

                context.SaveChanges();
            }

            if (!context.UserClaims.Any())
            {
                context.UserClaims.Add(new IdentityUserClaim<string>
                {
                    UserId = "s4ksHuV4vX",
                    ClaimType = "email",
                    ClaimValue = "DESCONHECIDO@DESCONHECIDO.pt"
                });

                context.SaveChanges();
            }
        }
#endif
    }
}