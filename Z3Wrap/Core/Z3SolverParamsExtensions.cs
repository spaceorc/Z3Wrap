namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Extension methods for setting solver parameters.
/// </summary>
public static class Z3SolverParamsExtensions
{
    /// <summary>
    /// Sets a boolean parameter on this solver.
    /// </summary>
    /// <param name="solver">The solver instance.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The boolean value.</param>
    public static void SetParam(this Z3Solver solver, string name, bool value)
    {
        var parameters = new Z3Params().Set(name, value);
        solver.SetParams(parameters);
    }

    /// <summary>
    /// Sets an unsigned integer parameter on this solver.
    /// </summary>
    /// <param name="solver">The solver instance.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The unsigned integer value.</param>
    public static void SetParam(this Z3Solver solver, string name, uint value)
    {
        var parameters = new Z3Params().Set(name, value);
        solver.SetParams(parameters);
    }

    /// <summary>
    /// Sets a double parameter on this solver.
    /// </summary>
    /// <param name="solver">The solver instance.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The double value.</param>
    public static void SetParam(this Z3Solver solver, string name, double value)
    {
        var parameters = new Z3Params().Set(name, value);
        solver.SetParams(parameters);
    }

    /// <summary>
    /// Sets a string parameter on this solver.
    /// </summary>
    /// <param name="solver">The solver instance.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The string value.</param>
    public static void SetParam(this Z3Solver solver, string name, string value)
    {
        var parameters = new Z3Params().Set(name, value);
        solver.SetParams(parameters);
    }

    /// <summary>
    /// Sets the timeout for this solver.
    /// </summary>
    /// <param name="solver">The solver instance.</param>
    /// <param name="timeout">The timeout duration.</param>
    public static void SetTimeout(this Z3Solver solver, TimeSpan timeout)
    {
        var timeoutMs = (uint)timeout.TotalMilliseconds;
        solver.SetParam("timeout", timeoutMs);
    }
}
