namespace Spaceorc.Z3Wrap.Core.Interop;

internal static class SafeNativeMethods
{
    // Configuration and context methods
    internal static IntPtr Z3MkConfig() => NativeMethods.Z3MkConfig();

    internal static void Z3DelConfig(IntPtr cfg) => NativeMethods.Z3DelConfig(cfg);

    internal static IntPtr Z3MkContextRc(IntPtr cfg)
    {
        var result = NativeMethods.Z3MkContextRc(cfg);
        if (result != IntPtr.Zero)
        {
            // No error check for context creation
            // Set up safe error handler (prevents crashes)
            NativeMethods.Z3SetErrorHandler(result, OnZ3ErrorSafe);
        }
        return result;
    }

    internal static void Z3DelContext(IntPtr ctx)
    {
        NativeMethods.Z3DelContext(ctx);
        // No error check needed for deletion
    }

    internal static void Z3UpdateParamValue(IntPtr ctx, IntPtr paramId, IntPtr paramValue)
    {
        NativeMethods.Z3UpdateParamValue(ctx, paramId, paramValue);
        CheckError(ctx);
    }

    // Reference counting
    internal static void Z3IncRef(IntPtr ctx, IntPtr ast)
    {
        NativeMethods.Z3IncRef(ctx, ast);
        // No error check needed for ref counting
    }

    internal static void Z3DecRef(IntPtr ctx, IntPtr ast)
    {
        NativeMethods.Z3DecRef(ctx, ast);
        // No error check needed for ref counting
    }

    // Symbol creation
    internal static IntPtr Z3MkStringSymbol(IntPtr ctx, IntPtr str)
    {
        var result = NativeMethods.Z3MkStringSymbol(ctx, str);
        CheckError(ctx);
        return result;
    }

