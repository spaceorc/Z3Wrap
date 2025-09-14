using System.Numerics;
using System.Runtime.InteropServices;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public sealed class Z3Model
{
    // Fields
    private readonly Z3Context context;
    private IntPtr modelHandle;
    private bool invalidated;

    // Constructor
    internal Z3Model(Z3Context context, IntPtr handle)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));

        if (handle == IntPtr.Zero)
            throw new ArgumentException("Invalid model handle", nameof(handle));

        modelHandle = handle;

        // Critical: increment ref count immediately to keep model alive
        NativeMethods.Z3ModelIncRef(context.Handle, handle);
    }

    // Properties
    public IntPtr Handle
    {
        get
        {
            ThrowIfInvalidated();
            return modelHandle;
        }
    }

    // Public Methods - Expression Evaluation
    public Z3Expr Evaluate(Z3Expr expr, bool modelCompletion = true)
    {
        ThrowIfInvalidated();

        if (!NativeMethods.Z3ModelEval(context.Handle, modelHandle, expr.Handle, modelCompletion, out var result))
            throw new InvalidOperationException("Failed to evaluate expression in model");

        return Z3Expr.Create(context, result);
    }

    // Public Methods - Value Extraction
    public BigInteger GetIntValue(Z3IntExpr expr)
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException($"Failed to parse integer value '{valueStr}' from expression {expr}");

        return value;
    }

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

    public Real GetRealValue(Z3RealExpr expr) => Real.Parse(GetNumericValueAsString(expr));

    public BitVec GetBitVec(Z3BitVecExpr expr)
    {
        var valueStr = GetNumericValueAsString(expr);

        if (!BigInteger.TryParse(valueStr, out var value))
            throw new InvalidOperationException($"Failed to parse bitvector value '{valueStr}' from expression {expr}");

        return new BitVec(value, expr.Size);
    }

    public string GetNumericValueAsString(Z3NumericExpr expr)
    {
        var evaluated = Evaluate(expr);
        return ExtractNumeralString(context, evaluated, expr);
    }

    // Object Overrides
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

    private static string ExtractNumeralString(Z3Context context, Z3Expr evaluatedExpr, Z3NumericExpr originalExpr)
    {
        if (!NativeMethods.Z3IsNumeralAst(context.Handle, evaluatedExpr.Handle))
            throw new InvalidOperationException($"Expression {originalExpr} does not evaluate to a numeric constant in this model");

        var ptr = NativeMethods.Z3GetNumeralString(context.Handle, evaluatedExpr.Handle);
        return Marshal.PtrToStringAnsi(ptr) ?? throw new InvalidOperationException($"Failed to extract numeric value from expression {originalExpr}");
    }
}