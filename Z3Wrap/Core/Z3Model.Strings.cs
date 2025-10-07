using System.Numerics;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Spaceorc.Z3Wrap.Core;

public sealed partial class Z3Model
{
    /// <summary>
    /// Gets the string value of a string expression in this model.
    /// </summary>
    /// <param name="expr">The string expression.</param>
    /// <returns>The string value.</returns>
    public string GetStringValue(StringExpr expr)
    {
        ThrowIfInvalidated();

        var evaluated = Evaluate(expr);

        if (!context.Library.IsString(context.Handle, evaluated.Handle))
            throw new InvalidOperationException(
                $"Expression {expr} does not evaluate to a string constant in this model"
            );

        return context.Library.GetString(context.Handle, evaluated.Handle);
    }

    /// <summary>
    /// Gets the character value (Unicode codepoint) of a character expression in this model.
    /// </summary>
    /// <param name="expr">The character expression.</param>
    /// <returns>The Unicode codepoint as an unsigned integer.</returns>
    public uint GetCharValue(CharExpr expr)
    {
        ThrowIfInvalidated();

        var evaluated = Evaluate(expr);

        // Characters in Z3 models are represented as character literals in format: "(_ Char 65)"
        var astString = context.Library.AstToString(context.Handle, evaluated.Handle);

        // Parse the format "(_ Char <codepoint>)" or just "<codepoint>"
        string codepointStr;
        if (astString.StartsWith("(_ Char ") && astString.EndsWith(")"))
        {
            // Extract codepoint from "(_ Char 65)"
            codepointStr = astString.Substring(8, astString.Length - 9).Trim();
        }
        else
        {
            // Try direct parse
            codepointStr = astString;
        }

        if (!BigInteger.TryParse(codepointStr, out var codepoint))
            throw new InvalidOperationException(
                $"Failed to parse character codepoint from '{astString}' (extracted: '{codepointStr}') for expression {expr}"
            );

        if (codepoint < 0 || codepoint > 0x10FFFF)
            throw new InvalidOperationException(
                $"Invalid Unicode codepoint {codepoint} for character expression {expr}"
            );

        return (uint)codepoint;
    }
}
