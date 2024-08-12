using Autofac;
using AutoMapper;
using Hangfire;
using Papara.Core.Repositories;
using Papara.Core.UnitOfWorks;
using Papara.Repository.Repositories;
using Papara.Repository.UnitOfWorks;
using Papara.Service.Mapping;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;
using Papara.Service.Services.Concrete;
using Papara.Service.Utilities;
using Module = Autofac.Module;
namespace Papara.API.Modules
{
	public class AppModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// Services
			builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
			builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();

			// Genel Repository ve Service
			builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();

			// İş kuralları
			builder.RegisterType<AuthBusinessRules>().InstancePerLifetimeScope();
			builder.RegisterType<BasketBusinessRules>().InstancePerLifetimeScope();
			builder.RegisterType<PaymentBusinessRules>().InstancePerLifetimeScope();
			builder.RegisterType<UserBusinessRules>().InstancePerLifetimeScope();
			builder.RegisterType<RoleBusinessRules>().InstancePerLifetimeScope();

			// UnitOfWork
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

			// Email Servisi
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();


			// Ödeme Servisi
			builder.RegisterType<PaymentService>().As<IPaymentService>().InstancePerLifetimeScope();

			// Sepet ve Sepet Öğeleri
			builder.RegisterType<BasketRepository>().As<IBasketRepository>().InstancePerLifetimeScope();
			builder.RegisterType<BasketService>().As<IBasketService>().InstancePerLifetimeScope();

			builder.RegisterType<BasketItemRepository>().As<IBasketItemRepository>().InstancePerLifetimeScope();
			builder.RegisterType<BasketItemService>().As<IBasketItemService>().InstancePerLifetimeScope();

			// Kupon Servisi
			builder.RegisterType<CouponRepository>().As<ICouponRepository>().InstancePerLifetimeScope();
			builder.RegisterType<CouponService>().As<ICouponService>().InstancePerLifetimeScope();

			// Kategori Servisi
			builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
			builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();

			// Dijital Cüzdan Servisi
			builder.RegisterType<DigitalWalletRepository>().As<IDigitalWalletRepository>().InstancePerLifetimeScope();
			builder.RegisterType<DigitalWalletService>().As<IDigitalWalletService>().InstancePerLifetimeScope();

			// Kullanıcı ve Rol Servisleri
			builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
			builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();

			// Sipariş Servisleri
			builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
			builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();

			builder.RegisterType<OrderDetailRepository>().As<IOrderDetailRepository>().InstancePerLifetimeScope();
			builder.RegisterType<OrderDetailService>().As<IOrderDetailService>().InstancePerLifetimeScope();

			// Kupon Kullanım Servisi
			builder.RegisterType<CouponUsageRepository>().As<ICouponUsageRepository>().InstancePerLifetimeScope();
			builder.RegisterType<CouponUsageService>().As<ICouponUsageService>().InstancePerLifetimeScope();

			// Ürün Kategori Servisi
			builder.RegisterType<ProductCategoryRepository>().As<IProductCategoryRepository>().InstancePerLifetimeScope();
			builder.RegisterType<ProductCategoryService>().As<IProductCategoryService>().InstancePerLifetimeScope();

			// Ürün Servisi
			builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
			builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();



			builder.RegisterType<RabbitMQPublisher>().AsSelf().SingleInstance();

			// Email Job Service
			builder.RegisterType<EmailJobService>().AsSelf().SingleInstance();

			// Hangfire Server
			builder.Register(context => new BackgroundJobServer()).AsSelf().SingleInstance();

			// AutoMapper
			builder.Register(context => new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MapProfile());
			}).CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
		}
	}
}
