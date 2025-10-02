using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Library
{
    /// <inheritdoc cref="Interop.NativeLibrary.ModelIncRef" />
    public void ModelIncRef(IntPtr ctx, IntPtr model)
    {
        nativeLibrary.ModelIncRef(ctx, model);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="Interop.NativeLibrary.ModelDecRef" />
    public void ModelDecRef(IntPtr ctx, IntPtr model)
    {
        nativeLibrary.ModelDecRef(ctx, model);
        // No error check needed for ref counting
    }

    /// <summary>
    ///     Converts a model to its string representation.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="model">Model handle.</param>
    /// <returns>String representation of the model, or null if conversion fails.</returns>
    public string? ModelToString(IntPtr ctx, IntPtr model)
    {
        var result = nativeLibrary.ModelToString(ctx, model);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(ModelToString)));
    }

    /// <summary>
    ///     Converts an AST node to its string representation.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="ast">AST handle.</param>
    /// <returns>String representation of the AST, or null if conversion fails.</returns>
    public string? AstToString(IntPtr ctx, IntPtr ast)
    {
        var result = nativeLibrary.AstToString(ctx, ast);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(AstToString)));
    }

    /// <inheritdoc cref="Interop.NativeLibrary.ModelEval" />
    public bool ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var returnValue = nativeLibrary.ModelEval(ctx, model, expr, modelCompletion, out result);
        CheckError(ctx);
        return returnValue;
    }

    /// <summary>
    ///     Gets the string representation of a numeral expression.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="expr">Numeral expression handle.</param>
    /// <returns>String representation of the numeral, or null if conversion fails.</returns>
    public string? GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.GetNumeralString(ctx, expr);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(GetNumeralString)));
    }

    /// <summary>
    ///     Gets the Boolean value of a Boolean expression.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="expr">Boolean expression handle.</param>
    /// <returns>The Boolean value (True, False, or Undefined).</returns>
    public Z3BoolValue GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.GetBoolValue(ctx, expr);
        CheckError(ctx);
        return (Z3BoolValue)result;
    }

    /// <inheritdoc cref="Interop.NativeLibrary.IsNumeralAst" />
    public bool IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.IsNumeralAst(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="Interop.NativeLibrary.GetSort" />
    public IntPtr GetSort(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.GetSort(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(GetSort));
    }

    /// <inheritdoc cref="Interop.NativeLibrary.GetSortKind" />
    public Z3SortKind GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetSortKind(ctx, sort);
        CheckError(ctx);
        return (Z3SortKind)result;
    }
}
