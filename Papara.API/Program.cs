using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Papara.API.Middlewares;
using Papara.API.Modules;
using Papara.Core.Models;
using Papara.Repository.Context;
using Papara.Service.Mapping;
using Papara.Service.Services.Concrete;
using Papara.Service.Utilities;
using Papara.Service.Validations;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

namespace Papara.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers()
			.AddFluentValidation(fv => {
				fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>(); // ProductValidator'ýn bulunduðu assembly'den validator'larý otomatik olarak kaydeder
			});



			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Papara Final API", Version = "v1.0" });
				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter JWT Bearer token **_only_**",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};
				c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
				{ securityScheme, new string[] { } }
					});
				});




			builder.Services.AddIdentity<AppUser, AppRole>(opt =>
			{
				opt.Password.RequireDigit = false;
				opt.Password.RequireLowercase = false;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequiredLength = 1;
			}).AddEntityFrameworkStores<MsSqlDbContext>()
			.AddDefaultTokenProviders();


			builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("JWT"));


			builder.Services.AddAuthentication(opt =>
			{

				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			

			}).AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = false,
							ValidIssuer = builder.Configuration["JWT:Issuer"],
							ValidateAudience = false,
							ValidAudience = builder.Configuration["JWT:Audience"],
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
							ClockSkew = TimeSpan.Zero
						};
					});

		
	
			// Database context
			builder.Services.AddDbContext<MsSqlDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
				{
					option.MigrationsAssembly(Assembly.GetAssembly(typeof(MsSqlDbContext)).GetName().Name);
				}).EnableSensitiveDataLogging();
			});


			var redisConfiguration = builder.Configuration.GetSection("Redis");

			builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
			{
				var configurationOptions = ConfigurationOptions.Parse($"{redisConfiguration["Host"]}:{redisConfiguration["Port"]}");
				configurationOptions.AbortOnConnectFail = false;
				return ConnectionMultiplexer.Connect(configurationOptions);
			});

			// Redis cache servisini ekleyin
			builder.Services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = $"{redisConfiguration["Host"]}:{redisConfiguration["Port"]},abortConnect=false";
				options.InstanceName = redisConfiguration["InstanceName"];
			});

		

			// Hangfire yapýlandýrmasý
			builder.Services.AddHangfire(configuration => configuration
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
				{
					CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
					SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
					QueuePollInterval = TimeSpan.Zero,
					UseRecommendedIsolationLevel = true,
					UsePageLocksOnDequeue = true,
					DisableGlobalLocks = true
				}));


			builder.Host.UseServiceProviderFactory
				(new AutofacServiceProviderFactory());
			builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new AppModule()));
			

			builder.Services.AddHttpContextAccessor();


			var app = builder.Build();
			var emailJobService = app.Services.GetRequiredService<EmailJobService>();
			Task.Run(() => emailJobService.ProcessEmailQueue());

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(opt =>
				{
					opt.DocExpansion(DocExpansion.None);
				});
			}

			app.UseHangfireDashboard();
		
			RecurringJob.AddOrUpdate<CouponService>("DeactivateExpiredCoupons",
				service => service.DeactivateExpiredCoupons(), "*/30 * * * *");

			app.UseHttpsRedirection();

			app.UseCustomException();

			app.UseRouting();

			app.UseAuthentication(); // Authentication middleware
			app.UseAuthorization();  // Authorization middleware


			app.MapControllers();

			app.Run();
		}
	}
}


