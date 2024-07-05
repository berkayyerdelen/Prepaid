using Prepaid.Application.Contracts;
using Prepaid.Application.Services;
using Prepaid.Domain.Policies;
using Prepaid.Domain.Policies.Contracts;
using Prepaid.Domain.Repositories;
using Prepaid.Infrastructure.Configurations;
using Prepaid.Infrastructure.Persistence;
using Prepaid.Infrastructure.Repositories;
using Prepaid.SharedKernel.Configurations;

namespace Prepaid.Api.Extensions;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBookingRepository, BookingRepository>();

        return serviceCollection;
    }

    private static IServiceCollection AddDomainPolicies(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var bookingRefundPolicyConfiguration = configuration.GetSection(nameof(BookingRefundPolicyConfiguration)).Get<BookingRefundPolicyConfiguration>();
        serviceCollection.AddSingleton(bookingRefundPolicyConfiguration);
        
        serviceCollection.AddScoped<IBookingRefundPolicy, BookingRefundPolicy>();

        return serviceCollection;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var mongoDbConfiguration = configuration.GetSection(nameof(MongoDbConfiguration)).Get<MongoDbConfiguration>();
        serviceCollection.AddSingleton(mongoDbConfiguration);
        serviceCollection.AddScoped<ApplicationContext>();

        return serviceCollection;
    }

    private static IServiceCollection AddInternalServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IMockPricingService, MockPricingService>();
        serviceCollection.AddScoped<IMockPaymentService, MockPaymentService>();
        serviceCollection.AddScoped<IBookingService, BookingService>();

        return serviceCollection;
    }


    public static IServiceCollection AddDependencies(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return AddRepositories(serviceCollection)
            .AddDomainPolicies(configuration)
            .AddPersistence(configuration)
            .AddInternalServices();
    }
}