using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackgipInventory.Config;
using StackgipInventory.Data;
using StackgipInventory.Filters;
using StackgipInventory.Helpers;
using StackgipInventory.Providers;
using StackgipInventory.Providers.Emailing;
using StackgipInventory.Repository;
using StackgipInventory.Repository.Generic;
using StackgipInventory.Services;
using StackgipInventory.Services.Identity;
using StackgipInventory.Services.InitilizerService;

namespace StackgipInventory
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
            string connStr = Configuration.GetSection(ConfigConstants.MsSqlConnection).Value;
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connStr));
            services.AddScoped<IConfig, Config.Config>();
            services.AddScoped<IIdentityConfig, IdentityConfig>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("MSSql.Connection")));

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).AddXmlSerializerFormatters();
            
            var tokensecret = Configuration.GetSection(ConfigConstants.IdentitySecret).Value;
            var key = Encoding.ASCII.GetBytes(tokensecret);

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    configureOptions.TokenValidationParameters = tokenValidationParameters;
                    configureOptions.SaveToken = true;
                });

            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(ValidateModelAttribute));
            })
            .AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddTransient<Seeder.Seed>();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<IProductInventoryRepository, ProductInventoryRepository>();
            services.AddScoped<ICustomerOrderRepository, CustomerOrderRepository>();
            services.AddScoped<IOrderLogRepository, OrderLogRepository>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HttpClient>();

            services.AddAsyncInitializer<AppInitializer>();

            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerFileOperationFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Stackgip Ecommerce",
                    Version = "v1",
                    Description = "Api for Ecommerce "

                });
               
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // Set the comments path for the Swagger JSON and UI.s
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seeder.Seed seeder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseMiddleware<ExceptionHandler>();
            }

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
            });
        }
    }
}