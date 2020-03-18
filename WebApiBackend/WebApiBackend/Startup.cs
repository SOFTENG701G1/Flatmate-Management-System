using System.IO;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiBackend.EF;
using WebApiBackend.Helpers;
using WebApiBackend.Model;

namespace WebApiBackend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string DevCorsPolicy = "_devCors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FlatManagementContext>();

            services.AddCors(options => {
                options.AddPolicy(DevCorsPolicy,
                    builder => {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            // JWT
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var settings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(settings.JWTSecret);

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x => {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddControllers();
            services.AddScoped<PaymentsRepository>();
            services.AddScoped<UserPaymentsRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<FlatRepository>();
            services.AddAutoMapper(typeof(Startup));

            //swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flatmate Management API", Version = "v1" });

                //Adds extra xml documentation into swagger
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebApiBackend.xml");
                c.IncludeXmlComments(filePath);

                //Adds ability to authorise with JWT token inside swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field, e.g Bearer <Token>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using var serviceScope = app.ApplicationServices.CreateScope();

                // Delete and recreate database
                var db = serviceScope.ServiceProvider.GetService<FlatManagementContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                // Add a test dataset for development
                var testDataGenerator = new DevelopmentDatabaseSetup(db);
                testDataGenerator.SetupDevelopmentDataSet();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flatmate Management API");
                c.RoutePrefix = string.Empty; // launch swagger from root
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseCors(DevCorsPolicy);
            }

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}