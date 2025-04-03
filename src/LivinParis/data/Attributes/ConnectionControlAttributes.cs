namespace LivinParisRoussilleTeynier.Data.Attributes;

/// <summary>
/// Indicates that the method or class requires automatic connection control.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ConnectionControlAttribute : Attribute { }
