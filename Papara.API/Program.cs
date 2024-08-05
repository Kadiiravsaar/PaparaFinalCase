using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Papara.Core.Models;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Repository.Context;
using Papara.Repository.Repositories;
using Papara.Repository.UnitOfWorks;
using Papara.Service.Mapping;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using Papara.Service.Services.Concrete;
using Papara.Service.Utilities;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Reflection;
using System.Text;

namespace Papara.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();

			//builder.Services.AddEndpointsApiExplorer();



			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Papara API", Version = "v1.0" });
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
			

			})
					.AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = false,
							ValidIssuer = builder.Configuration["JWT:Issuer"],
							ValidateAudience = false,
							ValidAudience = builder.Configuration["JWT:Audience"],
							ValidateLifetime = false,
							ValidateIssuerSigningKey = false,
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
							ClockSkew = TimeSpan.Zero
						};
					});

		
			//{
	
			// Database context
			builder.Services.AddDbContext<MsSqlDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
				{
					option.MigrationsAssembly(Assembly.GetAssembly(typeof(MsSqlDbContext)).GetName().Name);
				}).EnableSensitiveDataLogging();
			}); 

		
			// Repositories and services

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.AddAutoMapper(typeof(MapProfile));




			builder.Services.AddScoped<AuthBusinessRules>();
			builder.Services.AddScoped<PaymentBusinessRules>();
			builder.Services.AddScoped<UserBusinessRules>();



			#region Services Repos

			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddScoped<IAuthService, AuthService>();


			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			builder.Services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));


			builder.Services.AddScoped<IPaymentService, PaymentService>();



			builder.Services.AddScoped<IBasketRepository, BasketRepository>();
			builder.Services.AddScoped<IBasketService, BasketService>();


			builder.Services.AddScoped<IStockRepository, StockRepository>();
			builder.Services.AddScoped<IStockService, StockService>();

			builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
			builder.Services.AddScoped<IBasketItemService, BasketItemService>();


			builder.Services.AddScoped<ICouponRepository, CouponRepository>();
			builder.Services.AddScoped<ICouponService, CouponService>();



			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();


			builder.Services.AddScoped<IDigitalWalletRepository, DigitalWalletRepository>();
			builder.Services.AddScoped<IDigitalWalletService, DigitalWalletService>();

			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IRoleService, RoleService>();





			builder.Services.AddScoped<IOrderRepository, OrderRepository>();
			builder.Services.AddScoped<IOrderService, OrderService>();



			builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
			builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();



			builder.Services.AddScoped<ICouponUsageRepository, CouponUsageRepository>();
			builder.Services.AddScoped<ICouponUsageService, CouponUsageService>();


			builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
			builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<IProductService, ProductService>();

			#endregion

			builder.Services.AddHttpContextAccessor();



			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(opt =>
				{
					opt.DocExpansion(DocExpansion.None);
				});
			}
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication(); // Authentication middleware
			app.UseAuthorization();  // Authorization middleware


			app.MapControllers();

			app.Run();
		}
	}
}
