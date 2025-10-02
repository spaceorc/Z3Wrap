using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    private static void LoadFunctionsFunctionInterpretations(IntPtr handle, Dictionary<string, IntPtr> functionPointers)
    {
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_get_num_entries");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_get_entry");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_get_else");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_set_else");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_get_arity");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_interp_add_entry");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_entry_get_value");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_entry_get_num_args");
        LoadFunctionOrNull(handle, functionPointers, "Z3_func_entry_get_arg");
    }

    // Delegates
    private delegate uint FuncInterpGetNumEntriesDelegate(IntPtr ctx, IntPtr funcInterp);
    private delegate IntPtr FuncInterpGetEntryDelegate(IntPtr ctx, IntPtr funcInterp, uint i);
    private delegate IntPtr FuncInterpGetElseDelegate(IntPtr ctx, IntPtr funcInterp);
    private delegate void FuncInterpSetElseDelegate(IntPtr ctx, IntPtr funcInterp, IntPtr elseValue);
    private delegate uint FuncInterpGetArityDelegate(IntPtr ctx, IntPtr funcInterp);
    private delegate void FuncInterpAddEntryDelegate(IntPtr ctx, IntPtr funcInterp, IntPtr args, IntPtr value);
    private delegate IntPtr FuncEntryGetValueDelegate(IntPtr ctx, IntPtr entry);
    private delegate uint FuncEntryGetNumArgsDelegate(IntPtr ctx, IntPtr entry);
    private delegate IntPtr FuncEntryGetArgDelegate(IntPtr ctx, IntPtr entry, uint i);

    // Methods

    /// <summary>
    /// Retrieves the number of entries in a function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">The function interpretation handle.</param>
    /// <returns>Number of entries in the function interpretation's finite map.</returns>
    /// <remarks>
    /// Function interpretations are represented as finite maps plus an 'else' value.
    /// Each entry represents the function's value at a specific point.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint FuncInterpGetNumEntries(IntPtr ctx, IntPtr funcInterp)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_get_num_entries");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpGetNumEntriesDelegate>(funcPtr);
        return func(ctx, funcInterp);
    }

    /// <summary>
    /// Retrieves an entry from a function interpretation at the specified index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">The function interpretation handle.</param>
    /// <param name="i">Index of the entry (must be less than FuncInterpGetNumEntries).</param>
    /// <returns>Handle to the function entry at the specified index.</returns>
    /// <remarks>
    /// Each entry represents a point in the finite map encoding the function interpretation.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr FuncInterpGetEntry(IntPtr ctx, IntPtr funcInterp, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_get_entry");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpGetEntryDelegate>(funcPtr);
        return func(ctx, funcInterp, i);
    }

    /// <summary>
    /// Retrieves the 'else' value of a function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">The function interpretation handle.</param>
    /// <returns>AST node representing the default value for arguments not in the finite map.</returns>
    /// <remarks>
    /// Function interpretations consist of a finite map and an 'else' value.
    /// The 'else' value is returned for any input not explicitly in the map.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr FuncInterpGetElse(IntPtr ctx, IntPtr funcInterp)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_get_else");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpGetElseDelegate>(funcPtr);
        return func(ctx, funcInterp);
    }

    /// <summary>
    /// Sets the 'else' value of a function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">The function interpretation handle.</param>
    /// <param name="elseValue">AST node representing the new default value.</param>
    /// <remarks>
    /// Updates the default value returned for inputs not in the finite map.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FuncInterpSetElse(IntPtr ctx, IntPtr funcInterp, IntPtr elseValue)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_set_else");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpSetElseDelegate>(funcPtr);
        func(ctx, funcInterp, elseValue);
    }

    /// <summary>
    /// Retrieves the arity (number of arguments) of a function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">The function interpretation handle.</param>
    /// <returns>Number of arguments the function accepts.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint FuncInterpGetArity(IntPtr ctx, IntPtr funcInterp)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_get_arity");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpGetArityDelegate>(funcPtr);
        return func(ctx, funcInterp);
    }

    /// <summary>
    /// Adds an entry to a function interpretation.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="funcInterp">The function interpretation handle to update.</param>
    /// <param name="args">AST vector of argument values (must be constants of correct types).</param>
    /// <param name="value">AST node representing the function's value at these arguments.</param>
    /// <remarks>
    /// Arguments must be constant values (such as integers) of the same types as the function domain.
    /// Entries added should cover disjoint argument tuples.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal void FuncInterpAddEntry(IntPtr ctx, IntPtr funcInterp, IntPtr args, IntPtr value)
    {
        var funcPtr = GetFunctionPointer("Z3_func_interp_add_entry");
        var func = Marshal.GetDelegateForFunctionPointer<FuncInterpAddEntryDelegate>(funcPtr);
        func(ctx, funcInterp, args, value);
    }

    /// <summary>
    /// Retrieves the value of a function entry.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="entry">The function entry handle.</param>
    /// <returns>AST node representing the function's result value at this entry.</returns>
    /// <remarks>
    /// Function entries represent points in the finite map. This returns the output value.
    /// </remarks>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr FuncEntryGetValue(IntPtr ctx, IntPtr entry)
    {
        var funcPtr = GetFunctionPointer("Z3_func_entry_get_value");
        var func = Marshal.GetDelegateForFunctionPointer<FuncEntryGetValueDelegate>(funcPtr);
        return func(ctx, entry);
    }

    /// <summary>
    /// Retrieves the number of arguments in a function entry.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="entry">The function entry handle.</param>
    /// <returns>Number of argument values in the entry.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal uint FuncEntryGetNumArgs(IntPtr ctx, IntPtr entry)
    {
        var funcPtr = GetFunctionPointer("Z3_func_entry_get_num_args");
        var func = Marshal.GetDelegateForFunctionPointer<FuncEntryGetNumArgsDelegate>(funcPtr);
        return func(ctx, entry);
    }

    /// <summary>
    /// Retrieves an argument from a function entry at the specified index.
    /// </summary>
    /// <param name="ctx">The Z3 context handle.</param>
    /// <param name="entry">The function entry handle.</param>
    /// <param name="i">Index of the argument (must be less than FuncEntryGetNumArgs).</param>
    /// <returns>AST node representing the argument at the specified index.</returns>
    /// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
    internal IntPtr FuncEntryGetArg(IntPtr ctx, IntPtr entry, uint i)
    {
        var funcPtr = GetFunctionPointer("Z3_func_entry_get_arg");
        var func = Marshal.GetDelegateForFunctionPointer<FuncEntryGetArgDelegate>(funcPtr);
        return func(ctx, entry, i);
    }
}
