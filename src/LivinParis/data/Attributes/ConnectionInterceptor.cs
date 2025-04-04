using System.Reflection;
using Castle.DynamicProxy;
using DotNetEnv;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Attributes;

/// <summary>
/// Interceptor that manages MySQL connection lifecycle for methods marked with <see cref="ConnectionControlAttribute"/>.
/// </summary>
public class ConnectionInterceptor : IInterceptor
{
    /// <summary>
    /// Intercepts method invocations and manages connection lifecycle if needed.
    /// </summary>
    /// <param name="invocation">The intercepted method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
        Env.Load(Path.Combine("src", "database", ".env"));

        string connectionString =
            $"SERVER=localhost;DATABASE={Environment.GetEnvironmentVariable("MYSQL_DATABASE")};UID={Environment.GetEnvironmentVariable("MYSQL_USER")};PASSWORD={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")}";
        MySqlConnection connection = new(connectionString);

        var method = invocation.MethodInvocationTarget ?? invocation.Method;

        bool requiresConnectionControl = RequiresConnectionControl(method);

        MySqlCommand? command = null;

        try
        {
            if (requiresConnectionControl)
            {
                command = OpenConnectionAndInjectCommand(connection, invocation, method);
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
                CloseConnection(connection, command, method.Name);
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
        MySqlConnection connection,
        IInvocation invocation,
        MethodInfo method
    )
    {
        Console.WriteLine($"[LOG] Opening connection for {method.Name}");

        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        var command = connection.CreateCommand();

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
    private static void CloseConnection(
        MySqlConnection connection,
        MySqlCommand? command,
        string methodName
    )
    {
        Console.WriteLine($"[LOG] Closing connection for {methodName}");

        command?.Dispose();

        if (connection.State != System.Data.ConnectionState.Closed)
            connection.Close();

        Console.WriteLine($"[LOG] Connection closed for {methodName}");
    }
}
