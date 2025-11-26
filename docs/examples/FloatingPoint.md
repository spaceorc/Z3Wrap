# IEEE 754 Floating-Point Arithmetic

Floating-point constraints allow you to verify numerical algorithms, validate hardware FPU implementations, and analyze floating-point precision issues. Z3Wrap provides full IEEE 754 support with type-safe expressions for Half (16-bit), Single (32-bit), and Double (64-bit) formats.

## Overview

Z3Wrap provides IEEE 754 floating-point arithmetic with:

- **Three formats**: Float16 (Half), Float32 (Single), Float64 (Double)
- **All rounding modes**: NearestTiesToEven, NearestTiesToAway, TowardPositive, TowardNegative, TowardZero
- **Full arithmetic**: +, -, *, /, %, Neg, Abs, Sqrt, Fma, Min, Max, RoundToIntegral
- **Comparisons**: <, <=, >, >=, ==, !=
- **Predicates**: IsNaN, IsInfinite, IsZero, IsNormal, IsSubnormal, IsNegative, IsPositive
- **Special values**: NaN, ±Infinity, ±Zero
- **Conversions**: Between formats and to/from Real

## Basic Arithmetic

### Example: Simple Floating-Point Constraints

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Create floating-point constants
var x = context.FpConst<Float32>("x");
var y = context.FpConst<Float32>("y");

// Add constraints using natural operators
solver.Assert(x > context.Fp(0.0f));
solver.Assert(y > context.Fp(0.0f));
solver.Assert(x + y == context.Fp(10.0f));
solver.Assert(x * context.Fp(2.0f) == y);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetFloatValue(x)}");  // x = 3.333...
    Console.WriteLine($"y = {model.GetFloatValue(y)}");  // y = 6.666...
}
```

## Rounding Modes

### Example: Demonstrating Rounding Mode Effects

Different rounding modes can produce different results:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var x = context.Fp(1.5f);
var y = context.Fp(2.7f);

// Different rounding modes for same operation
var rmNearestEven = context.RoundingMode(RoundingMode.NearestTiesToEven);
var rmTowardZero = context.RoundingMode(RoundingMode.TowardZero);
var rmTowardPositive = context.RoundingMode(RoundingMode.TowardPositive);

var sumNearestEven = x.Add(y, rmNearestEven);
var sumTowardZero = x.Add(y, rmTowardZero);
var sumTowardPositive = x.Add(y, rmTowardPositive);

// Round to integral values
var roundedNearestEven = sumNearestEven.RoundToIntegral(rmNearestEven);
var roundedTowardZero = sumTowardZero.RoundToIntegral(rmTowardZero);
var roundedTowardPositive = sumTowardPositive.RoundToIntegral(rmTowardPositive);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Nearest even: {model.GetFloatValue(roundedNearestEven)}");    // 4.0
    Console.WriteLine($"Toward zero: {model.GetFloatValue(roundedTowardZero)}");      // 4.0
    Console.WriteLine($"Toward +∞: {model.GetFloatValue(roundedTowardPositive)}");    // 5.0
}
```

## Numerical Precision Issues

### Example: Detecting Catastrophic Cancellation

Verify when floating-point operations lose precision:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var x = context.FpConst<Float32>("x");
var y = context.FpConst<Float32>("y");

// Catastrophic cancellation: (x + y) - x should equal y
// But with floats, it might not!
var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);
var result = x.Add(y, rm).Sub(x, rm);

// Find cases where precision is lost
solver.Assert(result != y);
solver.Assert(x.IsNormal());  // Exclude special values
solver.Assert(y.IsNormal());

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var xVal = model.GetFloatValue(x);
    var yVal = model.GetFloatValue(y);
    var resultVal = model.GetFloatValue(result);

    Console.WriteLine($"Found precision loss:");
    Console.WriteLine($"  x = {xVal}");
    Console.WriteLine($"  y = {yVal}");
    Console.WriteLine($"  (x + y) - x = {resultVal}");
    Console.WriteLine($"  Expected: {yVal}");
    Console.WriteLine($"  Error: {Math.Abs(resultVal - yVal)}");
}
```

## Special Values

### Example: NaN Propagation and Infinity Arithmetic

IEEE 754 defines special behaviors for NaN and infinity:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// NaN propagation: any operation with NaN returns NaN
var nan = context.FpNaN<Float32>();
var x = context.Fp(5.0f);
var result = x + nan;

solver.Assert(result.IsNaN());

// Infinity arithmetic
var posInf = context.FpInfinity<Float32>(negative: false);
var negInf = context.FpInfinity<Float32>(negative: true);

var infSum = posInf + x;         // +∞ + x = +∞
var infDiff = posInf + negInf;   // +∞ + (-∞) = NaN!

solver.Assert(infSum.IsInfinite());
solver.Assert(infSum.IsPositive());
solver.Assert(infDiff.IsNaN());

// Signed zeros
var posZero = context.FpZero<Float32>(negative: false);
var negZero = context.FpZero<Float32>(negative: true);

solver.Assert(posZero == negZero);  // +0 == -0 is true
solver.Assert((context.Fp(1.0f) / posZero).IsInfinite());  // 1/+0 = +∞
solver.Assert((context.Fp(1.0f) / negZero).IsInfinite());  // 1/-0 = -∞

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("All IEEE 754 special value behaviors verified!");
}
```

