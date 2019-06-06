﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;
using WebApiDemo.Data;
using WebApiDemo.Infrastructure.Configuration;

namespace WebApiDemo
{
    public static class StartupExtensions
    {
        public static void AddJwt(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.SecretKey)),

                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = appSettings.Jwt.Audience,

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });
        }

        public static void SeedDatabase(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Log.Information("*** Seeding database with data ***");

                // seed database with data
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetService<DemoDbContext>();
                    DatabaseInitializer.AddDatabaseSeedData(dbContext);
                }
            }
        }

        public static void AddSerilogLogging(this ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var log = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            loggerFactory.AddSerilog(log);
            Log.Logger = log;

        }

    }
}