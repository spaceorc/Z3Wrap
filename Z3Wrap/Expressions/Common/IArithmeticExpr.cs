namespace Spaceorc.Z3Wrap.Expressions.Common;

/// <summary>
/// Marker interface for expressions supporting basic arithmetic operations (Add, Sub, Mul, Div, UnaryMinus).
/// Implemented by numeric types that support standard mathematical operations in Z3.
/// </summary>
public interface IArithmeticExpr : INumericExpr { }