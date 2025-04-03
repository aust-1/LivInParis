using System.Reflection;
using Castle.DynamicProxy;
using MySql.Data.MySqlClient;

namespace LivinParis.Data;

[AttributeUsage(AttributeTargets.Method)]
public class ConnectionControlAttribute : Attribute { }

public class ConnectionInterceptor : IInterceptor
{
    static readonly string connection =
        "SERVER=localhost;PORT=3306;DATABASE=LivinParis;UID=eliottfrancois;PASSWORD=PSI";
    static readonly MySqlConnection s_connection = new(connection);

    public void Intercept(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        bool shouldManageConnectionControl =
            method.GetCustomAttribute<ConnectionControlAttribute>() != null;

        if (shouldManageConnectionControl)
        {
            Console.WriteLine($"[LOG] Avant d'exécuter {method.Name}");
            s_connection.Open();
        }

        invocation.Proceed();

        if (shouldManageConnectionControl)
        {
            Console.WriteLine($"[LOG] Après avoir exécuté {method.Name}");
            s_connection.Close();
        }
    }
}
