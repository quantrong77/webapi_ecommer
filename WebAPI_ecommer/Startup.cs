using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using WebAPI_ecommer.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI_ecommer.Models;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebAPI_ecommer
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
            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));

            var symmetricKeyValue = Configuration["Jwt:Key"];
            var secretKey = Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
           
            var issuerValue = Configuration["Jwt:Issuer"];
            var audienceValue = Configuration["Jwt:Audience"];
            //AppSetting.ConnectString = Configuration["ConnectionStrings:Default"];

            //Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    // Tu cap token
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    // Ky vao token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes), // Khoa bi mat de giai ma token
                    ClockSkew = TimeSpan.Zero
                };
                
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI_VTH_Shop_admin_ecommer", Version = "v1" });
                // Thêm biểu tượng ổ khóa vào màn hình Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name= "Bearer",
                            In= ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

            });

            services.AddDbContext<myDBContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("Default")); //found in appsettings.json

            });
         
            //services.AddTransient<MySqlConnection>(_ => new MySqlConnection(Configuration["ConnectionStrings:Default"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
 
            }

            app.UseHttpsRedirection();

            app.UseRouting();
 
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            { c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI_ecommer v1"); });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
       
            });
        }
    }
}
