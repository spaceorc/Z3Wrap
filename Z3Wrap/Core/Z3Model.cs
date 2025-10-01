using System.Numerics;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Represents a Z3 model containing satisfying assignments for variables in constraints.
/// </summary>
public sealed class Z3Model
{
    private readonly Z3Context context;
    private IntPtr modelHandle;
    private bool invalidated;

    internal Z3Model(Z3Context context, IntPtr handle)
    {
        this.context = context;
        modelHandle = handle;

        // Critical: increment ref count immediately to keep model alive
        context.Library.Z3ModelIncRef(context.Handle, handle);
    }

    /// <summary>
    /// Gets the native Z3 model handle.
    /// </summary>
    public IntPtr Handle
    {
        get
        {
            ThrowIfInvalidated();
            return modelHandle;
        }
    }

    /// <summary>
    /// Evaluates an expression in this model.
    /// </summary>
    /// <typeparam name="T">The expression type.</typeparam>
    /// <param name="expr">The expression to evaluate.</param>
    /// <param name="modelCompletion">Whether to use model completion for undefined values.</param>
    /// <returns>The evaluated expression.</returns>
    public T Evaluate<T>(T expr, bool modelCompletion = true)
        where T : Z3Expr, IExprType<T>
    {
        ThrowIfInvalidated();

        if (!context.Library.Z3ModelEval(context.Handle, modelHandle, expr.Handle, modelCompletion, out var result))
            throw new InvalidOperationException("Failed to evaluate expression in model");

        return Z3Expr.Create<T>(context, result);
    }

    /// <summary>
    /// Gets the integer value of an integer expression in this model.
    /// </summary>
    /// <param name="expr">The integer expression.</param>
    /// <returns>The integer value.</returns>
    public BigInteger GetIntValue(IntExpr expr)
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException($"Failed to parse integer value '{valueStr}' from expression {expr}");

        return value;
    }

    /// <summary>
    /// Gets the boolean value of a boolean expression in this model.
    /// </summary>
    /// <param name="expr">The boolean expression.</param>
    /// <returns>The boolean value.</returns>
    public bool GetBoolValue(BoolExpr expr)
    {
        var evaluated = Evaluate(expr);

        var boolValue = context.Library.Z3GetBoolValue(context.Handle, evaluated.Handle);
        return boolValue switch
        {
            Z3BoolValue.False => false,
            Z3BoolValue.True => true,
            Z3BoolValue.Undefined => throw new InvalidOperationException(
                $"Expression {expr} does not evaluate to a boolean in this model"
            ),
            _ => throw new InvalidOperationException(
                $"Unexpected boolean value result {boolValue} from Z3_get_bool_value"
            ),
        };
    }

    /// <summary>
    /// Gets the real value of a real expression in this model.
    /// </summary>
    /// <param name="expr">The real expression.</param>
    /// <returns>The real value.</returns>
    public Real GetRealValue(RealExpr expr) => Real.Parse(GetNumericValueAsString(expr));

    /// <summary>
    /// Gets the bit-vector value of a bit-vector expression in this model.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size type.</typeparam>
    /// <param name="expr">The bit-vector expression.</param>
    /// <returns>The bit-vector value.</returns>
    public Bv<TSize> GetBitVec<TSize>(BvExpr<TSize> expr)
        where TSize : ISize
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException($"Failed to parse bitvector value '{valueStr}' from expression {expr}");

        return new Bv<TSize>(value);
    }

    /// <summary>
    /// Gets the string representation of a numeric expression's value in this model.
    /// </summary>
    /// <typeparam name="T">The numeric expression type.</typeparam>
    /// <param name="expr">The numeric expression.</param>
    /// <returns>The string representation of the value.</returns>
    public string GetNumericValueAsString<T>(T expr)
        where T : Z3Expr, INumericExpr, IExprType<T>
    {
        var evaluated = Evaluate(expr);
        return ExtractNumeralString(context, evaluated, expr);
    }

    /// <summary>
    /// Returns the string representation of this model.
    /// </summary>
    /// <returns>String representation of the model.</returns>
    public override string ToString()
    {
        if (invalidated)
            return "<invalidated>";

        return context.Library.Z3ModelToString(context.Handle, modelHandle) ?? "<invalid>";
    }

    internal void Invalidate()
    {
        if (!invalidated && modelHandle != IntPtr.Zero)
        {
            context.Library.Z3ModelDecRef(context.Handle, modelHandle);
            modelHandle = IntPtr.Zero;
            invalidated = true;
        }
    }

    private void ThrowIfInvalidated()
    {
        if (invalidated)
            throw new ObjectDisposedException(nameof(Z3Model), "Model has been invalidated due to solver state change");
    }

    private static string ExtractNumeralString<TExpr>(Z3Context context, TExpr evaluatedExpr, TExpr originalExpr)
        where TExpr : Z3Expr, INumericExpr, IExprType<TExpr>
    {
        if (!context.Library.Z3IsNumeralAst(context.Handle, evaluatedExpr.Handle))
            throw new InvalidOperationException(
                $"Expression {originalExpr} does not evaluate to a numeric constant in this model"
            );

        return context.Library.Z3GetNumeralString(context.Handle, evaluatedExpr.Handle)
            ?? throw new InvalidOperationException($"Failed to extract numeric value from expression {originalExpr}");
    }
}
