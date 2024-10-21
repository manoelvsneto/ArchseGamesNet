using Archse.Application;
using Archse.Cache;
using Archse.Data;
using Archse.Extensions;
using Archse.Publisher;
using Archse.Repository;
using Archse.Service;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Archse.WebApi
{

    public static class JwtConfiguration
    {
        public static void ConfigureJwtAuthentication(this IServiceCollection services,
            Action<JwtBearerOptions> configureOptions)
        {
            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(options =>
                {
                    ConfigureDefaultJwtAuthentication(options);
                    configureOptions.Invoke(options);
                });
        }

        private static void ConfigureDefaultJwtAuthentication(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.IncludeErrorDetails = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateLifetime = true
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    MapKeycloakRolesToRoleClaims(context);
                    return Task.CompletedTask;
                }
            };
        }

        public static void ConfigureJwtAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }

        private static void MapKeycloakRolesToRoleClaims(TokenValidatedContext context)
        {
            var resourceAccess = JObject.Parse(context.Principal.FindFirst("resource_access").Value);
            var clientResource = resourceAccess[context.Principal.FindFirstValue("aud")];
            var clientRoles = clientResource["roles"];
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return;
            }

            foreach (var clientRole in clientRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, clientRole.ToString()));
            }
        }
    }

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
            //var authenticationOptions = new KeycloakAuthenticationOptions
            //{
            //    AuthServerUrl = "http://backend.archse.eng.br:31786/",
            //    Realm = "dnc-demo",
            //    Resource = "test-client",
            //};

            //services.AddKeycloakAuthentication(authenticationOptions);
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            string connection = @$"{Configuration.GetConnectionString("RedisServer")},password={Configuration.GetConnectionString("RedisServerPassword")},ssl=False,abortConnect=False";
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "My Redis Instance";
                options.Configuration = connection;
            }); ;

            services.AddSingleton<RedisConnection>();
            services.Configure<JaegerExporterOptions>(Configuration.GetSection("OpenTelemetry:Jaeger"));

            services.AddOpenTelemetry().WithTracing(builder => builder
            .AddAspNetCoreInstrumentation().AddJaegerExporter().AddMassTransitInstrumentation().AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddSqlClientInstrumentation() );

            services.AddTransient<IGamesApplication, GamesApplication>();
            services.AddTransient<IGamesService, GamesService>();
            services.AddTransient<IGamesRepository, GamesRepository>();

            string connectionString = Configuration.GetSection("ConnectionStrings:ServiceBus").Value.ToString();
            services.AddSingleton<ServiceBusClient>(new ServiceBusClient(connectionString));
            services.AddSingleton<IPublisher, Archse.Publisher.Publisher>();
            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(Archse.Mapper.MappingProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Archse.WebApi", Version = "v1" });
                /*
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
                */
            });



            services.AddControllers()
                .AddJsonOptions(
                    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddDbContext<DataContext>(ServiceLifetime.Transient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Archse.WebApi v1"));

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}