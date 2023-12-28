using Autofac;
using FluentValidation;
using MediatR;
using TaskForMoodivationStack.WebApi.Behaviours;
using Module = Autofac.Module;

namespace TaskForMoodivationStack.WebApi.Infrastructure.DependencyInjection;

public class ServiceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // MediatR'ın kaydı
        builder.RegisterAssemblyTypes(typeof(ServiceModule).Assembly)
            .AsImplementedInterfaces();

        // Validator'ların kaydı
        builder.RegisterAssemblyTypes(typeof(ServiceModule).Assembly)
            .AsClosedTypesOf(typeof(IValidator<>));

        // Pipeline Behaviors kaydı
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>))
               .As(typeof(IPipelineBehavior<,>))
               .InstancePerDependency();

        //builder.RegisterType<Validator>().As<IValidationHandler>().InstancePerLifetimeScope();
        //builder.RegisterType<CustomerRegisterBusinessValidation>().As<IBusinessValidation<ApplicationDbContext>>()
        //   .InstancePerLifetimeScope();
        //builder.RegisterType<CustomerService>().As<ICustomerService<Command, CustomerResponse>>()
        //   .InstancePerLifetimeScope();
        //builder.RegisterType<CustomerRegisterRequestValidation>().As<IRequestValidation>()
        //   .InstancePerLifetimeScope();
        //builder.RegisterType<CustomerRegisterBusinessValidation>().As<IBusinessValidation<ApplicationDbContext>>()
        //   .InstancePerLifetimeScope();

    }
}
