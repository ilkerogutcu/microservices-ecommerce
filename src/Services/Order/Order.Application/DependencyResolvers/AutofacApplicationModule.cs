using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using FluentValidation;
using MediatR;
using Olcsan.Boilerplate.Utilities.Interceptors;
using Order.Application.Implements;
using Order.Application.Interfaces.Services;
using Module = Autofac.Module;

namespace Order.Application.DependencyResolvers
{
    public class AutofacApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IValidator<>));
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .SingleInstance();

            builder.RegisterType<IdentityService>()
                .As<IIdentityService>()
                .SingleInstance();
            
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}