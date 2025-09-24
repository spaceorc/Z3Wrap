namespace Spaceorc.Z3Wrap.BitVectors;

/// <summary>
/// Represents an 8-bit bitvector size.
/// </summary>
public readonly struct Size8 : ISize
{
    /// <summary>
    /// Gets the bit width (8 bits).
    /// </summary>
    public static uint Value => 8;
}

/// <summary>
/// Represents a 16-bit bitvector size.
/// </summary>
public readonly struct Size16 : ISize
{
    /// <summary>
    /// Gets the bit width (16 bits).
    /// </summary>
    public static uint Value => 16;
}

/// <summary>
/// Represents a 32-bit bitvector size.
/// </summary>
public readonly struct Size32 : ISize
{
    /// <summary>
    /// Gets the bit width (32 bits).
    /// </summary>
    public static uint Value => 32;
}

/// <summary>
/// Represents a 64-bit bitvector size.
/// </summary>
public readonly struct Size64 : ISize
{
    /// <summary>
    /// Gets the bit width (64 bits).
    /// </summary>
    public static uint Value => 64;
}

/// <summary>
/// Represents a 128-bit bitvector size.
/// </summary>
public readonly struct Size128 : ISize
{
    /// <summary>
    /// Gets the bit width (128 bits).
    /// </summary>
    public static uint Value => 128;
}
