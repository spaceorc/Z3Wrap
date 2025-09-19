namespace Spaceorc.Z3Wrap.Expressions;

/// <summary>
/// Base class for all numeric expressions (integers, reals, and bitvectors) in Z3.
/// Provides common functionality for numeric types that can be evaluated to concrete values.
/// </summary>
public abstract class Z3NumericExpr(Z3Context context, IntPtr handle) : Z3Expr(context, handle) { }
