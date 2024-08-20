using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Service;
using SewingFactory.Services.Mappings;
using Microsoft.OpenApi.Models;
using SewingFactory.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SewingFactory.Services;
using Microsoft.AspNetCore.Authorization;


namespace SewingFactory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure JSON options with ReferenceHandler.Preserve
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            // Add services to the container.
            // Register Services in Dependency Injection Container
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ValidationService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TaskService>();
            builder.Services.AddSingleton<ITokenService, TokenService>();

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            // Add services to the container.
            // Register Services in Dependency Injection Container
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IRoleService, RoleService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Configure Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sewing Factory API", Version = "v1" });
                
                //Enable annotations
                c.EnableAnnotations();

                // Add Bearer token support to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
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
            });

            // Configure the DbContext
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Configure AutoMapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            // Configure JWT authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.Zero
                    };

                    // Custom response for authorization failures
                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "You do not have access to this resource." });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            // Configure authorization services
            builder.Services.AddAuthorization(options =>
            {
                var policiesSection = builder.Configuration.GetSection("AuthorizationPolicies");
                var policies = policiesSection.Get<Dictionary<string, string[]>>();

                if (policies != null)
                {
                    foreach (var policy in policies)
                    {
                        // Create a policy with multiple roles
                        options.AddPolicy(policy.Key, policyBuilder =>
                        {
                            // Add role-based claims requirement
                            policyBuilder.RequireAssertion(context =>
                                context.User.Claims.Any(c =>
                                    c.Type == "roleName" && policy.Value.Contains(c.Value, StringComparer.OrdinalIgnoreCase)));

                            // Add custom requirement
                            policyBuilder.Requirements.Add(new UserStatusRequirement());
                        });
                    }
                }
                else
                {
                    throw new InvalidOperationException("Authorization policies not configured correctly in appsettings.json.");
                }
            });


            // Register the authorization handler as scoped
            builder.Services.AddScoped<IAuthorizationHandler, UserStatusHandler>();

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
                app.UseStaticFiles();
                app.UseRouting();

                app.UseCors();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();
            }

        }
    }
