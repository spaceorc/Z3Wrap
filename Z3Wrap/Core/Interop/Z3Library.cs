using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

/// <summary>
/// Provides a safe wrapper around the native Z3 library with error checking and crash prevention.
/// <para>
/// This class implements <see cref="IDisposable"/> and must be disposed when no longer needed,
/// unless ownership is transferred to the <see cref="Z3"/> static class by setting
/// <see cref="Z3.DefaultLibrary"/>. When set as the default library, ownership transfers
/// to the <see cref="Z3"/> class and you must NOT call <see cref="Dispose()"/> manually.
/// The <see cref="Z3"/> class will automatically dispose the previous library when a new
/// one is set or when the application exits.
/// </para>
/// </summary>
public sealed class Z3Library : IDisposable
{
    private readonly NativeLibrary nativeLibrary;
    private bool disposed;

    /// <summary>
    /// Gets the path to the native library that was loaded.
    /// </summary>
    public string LibraryPath => nativeLibrary.LibraryPath;

    private Z3Library(NativeLibrary nativeLibrary)
    {
        this.nativeLibrary = nativeLibrary ?? throw new ArgumentNullException(nameof(nativeLibrary));
    }

    /// <summary>
    /// Loads the Z3 native library from the specified path.
    /// </summary>
    /// <param name="libraryPath">The path to the Z3 native library file.</param>
    /// <returns>A new <see cref="Z3Library"/> instance. The caller is responsible for disposing it.</returns>
    /// <exception cref="ArgumentException">Thrown when the library path is null, empty, or whitespace.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the library file is not found at the specified path.</exception>
    public static Z3Library Load(string libraryPath)
    {
        var nativeLib = NativeLibrary.Load(libraryPath);
        return new Z3Library(nativeLib);
    }

    /// <summary>
    /// Loads the Z3 native library using automatic discovery based on the current platform.
    /// </summary>
    /// <returns>A new <see cref="Z3Library"/> instance. The caller is responsible for disposing it.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the Z3 library cannot be automatically located.</exception>
    public static Z3Library LoadAuto()
    {
        var nativeLib = NativeLibrary.LoadAuto();
        return new Z3Library(nativeLib);
    }

    /// <summary>
    /// Finalizer for Z3Library.
    /// </summary>
    ~Z3Library()
    {
        Dispose();
    }

    /// <summary>
    /// Releases all resources used by the Z3Library.
    /// <para>
    /// IMPORTANT: Do not call this method if you have transferred ownership by setting
    /// this instance as <see cref="Z3.DefaultLibrary"/>. The <see cref="Z3"/> class
    /// will handle disposal automatically.
    /// </para>
    /// </summary>
    public void Dispose()
    {
        if (disposed)
            return;

        nativeLibrary.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }

    // Configuration and context methods
    /// <inheritdoc cref="NativeLibrary.Z3MkConfig"/>
    internal IntPtr Z3MkConfig() => nativeLibrary.Z3MkConfig();

    /// <inheritdoc cref="NativeLibrary.Z3DelConfig"/>
    internal void Z3DelConfig(IntPtr cfg) => nativeLibrary.Z3DelConfig(cfg);

