using HR.Application;
using HR.Application.Commands;
using HR.Application.Services;
using HR.Application.ViewModels.SharedComponentsViewModels;
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

    public static IServiceCollection AddViewModelAndExtras<T>(this IServiceCollection services,bool layout=true) where T : class,IViewModel
    {
        services.AddTransient<T>();
        services.AddSingleton<Func<T>>(s => () => s.GetRequiredService<T>());
        services.AddSingleton<IFactory<T>,FactoryBase<T>>();
        if (layout)
        {
            services.AddSingleton<INavigationService<T>, LayoutNavigationService<T>>();
        }
        else
        {
            services.AddSingleton<INavigationService<T>, NavigationService<T>>();
        }
        services.AddSingleton<INavigationCommand<T>, NavigateCommand<T>>();

        // register layout dependencies
        //services.AddTransient<LayoutComponentViewModel<T>>();
        //services.AddSingleton<Func<LayoutComponentViewModel<T>>>(s=>()=>s.GetRequiredService<LayoutComponentViewModel<T>>());
        //services.AddFactory<IFactory<LayoutComponentViewModel<T>>,FactoryBase<LayoutComponentViewModel<T>>>();
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
