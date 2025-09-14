using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Bitvector constant creation
    public static Z3BitVecExpr BitVecConst(this Z3Context context, string name, uint size)
    {
        var sort = NativeMethods.Z3MkBvSort(context.Handle, size);
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, sort);

        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitvector value creation from int
    public static Z3BitVecExpr BitVec(this Z3Context context, int value, uint size)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sort = NativeMethods.Z3MkBvSort(context.Handle, size);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, numeralPtr, sort);

        return Z3BitVecExpr.Create(context, handle);
    }
}