    // Sort creation
    internal static IntPtr Z3MkBoolSort(IntPtr ctx)
    {
        var result = NativeMethods.Z3MkBoolSort(ctx);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkIntSort(IntPtr ctx)
    {
        var result = NativeMethods.Z3MkIntSort(ctx);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkRealSort(IntPtr ctx)
    {
        var result = NativeMethods.Z3MkRealSort(ctx);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSort(IntPtr ctx, uint size)
    {
        var result = NativeMethods.Z3MkBvSort(ctx, size);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkArraySort(IntPtr ctx, IntPtr indexSort, IntPtr valueSort)
    {
        var result = NativeMethods.Z3MkArraySort(ctx, indexSort, valueSort);
        CheckError(ctx);
        return result;
    }

    // Expression creation
    internal static IntPtr Z3MkConst(IntPtr ctx, IntPtr symbol, IntPtr sort)
    {
        var result = NativeMethods.Z3MkConst(ctx, symbol, sort);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkTrue(IntPtr ctx)
    {
        var result = NativeMethods.Z3MkTrue(ctx);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkFalse(IntPtr ctx)
    {
        var result = NativeMethods.Z3MkFalse(ctx);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkNumeral(IntPtr ctx, IntPtr numeral, IntPtr sort)
    {
        var result = NativeMethods.Z3MkNumeral(ctx, numeral, sort);
        CheckError(ctx);
        return result;
    }

    // Boolean operations
    internal static IntPtr Z3MkAnd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = NativeMethods.Z3MkAnd(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkOr(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = NativeMethods.Z3MkOr(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkNot(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3MkNot(ctx, expr);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkImplies(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = NativeMethods.Z3MkImplies(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkIff(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = NativeMethods.Z3MkIff(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkXor(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = NativeMethods.Z3MkXor(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkIte(IntPtr ctx, IntPtr condition, IntPtr thenExpr, IntPtr elseExpr)
    {
        var result = NativeMethods.Z3MkIte(ctx, condition, thenExpr, elseExpr);
        CheckError(ctx);
        return result;
    }

    // Arithmetic operations
    internal static IntPtr Z3MkAdd(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = NativeMethods.Z3MkAdd(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkSub(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = NativeMethods.Z3MkSub(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkMul(IntPtr ctx, uint numArgs, IntPtr[] args)
    {
        var result = NativeMethods.Z3MkMul(ctx, numArgs, args);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkDiv(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = NativeMethods.Z3MkDiv(ctx, arg1, arg2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkMod(IntPtr ctx, IntPtr arg1, IntPtr arg2)
    {
        var result = NativeMethods.Z3MkMod(ctx, arg1, arg2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkUnaryMinus(IntPtr ctx, IntPtr arg)
    {
        var result = NativeMethods.Z3MkUnaryMinus(ctx, arg);
        CheckError(ctx);
        return result;
    }

    // Comparison operations
    internal static IntPtr Z3MkEq(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkEq(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkLt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = NativeMethods.Z3MkLt(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkLe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = NativeMethods.Z3MkLe(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkGt(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = NativeMethods.Z3MkGt(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkGe(IntPtr ctx, IntPtr t1, IntPtr t2)
    {
        var result = NativeMethods.Z3MkGe(ctx, t1, t2);
        CheckError(ctx);
        return result;
    }

    // Type conversions
    internal static IntPtr Z3MkInt2Real(IntPtr ctx, IntPtr term)
    {
        var result = NativeMethods.Z3MkInt2Real(ctx, term);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkReal2Int(IntPtr ctx, IntPtr term)
    {
        var result = NativeMethods.Z3MkReal2Int(ctx, term);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkInt2Bv(IntPtr ctx, uint size, IntPtr term)
    {
        var result = NativeMethods.Z3MkInt2Bv(ctx, size, term);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBv2Int(IntPtr ctx, IntPtr term, bool isSigned)
    {
        var result = NativeMethods.Z3MkBv2Int(ctx, term, isSigned);
        CheckError(ctx);
        return result;
    }

    // Quantifier operations
    internal static IntPtr Z3MkForallConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = NativeMethods.Z3MkForallConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkExistsConst(
        IntPtr ctx,
        uint weight,
        uint numBound,
        IntPtr[] bound,
        uint numPatterns,
        IntPtr[] patterns,
        IntPtr body
    )
    {
        var result = NativeMethods.Z3MkExistsConst(ctx, weight, numBound, bound, numPatterns, patterns, body);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkPattern(IntPtr ctx, uint numPatterns, IntPtr[] terms)
    {
        var result = NativeMethods.Z3MkPattern(ctx, numPatterns, terms);
        CheckError(ctx);
        return result;
    }

    // Function declaration and application operations
    internal static IntPtr Z3MkFuncDecl(IntPtr ctx, IntPtr symbol, uint domainSize, IntPtr[] domain, IntPtr range)
    {
        var result = NativeMethods.Z3MkFuncDecl(ctx, symbol, domainSize, domain, range);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkApp(IntPtr ctx, IntPtr funcDecl, uint numArgs, IntPtr[] args)
    {
        var result = NativeMethods.Z3MkApp(ctx, funcDecl, numArgs, args);
        CheckError(ctx);
        return result;
    }

    // Solver operations
    internal static IntPtr Z3MkSolver(IntPtr ctx)
    {
        var result = NativeMethods.Z3MkSolver(ctx);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkSimpleSolver(IntPtr ctx)
    {
        var result = NativeMethods.Z3MkSimpleSolver(ctx);
        CheckError(ctx);
        return result;
    }

    internal static void Z3SolverIncRef(IntPtr ctx, IntPtr solver)
    {
        NativeMethods.Z3SolverIncRef(ctx, solver);
        // No error check needed for ref counting
    }

    internal static void Z3SolverDecRef(IntPtr ctx, IntPtr solver)
    {
        NativeMethods.Z3SolverDecRef(ctx, solver);
        // No error check needed for ref counting
    }

    internal static void Z3SolverAssert(IntPtr ctx, IntPtr solver, IntPtr expr)
    {
        NativeMethods.Z3SolverAssert(ctx, solver, expr);
        CheckError(ctx);
    }

    internal static int Z3SolverCheck(IntPtr ctx, IntPtr solver)
    {
        var result = NativeMethods.Z3SolverCheck(ctx, solver);
        CheckError(ctx);
        return result;
    }

    internal static void Z3SolverPush(IntPtr ctx, IntPtr solver)
    {
        NativeMethods.Z3SolverPush(ctx, solver);
        CheckError(ctx);
    }

    internal static void Z3SolverPop(IntPtr ctx, IntPtr solver, uint numScopes)
    {
        NativeMethods.Z3SolverPop(ctx, solver, numScopes);
        CheckError(ctx);
    }

    internal static IntPtr Z3SolverGetModel(IntPtr ctx, IntPtr solver)
    {
        var result = NativeMethods.Z3SolverGetModel(ctx, solver);
        CheckError(ctx);
        return result;
    }

    // Model operations
    internal static void Z3ModelIncRef(IntPtr ctx, IntPtr model)
    {
        NativeMethods.Z3ModelIncRef(ctx, model);
        // No error check needed for ref counting
    }

    internal static void Z3ModelDecRef(IntPtr ctx, IntPtr model)
    {
        NativeMethods.Z3ModelDecRef(ctx, model);
        // No error check needed for ref counting
    }

    internal static IntPtr Z3ModelToString(IntPtr ctx, IntPtr model)
    {
        var result = NativeMethods.Z3ModelToString(ctx, model);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3AstToString(IntPtr ctx, IntPtr ast)
    {
        var result = NativeMethods.Z3AstToString(ctx, ast);
        CheckError(ctx);
        return result;
    }

    internal static bool Z3ModelEval(IntPtr ctx, IntPtr model, IntPtr expr, bool modelCompletion, out IntPtr result)
    {
        var returnValue = NativeMethods.Z3ModelEval(ctx, model, expr, modelCompletion, out result);
        CheckError(ctx);
        return returnValue;
    }

    internal static IntPtr Z3GetNumeralString(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3GetNumeralString(ctx, expr);
        CheckError(ctx);
        return result;
    }

    internal static int Z3GetBoolValue(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3GetBoolValue(ctx, expr);
        CheckError(ctx);
        return result;
    }

    internal static bool Z3IsNumeralAst(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3IsNumeralAst(ctx, expr);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3GetSort(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3GetSort(ctx, expr);
        CheckError(ctx);
        return result;
    }

    internal static int Z3GetSortKind(IntPtr ctx, IntPtr sort)
    {
        var result = NativeMethods.Z3GetSortKind(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // Array operations
    internal static IntPtr Z3MkConstArray(IntPtr ctx, IntPtr sort, IntPtr value)
    {
        var result = NativeMethods.Z3MkConstArray(ctx, sort, value);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkStore(IntPtr ctx, IntPtr array, IntPtr index, IntPtr value)
    {
        var result = NativeMethods.Z3MkStore(ctx, array, index, value);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkSelect(IntPtr ctx, IntPtr array, IntPtr index)
    {
        var result = NativeMethods.Z3MkSelect(ctx, array, index);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3GetArraySortDomain(IntPtr ctx, IntPtr sort)
    {
        var result = NativeMethods.Z3GetArraySortDomain(ctx, sort);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3GetArraySortRange(IntPtr ctx, IntPtr sort)
    {
        var result = NativeMethods.Z3GetArraySortRange(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // BitVector operations
    internal static IntPtr Z3MkBvAdd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvAdd(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSub(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSub(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvMul(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvMul(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSDiv(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvUDiv(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvUDiv(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSRem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSRem(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvURem(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvURem(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSMod(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSMod(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvNeg(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3MkBvNeg(ctx, expr);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvAnd(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvAnd(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvOr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvOr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvXor(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvXor(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvNot(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3MkBvNot(ctx, expr);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvShl(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvShl(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvAShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvAShr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvLShr(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvLShr(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSLt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSLt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvULt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvULt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSLe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSLe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvULe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvULe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSGt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvUGt(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvUGt(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSGe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvUGe(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvUGe(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    // BitVector functions
    internal static IntPtr Z3MkSignExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = NativeMethods.Z3MkSignExt(ctx, extra, expr);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkZeroExt(IntPtr ctx, uint extra, IntPtr expr)
    {
        var result = NativeMethods.Z3MkZeroExt(ctx, extra, expr);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkExtract(IntPtr ctx, uint high, uint low, IntPtr expr)
    {
        var result = NativeMethods.Z3MkExtract(ctx, high, low, expr);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkRepeat(IntPtr ctx, uint count, IntPtr expr)
    {
        var result = NativeMethods.Z3MkRepeat(ctx, count, expr);
        CheckError(ctx);
        return result;
    }

    internal static uint Z3GetBvSortSize(IntPtr ctx, IntPtr sort)
    {
        var result = NativeMethods.Z3GetBvSortSize(ctx, sort);
        CheckError(ctx);
        return result;
    }

    // BitVector overflow checks
    internal static IntPtr Z3MkBvAddNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = NativeMethods.Z3MkBvAddNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSubNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSubNoOverflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSubNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = NativeMethods.Z3MkBvSubNoUnderflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvMulNoOverflow(IntPtr ctx, IntPtr left, IntPtr right, bool isSigned)
    {
        var result = NativeMethods.Z3MkBvMulNoOverflow(ctx, left, right, isSigned);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvMulNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvMulNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvAddNoUnderflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvAddNoUnderflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvSDivNoOverflow(IntPtr ctx, IntPtr left, IntPtr right)
    {
        var result = NativeMethods.Z3MkBvSDivNoOverflow(ctx, left, right);
        CheckError(ctx);
        return result;
    }

    internal static IntPtr Z3MkBvNegNoOverflow(IntPtr ctx, IntPtr expr)
    {
        var result = NativeMethods.Z3MkBvNegNoOverflow(ctx, expr);
        CheckError(ctx);
        return result;
    }

    // Solver operations - add missing methods
    internal static IntPtr Z3SolverGetReasonUnknown(IntPtr ctx, IntPtr solver)
    {
        var result = NativeMethods.Z3SolverGetReasonUnknown(ctx, solver);
        CheckError(ctx);
        return result;
    }

    internal static void Z3SolverReset(IntPtr ctx, IntPtr solver)
    {
        NativeMethods.Z3SolverReset(ctx, solver);
        CheckError(ctx);
    }

    private static void CheckError(IntPtr ctx)
    {
        var errorCode = NativeMethods.Z3GetErrorCode(ctx);
        if (errorCode == Z3ErrorCode.Ok)
            return;
        var message = NativeMethods.Z3GetErrorMsg(ctx, errorCode);
        throw new Z3Exception(errorCode, message);
    }

    private static void OnZ3ErrorSafe(IntPtr ctx, int errorCode)
    {
        // DO NOT THROW EXCEPTIONS HERE - this is called from native Z3 code!
        var z3ErrorCode = (Z3ErrorCode)errorCode;
        var message = NativeMethods.Z3GetErrorMsg(ctx, z3ErrorCode);
        System.Diagnostics.Debug.WriteLine($"Z3 Error: {z3ErrorCode}: {message}");
    }
}
