using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GraphQLDemo.Data;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using GraphQLDemo.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights.Extensibility;
using FluentValidation.AspNetCore;
using GraphQLDemo.Infrastructure.Configuration;
using GraphQLDemo.Data.Repositories;
using GraphQLDemo.Infrastructure.Errors;
using GraphiQl;
using GraphQLDemo.Data.GraphQL;
using GraphQL.Server;

namespace WebApiDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("App").Get<AppSettings>();
            services.Configure<AppSettings>(Configuration.GetSection("App"));

            services.AddJwt(appSettings);

            services.AddCors();

            services.AddDbContext<DemoDbContext>(opt => opt.UseInMemoryDatabase("demoDb"));

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidatorActionFilter));
            })
            .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); }); ;

            services.AddAutoMapper();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WebApiDemo API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            //register types for DI
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddSingleton<AppConfig, AppConfig>();

            //GraphQL types
            services.AddScoped<GraphQL.IDependencyResolver>(s => new GraphQL.FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<GraphQL.Types.ISchema, QuerySchema>();
            services.AddGraphQL(x =>
            {
                x.ExposeExceptions = true; 
            })
            .AddGraphTypes(ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilogLogging(Configuration);

            app.UseMiddleware<ErrorHandlingMiddleware>();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseAuthentication();

            app.UseCors(policyBuilder =>
                policyBuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );

            app.UseGraphiQl("/graphql");

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiDemo V1");
            });

            app.SeedDatabase(env);

#if DEBUG
            TelemetryConfiguration.Active.DisableTelemetry = true;
#endif

        }
    }
}
