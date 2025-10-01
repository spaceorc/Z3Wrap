using System.Diagnostics;
using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Core.Interop;
using NativeLibrary = Spaceorc.Z3Wrap.Core.Interop.NativeLibrary;

namespace Spaceorc.Z3Wrap.Core;

/// <summary>
///     Provides a safe wrapper around the native Z3 library with error checking and crash prevention.
///     <para>
///         This class implements <see cref="IDisposable" /> and must be disposed when no longer needed,
///         unless ownership is transferred to the <see cref="Z3" /> static class by setting
///         <see cref="Z3.Library" />. When set as the default library, ownership transfers
///         to the <see cref="Z3" /> class and you must NOT call <see cref="Dispose()" /> manually.
///         The <see cref="Z3" /> class will automatically dispose the previous library when a new
///         one is set or when the application exits.
///     </para>
/// </summary>
public sealed class Z3Library : IDisposable
{
    private readonly NativeLibrary nativeLibrary;
    private bool disposed;

    private Z3Library(NativeLibrary nativeLibrary)
    {
        this.nativeLibrary = nativeLibrary ?? throw new ArgumentNullException(nameof(nativeLibrary));
    }

    /// <summary>
    ///     Gets the path to the native library that was loaded.
    /// </summary>
    public string LibraryPath => nativeLibrary.LibraryPath;

    /// <summary>
    ///     Releases all resources used by the Z3Library.
    ///     <para>
    ///         IMPORTANT: Do not call this method if you have transferred ownership by setting
    ///         this instance as <see cref="Z3.Library" />. The <see cref="Z3" /> class
    ///         will handle disposal automatically.
    ///     </para>
    /// </summary>
    public void Dispose()
    {
        if (disposed)
            return;

        nativeLibrary.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Loads the Z3 native library from the specified path.
    /// </summary>
    /// <param name="libraryPath">The path to the Z3 native library file.</param>
    /// <returns>A new <see cref="Z3Library" /> instance. The caller is responsible for disposing it.</returns>
    /// <exception cref="ArgumentException">Thrown when the library path is null, empty, or whitespace.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the library file is not found at the specified path.</exception>
    public static Z3Library Load(string libraryPath)
    {
        var nativeLib = NativeLibrary.Load(libraryPath);
        return new Z3Library(nativeLib);
    }

    /// <summary>
    ///     Loads the Z3 native library using automatic discovery based on the current platform.
    /// </summary>
    /// <returns>A new <see cref="Z3Library" /> instance. The caller is responsible for disposing it.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the Z3 library cannot be automatically located.</exception>
    public static Z3Library LoadAuto()
    {
        var nativeLib = NativeLibrary.LoadAuto();
        return new Z3Library(nativeLib);
    }

    /// <summary>
    ///     Finalizer for Z3Library.
    /// </summary>
    ~Z3Library()
    {
        Dispose();
    }

    // Configuration and context methods
    /// <inheritdoc cref="NativeLibrary.MkConfig" />
    public IntPtr MkConfig()
    {
        return nativeLibrary.MkConfig();
    }

    /// <inheritdoc cref="NativeLibrary.DelConfig" />
    public void DelConfig(IntPtr cfg)
    {
        nativeLibrary.DelConfig(cfg);
    }

    /// <summary>
    ///     Sets a configuration parameter value.
    /// </summary>
    /// <param name="cfg">Configuration handle.</param>
    /// <param name="paramId">Parameter name.</param>
    /// <param name="paramValue">Parameter value.</param>
    public void SetParamValue(IntPtr cfg, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.SetParamValue(cfg, paramIdPtr, paramValuePtr);
    }

    /// <inheritdoc cref="NativeLibrary.MkContextRc" />
    public IntPtr MkContextRc(IntPtr cfg)
    {
        var result = CheckHandle(nativeLibrary.MkContextRc(cfg), nameof(MkContextRc));

        // No error check for context creation
        // Set up safe error handler (prevents crashes)
        nativeLibrary.SetErrorHandler(result, OnZ3ErrorSafe);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.DelContext" />
    public void DelContext(IntPtr ctx)
    {
        nativeLibrary.DelContext(ctx);
        // No error check needed for deletion
    }

    /// <summary>
    ///     Updates a parameter value for an existing context.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="paramId">Parameter name.</param>
    /// <param name="paramValue">Parameter value.</param>
    public void UpdateParamValue(IntPtr ctx, string paramId, string paramValue)
    {
        using var paramIdPtr = new AnsiStringPtr(paramId);
        using var paramValuePtr = new AnsiStringPtr(paramValue);
        nativeLibrary.UpdateParamValue(ctx, paramIdPtr, paramValuePtr);
        CheckError(ctx);
    }

    // Reference counting
    /// <inheritdoc cref="NativeLibrary.IncRef" />
    public void IncRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.IncRef(ctx, ast);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.DecRef" />
    public void DecRef(IntPtr ctx, IntPtr ast)
    {
        nativeLibrary.DecRef(ctx, ast);
        // No error check needed for ref counting
    }

    // Sort creation
    /// <inheritdoc cref="NativeLibrary.MkBoolSort" />
    public IntPtr MkBoolSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkBoolSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBoolSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkIntSort" />
    public IntPtr MkIntSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkIntSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkIntSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkRealSort" />
    public IntPtr MkRealSort(IntPtr ctx)
    {
        var result = nativeLibrary.MkRealSort(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRealSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSort" />
    public IntPtr MkBvSort(IntPtr ctx, uint size)
    {
        var result = nativeLibrary.MkBvSort(ctx, size);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSort));
    }

    /// <inheritdoc cref="NativeLibrary.MkArraySort" />
    public IntPtr MkArraySort(IntPtr ctx, IntPtr indexSort, IntPtr valueSort)
    {
        var result = nativeLibrary.MkArraySort(ctx, indexSort, valueSort);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkArraySort));
    }

