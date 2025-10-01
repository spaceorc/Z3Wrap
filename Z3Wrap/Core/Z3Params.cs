using System.Collections;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a set of parameters that can be applied to solvers and other Z3 objects.
/// </summary>
public sealed class Z3Params : IEnumerable<KeyValuePair<string, object>>
{
    private readonly Dictionary<string, object> parameters = new();

    /// <summary>
    /// Sets a boolean parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The boolean value.</param>
    /// <returns>This parameters object for fluent chaining.</returns>
    public Z3Params Set(string name, bool value)
    {
        parameters[name] = value;
        return this;
    }

    /// <summary>
    /// Sets an unsigned integer parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The unsigned integer value.</param>
    /// <returns>This parameters object for fluent chaining.</returns>
    public Z3Params Set(string name, uint value)
    {
        parameters[name] = value;
        return this;
    }

    /// <summary>
    /// Sets a double parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The double value.</param>
    /// <returns>This parameters object for fluent chaining.</returns>
    public Z3Params Set(string name, double value)
    {
        parameters[name] = value;
        return this;
    }

    /// <summary>
    /// Sets a string parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The string value.</param>
    /// <returns>This parameters object for fluent chaining.</returns>
    public Z3Params Set(string name, string value)
    {
        parameters[name] = value;
        return this;
    }

    /// <summary>
    /// Adds a boolean parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The boolean value.</param>
    public void Add(string name, bool value)
    {
        parameters.Add(name, value);
    }

    /// <summary>
    /// Adds an unsigned integer parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The unsigned integer value.</param>
    public void Add(string name, uint value)
    {
        parameters.Add(name, value);
    }

    /// <summary>
    /// Adds a double parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The double value.</param>
    public void Add(string name, double value)
    {
        parameters.Add(name, value);
    }

    /// <summary>
    /// Adds a string parameter.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The string value.</param>
    public void Add(string name, string value)
    {
        parameters.Add(name, value);
    }

    /// <summary>
    /// Gets an enumerator for the parameters.
    /// </summary>
    /// <returns>An enumerator for KeyValuePair&lt;string, object&gt;.</returns>
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return parameters.GetEnumerator();
    }

    /// <summary>
    /// Gets an enumerator for the parameters.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Converts the parameters to a string representation.
    /// </summary>
    /// <returns>A string representation of the parameters.</returns>
    public override string ToString()
    {
        return string.Join(", ", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    }

    internal void ApplyTo(Z3Context context, IntPtr solverHandle)
    {
        var paramsHandle = context.Library.Z3MkParams(context.Handle);
        context.Library.Z3ParamsIncRef(context.Handle, paramsHandle);
        try
        {
            foreach (var param in parameters)
            {
                switch (param.Value)
                {
                    case bool boolValue:
                        context.Library.Z3ParamsSetBool(context.Handle, paramsHandle, param.Key, boolValue);
                        break;
                    case uint uintValue:
                        context.Library.Z3ParamsSetUInt(context.Handle, paramsHandle, param.Key, uintValue);
                        break;
                    case double doubleValue:
                        context.Library.Z3ParamsSetDouble(context.Handle, paramsHandle, param.Key, doubleValue);
                        break;
                    case string stringValue:
                        context.Library.Z3ParamsSetSymbol(context.Handle, paramsHandle, param.Key, stringValue);
                        break;
                }
            }

            context.Library.Z3SolverSetParams(context.Handle, solverHandle, paramsHandle);
        }
        finally
        {
            context.Library.Z3ParamsDecRef(context.Handle, paramsHandle);
        }
    }
}
