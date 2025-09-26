using System.Numerics;
using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.BitVecTheory;
using Spaceorc.Z3Wrap.BoolTheory;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

/// <summary>
/// Represents a Z3 model that provides variable assignments satisfying a set of constraints.
/// Supports extraction of values for Boolean, integer, real, and bitvector expressions with unlimited precision.
/// </summary>
public sealed class Z3Model
{
    private readonly Z3Context context;
    private IntPtr modelHandle;
    private bool invalidated;

    internal Z3Model(Z3Context context, IntPtr handle)
    {
        this.context = context;

        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid model handle", nameof(handle));

        modelHandle = handle;

        // Critical: increment ref count immediately to keep model alive
        SafeNativeMethods.Z3ModelIncRef(context.Handle, handle);
    }

    internal IntPtr Handle
    {
        get
        {
            ThrowIfInvalidated();
            return modelHandle;
        }
    }

    /// <summary>
    /// Evaluates an expression in this model, returning a concrete value expression.
    /// </summary>
    /// <param name="expr">The expression to evaluate.</param>
    /// <param name="modelCompletion">Whether to use model completion for undefined values (defaults to true).</param>
    /// <returns>A concrete Z3 expression representing the evaluated result.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the model has been invalidated.</exception>
    /// <exception cref="InvalidOperationException">Thrown when expression evaluation fails.</exception>
    public T Evaluate<T>(T expr, bool modelCompletion = true)
        where T : Z3Expr, IZ3ExprType<T>
    {
        ThrowIfInvalidated();

        if (
            !SafeNativeMethods.Z3ModelEval(
                context.Handle,
                modelHandle,
                expr.Handle,
                modelCompletion,
                out var result
            )
        )
            throw new InvalidOperationException("Failed to evaluate expression in model");

        return Z3Expr.Create<T>(context, result);
    }

    /// <summary>
    /// Extracts the integer value of an integer expression from this model.
    /// Supports unlimited precision using BigInteger.
    /// </summary>
    /// <param name="expr">The integer expression to evaluate.</param>
    /// <returns>The BigInteger value of the expression in this model.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the model has been invalidated.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the expression does not evaluate to an integer or parsing fails.</exception>
    public BigInteger GetIntValue(Z3IntExpr expr)
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException(
                $"Failed to parse integer value '{valueStr}' from expression {expr}"
            );

        return value;
    }

    /// <summary>
    /// Extracts the Boolean value of a Boolean expression from this model.
    /// </summary>
    /// <param name="expr">The Boolean expression to evaluate.</param>
    /// <returns>The Boolean value of the expression in this model.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the model has been invalidated.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the expression does not evaluate to a Boolean value.</exception>
    public bool GetBoolValue(Z3Bool expr)
    {
        var evaluated = Evaluate(expr);

        var boolValue = SafeNativeMethods.Z3GetBoolValue(context.Handle, evaluated.Handle);
        return (Z3BoolValue)boolValue switch
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
    /// Extracts the real number value of a real expression from this model.
    /// Returns an exact rational number with unlimited precision.
    /// </summary>
    /// <param name="expr">The real expression to evaluate.</param>
    /// <returns>The exact Real value of the expression in this model.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the model has been invalidated.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the expression does not evaluate to a real number or parsing fails.</exception>
    public Real GetRealValue(Z3RealExpr expr) => Real.Parse(GetNumericValueAsString(expr));

    /// <summary>
    /// Extracts the bitvector value of a bitvector expression from this model.
    /// </summary>
    /// <param name="expr">The bitvector expression to evaluate.</param>
    /// <returns>The BitVec value of the expression in this model.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the model has been invalidated.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the expression does not evaluate to a bitvector or parsing fails.</exception>
    public BitVec<TSize> GetBitVec<TSize>(Z3BitVec<TSize> expr)
        where TSize : ISize
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException(
                $"Failed to parse bitvector value '{valueStr}' from expression {expr}"
            );

        return new BitVec<TSize>(value);
    }

    /// <summary>
    /// Extracts the string representation of a numeric expression's value from this model.
    /// Useful for custom parsing or when exact string representation is needed.
    /// </summary>
    /// <param name="expr">The numeric expression to evaluate.</param>
    /// <returns>The string representation of the numeric value.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the model has been invalidated.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the expression does not evaluate to a numeric value.</exception>
    public string GetNumericValueAsString<T>(T expr)
        where T : Z3NumericExpr, IZ3ExprType<T>
    {
        var evaluated = Evaluate(expr);
        return ExtractNumeralString(context, evaluated, expr);
    }

    /// <summary>
    /// Returns a string representation of this model showing all variable assignments.
    /// </summary>
    /// <returns>A human-readable string representation of the model, or status information if invalidated/disposed.</returns>
    public override string ToString()
    {
        if (invalidated)
            return "<invalidated>";

        var ptr = SafeNativeMethods.Z3ModelToString(context.Handle, modelHandle);
        return Marshal.PtrToStringAnsi(ptr) ?? "<invalid>";
    }

    internal void Invalidate()
    {
        if (!invalidated && modelHandle != IntPtr.Zero)
        {
            SafeNativeMethods.Z3ModelDecRef(context.Handle, modelHandle);
            modelHandle = IntPtr.Zero;
            invalidated = true;
        }
    }

    private void ThrowIfInvalidated()
    {
        if (invalidated)
            throw new ObjectDisposedException(
                nameof(Z3Model),
                "Model has been invalidated due to solver state change"
            );
    }

    private static string ExtractNumeralString(
        Z3Context context,
        Z3Expr evaluatedExpr,
        Z3NumericExpr originalExpr
    )
    {
        if (!SafeNativeMethods.Z3IsNumeralAst(context.Handle, evaluatedExpr.Handle))
            throw new InvalidOperationException(
                $"Expression {originalExpr} does not evaluate to a numeric constant in this model"
            );

        var ptr = SafeNativeMethods.Z3GetNumeralString(context.Handle, evaluatedExpr.Handle);
        return Marshal.PtrToStringAnsi(ptr)
            ?? throw new InvalidOperationException(
                $"Failed to extract numeric value from expression {originalExpr}"
            );
    }
}
