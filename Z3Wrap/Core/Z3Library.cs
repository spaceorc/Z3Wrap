using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;
using NativeLibrary = Spaceorc.Z3Wrap.Core.Interop.NativeLibrary;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
/// Provides a safe wrapper around the native Z3 library with error checking and crash prevention.
/// <para>
/// This class implements <see cref="IDisposable"/> and must be disposed when no longer needed,
/// unless ownership is transferred to the <see cref="Z3"/> static class by setting
/// <see cref="Z3.Library"/>. When set as the default library, ownership transfers
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
    /// this instance as <see cref="Z3.Library"/>. The <see cref="Z3"/> class
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
    public IntPtr Z3MkConfig() => nativeLibrary.Z3MkConfig();

    /// <inheritdoc cref="NativeLibrary.Z3DelConfig"/>
    public void Z3DelConfig(IntPtr cfg) => nativeLibrary.Z3DelConfig(cfg);

    /// <summary>
    /// Sets a configuration parameter value.
    /// </summary>
    /// <param name="cfg">Configuration handle.</param>
    /// <param name="paramId">Parameter name.</param>
    /// <param name="paramValue">Parameter value.</param>
    public void Z3SetParamValue(IntPtr cfg, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.Z3SetParamValue(cfg, paramIdPtr, paramValuePtr);
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkContextRc"/>
    public IntPtr Z3MkContextRc(IntPtr cfg)
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
    public void Z3DelContext(IntPtr ctx)
    {
        nativeLibrary.Z3DelContext(ctx);
        // No error check needed for deletion
    }

    /// <summary>
    /// Updates a parameter value for an existing context.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="paramId">Parameter name.</param>
    /// <param name="paramValue">Parameter value.</param>
    public void Z3UpdateParamValue(IntPtr ctx, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.Z3UpdateParamValue(ctx, paramIdPtr, paramValuePtr);
        CheckError(ctx);
    }

    // Reference counting
    /// <inheritdoc cref="NativeLibrary.Z3IncRef"/>
    public void Z3IncRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.Z3IncRef(ctx, ast);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3DecRef"/>
    public void Z3DecRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.Z3DecRef(ctx, ast);
        // No error check needed for ref counting
    }

    // Sort creation
    /// <inheritdoc cref="NativeLibrary.Z3MkBoolSort"/>
    public IntPtr Z3MkBoolSort(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkBoolSort(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkIntSort"/>
    public IntPtr Z3MkIntSort(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkIntSort(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkRealSort"/>
    public IntPtr Z3MkRealSort(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkRealSort(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSort"/>
    public IntPtr Z3MkBvSort(IntPtr ctx, uint size)
    {
        var result = nativeLibrary.Z3MkBvSort(ctx, size);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkArraySort"/>
    public IntPtr Z3MkArraySort(IntPtr ctx, IntPtr indexSort, IntPtr valueSort)
    {
        var result = nativeLibrary.Z3MkArraySort(ctx, indexSort, valueSort);
        CheckError(ctx);
        return result;
    }

    // Expression creation
    /// <summary>
    /// Creates a constant expression with the given name and sort.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="name">Name of the constant.</param>
    /// <param name="sort">Sort of the constant.</param>
    /// <returns>AST handle for the constant expression.</returns>
    public IntPtr Z3MkConst(IntPtr ctx, string name, IntPtr sort)
    {
        using var strPtr = new AnsiStringPtr(name);

        var symbol = nativeLibrary.Z3MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        var result = nativeLibrary.Z3MkConst(ctx, symbol, sort);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkTrue"/>
    public IntPtr Z3MkTrue(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkTrue(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkFalse"/>
    public IntPtr Z3MkFalse(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkFalse(ctx);
        CheckError(ctx);
        return result;
    }

    /// <summary>
    /// Creates a numeral expression from a string representation.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="numeral">String representation of the numeral.</param>
    /// <param name="sort">Sort of the numeral.</param>
    /// <returns>AST handle for the numeral expression.</returns>
    public IntPtr Z3MkNumeral(IntPtr ctx, string numeral, IntPtr sort)
    {
        using var numeralPtr = new AnsiStringPtr(numeral);
        var result = nativeLibrary.Z3MkNumeral(ctx, numeralPtr, sort);
        CheckError(ctx);
        return result;
    }

    // Boolean operations
    /// <inheritdoc cref="NativeLibrary.Z3MkAnd"/>
    public IntPtr Z3MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkAnd(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkOr"/>
    public IntPtr Z3MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkOr(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkNot"/>
    public IntPtr Z3MkNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkNot(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkImplies"/>
    public IntPtr Z3MkImplies(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkImplies(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkIff"/>
    public IntPtr Z3MkIff(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkIff(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkXor"/>
    public IntPtr Z3MkXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkXor(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkIte"/>
    public IntPtr Z3MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var result = nativeLibrary.Z3MkIte(ctx, condition, thenExpr, elseExpr);
        CheckError(ctx);
        return result;
    }

    // Arithmetic operations
    /// <inheritdoc cref="NativeLibrary.Z3MkAdd"/>
    public IntPtr Z3MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkAdd(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkSub"/>
    public IntPtr Z3MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkSub(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkMul"/>
    public IntPtr Z3MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkMul(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkDiv"/>
    public IntPtr Z3MkDiv(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.Z3MkDiv(ctx, arg1, arg2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkMod"/>
    public IntPtr Z3MkMod(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.Z3MkMod(ctx, arg1, arg2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkUnaryMinus"/>
    public IntPtr Z3MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var result = nativeLibrary.Z3MkUnaryMinus(ctx, arg);
        CheckError(ctx);
        return result;
    }

    // Comparison operations
    /// <inheritdoc cref="NativeLibrary.Z3MkEq"/>
    public IntPtr Z3MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkEq(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkLt"/>
    public IntPtr Z3MkLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkLt(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkLe"/>
    public IntPtr Z3MkLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkLe(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkGt"/>
    public IntPtr Z3MkGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkGt(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkGe"/>
    public IntPtr Z3MkGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.Z3MkGe(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    // Type conversions
    /// <inheritdoc cref="NativeLibrary.Z3MkInt2Real"/>
    public IntPtr Z3MkInt2Real(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.Z3MkInt2Real(ctx, term);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkReal2Int"/>
    public IntPtr Z3MkReal2Int(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.Z3MkReal2Int(ctx, term);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkInt2Bv"/>
    public IntPtr Z3MkInt2Bv(IntPtr ctx, uint size, IntPtr term)
    {
        var result = nativeLibrary.Z3MkInt2Bv(ctx, size, term);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBv2Int"/>
    public IntPtr Z3MkBv2Int(IntPtr ctx, IntPtr term, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBv2Int(ctx, term, isSigned);
        CheckError(ctx);
        return result;
    }

    // Quantifier operations
    /// <inheritdoc cref="NativeLibrary.Z3MkForallConst"/>
    public IntPtr Z3MkForallConst(
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
    public IntPtr Z3MkExistsConst(
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
    public IntPtr Z3MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var result = nativeLibrary.Z3MkPattern(ctx, numPatterns, terms);
        CheckError(ctx);
        return result;
    }

    // Function declaration and application operations
    /// <summary>
    /// Creates a function declaration with the given name, domain, and range.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="name">Name of the function.</param>
    /// <param name="domainSize">Number of arguments.</param>
    /// <param name="domain">Array of argument sorts.</param>
    /// <param name="range">Return sort.</param>
    /// <returns>Function declaration handle.</returns>
    public IntPtr Z3MkFuncDecl(IntPtr ctx, string name, uint domainSize, IntPtr[] domain, IntPtr range)
    {
        using var strPtr = new AnsiStringPtr(name);

        var symbol = nativeLibrary.Z3MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        var result = nativeLibrary.Z3MkFuncDecl(ctx, symbol, domainSize, domain, range);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkApp"/>
    public IntPtr Z3MkApp(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.Z3MkApp(ctx, funcDecl, numArgs, args);
        CheckError(ctx);
        return result;
    }

    // Solver operations
    /// <inheritdoc cref="NativeLibrary.Z3MkSolver"/>
    public IntPtr Z3MkSolver(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkSolver(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkSimpleSolver"/>
    public IntPtr Z3MkSimpleSolver(IntPtr ctx)
    {
        var result = nativeLibrary.Z3MkSimpleSolver(ctx);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverIncRef"/>
    public void Z3SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.Z3SolverIncRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverDecRef"/>
    public void Z3SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.Z3SolverDecRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverAssert"/>
    public void Z3SolverAssert(IntPtr ctx, IntPtr solver, IntPtr expr)
    {
        nativeLibrary.Z3SolverAssert(ctx, solver, expr);
        CheckError(ctx);
    }

    /// <summary>
    /// Checks the satisfiability of the assertions in the solver.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="solver">Solver handle.</param>
    /// <returns>The satisfiability result (Satisfiable, Unsatisfiable, or Unknown).</returns>
    public Z3Status Z3SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.Z3SolverCheck(ctx, solver);
        CheckError(ctx);
        return (Z3BoolValue)result switch
        {
            Z3BoolValue.False => Z3Status.Unsatisfiable,
            Z3BoolValue.True => Z3Status.Satisfiable,
            Z3BoolValue.Undefined => Z3Status.Unknown,
            _ => throw new InvalidOperationException($"Unexpected boolean value result {result} from Z3_solver_check"),
        };
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverPush"/>
    public void Z3SolverPush(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.Z3SolverPush(ctx, solver);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverPop"/>
    public void Z3SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        nativeLibrary.Z3SolverPop(ctx, solver, numScopes);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverGetModel"/>
    public IntPtr Z3SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.Z3SolverGetModel(ctx, solver);
        CheckError(ctx);
        return result;
    }

    // Model operations
    /// <inheritdoc cref="NativeLibrary.Z3ModelIncRef"/>
    public void Z3ModelIncRef(IntPtr ctx, IntPtr model)
    {
        nativeLibrary.Z3ModelIncRef(ctx, model);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.Z3ModelDecRef"/>
    public void Z3ModelDecRef(IntPtr ctx, IntPtr model)
    {
        nativeLibrary.Z3ModelDecRef(ctx, model);
        // No error check needed for ref counting
    }

    /// <summary>
    /// Converts a model to its string representation.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="model">Model handle.</param>
    /// <returns>String representation of the model, or null if conversion fails.</returns>
    public string? Z3ModelToString(IntPtr ctx, IntPtr model)
    {
        var result = nativeLibrary.Z3ModelToString(ctx, model);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <summary>
    /// Converts an AST node to its string representation.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="ast">AST handle.</param>
    /// <returns>String representation of the AST, or null if conversion fails.</returns>
    public string? Z3AstToString(IntPtr ctx, IntPtr ast)
    {
        var result = nativeLibrary.Z3AstToString(ctx, ast);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <inheritdoc cref="NativeLibrary.Z3ModelEval"/>
    public bool Z3ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var returnValue = nativeLibrary.Z3ModelEval(ctx, model, expr, modelCompletion, out result);
        CheckError(ctx);
        return returnValue;
    }

    /// <summary>
    /// Gets the string representation of a numeral expression.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="expr">Numeral expression handle.</param>
    /// <returns>String representation of the numeral, or null if conversion fails.</returns>
    public string? Z3GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3GetNumeralString(ctx, expr);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <summary>
    /// Gets the Boolean value of a Boolean expression.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="expr">Boolean expression handle.</param>
    /// <returns>The Boolean value (True, False, or Undefined).</returns>
    public Z3BoolValue Z3GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3GetBoolValue(ctx, expr);
        CheckError(ctx);
        return (Z3BoolValue)result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3IsNumeralAst"/>
    public bool Z3IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3IsNumeralAst(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetSort"/>
    public IntPtr Z3GetSort(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3GetSort(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetSortKind"/>
    public Z3SortKind Z3GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetSortKind(ctx, sort);
        CheckError(ctx);
        return (Z3SortKind)result;
    }

    // Array operations
    /// <inheritdoc cref="NativeLibrary.Z3MkConstArray"/>
    public IntPtr Z3MkConstArray(IntPtr ctx, IntPtr sort, IntPtr value)
    {
        var result = nativeLibrary.Z3MkConstArray(ctx, sort, value);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkStore"/>
    public IntPtr Z3MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var result = nativeLibrary.Z3MkStore(ctx, array, index, value);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkSelect"/>
    public IntPtr Z3MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var result = nativeLibrary.Z3MkSelect(ctx, array, index);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetArraySortDomain"/>
    public IntPtr Z3GetArraySortDomain(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetArraySortDomain(ctx, sort);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetArraySortRange"/>
    public IntPtr Z3GetArraySortRange(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetArraySortRange(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // BitVector operations
    /// <inheritdoc cref="NativeLibrary.Z3MkBvAdd"/>
    public IntPtr Z3MkBvAdd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAdd(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSub"/>
    public IntPtr Z3MkBvSub(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSub(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvMul"/>
    public IntPtr Z3MkBvMul(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvMul(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSDiv"/>
    public IntPtr Z3MkBvSDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSDiv(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvUDiv"/>
    public IntPtr Z3MkBvUDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvUDiv(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSRem"/>
    public IntPtr Z3MkBvSRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSRem(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvURem"/>
    public IntPtr Z3MkBvURem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvURem(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSMod"/>
    public IntPtr Z3MkBvSMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSMod(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvNeg"/>
    public IntPtr Z3MkBvNeg(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkBvNeg(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvAnd"/>
    public IntPtr Z3MkBvAnd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAnd(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvOr"/>
    public IntPtr Z3MkBvOr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvOr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvXor"/>
    public IntPtr Z3MkBvXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvXor(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvNot"/>
    public IntPtr Z3MkBvNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkBvNot(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvShl"/>
    public IntPtr Z3MkBvShl(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvShl(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvAShr"/>
    public IntPtr Z3MkBvAShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAShr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvLShr"/>
    public IntPtr Z3MkBvLShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvLShr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSLt"/>
    public IntPtr Z3MkBvSLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSLt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvULt"/>
    public IntPtr Z3MkBvULt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvULt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSLe"/>
    public IntPtr Z3MkBvSLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSLe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvULe"/>
    public IntPtr Z3MkBvULe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvULe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSGt"/>
    public IntPtr Z3MkBvSGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSGt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvUGt"/>
    public IntPtr Z3MkBvUGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvUGt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSGe"/>
    public IntPtr Z3MkBvSGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSGe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvUGe"/>
    public IntPtr Z3MkBvUGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvUGe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    // BitVector functions
    /// <inheritdoc cref="NativeLibrary.Z3MkSignExt"/>
    public IntPtr Z3MkSignExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkSignExt(ctx, extra, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkZeroExt"/>
    public IntPtr Z3MkZeroExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkZeroExt(ctx, extra, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkExtract"/>
    public IntPtr Z3MkExtract(IntPtr ctx, uint high, uint low, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkExtract(ctx, high, low, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkRepeat"/>
    public IntPtr Z3MkRepeat(IntPtr ctx, uint count, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkRepeat(ctx, count, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3GetBvSortSize"/>
    public uint Z3GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.Z3GetBvSortSize(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // BitVector overflow checks
    /// <inheritdoc cref="NativeLibrary.Z3MkBvAddNoOverflow"/>
    public IntPtr Z3MkBvAddNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBvAddNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSubNoOverflow"/>
    public IntPtr Z3MkBvSubNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSubNoOverflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSubNoUnderflow"/>
    public IntPtr Z3MkBvSubNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBvSubNoUnderflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvMulNoOverflow"/>
    public IntPtr Z3MkBvMulNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.Z3MkBvMulNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvMulNoUnderflow"/>
    public IntPtr Z3MkBvMulNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvMulNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvAddNoUnderflow"/>
    public IntPtr Z3MkBvAddNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvAddNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvSDivNoOverflow"/>
    public IntPtr Z3MkBvSDivNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.Z3MkBvSDivNoOverflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.Z3MkBvNegNoOverflow"/>
    public IntPtr Z3MkBvNegNoOverflow(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.Z3MkBvNegNoOverflow(ctx, expr);
        CheckError(ctx);
        return result;
    }

    // Solver operations - add missing methods
    /// <summary>
    /// Gets the reason why the solver returned unknown status.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="solver">Solver handle.</param>
    /// <returns>String describing the reason for unknown status, or null if not available.</returns>
    public string? Z3SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.Z3SolverGetReasonUnknown(ctx, solver);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(result);
    }

    /// <inheritdoc cref="NativeLibrary.Z3SolverReset"/>
    public void Z3SolverReset(IntPtr ctx, IntPtr solver)
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
