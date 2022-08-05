using HR.Application;
using HR.Application.Commands;
using HR.Application.Services;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace HR.UI.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        Assembly assembly = Helper.GetApplicationAss();
        Type[] viewModelTypes = assembly
            .GetTypes()
            .Where(t => t.Name.EndsWith("ViewModel", StringComparison.Ordinal))
            .ToArray();
        
        foreach(Type viewModelType in viewModelTypes)
        {
            if(!viewModelType.IsAbstract && viewModelType.IsClass)
            {
                services.AddTransient(viewModelType);
                //var fn = Type.MakeGenericSignatureType(typeof(Func<>), viewModelType);
                //var fn = typeof(Func<>).MakeGenericType(viewModelType);
                //services.AddSingleton(fn, s => () => s.GetRequiredService(viewModelType));
                var fact = Type.MakeGenericSignatureType(typeof(IFactory<>), viewModelType);
            }
        }
        return services;
    }

    public static IServiceCollection AddViewModelAndExtras<T>(this IServiceCollection services) where T : class,IViewModel
    {
        services.AddTransient<T>();
        services.AddSingleton<Func<T>>(s => () => s.GetRequiredService<T>());
        services.AddSingleton<IFactory<T>,FactoryBase<T>>();
        services.AddSingleton<INavigationService<T>, NavigationService<T>>();
        services.AddSingleton<INavigationCommand<T>, NavigateCommand<T>>();
        return services;
    }
    public static IServiceCollection AddFactory<TInterface,TImpl>(this IServiceCollection services) 
        where TImpl : class,TInterface
        where TInterface : class
    {
        //services.AddTransient<TInterface,TImpl>();
        services.AddSingleton<Func<TInterface>>(s =>()=>s.GetRequiredService<TInterface>());
        services.AddSingleton<IFactory<TInterface>, FactoryBase<TInterface>>();
        return services;
    }
}
