using System.Numerics;
using System.Runtime.InteropServices;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public sealed class Z3Model
{
    private readonly Z3Context context;
    private IntPtr modelHandle;
    private bool invalidated;

    // Constructor & Properties
    internal Z3Model(Z3Context context, IntPtr handle)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));

        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid model handle", nameof(handle));

        modelHandle = handle;

        // Critical: increment ref count immediately to keep model alive
        NativeMethods.Z3ModelIncRef(context.Handle, handle);
    }

    public IntPtr Handle
    {
        get
        {
            ThrowIfInvalidated();
            return modelHandle;
        }
    }

    // Evaluation Methods
    public Z3Expr Evaluate(Z3Expr expr, bool modelCompletion = true)
    {
        ThrowIfInvalidated();

        if (!NativeMethods.Z3ModelEval(context.Handle, modelHandle, expr.Handle, modelCompletion, out var result))
            throw new InvalidOperationException("Failed to evaluate expression in model");

        return Z3Expr.Create(context, result);
    }

    // Value Extraction Methods - Integer
    public BigInteger GetIntValue(Z3IntExpr expr)
    {
        var evaluated = Evaluate(expr);

        if (!NativeMethods.Z3IsNumeralAst(context.Handle, evaluated.Handle))
            throw new InvalidOperationException($"Expression {expr} does not evaluate to a numeric constant in this model");

        var ptr = NativeMethods.Z3GetNumeralString(context.Handle, evaluated.Handle);
        var valueStr = Marshal.PtrToStringAnsi(ptr) ?? throw new InvalidOperationException($"Failed to extract integer value from expression {expr}");

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException($"Failed to parse integer value '{valueStr}' from expression {expr}");

        return value;
    }

    // Value Extraction Methods - Boolean
    public bool GetBoolValue(Z3BoolExpr expr)
    {
        var evaluated = Evaluate(expr);

        var boolValue = NativeMethods.Z3GetBoolValue(context.Handle, evaluated.Handle);
        return (Z3BoolValue)boolValue switch
        {
            Z3BoolValue.False => false,
            Z3BoolValue.True => true,
            Z3BoolValue.Undefined => throw new InvalidOperationException($"Expression {expr} does not evaluate to a boolean in this model"),
            _ => throw new InvalidOperationException($"Unexpected boolean value result {boolValue} from Z3_get_bool_value"),
        };
    }

    // Value Extraction Methods - Real
    public Real GetRealValue(Z3RealExpr expr) => Real.Parse(GetRealValueAsString(expr));

    public string GetRealValueAsString(Z3RealExpr expr)
    {
        var evaluated = Evaluate(expr);

        if (!NativeMethods.Z3IsNumeralAst(context.Handle, evaluated.Handle))
            throw new InvalidOperationException($"Expression {expr} does not evaluate to a numeric constant in this model");

        var ptr = NativeMethods.Z3GetNumeralString(context.Handle, evaluated.Handle);
        return Marshal.PtrToStringAnsi(ptr) ?? throw new InvalidOperationException($"Failed to extract real value from expression {expr}");
    }

    // Value Extraction Methods - BitVector
    public string GetBitVecValueAsString(Z3BitVecExpr expr)
    {
        var evaluated = Evaluate(expr);

        if (!NativeMethods.Z3IsNumeralAst(context.Handle, evaluated.Handle))
            throw new InvalidOperationException($"Expression {expr} does not evaluate to a numeric constant in this model");

        var ptr = NativeMethods.Z3GetNumeralString(context.Handle, evaluated.Handle);
        return Marshal.PtrToStringAnsi(ptr) ?? throw new InvalidOperationException($"Failed to extract bitvector value from expression {expr}");
    }

    // Object Methods
    public override string ToString()
    {
        if (invalidated)
            return "<invalidated>";

        try
        {
            var ptr = NativeMethods.Z3ModelToString(context.Handle, modelHandle);
            return Marshal.PtrToStringAnsi(ptr) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
        catch
        {
            return "<error>";
        }
    }

    // Internal Methods
    internal void Invalidate()
    {
        if (!invalidated && modelHandle != IntPtr.Zero)
        {
            try
            {
                NativeMethods.Z3ModelDecRef(context.Handle, modelHandle);
            }
            catch
            {
                // Context might be disposed, ignore cleanup errors
                // The native Z3 context cleanup will handle the model cleanup
            }

            modelHandle = IntPtr.Zero;
            invalidated = true;
        }
    }

    // Private Methods
    private void ThrowIfInvalidated()
    {
        if (invalidated)
            throw new ObjectDisposedException(nameof(Z3Model), "Model has been invalidated due to solver state change");
    }
}