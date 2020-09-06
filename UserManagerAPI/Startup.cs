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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserManagerAPI.Configs;
using UserManagerAPI.Hubs;
using System.Threading.Tasks;

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

            services.AddScoped<AuthConfigsManager>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            AuthConfigsManager authConfigsManager = serviceProvider.GetService<AuthConfigsManager>();

            services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                    .AddJwtBearer( options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = authConfigsManager.GetIssuer(),

                            ValidateAudience = true,
                            ValidAudience = authConfigsManager.GetAudience(),
                            ValidateLifetime = true,

                            IssuerSigningKey = authConfigsManager.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];
                                var path = context.HttpContext.Request.Path;
                                if (!string.IsNullOrEmpty( accessToken ) &&
                                    (path.StartsWithSegments( "/chat" )))
                                {
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    } );

            services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies() );

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddSignalR();

            services.AddCors( options => options.AddPolicy( "CorsPolicy",
             builder =>
             {
                 builder.AllowAnyMethod().AllowAnyHeader()
                        .WithOrigins( "http://127.0.0.1:5500/websocket.html" );//.AllowCredentials();//AllowAnyOrigin();
             } ) );
        }

        public void Configure( IApplicationBuilder app, IWebHostEnvironment env, UserContext context )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( "CorsPolicy" );

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            DataSeed.SeedUsers( context );

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllers();
                 endpoints.MapHub<ChatHub>( "/chat");
             } );
        }
    }
}
