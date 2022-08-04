using HR.Domain.Interfaces;

namespace HR.Domain.Abstracts;
public class FactoryBase<T> : IFactory<T>
{
    private readonly Func<T> _factory;

    public FactoryBase(Func<T> factory)
    {
        _factory = factory;
    }
    public T Create()
    {
        return _factory();
    }
}
