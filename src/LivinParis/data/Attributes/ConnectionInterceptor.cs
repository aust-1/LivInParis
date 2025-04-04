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
        Env.Load(Path.Combine("..", "database", ".env"));

        string connectionString =
            $"SERVER={Environment.GetEnvironmentVariable("DB_HOST")};PORT={Environment.GetEnvironmentVariable("DB_PORT")};DATABASE={Environment.GetEnvironmentVariable("DB_NAME")};UID={Environment.GetEnvironmentVariable("DB_USER")};PASSWORD={Environment.GetEnvironmentVariable("DB_PASSWORD")}";
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
    /// <param name="method">The method to check.</param>
    /// <returns>True if the method or its declaring type has the attribute; otherwise, false.</returns>
    private static bool RequiresConnectionControl(MethodInfo method)
    {
        return method.GetCustomAttribute<ConnectionControlAttribute>() != null
            || method.DeclaringType?.GetCustomAttribute<ConnectionControlAttribute>() != null;
    }

    /// <summary>
    /// Opens the shared connection and injects a new command object into the method arguments.
    /// </summary>
    /// <param name="connection">The MySQL connection to open.</param>
    /// <param name="invocation">The intercepted method invocation.</param>
    /// <param name="method">The method being invoked.</param>
    /// <returns>The created MySQL command.</returns>
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
    /// <param name="invocation">The intercepted method invocation.</param>
    /// <param name="method">The method being invoked.</param>
    /// <param name="command">The MySQL command to inject.</param>
    /// <exception cref="ArgumentException">Thrown if the command parameter is not found.</exception>
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
    /// <param name="connection">The MySQL connection to close.</param>
    /// <param name="command">The MySQL command to dispose.</param>
    /// <param name="methodName">The name of the method being invoked.</param>
    /// <exception cref="InvalidOperationException">Thrown if the connection is not in a valid state.</exception>
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
