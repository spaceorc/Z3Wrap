using Spaceorc.Z3Wrap.Expressions.FloatingPoint;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Model
{
    /// <summary>
    /// Gets the Half value of a Float16 expression in this model.
    /// </summary>
    /// <param name="expr">The Float16 expression.</param>
    /// <returns>The Half value.</returns>
    public Half GetHalfValue(FpExpr<Float16> expr)
    {
        var str = GetFpStringValue(expr);

        // Try to parse as a normal float first
        if (Half.TryParse(str, out var result))
            return result;

        // Handle special IEEE values in Z3 format
        if (str.Contains("NaN") || str.Contains("nan"))
            return Half.NaN;
        if (str.Contains("+oo") || str.Contains("+infinity") || str == "oo")
            return Half.PositiveInfinity;
        if (str.Contains("-oo") || str.Contains("-infinity"))
            return Half.NegativeInfinity;
        if (str.Contains("+zero"))
            return (Half)0.0;
        if (str.Contains("-zero"))
            return BitConverter.UInt16BitsToHalf(0x8000); // -0.0

        // Handle IEEE bit representation: (fp #bS #bEEEEE #bSSSSSSSSS)
        if (str.StartsWith("(fp "))
        {
            var parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                var sign = ParseFpComponent(parts[1]);
                var exp = ParseFpComponent(parts[2]);
                var sig = ParseFpComponent(parts[3].TrimEnd(')'));

                var bits = (ushort)((sign << 15) | (exp << 10) | sig);
                return BitConverter.UInt16BitsToHalf(bits);
            }
        }

        throw new InvalidOperationException($"Failed to parse Float16 value '{str}' from expression {expr}");
    }

    /// <summary>
    /// Gets the float value of a Float32 expression in this model.
    /// </summary>
    /// <param name="expr">The Float32 expression.</param>
    /// <returns>The float value.</returns>
    public float GetFloatValue(FpExpr<Float32> expr)
    {
        var str = GetFpStringValue(expr);

        // Try to parse as a normal float first
        if (float.TryParse(str, out var result))
            return result;

        // Handle special IEEE values in Z3 format
        if (str.Contains("NaN") || str.Contains("nan"))
            return float.NaN;
        if (str.Contains("+oo") || str.Contains("+infinity") || str == "oo")
            return float.PositiveInfinity;
        if (str.Contains("-oo") || str.Contains("-infinity"))
            return float.NegativeInfinity;
        if (str.Contains("+zero"))
            return 0.0f;
        if (str.Contains("-zero"))
            return BitConverter.Int32BitsToSingle(unchecked((int)0x80000000)); // -0.0f

        // Handle IEEE bit representation: (fp #bS #xEE #bSSSSSSSSSSSSSSSSSSSSSSS)
        if (str.StartsWith("(fp "))
        {
            var parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                var sign = (uint)ParseFpComponent(parts[1]);
                var exp = (uint)ParseFpComponent(parts[2]);
                var sig = (uint)ParseFpComponent(parts[3].TrimEnd(')'));

                var bits = (uint)((sign << 31) | (exp << 23) | sig);
                return BitConverter.UInt32BitsToSingle(bits);
            }
        }

        throw new InvalidOperationException($"Failed to parse Float32 value '{str}' from expression {expr}");
    }

    /// <summary>
    /// Gets the double value of a Float64 expression in this model.
    /// </summary>
    /// <param name="expr">The Float64 expression.</param>
    /// <returns>The double value.</returns>
    public double GetDoubleValue(FpExpr<Float64> expr)
    {
        var str = GetFpStringValue(expr);

        // Try to parse as a normal double first
        if (double.TryParse(str, out var result))
            return result;

        // Handle special IEEE values in Z3 format
        if (str.Contains("NaN") || str.Contains("nan"))
            return double.NaN;
        if (str.Contains("+oo") || str.Contains("+infinity") || str == "oo")
            return double.PositiveInfinity;
        if (str.Contains("-oo") || str.Contains("-infinity"))
            return double.NegativeInfinity;
        if (str.Contains("+zero"))
            return 0.0;
        if (str.Contains("-zero"))
            return BitConverter.Int64BitsToDouble(unchecked((long)0x8000000000000000)); // -0.0

        // Handle IEEE bit representation: (fp #bS #bEEEEEEEEEEE #xSSSSSSSSSSSSS)
        if (str.StartsWith("(fp "))
        {
            var parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                var sign = ParseFpComponent(parts[1]);
                var exp = ParseFpComponent(parts[2]);
                var sig = ParseFpComponent(parts[3].TrimEnd(')'));

                var bits = (ulong)((sign << 63) | (exp << 52) | sig);
                return BitConverter.UInt64BitsToDouble(bits);
            }
        }

        throw new InvalidOperationException($"Failed to parse Float64 value '{str}' from expression {expr}");
    }

    /// <summary>
    /// Gets the string representation of a floating-point expression's value in this model.
    /// </summary>
    /// <typeparam name="TFormat">The floating-point format.</typeparam>
    /// <param name="expr">The floating-point expression.</param>
    /// <returns>The string representation of the value.</returns>
    public string GetFpStringValue<TFormat>(FpExpr<TFormat> expr)
        where TFormat : IFloatFormat
    {
        var evaluated = Evaluate(expr);
        return context.Library.AstToString(context.Handle, evaluated.Handle);
    }

    private static ulong ParseFpComponent(string component)
    {
        if (component.StartsWith("#b"))
            return Convert.ToUInt64(component[2..], 2);
        if (component.StartsWith("#x"))
            return Convert.ToUInt64(component[2..], 16);
        return Convert.ToUInt64(component);
    }
}
