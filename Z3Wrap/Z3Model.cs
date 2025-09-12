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

    public Z3Expr Evaluate(Z3Expr expr, bool modelCompletion = true)
    {
        ThrowIfInvalidated();
        context.ThrowIfDisposed();
        
        if (!NativeMethods.Z3ModelEval(context.Handle, modelHandle, expr.Handle, modelCompletion, out IntPtr result))
            throw new InvalidOperationException("Failed to evaluate expression in model");
        
        return context.WrapExpr(result);
    }

    public BigInteger GetIntValue(Z3IntExpr expr)
    {
        var evaluated = Evaluate(expr);
        
        if (!NativeMethods.Z3IsNumeralAst(context.Handle, evaluated.Handle))
            throw new InvalidOperationException($"Expression {expr} does not evaluate to a numeric constant in this model");
        
        var ptr = NativeMethods.Z3GetNumeralString(context.Handle, evaluated.Handle);
        var valueStr = Marshal.PtrToStringAnsi(ptr) ?? throw new InvalidOperationException($"Failed to extract integer value from expression {expr}");
        
        if (!BigInteger.TryParse(valueStr, out BigInteger value))
            throw new InvalidOperationException($"Failed to parse integer value '{valueStr}' from expression {expr}");
            
        return value;
    }

    public Z3BoolValue GetBoolValue(Z3BoolExpr expr)
    {
        var evaluated = Evaluate(expr);
        
        var boolValue = NativeMethods.Z3GetBoolValue(context.Handle, evaluated.Handle);
        
        return boolValue switch
        {
            1 => Z3BoolValue.True,
            0 => Z3BoolValue.False,
            -1 => Z3BoolValue.Undefined,
            _ => throw new InvalidOperationException($"Invalid boolean value {boolValue} from Z3")
        };
    }

    public Real GetRealValue(Z3RealExpr expr) => Real.Parse(GetRealValueAsString(expr));

    public string GetRealValueAsString(Z3RealExpr expr)
    {
        var evaluated = Evaluate(expr);
        
        if (!NativeMethods.Z3IsNumeralAst(context.Handle, evaluated.Handle))
            throw new InvalidOperationException($"Expression {expr} does not evaluate to a numeric constant in this model");
        
        var ptr = NativeMethods.Z3GetNumeralString(context.Handle, evaluated.Handle);
        return Marshal.PtrToStringAnsi(ptr) ?? throw new InvalidOperationException($"Failed to extract real value from expression {expr}");
    }

    public override string ToString()
    {
        if (invalidated) 
            return "<invalidated>";
        
        try
        {
            context.ThrowIfDisposed();
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

    internal void Invalidate()
    {
        if (!invalidated && modelHandle != IntPtr.Zero)
        {
            try
            {
                // Only dec ref if context is still alive
                context.ThrowIfDisposed(); // This will throw if disposed
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

    private void ThrowIfInvalidated()
    {
        if (invalidated)
            throw new ObjectDisposedException(nameof(Z3Model), "Model has been invalidated due to solver state change");
    }
}