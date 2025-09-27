using System.Numerics;
using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Core;

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

    public T Evaluate<T>(T expr, bool modelCompletion = true)
        where T : Z3Expr, IExprType<T>
    {
        ThrowIfInvalidated();

        if (!SafeNativeMethods.Z3ModelEval(context.Handle, modelHandle, expr.Handle, modelCompletion, out var result))
            throw new InvalidOperationException("Failed to evaluate expression in model");

        return Z3Expr.Create<T>(context, result);
    }

    public BigInteger GetIntValue(IntExpr expr)
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException($"Failed to parse integer value '{valueStr}' from expression {expr}");

        return value;
    }

    public bool GetBoolValue(BoolExpr expr)
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

    public Real GetRealValue(RealExpr expr) => Real.Parse(GetNumericValueAsString(expr));

    public Bv<TSize> GetBitVec<TSize>(BvExpr<TSize> expr)
        where TSize : ISize
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException($"Failed to parse bitvector value '{valueStr}' from expression {expr}");

        return new Bv<TSize>(value);
    }

    public string GetNumericValueAsString<T>(T expr)
        where T : Z3Expr, INumericExpr, IExprType<T>
    {
        var evaluated = Evaluate(expr);
        return ExtractNumeralString(context, evaluated, expr);
    }

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
            throw new ObjectDisposedException(nameof(Z3Model), "Model has been invalidated due to solver state change");
    }

    private static string ExtractNumeralString<TExpr>(Z3Context context, TExpr evaluatedExpr, TExpr originalExpr)
        where TExpr : Z3Expr, INumericExpr, IExprType<TExpr>
    {
        if (!SafeNativeMethods.Z3IsNumeralAst(context.Handle, evaluatedExpr.Handle))
            throw new InvalidOperationException(
                $"Expression {originalExpr} does not evaluate to a numeric constant in this model"
            );

        var ptr = SafeNativeMethods.Z3GetNumeralString(context.Handle, evaluatedExpr.Handle);
        return Marshal.PtrToStringAnsi(ptr)
            ?? throw new InvalidOperationException($"Failed to extract numeric value from expression {originalExpr}");
    }
}
