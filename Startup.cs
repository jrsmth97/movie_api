using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using movie_api.Contexts;
using movie_api.Middlewares;
using movie_api.Repositories;
using FluentValidation.AspNetCore;

namespace movie_api
{
    public class Startup
    {
        private static string connString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connString = Configuration.GetConnectionString("Default");
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseMiddleware<AuthMiddleware>();
            app.UseMiddleware<APIResponseMiddleware>();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            loggerFactory.AddFile("Logs/movie_api_log-{Date}.txt");
            app.Run(async context => await context.Response.WriteAsync("Server is running"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IStudioRepository, StudioRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IMovieTagRepository, MovieTagRepository>();
            services.AddScoped<IMovieScheduleRepository, MovieScheduleRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddDbContextPool<DBContext>(poolHanderOptions);
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromHours(1);  
            });

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });;
        }

        private static void poolHanderOptions(DbContextOptionsBuilder options) 
        {
            options.UseMySql(Startup.connString, ServerVersion.AutoDetect(Startup.connString));
        }

    }
}
