using System.Reflection;

namespace HR.Application;
public class Helper
{
    public static Assembly GetApplicationAss()
    {
        return Assembly.GetExecutingAssembly();
    }
}