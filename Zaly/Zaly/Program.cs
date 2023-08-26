using Microsoft.EntityFrameworkCore;
using Zaly.Models;
using Zaly.Models.Database;
namespace Zaly {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
			services.AddControllersWithViews();
            services.AddSession();

            //services.AddScoped(typeof(IRepository<User>),typeof(UserRepository));
            //services.AddScoped(typeof(IRepository<Question>),typeof(QuestionRepository));
            //services.AddScoped(typeof(IRepository<UserToQuestion>),typeof(UserToQuestionRepository));
            //services.AddScoped(typeof(IRepository<TeamRepository>),typeof(TeamRepository));
            //services.AddScoped(typeof(IRepository<MultipartAnswer>),typeof(MultipartAnswerRepository));
            
            services.AddScoped<IRepository<Admin>, AdminRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Question>, QuestionRepository>();
            services.AddScoped<IRepository<UserToQuestion>, UserToQuestionRepository>();
            services.AddScoped<IRepository<Team>, TeamRepository>();
            services.AddScoped<IRepository<MultipartAnswer>,  MultipartAnswerRepository>();
			// Add services to the container.
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Index}/{id?}");

            app.Run();
        }
    }
}