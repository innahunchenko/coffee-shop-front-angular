namespace CoffeeShop.Infrastructure
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Reflection;

    public static class AutofacExtensions
    {
        public static void ConfigureAutofac(this IHostBuilder hostBuilder, Assembly assembly)
        {
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
            {
                containerBuilder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

                containerBuilder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

                //containerBuilder.RegisterType<RedisCacheRepository>()
                //    .As<IRedisCacheRepository>()
                //    //.WithParameter(new TypedParameter(typeof(IConfiguration), configuration))
                //    .SingleInstance();

                //containerBuilder.RegisterType<RedisCacheService>()
                //    .As<IRedisCacheService>()
                //    .SingleInstance();
            });
        }
    }
}
