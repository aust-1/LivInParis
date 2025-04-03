using System.Reflection;
using Castle.DynamicProxy;
using MySql.Data.MySqlClient;

namespace LivinParis.Data;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
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
            method.GetCustomAttribute<ConnectionControlAttribute>() != null
            || method.DeclaringType?.GetCustomAttribute<ConnectionControlAttribute>() != null;

        MySqlCommand? command = null;

        if (shouldManageConnectionControl)
        {
            Console.WriteLine($"[LOG] Ouverture de la connexion pour {method.Name}");
            s_connection.Open();
            command = s_connection.CreateCommand();
            Console.WriteLine($"[LOG] Connexion ouverte pour {method.Name}");

            var parameters = invocation.Arguments;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (method.GetParameters()[i].ParameterType == typeof(MySqlCommand))
                {
                    invocation.SetArgumentValue(i, command);
                    break;
                }
            }
        }

        try
        {
            invocation.Proceed();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LOG] Erreur pendant {method.Name} : {ex.Message}");
            throw;
        }
        finally
        {
            if (shouldManageConnectionControl)
            {
                Console.WriteLine($"[LOG] Fermeture de la connexion pour {method.Name}");
                command?.Dispose();
                s_connection.Close();
                Console.WriteLine($"[LOG] Connexion fermée pour {method.Name}");
            }
        }
    }
}
