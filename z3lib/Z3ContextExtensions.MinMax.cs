namespace z3lib;

public static partial class Z3ContextExtensions
{
    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Ite(context.Le(left, right), left, right);
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Ite(context.Le(left, right), left, right);
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, Z3IntExpr right) => context.Ite(context.Ge(left, right), left, right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Z3RealExpr right) => context.Ite(context.Ge(left, right), left, right);

    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, int right) => context.Min(left, context.Int(right));
    public static Z3IntExpr Min(this Z3Context context, int left, Z3IntExpr right) => context.Min(context.Int(left), right);
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, int right) => context.Max(left, context.Int(right));
    public static Z3IntExpr Max(this Z3Context context, int left, Z3IntExpr right) => context.Max(context.Int(left), right);

    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, double right) => context.Min(left, context.Real(right));
    public static Z3RealExpr Min(this Z3Context context, double left, Z3RealExpr right) => context.Min(context.Real(left), right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, double right) => context.Max(left, context.Real(right));
    public static Z3RealExpr Max(this Z3Context context, double left, Z3RealExpr right) => context.Max(context.Real(left), right);

    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, int right) => context.Min(left, context.Real(right));
    public static Z3RealExpr Min(this Z3Context context, int left, Z3RealExpr right) => context.Min(context.Real(left), right);
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, int right) => context.Max(left, context.Real(right));
    public static Z3RealExpr Max(this Z3Context context, int left, Z3RealExpr right) => context.Max(context.Real(left), right);
}