using Archse.Application;
using Archse.Cache;
using Archse.Data;
using Archse.Extensions;
using Archse.Publisher;
using Archse.Repository;
using Archse.Service;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace Archse.Worker
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
            string connection = @$"{Configuration.GetConnectionString("RedisServer")},password={Configuration.GetConnectionString("RedisServerPassword")},ssl=False,abortConnect=False";
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "My Redis Instance";
                options.Configuration = connection;
            }); ;



            services.AddSingleton<RedisConnection>();
            services.AddMassTransitConsumer(Configuration);

            services.AddTransient<IGamesApplication, GamesApplication>();
            services.AddTransient<IGamesService, GamesService>();
            services.AddTransient<IGamesRepository, GamesRepository>();
            string connectionString = Configuration.GetSection("ConnectionStrings:ServiceBus").Value.ToString();
            services.AddSingleton<ServiceBusClient>(new ServiceBusClient(connectionString));
            services.AddSingleton<IPublisher, Archse.Publisher.Publisher>();

            services.Configure<JaegerExporterOptions>(Configuration.GetSection("OpenTelemetry:Jaeger"));

            services.AddOpenTelemetry().WithTracing(builder => builder
            .AddAspNetCoreInstrumentation().AddJaegerExporter().AddMassTransitInstrumentation().AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddSqlClientInstrumentation());

            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(Archse.Mapper.MappingProfile));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Archse.Worker", Version = "v1" });
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Archse.Worker v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
