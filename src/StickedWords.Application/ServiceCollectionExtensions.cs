﻿using Microsoft.Extensions.DependencyInjection;

namespace StickedWords.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(opts => opts.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        return services;
    }
}
