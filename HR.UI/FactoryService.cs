using HR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.UI;
internal class FactoryService<T> : IFactory<T>
{
    private readonly IServiceProvider _serviceProvider;

    public FactoryService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public T Create()
    {
        return (T)_serviceProvider.GetService(typeof(T));
    }
}
