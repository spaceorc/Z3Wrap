namespace z3lib;

public static class Z3ContextExtensions
{
    // Min operations with all overloads
    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var condition = context.MkLe(left, right); // left <= right
        return (Z3IntExpr)context.MkIte(condition, left, right);
    }
    
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var condition = context.MkLe(left, right); // left <= right
        return (Z3RealExpr)context.MkIte(condition, left, right);
    }
    
    // Max operations with all overloads
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, Z3IntExpr right)
    {
        var condition = context.MkGe(left, right); // left >= right
        return (Z3IntExpr)context.MkIte(condition, left, right);
    }
    
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, Z3RealExpr right)
    {
        var condition = context.MkGe(left, right); // left >= right
        return (Z3RealExpr)context.MkIte(condition, left, right);
    }
    
    // Mixed-type Min operations (Z3IntExpr with int literal)
    public static Z3IntExpr Min(this Z3Context context, Z3IntExpr left, int right)
        => context.Min(left, context.MkInt(right));
    
    public static Z3IntExpr Min(this Z3Context context, int left, Z3IntExpr right)
        => context.Min(context.MkInt(left), right);
    
    // Mixed-type Max operations (Z3IntExpr with int literal)
    public static Z3IntExpr Max(this Z3Context context, Z3IntExpr left, int right)
        => context.Max(left, context.MkInt(right));
    
    public static Z3IntExpr Max(this Z3Context context, int left, Z3IntExpr right)
        => context.Max(context.MkInt(left), right);
    
    // Mixed-type Min operations (Z3RealExpr with double literal)
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, double right)
        => context.Min(left, context.MkReal(right));
    
    public static Z3RealExpr Min(this Z3Context context, double left, Z3RealExpr right)
        => context.Min(context.MkReal(left), right);
    
    // Mixed-type Max operations (Z3RealExpr with double literal)
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, double right)
        => context.Max(left, context.MkReal(right));
    
    public static Z3RealExpr Max(this Z3Context context, double left, Z3RealExpr right)
        => context.Max(context.MkReal(left), right);
    
    // Mixed-type Min operations (Z3RealExpr with int literal for convenience)
    public static Z3RealExpr Min(this Z3Context context, Z3RealExpr left, int right)
        => context.Min(left, context.MkReal(right));
    
    public static Z3RealExpr Min(this Z3Context context, int left, Z3RealExpr right)
        => context.Min(context.MkReal(left), right);
    
    // Mixed-type Max operations (Z3RealExpr with int literal for convenience)
    public static Z3RealExpr Max(this Z3Context context, Z3RealExpr left, int right)
        => context.Max(left, context.MkReal(right));
    
    public static Z3RealExpr Max(this Z3Context context, int left, Z3RealExpr right)
        => context.Max(context.MkReal(left), right);
}