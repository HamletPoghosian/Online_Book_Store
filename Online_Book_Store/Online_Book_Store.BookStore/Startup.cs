using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Book_Store.BookStore.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookStoreOnline.Service;
using Online_Book_Store.BookStore.Service.Category;

namespace Online_Book_Store.BookStore
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddBook();
            services.AddCategory();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context, RoleManager<IdentityRole> rolMeneger)
        {
            context.Database.Migrate();
            AddRole(rolMeneger).Wait();
            AddBook(context).Wait();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Books/Index");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Books}/{action=Index}/{id?}");
            });
        }
        public async Task AddBook(ApplicationDbContext context)
        {
            Random rd = new Random();
            for (int i = 0; i < 15; i++)
            {
                await context.Books.AddAsync(
                    new Book
                    {
                        Id = new Guid(),
                        Author = "Author " + rd.Next(10,100),
                        Name = "Book " + rd.Next(10, 100),
                        Popular = rd.Next(1, 6),
                        Price = rd.Next(1000,50000),
                        Publish = DateTime.Now,
                        Category = new Category
                        {
                            Id = new Guid(),
                            Name = "History "+rd.Next(80,900),
                            Discription = "In our History Books section you'll find used books on local history and histories of international events, histories by epoch and histories by continent. Whether history is a passionate interest, or your field of study, our low prices will open up any field to you whether you're interested in the last decade of the last millennium. When you ship history books at you read more and spend less."

                        },

                    });
                await context.SaveChangesAsync();
  
            }
        }
        public async Task AddRole(RoleManager<IdentityRole> rolMeneger)
        {
            if (!(await rolMeneger.RoleExistsAsync("User")))
            {
                IdentityRole e = new IdentityRole("User");

                await rolMeneger.CreateAsync(e);

            }
            if (!(await rolMeneger.RoleExistsAsync("Admin")))
            {
                IdentityRole e = new IdentityRole("Admin");

                await rolMeneger.CreateAsync(e);

            }


        }
    }
}