    /// <inheritdoc cref="NativeLibrary.Z3SetParamValue"/>
    internal void Z3SetParamValue(IntPtr cfg, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.Z3SetParamValue(cfg, paramIdPtr, paramValuePtr);
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkContextRc"/>
    internal IntPtr Z3MkContextRc(IntPtr cfg)
    {
        var result = nativeLibrary.Z3MkContextRc(cfg);
        if (result != IntPtr.Zero)
        {
            // No error check for context creation
            // Set up safe error handler (prevents crashes)
            nativeLibrary.Z3SetErrorHandler(result, OnZ3ErrorSafe);
        }
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3DelContext"/>
    internal void Z3DelContext(IntPtr ctx)
    {
        nativeLibrary.Z3DelContext(ctx);
        // No error check needed for deletion
    }

    /// <inheritdoc cref="NativeLibrary.Z3UpdateParamValue"/>
    internal void Z3UpdateParamValue(IntPtr ctx, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.Z3UpdateParamValue(ctx, paramIdPtr, paramValuePtr);
        CheckError(ctx);
    }

    // Reference counting
    /// <inheritdoc cref="NativeLibrary.Z3IncRef"/>
    internal void Z3IncRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.Z3IncRef(ctx, ast);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3DecRef"/>
    internal void Z3DecRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.Z3DecRef(ctx, ast);
        // No error check needed for ref counting
    }

    // Symbol creation
    /// <inheritdoc cref="NativeLibrary.Z3MkStringSymbol"/>
    internal IntPtr Z3MkStringSymbol(IntPtr ctx, string str)
    {
        using var strPtr = new AnsiStringPtr(str);
        var result = nativeLibrary.Z3MkStringSymbol(ctx, strPtr);
        CheckError(ctx);
        return result;
    }

    // Sort creation
    /// <inheritdoc cref="NativeLibrary.Z3MkBoolSort"/>
    internal IntPtr Z3MkBoolSort(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkBoolSort(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkIntSort"/>
    internal IntPtr Z3MkIntSort(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkIntSort(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkRealSort"/>
    internal IntPtr Z3MkRealSort(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkRealSort(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSort"/>
    internal IntPtr Z3MkBvSort(IntPtr ctx, uint size)
    {
        var result = nativeLibrary.Z3MkBvSort(ctx, size);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkArraySort"/>
    internal IntPtr Z3MkArraySort(IntPtr ctx, IntPtr indexSort, IntPtr valueSort)
    {
        var result = nativeLibrary.Z3MkArraySort(ctx, indexSort, valueSort);
        CheckError(ctx);
        return result;
    }

    // Expression creation
    /// <inheritdoc cref="NativeLibrary.Z3MkConst"/>
    internal IntPtr Z3MkConst(IntPtr ctx, IntPtr symbol, IntPtr sort)
    {
        var result = nativeLibrary.Z3MkConst(ctx, symbol, sort);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkTrue"/>
    internal IntPtr Z3MkTrue(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkTrue(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkFalse"/>
    internal IntPtr Z3MkFalse(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkFalse(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkNumeral"/>
    internal IntPtr Z3MkNumeral(IntPtr ctx, string numeral, IntPtr sort)
    {
        using var numeralPtr = new AnsiStringPtr(numeral);
        var result = nativeLibrary.Z3MkNumeral(ctx, numeralPtr, sort);
        CheckError(ctx);
        return result;
    }

    // Boolean operations
    /// <inheritdoc cref="NativeLibrary.Z3MkAnd"/>
    internal IntPtr Z3MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkAnd(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkOr"/>
    internal IntPtr Z3MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkOr(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkNot"/>
    internal IntPtr Z3MkNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkNot(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkImplies"/>
    internal IntPtr Z3MkImplies(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkImplies(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkIff"/>
    internal IntPtr Z3MkIff(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkIff(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkXor"/>
    internal IntPtr Z3MkXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkXor(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkIte"/>
    internal IntPtr Z3MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var result = nativeLibrary.Z3MkIte(ctx, condition, thenExpr, elseExpr);
        CheckError(ctx);
        return result;
    }

    // Arithmetic operations
    /// <inheritdoc cref="NativeLibrary.Z3MkAdd"/>
    internal IntPtr Z3MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkAdd(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkSub"/>
    internal IntPtr Z3MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkSub(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkMul"/>
    internal IntPtr Z3MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkMul(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkDiv"/>
    internal IntPtr Z3MkDiv(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.Z3MkDiv(ctx, arg1, arg2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkMod"/>
    internal IntPtr Z3MkMod(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.Z3MkMod(ctx, arg1, arg2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkUnaryMinus"/>
    internal IntPtr Z3MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var result = nativeLibrary.Z3MkUnaryMinus(ctx, arg);
        CheckError(ctx);
        return result;
    }

    // Comparison operations
    /// <inheritdoc cref="NativeLibrary.Z3MkEq"/>
    internal IntPtr Z3MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkEq(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkLt"/>
    internal IntPtr Z3MkLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkLt(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkLe"/>
    internal IntPtr Z3MkLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkLe(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkGt"/>
    internal IntPtr Z3MkGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkGt(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkGe"/>
    internal IntPtr Z3MkGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkGe(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    // Type conversions
    /// <inheritdoc cref="NativeLibrary.Z3MkInt2Real"/>
    internal IntPtr Z3MkInt2Real(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.Z3MkInt2Real(ctx, term);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkReal2Int"/>
    internal IntPtr Z3MkReal2Int(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.Z3MkReal2Int(ctx, term);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkInt2Bv"/>
    internal IntPtr Z3MkInt2Bv(IntPtr ctx, uint size, IntPtr term)
    {
        var result = nativeLibrary.Z3MkInt2Bv(ctx, size, term);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBv2Int"/>
    internal IntPtr Z3MkBv2Int(IntPtr ctx, IntPtr term, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBv2Int(ctx, term, isSigned);
        CheckError(ctx);
        return result;
    }

    // Quantifier operations
    /// <inheritdoc cref="NativeLibrary.Z3MkForallConst"/>
    internal IntPtr Z3MkForallConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = nativeLibrary.Z3MkForallConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkExistsConst"/>
    internal IntPtr Z3MkExistsConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = nativeLibrary.Z3MkExistsConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkPattern"/>
    internal IntPtr Z3MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var result = nativeLibrary.Z3MkPattern(ctx, numPatterns, terms);
        CheckError(ctx);
        return result;
    }

    // Function declaration and application operations
    /// <inheritdoc cref="NativeLibrary.Z3MkFuncDecl"/>
    internal IntPtr Z3MkFuncDecl(IntPtr ctx, IntPtr symbol, uint domainSize, IntPtr[] domain, IntPtr range)
    {
        var result = nativeLibrary.Z3MkFuncDecl(ctx, symbol, domainSize, domain, range);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkApp"/>
    internal IntPtr Z3MkApp(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkApp(ctx, funcDecl, numArgs, args);
        CheckError(ctx);
        return result;
    }

    // Solver operations
    /// <inheritdoc cref="NativeLibrary.Z3MkSolver"/>
    internal IntPtr Z3MkSolver(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkSolver(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkSimpleSolver"/>
    internal IntPtr Z3MkSimpleSolver(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkSimpleSolver(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverIncRef"/>
    internal void Z3SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.Z3SolverIncRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverDecRef"/>
    internal void Z3SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.Z3SolverDecRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverAssert"/>
    internal void Z3SolverAssert(IntPtr ctx, IntPtr solver, IntPtr expr)
    {
        nativeLibrary.Z3SolverAssert(ctx, solver, expr);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverCheck"/>
    internal int Z3SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.Z3SolverCheck(ctx, solver);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverPush"/>
    internal void Z3SolverPush(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.Z3SolverPush(ctx, solver);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverPop"/>
    internal void Z3SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        nativeLibrary.Z3SolverPop(ctx, solver, numScopes);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverGetModel"/>
    internal IntPtr Z3SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.Z3SolverGetModel(ctx, solver);
        CheckError(ctx);
        return result;
    }

    // Model operations
    /// <inheritdoc cref="NativeLibrary.Z3ModelIncRef"/>
    internal void Z3ModelIncRef(IntPtr ctx, IntPtr model)
    {
        nativeLibrary.Z3ModelIncRef(ctx, model);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3ModelDecRef"/>
    internal void Z3ModelDecRef(IntPtr ctx, IntPtr model)
    {
        nativeLibrary.Z3ModelDecRef(ctx, model);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3ModelToString"/>
    internal string? Z3ModelToString(IntPtr ctx, IntPtr model)
    {
        var result = nativeLibrary.Z3ModelToString(ctx, model);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <inheritdoc cref="NativeLibrary.Z3AstToString"/>
    internal string? Z3AstToString(IntPtr ctx, IntPtr ast)
    {
        var result = nativeLibrary.Z3AstToString(ctx, ast);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <inheritdoc cref="NativeLibrary.Z3ModelEval"/>
    internal bool Z3ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var returnValue = nativeLibrary.Z3ModelEval(ctx, model, expr, modelCompletion, out result);
        CheckError(ctx);
        return returnValue;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetNumeralString"/>
    internal string? Z3GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3GetNumeralString(ctx, expr);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetBoolValue"/>
    internal int Z3GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3GetBoolValue(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3IsNumeralAst"/>
    internal bool Z3IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3IsNumeralAst(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetSort"/>
    internal IntPtr Z3GetSort(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3GetSort(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetSortKind"/>
    internal int Z3GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetSortKind(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // Array operations
    /// <inheritdoc cref="NativeLibrary.Z3MkConstArray"/>
    internal IntPtr Z3MkConstArray(IntPtr ctx, IntPtr sort, IntPtr value)
    {
        var result = nativeLibrary.Z3MkConstArray(ctx, sort, value);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkStore"/>
    internal IntPtr Z3MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var result = nativeLibrary.Z3MkStore(ctx, array, index, value);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkSelect"/>
    internal IntPtr Z3MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var result = nativeLibrary.Z3MkSelect(ctx, array, index);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetArraySortDomain"/>
    internal IntPtr Z3GetArraySortDomain(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetArraySortDomain(ctx, sort);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetArraySortRange"/>
    internal IntPtr Z3GetArraySortRange(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetArraySortRange(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // BitVector operations
    /// <inheritdoc cref="NativeLibrary.Z3MkBvAdd"/>
    internal IntPtr Z3MkBvAdd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAdd(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSub"/>
    internal IntPtr Z3MkBvSub(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSub(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvMul"/>
    internal IntPtr Z3MkBvMul(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvMul(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSDiv"/>
    internal IntPtr Z3MkBvSDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSDiv(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvUDiv"/>
    internal IntPtr Z3MkBvUDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvUDiv(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSRem"/>
    internal IntPtr Z3MkBvSRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSRem(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvURem"/>
    internal IntPtr Z3MkBvURem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvURem(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSMod"/>
    internal IntPtr Z3MkBvSMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSMod(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvNeg"/>
    internal IntPtr Z3MkBvNeg(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkBvNeg(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvAnd"/>
    internal IntPtr Z3MkBvAnd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAnd(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvOr"/>
    internal IntPtr Z3MkBvOr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvOr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvXor"/>
    internal IntPtr Z3MkBvXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvXor(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvNot"/>
    internal IntPtr Z3MkBvNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkBvNot(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvShl"/>
    internal IntPtr Z3MkBvShl(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvShl(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvAShr"/>
    internal IntPtr Z3MkBvAShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAShr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvLShr"/>
    internal IntPtr Z3MkBvLShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvLShr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSLt"/>
    internal IntPtr Z3MkBvSLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSLt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvULt"/>
    internal IntPtr Z3MkBvULt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvULt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSLe"/>
    internal IntPtr Z3MkBvSLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSLe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvULe"/>
    internal IntPtr Z3MkBvULe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvULe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSGt"/>
    internal IntPtr Z3MkBvSGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSGt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvUGt"/>
    internal IntPtr Z3MkBvUGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvUGt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSGe"/>
    internal IntPtr Z3MkBvSGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSGe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvUGe"/>
    internal IntPtr Z3MkBvUGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvUGe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    // BitVector functions
    /// <inheritdoc cref="NativeLibrary.Z3MkSignExt"/>
    internal IntPtr Z3MkSignExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkSignExt(ctx, extra, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkZeroExt"/>
    internal IntPtr Z3MkZeroExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkZeroExt(ctx, extra, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkExtract"/>
    internal IntPtr Z3MkExtract(IntPtr ctx, uint high, uint low, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkExtract(ctx, high, low, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkRepeat"/>
    internal IntPtr Z3MkRepeat(IntPtr ctx, uint count, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkRepeat(ctx, count, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetBvSortSize"/>
    internal uint Z3GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetBvSortSize(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // BitVector overflow checks
    /// <inheritdoc cref="NativeLibrary.Z3MkBvAddNoOverflow"/>
    internal IntPtr Z3MkBvAddNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBvAddNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSubNoOverflow"/>
    internal IntPtr Z3MkBvSubNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSubNoOverflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSubNoUnderflow"/>
    internal IntPtr Z3MkBvSubNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBvSubNoUnderflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvMulNoOverflow"/>
    internal IntPtr Z3MkBvMulNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBvMulNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvMulNoUnderflow"/>
    internal IntPtr Z3MkBvMulNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvMulNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvAddNoUnderflow"/>
    internal IntPtr Z3MkBvAddNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAddNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSDivNoOverflow"/>
    internal IntPtr Z3MkBvSDivNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSDivNoOverflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvNegNoOverflow"/>
    internal IntPtr Z3MkBvNegNoOverflow(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkBvNegNoOverflow(ctx, expr);
        CheckError(ctx);
        return result;
    }

    // Solver operations - add missing methods
    /// <inheritdoc cref="NativeLibrary.Z3SolverGetReasonUnknown"/>
    internal string? Z3SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.Z3SolverGetReasonUnknown(ctx, solver);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverReset"/>
    internal void Z3SolverReset(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.Z3SolverReset(ctx, solver);
        CheckError(ctx);
    }

    private void CheckError(IntPtr ctx)
    {
        var z3ErrorCode = nativeLibrary.Z3GetErrorCode(ctx);
        if (z3ErrorCode == Z3ErrorCode.Ok)
            return;
        var msgPtr = nativeLibrary.Z3GetErrorMsg(ctx, z3ErrorCode);
        var message = Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
        throw new Z3Exception(z3ErrorCode, message);
    }

    private void OnZ3ErrorSafe(IntPtr ctx, int errorCode)
    {
        // DO NOT THROW EXCEPTIONS HERE - this is called from native Z3 code!
        var z3ErrorCode = (Z3ErrorCode)errorCode;
        var msgPtr = nativeLibrary.Z3GetErrorMsg(ctx, z3ErrorCode);
        var message = Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
        System.Diagnostics.Debug.WriteLine($"Z3 Error: {z3ErrorCode}: {message}");
    }
}
