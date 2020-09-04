using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagerAPI.Data;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using System;
using UserManagerAPI.Repositories.Interfaces;
using UserManagerAPI.Services.Interfaces;
using UserManagerAPI.Repositories;
using UserManagerAPI.Services;

namespace UserManagerAPI
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services )
        {
            services.AddDbContext<UserContext>( opt => opt.UseSqlServer
             ( Configuration.GetConnectionString( "UserManagerApiConnection" ) ).EnableSensitiveDataLogging() ) ;

            services.AddControllers().AddNewtonsoftJson( s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            } );

            services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies() );

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

        }

        public void Configure( IApplicationBuilder app, IWebHostEnvironment env, UserContext context )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            DataSeed.SeedUsers( context );

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllers();
             } );
        }
    }
}