    // Expression creation
    /// <summary>
    ///     Creates a constant expression with the given name and sort.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="name">Name of the constant.</param>
    /// <param name="sort">Sort of the constant.</param>
    /// <returns>AST handle for the constant expression.</returns>
    public IntPtr MkConst(IntPtr ctx, string name, IntPtr sort)
    {
        using var strPtr = new AnsiStringPtr(name);

        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        var result = nativeLibrary.MkConst(ctx, symbol, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkConst));
    }

    /// <inheritdoc cref="NativeLibrary.MkTrue" />
    public IntPtr MkTrue(IntPtr ctx)
    {
        var result = nativeLibrary.MkTrue(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkTrue));
    }

    /// <inheritdoc cref="NativeLibrary.MkFalse" />
    public IntPtr MkFalse(IntPtr ctx)
    {
        var result = nativeLibrary.MkFalse(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkFalse));
    }

    /// <summary>
    ///     Creates a numeral expression from a string representation.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="numeral">String representation of the numeral.</param>
    /// <param name="sort">Sort of the numeral.</param>
    /// <returns>AST handle for the numeral expression.</returns>
    public IntPtr MkNumeral(IntPtr ctx, string numeral, IntPtr sort)
    {
        using var numeralPtr = new AnsiStringPtr(numeral);
        var result = nativeLibrary.MkNumeral(ctx, numeralPtr, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkNumeral));
    }

    // Boolean operations
    /// <inheritdoc cref="NativeLibrary.MkAnd" />
    public IntPtr MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkAnd(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkAnd));
    }

    /// <inheritdoc cref="NativeLibrary.MkOr" />
    public IntPtr MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkOr(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkOr));
    }

    /// <inheritdoc cref="NativeLibrary.MkNot" />
    public IntPtr MkNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkNot(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkNot));
    }

    /// <inheritdoc cref="NativeLibrary.MkImplies" />
    public IntPtr MkImplies(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkImplies(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkImplies));
    }

    /// <inheritdoc cref="NativeLibrary.MkIff" />
    public IntPtr MkIff(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkIff(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkIff));
    }

    /// <inheritdoc cref="NativeLibrary.MkXor" />
    public IntPtr MkXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkXor(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkXor));
    }

    /// <inheritdoc cref="NativeLibrary.MkIte" />
    public IntPtr MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var result = nativeLibrary.MkIte(ctx, condition, thenExpr, elseExpr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkIte));
    }

    // Arithmetic operations
    /// <inheritdoc cref="NativeLibrary.MkAdd" />
    public IntPtr MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkAdd(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkAdd));
    }

    /// <inheritdoc cref="NativeLibrary.MkSub" />
    public IntPtr MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkSub(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSub));
    }

    /// <inheritdoc cref="NativeLibrary.MkMul" />
    public IntPtr MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkMul(ctx, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkMul));
    }

    /// <inheritdoc cref="NativeLibrary.MkDiv" />
    public IntPtr MkDiv(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkDiv(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkDiv));
    }

    /// <inheritdoc cref="NativeLibrary.MkMod" />
    public IntPtr MkMod(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = nativeLibrary.MkMod(ctx, arg1, arg2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkMod));
    }

    /// <inheritdoc cref="NativeLibrary.MkUnaryMinus" />
    public IntPtr MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var result = nativeLibrary.MkUnaryMinus(ctx, arg);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkUnaryMinus));
    }

    // Comparison operations
    /// <inheritdoc cref="NativeLibrary.MkEq" />
    public IntPtr MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkEq(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkEq));
    }

    /// <inheritdoc cref="NativeLibrary.MkLt" />
    public IntPtr MkLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkLt(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkLt));
    }

    /// <inheritdoc cref="NativeLibrary.MkLe" />
    public IntPtr MkLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkLe(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkLe));
    }

    /// <inheritdoc cref="NativeLibrary.MkGt" />
    public IntPtr MkGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkGt(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkGt));
    }

    /// <inheritdoc cref="NativeLibrary.MkGe" />
    public IntPtr MkGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = nativeLibrary.MkGe(ctx, t1, t2);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkGe));
    }

    // Type conversions
    /// <inheritdoc cref="NativeLibrary.MkInt2Real" />
    public IntPtr MkInt2Real(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.MkInt2Real(ctx, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkInt2Real));
    }

    /// <inheritdoc cref="NativeLibrary.MkReal2Int" />
    public IntPtr MkReal2Int(IntPtr ctx, IntPtr term)
    {
        var result = nativeLibrary.MkReal2Int(ctx, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkReal2Int));
    }

    /// <inheritdoc cref="NativeLibrary.MkInt2Bv" />
    public IntPtr MkInt2Bv(IntPtr ctx, uint size, IntPtr term)
    {
        var result = nativeLibrary.MkInt2Bv(ctx, size, term);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkInt2Bv));
    }

    /// <inheritdoc cref="NativeLibrary.MkBv2Int" />
    public IntPtr MkBv2Int(IntPtr ctx, IntPtr term, bool isSigned)
    {
        var result = nativeLibrary.MkBv2Int(ctx, term, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBv2Int));
    }

    // Quantifier operations
    /// <inheritdoc cref="NativeLibrary.MkForallConst" />
    public IntPtr MkForallConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = nativeLibrary.MkForallConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkForallConst));
    }

    /// <inheritdoc cref="NativeLibrary.MkExistsConst" />
    public IntPtr MkExistsConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = nativeLibrary.MkExistsConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkExistsConst));
    }

    /// <inheritdoc cref="NativeLibrary.MkPattern" />
    public IntPtr MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var result = nativeLibrary.MkPattern(ctx, numPatterns, terms);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkPattern));
    }

    // Function declaration and application operations
    /// <summary>
    ///     Creates a function declaration with the given name, domain, and range.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="name">Name of the function.</param>
    /// <param name="domainSize">Number of arguments.</param>
    /// <param name="domain">Array of argument sorts.</param>
    /// <param name="range">Return sort.</param>
    /// <returns>Function declaration handle.</returns>
    public IntPtr MkFuncDecl(IntPtr ctx, string name, uint domainSize, IntPtr[] domain, IntPtr range)
    {
        using var strPtr = new AnsiStringPtr(name);

        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        var result = nativeLibrary.MkFuncDecl(ctx, symbol, domainSize, domain, range);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkFuncDecl));
    }

    /// <inheritdoc cref="NativeLibrary.MkApp" />
    public IntPtr MkApp(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args)
    {
        var result = nativeLibrary.MkApp(ctx, funcDecl, numArgs, args);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkApp));
    }

    // Solver operations
    /// <inheritdoc cref="NativeLibrary.MkSolver" />
    public IntPtr MkSolver(IntPtr ctx)
    {
        var result = nativeLibrary.MkSolver(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSolver));
    }

    /// <inheritdoc cref="NativeLibrary.MkSimpleSolver" />
    public IntPtr MkSimpleSolver(IntPtr ctx)
    {
        var result = nativeLibrary.MkSimpleSolver(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSimpleSolver));
    }

    /// <inheritdoc cref="NativeLibrary.SolverIncRef" />
    public void SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverIncRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.SolverDecRef" />
    public void SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverDecRef(ctx, solver);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.SolverAssert" />
    public void SolverAssert(IntPtr ctx, IntPtr solver, IntPtr expr)
    {
        nativeLibrary.SolverAssert(ctx, solver, expr);
        CheckError(ctx);
    }

    /// <summary>
    ///     Checks the satisfiability of the assertions in the solver.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="solver">Solver handle.</param>
    /// <returns>The satisfiability result (Satisfiable, Unsatisfiable, or Unknown).</returns>
    public Z3Status SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.SolverCheck(ctx, solver);
        CheckError(ctx);
        return (Z3BoolValue)result switch
        {
            Z3BoolValue.False => Z3Status.Unsatisfiable,
            Z3BoolValue.True => Z3Status.Satisfiable,
            Z3BoolValue.Undefined => Z3Status.Unknown,
            _ => throw new InvalidOperationException($"Unexpected boolean value result {result} from Z3_solver_check"),
        };
    }

    /// <inheritdoc cref="NativeLibrary.SolverPush" />
    public void SolverPush(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverPush(ctx, solver);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.SolverPop" />
    public void SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        nativeLibrary.SolverPop(ctx, solver, numScopes);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.SolverGetModel" />
    public IntPtr SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.SolverGetModel(ctx, solver);
        CheckError(ctx);
        return CheckHandle(result, nameof(SolverGetModel));
    }

    // Model operations
    /// <inheritdoc cref="NativeLibrary.ModelIncRef" />
    public void ModelIncRef(IntPtr ctx, IntPtr model)
    {
        nativeLibrary.ModelIncRef(ctx, model);
        // No error check needed for ref counting
    }

    /// <inheritdoc cref="NativeLibrary.ModelDecRef" />
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

    /// <inheritdoc cref="NativeLibrary.ModelEval" />
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

    /// <inheritdoc cref="NativeLibrary.IsNumeralAst" />
    public bool IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.IsNumeralAst(ctx, expr);
        CheckError(ctx);
        return result;
    }

    /// <inheritdoc cref="NativeLibrary.GetSort" />
    public IntPtr GetSort(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.GetSort(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(GetSort));
    }

    /// <inheritdoc cref="NativeLibrary.GetSortKind" />
    public Z3SortKind GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetSortKind(ctx, sort);
        CheckError(ctx);
        return (Z3SortKind)result;
    }

    // Array operations
    /// <inheritdoc cref="NativeLibrary.MkConstArray" />
    public IntPtr MkConstArray(IntPtr ctx, IntPtr sort, IntPtr value)
    {
        var result = nativeLibrary.MkConstArray(ctx, sort, value);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkConstArray));
    }

    /// <inheritdoc cref="NativeLibrary.MkStore" />
    public IntPtr MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var result = nativeLibrary.MkStore(ctx, array, index, value);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkStore));
    }

    /// <inheritdoc cref="NativeLibrary.MkSelect" />
    public IntPtr MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var result = nativeLibrary.MkSelect(ctx, array, index);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSelect));
    }

    /// <inheritdoc cref="NativeLibrary.GetArraySortDomain" />
    public IntPtr GetArraySortDomain(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetArraySortDomain(ctx, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(GetArraySortDomain));
    }

    /// <inheritdoc cref="NativeLibrary.GetArraySortRange" />
    public IntPtr GetArraySortRange(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetArraySortRange(ctx, sort);
        CheckError(ctx);
        return CheckHandle(result, nameof(GetArraySortRange));
    }

    // BitVector operations
    /// <inheritdoc cref="NativeLibrary.MkBvAdd" />
    public IntPtr MkBvAdd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAdd(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAdd));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSub" />
    public IntPtr MkBvSub(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSub(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSub));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvMul" />
    public IntPtr MkBvMul(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvMul(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMul));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSDiv" />
    public IntPtr MkBvSDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSDiv(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSDiv));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvUDiv" />
    public IntPtr MkBvUDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUDiv(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUDiv));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSRem" />
    public IntPtr MkBvSRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSRem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSRem));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvURem" />
    public IntPtr MkBvURem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvURem(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvURem));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSMod" />
    public IntPtr MkBvSMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSMod(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSMod));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvNeg" />
    public IntPtr MkBvNeg(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNeg(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNeg));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvAnd" />
    public IntPtr MkBvAnd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAnd(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAnd));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvOr" />
    public IntPtr MkBvOr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvOr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvOr));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvXor" />
    public IntPtr MkBvXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvXor(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvXor));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvNot" />
    public IntPtr MkBvNot(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNot(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNot));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvShl" />
    public IntPtr MkBvShl(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvShl(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvShl));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvAShr" />
    public IntPtr MkBvAShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAShr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAShr));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvLShr" />
    public IntPtr MkBvLShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvLShr(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvLShr));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSLt" />
    public IntPtr MkBvSLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSLt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvULt" />
    public IntPtr MkBvULt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvULt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSLe" />
    public IntPtr MkBvSLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSLe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSLe));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvULe" />
    public IntPtr MkBvULe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvULe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvULe));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSGt" />
    public IntPtr MkBvSGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSGt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvUGt" />
    public IntPtr MkBvUGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUGt(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUGt));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSGe" />
    public IntPtr MkBvSGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSGe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSGe));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvUGe" />
    public IntPtr MkBvUGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvUGe(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvUGe));
    }

    // BitVector functions
    /// <inheritdoc cref="NativeLibrary.MkSignExt" />
    public IntPtr MkSignExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.MkSignExt(ctx, extra, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkSignExt));
    }

    /// <inheritdoc cref="NativeLibrary.MkZeroExt" />
    public IntPtr MkZeroExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = nativeLibrary.MkZeroExt(ctx, extra, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkZeroExt));
    }

    /// <inheritdoc cref="NativeLibrary.MkExtract" />
    public IntPtr MkExtract(IntPtr ctx, uint high, uint low, IntPtr expr)
    {
        var result = nativeLibrary.MkExtract(ctx, high, low, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkExtract));
    }

    /// <inheritdoc cref="NativeLibrary.MkRepeat" />
    public IntPtr MkRepeat(IntPtr ctx, uint count, IntPtr expr)
    {
        var result = nativeLibrary.MkRepeat(ctx, count, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkRepeat));
    }

    /// <inheritdoc cref="NativeLibrary.GetBvSortSize" />
    public uint GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var result = nativeLibrary.GetBvSortSize(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // BitVector overflow checks
    /// <inheritdoc cref="NativeLibrary.MkBvAddNoOverflow" />
    public IntPtr MkBvAddNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvAddNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSubNoOverflow" />
    public IntPtr MkBvSubNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSubNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSubNoUnderflow" />
    public IntPtr MkBvSubNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvSubNoUnderflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSubNoUnderflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvMulNoOverflow" />
    public IntPtr MkBvMulNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = nativeLibrary.MkBvMulNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvMulNoUnderflow" />
    public IntPtr MkBvMulNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvMulNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvMulNoUnderflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvAddNoUnderflow" />
    public IntPtr MkBvAddNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvAddNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvAddNoUnderflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvSDivNoOverflow" />
    public IntPtr MkBvSDivNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = nativeLibrary.MkBvSDivNoOverflow(ctx, left, right);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvSDivNoOverflow));
    }

    /// <inheritdoc cref="NativeLibrary.MkBvNegNoOverflow" />
    public IntPtr MkBvNegNoOverflow(IntPtr ctx, IntPtr expr)
    {
        var result = nativeLibrary.MkBvNegNoOverflow(ctx, expr);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkBvNegNoOverflow));
    }

    // Solver operations - add missing methods
    /// <summary>
    ///     Gets the reason why the solver returned unknown status.
    /// </summary>
    /// <param name="ctx">Z3 context.</param>
    /// <param name="solver">Solver handle.</param>
    /// <returns>String describing the reason for unknown status, or null if not available.</returns>
    public string? SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var result = nativeLibrary.SolverGetReasonUnknown(ctx, solver);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(SolverGetReasonUnknown)));
    }

    /// <inheritdoc cref="NativeLibrary.SolverReset" />
    public void SolverReset(IntPtr ctx, IntPtr solver)
    {
        nativeLibrary.SolverReset(ctx, solver);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.SolverSetParams" />
    public void SolverSetParams(IntPtr ctx, IntPtr solver, IntPtr paramsHandle)
    {
        nativeLibrary.SolverSetParams(ctx, solver, paramsHandle);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.MkParams" />
    public IntPtr MkParams(IntPtr ctx)
    {
        var result = nativeLibrary.MkParams(ctx);
        CheckError(ctx);
        return CheckHandle(result, nameof(MkParams));
    }

    /// <inheritdoc cref="NativeLibrary.ParamsIncRef" />
    public void ParamsIncRef(IntPtr ctx, IntPtr paramsHandle)
    {
        nativeLibrary.ParamsIncRef(ctx, paramsHandle);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.ParamsDecRef" />
    public void ParamsDecRef(IntPtr ctx, IntPtr paramsHandle)
    {
        nativeLibrary.ParamsDecRef(ctx, paramsHandle);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets a boolean parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The boolean value to set.</param>
    public void ParamsSetBool(IntPtr ctx, IntPtr paramsHandle, string name, bool value)
    {
        using var strPtr = new AnsiStringPtr(name);
        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetBool(ctx, paramsHandle, symbol, value ? 1 : 0);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets an unsigned integer parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The unsigned integer value to set.</param>
    public void ParamsSetUInt(IntPtr ctx, IntPtr paramsHandle, string name, uint value)
    {
        using var strPtr = new AnsiStringPtr(name);
        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetUInt(ctx, paramsHandle, symbol, value);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets a double parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The double value to set.</param>
    public void ParamsSetDouble(IntPtr ctx, IntPtr paramsHandle, string name, double value)
    {
        using var strPtr = new AnsiStringPtr(name);
        var symbol = nativeLibrary.MkStringSymbol(ctx, strPtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetDouble(ctx, paramsHandle, symbol, value);
        CheckError(ctx);
    }

    /// <summary>
    ///     Sets a symbol parameter in the parameter set.
    /// </summary>
    /// <param name="ctx">The Z3 context.</param>
    /// <param name="paramsHandle">The parameter set handle.</param>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The symbol value as a string.</param>
    public void ParamsSetSymbol(IntPtr ctx, IntPtr paramsHandle, string name, string value)
    {
        using var namePtr = new AnsiStringPtr(name);
        var nameSymbol = nativeLibrary.MkStringSymbol(ctx, namePtr);
        CheckError(ctx);

        using var valuePtr = new AnsiStringPtr(value);
        var valueSymbol = nativeLibrary.MkStringSymbol(ctx, valuePtr);
        CheckError(ctx);

        nativeLibrary.ParamsSetSymbol(ctx, paramsHandle, nameSymbol, valueSymbol);
        CheckError(ctx);
    }

    /// <inheritdoc cref="NativeLibrary.ParamsToString" />
    public string? ParamsToString(IntPtr ctx, IntPtr paramsHandle)
    {
        var result = nativeLibrary.ParamsToString(ctx, paramsHandle);
        CheckError(ctx);
        return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(ParamsToString)));
    }

    private IntPtr CheckHandle(IntPtr handle, string methodName)
    {
        if (handle == IntPtr.Zero)
            throw new InvalidOperationException($"{methodName} returned null handle");

        return handle;
    }

    private void CheckError(IntPtr ctx)
    {
        var z3ErrorCode = nativeLibrary.GetErrorCode(ctx);
        if (z3ErrorCode == Z3ErrorCode.Ok)
            return;
        var msgPtr = nativeLibrary.GetErrorMsg(ctx, z3ErrorCode);
        var message = Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
        throw new Z3Exception(z3ErrorCode, message);
    }

    private void OnZ3ErrorSafe(IntPtr ctx, int errorCode)
    {
        // DO NOT THROW EXCEPTIONS HERE - this is called from native Z3 code!
        var z3ErrorCode = (Z3ErrorCode)errorCode;
        var msgPtr = nativeLibrary.GetErrorMsg(ctx, z3ErrorCode);
        var message = Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown error";
        Debug.WriteLine($"Z3 Error: {z3ErrorCode}: {message}");
    }
}