## Format Conversions

### Example: Precision Loss When Converting Formats

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Start with a precise double value
var doubleVal = context.Fp(Math.PI);  // 3.141592653589793

// Convert to float (loses precision)
var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);
var floatVal = doubleVal.ToFloat32(rm);

// Convert back to double
var doubleAgain = floatVal.ToFloat64(rm);

// Verify precision was lost
solver.Assert(doubleVal != doubleAgain);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Original:  {model.GetDoubleValue(doubleVal):F16}");
    Console.WriteLine($"As float:  {model.GetFloatValue(floatVal):F16}");
    Console.WriteLine($"Back to double: {model.GetDoubleValue(doubleAgain):F16}");
}
```

## Hardware FPU Verification

### Example: Verify Fused Multiply-Add

Verify that FMA (fused multiply-add) differs from separate operations:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var a = context.FpConst<Float32>("a");
var b = context.FpConst<Float32>("b");
var c = context.FpConst<Float32>("c");

var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

// IEEE 754 fused multiply-add (single rounding)
var fmaResult = a.Fma(b, c, rm);

// Separate operations (two roundings)
var separateResult = a.Mul(b, rm).Add(c, rm);

// Find cases where they differ
solver.Assert(fmaResult != separateResult);
solver.Assert(a.IsNormal());
solver.Assert(b.IsNormal());
solver.Assert(c.IsNormal());

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var aVal = model.GetFloatValue(a);
    var bVal = model.GetFloatValue(b);
    var cVal = model.GetFloatValue(c);
    var fmaVal = model.GetFloatValue(fmaResult);
    var sepVal = model.GetFloatValue(separateResult);

    Console.WriteLine($"Found case where FMA differs:");
    Console.WriteLine($"  a = {aVal}, b = {bVal}, c = {cVal}");
    Console.WriteLine($"  FMA(a,b,c) = {fmaVal}");
    Console.WriteLine($"  (a*b)+c = {sepVal}");
    Console.WriteLine($"  Difference: {Math.Abs(fmaVal - sepVal)}");
}
```

## All Three Formats

### Example: Using Half, Float, and Double

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Half precision (16-bit) - limited range and precision
var half = context.Fp((Half)3.14f);

// Single precision (32-bit)
var single = context.Fp(3.14f);

// Double precision (64-bit)
var double64 = context.Fp(3.14159265358979);

var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

// Convert between formats
var halfToSingle = half.ToFloat32(rm);
var singleToDouble = single.ToFloat64(rm);
var doubleToSingle = double64.ToFloat32(rm);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Half:   {model.GetHalfValue(half)}");
    Console.WriteLine($"Single: {model.GetFloatValue(single)}");
    Console.WriteLine($"Double: {model.GetDoubleValue(double64)}");
    Console.WriteLine();
    Console.WriteLine($"Half→Single: {model.GetFloatValue(halfToSingle)}");
    Console.WriteLine($"Single→Double: {model.GetDoubleValue(singleToDouble)}");
    Console.WriteLine($"Double→Single: {model.GetFloatValue(doubleToSingle)} (precision loss)");
}
```

## Conversion to Real

### Example: Converting Between Floating-Point and Real

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Float to Real conversion
var floatVal = context.Fp(3.5f);
var realVal = floatVal.ToReal();

// Real to Float conversion
var realExact = context.Real(new Spaceorc.Z3Wrap.Values.Numerics.Real(7, 2));  // 7/2 = 3.5
var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);
var backToFloat = context.FpFromReal<Float32>(realExact, rm);

solver.Assert(floatVal == backToFloat);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    var real = model.GetRealValue(realVal);
    Console.WriteLine($"Float: {model.GetFloatValue(floatVal)}");
    Console.WriteLine($"Real: {real.Numerator}/{real.Denominator} = {(double)real.Numerator / (double)real.Denominator}");
    Console.WriteLine($"Back to Float: {model.GetFloatValue(backToFloat)}");
}
```

## Practical Tips

### Choosing the Right Format

- **Float16 (Half)**: Graphics, ML inference, memory-constrained systems (range: ±65504, ~3 decimal digits)
- **Float32 (Single)**: Most numerical computations, graphics (range: ±3.4×10³⁸, ~7 decimal digits)
- **Float64 (Double)**: High-precision scientific computing (range: ±1.8×10³⁰⁸, ~15 decimal digits)

### Rounding Mode Selection

- **NearestTiesToEven** (default): Best for general use, minimizes bias
- **TowardZero**: Useful for truncation operations
- **TowardPositive/TowardNegative**: Interval arithmetic
- **NearestTiesToAway**: Matches grade-school rounding

### Common Pitfalls

1. **Operator precedence**: Use parentheses to clarify: `(x + y) * z` vs `x + (y * z)`
2. **Default rounding**: Operators use NearestTiesToEven; use explicit methods for other modes
3. **NaN comparisons**: `NaN != NaN` is always true (use `IsNaN()` predicate)
4. **Signed zeros**: `+0 == -0` but `1/+0 != 1/-0`
5. **Precision loss**: Converting between formats can lose precision

## See Also

- **Real Expressions**: For exact arithmetic without floating-point errors (can also be used with optimizer for finding max/min values)
- **BitVectors**: For low-level bit manipulation
