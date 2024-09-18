using companyappbasic.Data.Context;
using companyappbasic.Data.Entity;
using companyappbasic.Services.AdminServices;
using companyappbasic.Services.AppUserServices;
using companyappbasic.Services.BackgroundServices;
using companyappbasic.Services.EmailServices;
using companyappbasic.Services.EmployeeServices;
using companyappbasic.Services.RabbitMQServices;
using companyappbasic.Services.TaskServices;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Mail;
using static companyappbasic.Services.BackgroundServices.BackgroundServi;

namespace companyappbasic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); 



            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Lütfen 'Bearer' ardindan bosluk birakarak token girin",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey, //APÝKEY HTTP?
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Scoped);


            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHangfireServer();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
            })
                .AddEntityFrameworkStores<ApplicationDBContext>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
            });
            var key = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, 
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true, 
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            builder.Services.AddScoped<IEmployee, EmployeeServi>();
            builder.Services.AddScoped<ITask, TaskServi>();
            builder.Services.AddScoped<IAdmin, Adminservi>();
            builder.Services.AddScoped<IToken, TokenServi>();
            builder.Services.AddTransient<BackgroundServi>();
            builder.Services.AddSingleton<IEmail, EmailServi>();
            builder.Services.AddSingleton<RabbitConsumer>();
            builder.Services.AddSingleton<RabbitProducer>();

            var app = builder.Build();
           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty; // Swagger UI'yi kök URL'ye ayarlamak için
                });
            }

            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<BackgroundServi>(
             "kontrol ve güncelleme kayýt",
             jobService => jobService.CheckAndUpdateRecords(),
             "*/1 * * * *"); // Bu iþ her dakika çalýþacak


            //var rabbitConsumer = app.Services.GetRequiredService<RabbitConsumer>();

            //RecurringJob.AddOrUpdate(
            //    "rabbitmq-consumer-job", // Job'un ID'si
            //    () => rabbitConsumer.StartConsuming(), // Çalýþtýrýlacak metot
            //    "*/15 * * * * *");

        

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            var emailConsumer = app.Services.GetRequiredService<RabbitConsumer>();
            emailConsumer.StartConsuming();

            app.Run();
        }
    }
}
