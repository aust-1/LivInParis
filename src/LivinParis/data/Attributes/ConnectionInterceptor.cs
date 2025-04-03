using System.Reflection;
using Castle.DynamicProxy;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Attributes;

/// <summary>
/// Interceptor that manages MySQL connection lifecycle for methods marked with <see cref="ConnectionControlAttribute"/>.
/// </summary>
public class ConnectionInterceptor : IInterceptor
{
    private static readonly string s_connectionString =
        "SERVER=localhost;PORT=3306;DATABASE=LivinParis;UID=eliottfrancois;PASSWORD=PSI";

    private static readonly MySqlConnection s_connection = new(s_connectionString);

    /// <summary>
    /// Intercepts method invocations and manages connection lifecycle if needed.
    /// </summary>
    /// <param name="invocation">The intercepted method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;

        bool requiresConnectionControl = RequiresConnectionControl(method);

        MySqlCommand? command = null;

        try
        {
            if (requiresConnectionControl)
            {
                command = OpenConnectionAndInjectCommand(invocation, method);
            }

            invocation.Proceed();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LOG] Exception during {method.Name}: {ex.Message}");
            throw;
        }
        finally
        {
            if (requiresConnectionControl)
            {
                CloseConnection(command, method.Name);
            }
        }
    }

    /// <summary>
    /// Determines whether the method or its declaring type has the <see cref="ConnectionControlAttribute"/>.
    /// </summary>
    private static bool RequiresConnectionControl(MethodInfo method)
    {
        return method.GetCustomAttribute<ConnectionControlAttribute>() != null
            || method.DeclaringType?.GetCustomAttribute<ConnectionControlAttribute>() != null;
    }

    /// <summary>
    /// Opens the shared connection and injects a new command object into the method arguments.
    /// </summary>
    private static MySqlCommand OpenConnectionAndInjectCommand(
        IInvocation invocation,
        MethodInfo method
    )
    {
        Console.WriteLine($"[LOG] Opening connection for {method.Name}");

        if (s_connection.State != System.Data.ConnectionState.Open)
        {
            s_connection.Open();
        }

        var command = s_connection.CreateCommand();

        InjectCommandIntoArguments(invocation, method, command);

        Console.WriteLine($"[LOG] Connection opened for {method.Name}");
        return command;
    }

    /// <summary>
    /// Injects the provided command into the correct parameter slot in the method arguments.
    /// </summary>
    private static void InjectCommandIntoArguments(
        IInvocation invocation,
        MethodInfo method,
        MySqlCommand command
    )
    {
        var parameters = method.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(MySqlCommand))
            {
                invocation.SetArgumentValue(i, command);
                break;
            }
        }
    }

    /// <summary>
    /// Closes the connection and disposes the command.
    /// </summary>
    private static void CloseConnection(MySqlCommand? command, string methodName)
    {
        Console.WriteLine($"[LOG] Closing connection for {methodName}");

        command?.Dispose();

        if (s_connection.State != System.Data.ConnectionState.Closed)
            s_connection.Close();

        Console.WriteLine($"[LOG] Connection closed for {methodName}");
    }
}
