namespace z3lib;

public static class Z3ContextExtensions
{
    public static Z3IntExpr Min(this Z3Context _, Z3IntExpr left, Z3IntExpr right) => (left <= right).If(left, right);
    public static Z3RealExpr Min(this Z3Context _, Z3RealExpr left, Z3RealExpr right) => (left <= right).If(left, right);
    public static Z3IntExpr Max(this Z3Context _, Z3IntExpr left, Z3IntExpr right) => (left >= right).If(left, right);
    public static Z3RealExpr Max(this Z3Context _, Z3RealExpr left, Z3RealExpr right) => (left >= right).If(left, right);
    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, int right) => context.Min(left, context.MkInt(right));
    public static Z3IntExpr Min(this Z3Context context, int left, Z3IntExpr right) => context.Min(context.MkInt(left), right);
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, int right) => context.Max(left, context.MkInt(right));
    public static Z3IntExpr Max(this Z3Context context, int left, Z3IntExpr right) => context.Max(context.MkInt(left), right);
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, double right) => context.Min(left, context.MkReal(right));
    public static Z3RealExpr Min(this Z3Context context, double left, Z3RealExpr right) => context.Min(context.MkReal(left), right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, double right) => context.Max(left, context.MkReal(right));
    public static Z3RealExpr Max(this Z3Context context, double left, Z3RealExpr right) => context.Max(context.MkReal(left), right);
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, int right) => context.Min(left, context.MkReal(right));
    public static Z3RealExpr Min(this Z3Context context, int left, Z3RealExpr right) => context.Min(context.MkReal(left), right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, int right) => context.Max(left, context.MkReal(right));
    public static Z3RealExpr Max(this Z3Context context, int left, Z3RealExpr right) => context.Max(context.MkReal(left), right);
}