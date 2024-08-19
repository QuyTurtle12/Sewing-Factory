using Microsoft.EntityFrameworkCore;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;
using SewingFactory.Services;
using SewingFactory.Services.Service;

namespace SewingFactory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("Secret");

            // Configure the DbContext
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register the CategoryService
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            // Register the ProductService
            builder.Services.AddScoped<IProductService, ProductService>();

            // Configure Authorization Policies
            //builder.Services.AddAuthorization(option =>
            //{
            //    option.AddPolicy("Product-Manager-Policy", policy => policy.RequireClaim("roleName", "Product Manager"));
            //});

            // Load and configure authorization policies from appsettings.json
            var policiesSection = builder.Configuration.GetSection("AuthorizationPolicies");
            var policies = policiesSection.Get<Dictionary<string, string[]>>();

            if (policies != null)
            {
                builder.Services.AddAuthorization(options =>
                {
                    foreach (var policy in policies)
                    {
                        // Create a policy with multiple roles
                        options.AddPolicy(policy.Key, policyBuilder =>
                        policyBuilder.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "roleName" && policy.Value.Contains(c.Value))));
                    }
                });
            }
            else
            {
                throw new InvalidOperationException("Authorization policies not configured correctly in appsettings.json.");
            }

            var app = builder.Build();

            // Apply pending migrations and seed the database
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.Migrate(); // Applies pending migrations
            }

            // Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sewing Factory API V1");
                    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
