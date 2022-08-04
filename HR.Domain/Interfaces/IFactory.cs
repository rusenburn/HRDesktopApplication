using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Interfaces;
public interface IFactory<T>
{
    T Create();
}
